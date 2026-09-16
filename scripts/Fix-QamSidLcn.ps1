$ErrorActionPreference = 'Stop'

function AuthH($u,$p) {
  @{ Authorization = ('Basic ' + [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes("${u}:${p}"))); 'Content-Type' = 'application/x-www-form-urlencoded' }
}
function Norm-Name([string]$n) {
  if (-not $n) { return '' }
  $s = $n.Trim()
  $s = $s -replace '(?i)[\s\-_/]*(\-)?(FREE|PAY|FTA)\b',''
  $s = $s -replace '(?i)\s*RS[\s\.\-]*\d+(\.\d+)?',''
  $s = $s -replace '\s+',' '
  return $s.Trim().ToUpperInvariant()
}
function BcdFreq([string]$hex) {
  if (-not $hex -or $hex.Length -lt 4) { return 0 }
  return [int]$hex.Substring(0,4)
}
function Parse-LcnHex([string]$hex) {
  $map = @{}
  if (-not $hex) { return $map }
  $hex = $hex.Trim()
  for ($i=0; $i+7 -lt $hex.Length; $i+=8) {
    $sid = [Convert]::ToInt32($hex.Substring($i,4),16)
    $b2 = [Convert]::ToInt32($hex.Substring($i+4,2),16)
    $b3 = [Convert]::ToInt32($hex.Substring($i+6,2),16)
    $map[$sid] = (($b2 -band 0x3F) -shl 8) + $b3
  }
  return $map
}
function Cgi($ip,$h,$proc,$body) {
  (Invoke-WebRequest -Uri "http://$ip/cgi.php?proctype=$proc" -Headers $h -Method POST -Body $body -TimeoutSec 30 -UseBasicParsing).Content
}
function HexPid([int]$v) { '0x{0:x4}' -f $v }

# EPG maps
$epgProg = sqlcmd -S localhost -E -d EPG2 -s '|' -W -h -1 -Q "SET NOCOUNT ON; SELECT t.ts_name, p.service_id, p.program_name FROM program_info p JOIN ts_info t ON t.id=p.ts_table_id"
$epgDes = sqlcmd -S localhost -E -d EPG2 -s '|' -W -h -1 -Q "SET NOCOUNT ON; SELECT t.ts_name, typ.des_tag, CONVERT(varchar(max), d.des_value, 2) FROM ts_des_values d JOIN descriptor_type typ ON typ.id=d.des_type_table_id JOIN ts_info t ON t.id=d.ts_table_id WHERE typ.des_tag IN (68,131)"

$freqOf = @{}
$lcnOfTs = @{}
foreach ($line in $epgDes) {
  if ($line -notmatch '\S') { continue }
  $p = $line.Split('|')
  if ($p.Count -lt 3) { continue }
  $ts = $p[0].Trim(); $tag = [int]$p[1]; $hex = $p[2].Trim()
  if ($tag -eq 68) { $freqOf[$ts] = BcdFreq $hex }
  if ($tag -eq 131) { $lcnOfTs[$ts] = Parse-LcnHex $hex }
}

$epgByFreqSid = @{}
$epgByFreqName = @{}
foreach ($line in $epgProg) {
  if ($line -notmatch '\S') { continue }
  $p = $line.Split('|')
  if ($p.Count -lt 3) { continue }
  $ts = $p[0].Trim(); $sid = [int]$p[1]; $name = $p[2]
  if (-not $freqOf.ContainsKey($ts)) { continue }
  $freq = [int]$freqOf[$ts]
  $lcn = -1
  if ($lcnOfTs.ContainsKey($ts) -and $lcnOfTs[$ts].ContainsKey($sid)) { $lcn = [int]$lcnOfTs[$ts][$sid] }
  $row = [pscustomobject]@{ Ts=$ts; Freq=$freq; Sid=$sid; Lcn=$lcn; Name=$name; Norm=(Norm-Name $name) }
  $epgByFreqSid["$freq|$sid"] = $row
  $nk = "$freq|$($row.Norm)"
  if (-not $epgByFreqName.ContainsKey($nk)) { $epgByFreqName[$nk] = $row }
}

function Get-Prg($ip,$h,$chIdx,$prgIdx) {
  $body = "op_code=3&tsout_ch_index=$chIdx&tsin_ch_index=0&dropdown_select=$chIdx&tab_index=1&tab_ts_mux=tab_ts_mux&hPrgDetailIndex=$prgIdx"
  $j = Cgi $ip $h setPrgInfo $body | ConvertFrom-Json
  return $j.prg_info
}
function Set-Prg($ip,$h,$chIdx,$prg,$newSid,$newLcn) {
  $sb = New-Object System.Text.StringBuilder
  [void]$sb.Append("op_code=1&tsout_ch_index=$chIdx&tsin_ch_index=0&dropdown_select=$chIdx&tab_index=1&tab_ts_mux=tab_ts_mux")
  [void]$sb.Append("&hPrgDetailIndex=$($prg.prg_index)")
  [void]$sb.Append("&prg_num=$newSid&lcn_num=$newLcn")
  [void]$sb.Append("&service_name=$([uri]::EscapeDataString([string]$prg.service_name))")
  [void]$sb.Append("&service_provider=$([uri]::EscapeDataString([string]$prg.service_provider))")
  [void]$sb.Append("&ser_type=0x{0:x2}" -f [int]$prg.service_type)
  [void]$sb.Append("&prg_pmt=$(HexPid ([int]$prg.pmt_pid))&prg_pcr=$(HexPid ([int]$prg.pcr_pid))")
  [void]$sb.Append("&SPTSOutput=$($prg.spts_channel)&major_num=$($prg.major_num)&minor_num=$($prg.minor_num)&source_id=$($prg.source_id)")
  [void]$sb.Append("&short_name=$([uri]::EscapeDataString([string]$prg.short_name))")
  [void]$sb.Append("&pmt_desc_en=$($prg.pmt_desc_en)&pmt_desc_tag=0x{0:x2}&pmt_desc_data=$($prg.pmt_desc_data)" -f [int]$prg.pmt_desc_tag)
  $i=0
  foreach ($es in @($prg.es_info)) {
    if ([int]$es.stream_effect -eq 1) { [void]$sb.Append("&eft_prg_elem$i=on") }
    [void]$sb.Append("&prg_elem$i=$(HexPid ([int]$es.stream_pid))")
    $i++
  }
  $raw = Cgi $ip $h setPrgInfo $sb.ToString()
  return $raw
}

function Collect-Fixes($ip,$user,$pass,$skipFreqs) {
  $h = AuthH $user $pass
  $mod = (Cgi $ip $h modulator_dvbc 'op_code=5&modu_index=0&rf_index=0') | ConvertFrom-Json
  $fixes = @()
  for ($i=0; $i -lt $mod.channels.Count; $i++) {
    $c = $mod.channels[$i]
    if ([int]$c.enable -ne 1) { continue }
    $freq = [int]$c.freq
    if ($skipFreqs -contains $freq) { continue }
    $j = (Cgi $ip $h refreshPrg "op_code=0&tsout_ch_index=$i&tsin_ch_index=0&dropdown_select=$i&tab_index=1&tab_ts_mux=tab_ts_mux") | ConvertFrom-Json
    $idx = 0
    foreach ($p in @($j.tsout.prg_info)) {
      $qSid = [int]$p.program_number
      $qLcn = [int]$p.logic_channel_number
      $norm = Norm-Name $p.service_name
      $epg = $null
      if ($epgByFreqSid.ContainsKey("$freq|$qSid")) { $epg = $epgByFreqSid["$freq|$qSid"] }
      $byName = $null
      if ($epgByFreqName.ContainsKey("$freq|$norm")) { $byName = $epgByFreqName["$freq|$norm"] }
      # Prefer name match on this frequency when SID does not match EPG
      $want = $null
      if ($byName) { $want = $byName }
      elseif ($epg) { $want = $epg }
      $need = $false
      $why = ''
      if ($want) {
        if ([int]$want.Sid -ne $qSid) { $need = $true; $why = "SID $qSid -> $($want.Sid)" }
        if ($freq -eq 194 -and [int]$want.Lcn -ge 0 -and [int]$want.Lcn -ne $qLcn) {
          $need = $true
          if ($why) { $why += '; ' }
          $why += "LCN $qLcn -> $($want.Lcn)"
        }
      }
      if ($need) {
        $fixes += [pscustomobject]@{
          Ip=$ip; User=$user; Pass=$pass; ChIdx=$i; Ch=($i+1); Freq=$freq
          PrgIdx=$idx; Name=$p.service_name; CurSid=$qSid; CurLcn=$qLcn
          NewSid=[int]$want.Sid; NewLcn=$(if([int]$want.Lcn -ge 0){[int]$want.Lcn}else{$qLcn})
          Why=$why
        }
      }
      $idx++
    }
  }
  $fixes
}

$skip = @(546)
$fixes = @()
$fixes += Collect-Fixes '192.168.5.32' admin ssadmin $skip
$fixes += Collect-Fixes '192.168.6.32' admin admin $skip

"`n=== PLANNED FIXES ==="
$fixes | Format-Table Freq,Ch,Name,CurSid,NewSid,CurLcn,NewLcn,Why -AutoSize
"count=$($fixes.Count)"

if ($fixes.Count -eq 0) { return }

# Two-pass SID change to avoid collisions
$groups = $fixes | Group-Object { "$($_.Ip)|$($_.ChIdx)" }
foreach ($g in $groups) {
  $f0 = $g.Group[0]
  $h = AuthH $f0.User $f0.Pass
  $chIdx = [int]$f0.ChIdx
  "--- pass1 temp SIDs $($f0.Ip) CH$($f0.Ch) $($f0.Freq) ---"
  foreach ($f in $g.Group) {
    $prg = Get-Prg $f0.Ip $h $chIdx ([int]$f.PrgIdx)
    $tmp = 40000 + [int]$f.PrgIdx
    $null = Set-Prg $f0.Ip $h $chIdx $prg $tmp ([int]$f.CurLcn)
    "  tmp $($f.Name) $($f.CurSid) -> $tmp"
  }
  "--- pass2 final SID/LCN ---"
  foreach ($f in $g.Group) {
    $prg = Get-Prg $f0.Ip $h $chIdx ([int]$f.PrgIdx)
    $resp = Set-Prg $f0.Ip $h $chIdx $prg ([int]$f.NewSid) ([int]$f.NewLcn)
    $ok = $resp -match '"name":"setPrgInfo"'
    "  set $($f.Name) -> SID $($f.NewSid) LCN $($f.NewLcn) ok=$ok"
  }
}

"`n=== VERIFY ==="
foreach ($g in $groups) {
  $f0 = $g.Group[0]
  $h = AuthH $f0.User $f0.Pass
  $j = (Cgi $f0.Ip $h refreshPrg "op_code=0&tsout_ch_index=$($f0.ChIdx)&tsin_ch_index=0&dropdown_select=$($f0.ChIdx)&tab_index=1&tab_ts_mux=tab_ts_mux") | ConvertFrom-Json
  "CH$($f0.Ch) $($f0.Freq) pid_map=$($j.tsout.pid_map_mode)"
  $j.tsout.prg_info | ForEach-Object { "  sid=$($_.program_number) lcn=$($_.logic_channel_number) $($_.service_name)" }
}
