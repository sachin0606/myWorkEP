<#
.SYNOPSIS
  Keep PREVIOUS LCN series per genre. Within each genre, order by main-server ranking.
  Insert exactly 3 free LCNs between genres. Do NOT pack all genres from LCN 1.
#>
$ErrorActionPreference = 'Stop'
$epgDir = 'C:\Users\alsha\Desktop\EPG'
$gapBetweenGenres = 3
$maxLcnAllowed = 999

function Norm([string]$n) {
  if (-not $n) { return '' }
  $s = $n.ToUpperInvariant()
  $s = $s -replace '(?i)[\s\-_/]*(\-)?(FREE|PAY|FTA)\b',''
  $s = $s -replace '(?i)\s*RS[\s\.\-]*\d+(\.\d+)?',''
  $s = $s -replace '[^A-Z0-9]+',' '
  return $s.Trim()
}

# Kannada sub-genres — aligned to main-server 800-series order clusters
function Get-KannadaSubBlock([string]$name) {
  $n = $name.ToUpperInvariant()
  if ($n -match 'MOVIE|CINEMA|PICCHAR|DINKI|PUBLIC MOV') { return 'KANNADA_MOVIES' }
  # News cluster on main: R.KANNADA, News18, Suvarna News, Guarantee, News 1st, Sampoorna, Zee News, Public TV, TV9, Raj News, News Beat/Alert
  if ($n -match '^R\.?\s*KANNADA\b' -or $n -match '\bR KANNADA\b') { return 'KANNADA_NEWS' }
  if ($n -match 'NEWS|TV9|PUBLIC TV|SUVARNA NEWS|NEWS18|NEWS 1ST|SAMPOORNA|GUARANTEE|RAJ NEWS|NEWS BEAT|NEWS ALERT') {
    return 'KANNADA_NEWS'
  }
  if ($n -match 'MUSIC|MUSIX|PUBLIC MUSIC|UDAYA MUSIC|RAJ MUSIX') { return 'KANNADA_MUSIC' }
  if ($n -match 'SVBC|SANKARA|SHIRDI|AASTHA|SANSKAR') { return 'DEVOTIONAL' }
  return 'KANNADA_GEC'
}

# Map MAIN-SERVER remote LCN band -> local genre (source of truth for language/genre)
function Get-BlockFromRemoteLcn([int]$rlcn, [string]$name) {
  $n = $name.ToUpperInvariant()

  # Hard overrides that must win everywhere
  if ($n -match 'SPORT|EURO SPORT|SS KHEL|\bSS1\b|\bSS2\b|STAR SPORTS') {
    if ($n -match '\bHD\b' -or $n -match ' HD') { return 'HD_SPORTS' }
    return 'SPORTS'
  }
  if ($n -match 'REPUBLIC' -and $n -match 'BHARAT') { return 'HINDI_NEWS' }
  if ($n -match 'REPUBLIC' -and $n -match 'KANNADA') { return 'KANNADA_NEWS' }
  if ($n -match 'REPUBLIC\s*TV' -and $n -notmatch 'BHARAT|KANNADA|BANGLA') { return 'ENGLISH_NEWS' }

  if ($n -match '\bHD\b' -or $n -match ' HD') {
    if ($n -match 'KANNADA|SUVARNA|UDAYA|PICCHAR|PICHURE') { return 'HD_KANNADA' }
    if ($rlcn -ge 951 -and $rlcn -le 960) { return 'HD_SPORTS' }
    if ($n -match 'MARATHI|DISCOVERY|ANIMAL|TRAVEL|MN\+|MNX|MOVIES NOW') { return 'HD_OTHER' }
    return 'HD_HINDI'
  }

  if ($rlcn -ge 800 -and $rlcn -le 849) { return (Get-KannadaSubBlock $name) }
  if ($rlcn -ge 850 -and $rlcn -le 899) { return 'URDU' }
  if ($rlcn -ge 951 -and $rlcn -le 960) { return 'HD_SPORTS' }
  if ($rlcn -ge 901 -and $rlcn -le 950) {
    if ($n -match 'KANNADA|SUVARNA|PICHURE|PICCHAR') { return 'HD_KANNADA' }
    if ($n -match 'SPORT') { return 'HD_SPORTS' }
    if ($n -match 'MARATHI|MN\+|MNX|DISCOVERY|ANIMAL') { return 'HD_OTHER' }
    return 'HD_HINDI'
  }
  if ($rlcn -ge 700 -and $rlcn -le 749) { return 'TELUGU' }
  if ($rlcn -ge 775 -and $rlcn -le 799) { return 'MALAYALAM' }
  if ($rlcn -ge 650 -and $rlcn -le 699) { return 'TAMIL' }
  if ($rlcn -ge 520 -and $rlcn -le 549) {
    if ($n -match 'TELUGU') { return 'TELUGU' }
    return 'MARATHI'
  }
  if ($rlcn -ge 470 -and $rlcn -le 499) { return 'DEVOTIONAL' }
  if ($rlcn -ge 412 -and $rlcn -le 430) {
    if ($n -match '9XM|MTV|MUSIC|ZING|ZOOM|TASHAN|INSYNC|E 24') { return 'HINDI_MUSIC' }
    return 'SPORTS'
  }
  if ($rlcn -ge 431 -and $rlcn -le 469) {
    if ($n -match 'BHOJPURI|PITAARA|DABANGG|FILMY|SHOWBOX|B4U MOV') { return 'HINDI_MOVIES' }
    return 'HINDI_MUSIC'
  }
  if ($rlcn -ge 400 -and $rlcn -le 411) { return 'ENGLISH_NEWS' }
  if ($rlcn -ge 350 -and $rlcn -le 399) { return 'HINDI_NEWS' }
  if ($rlcn -ge 300 -and $rlcn -le 349) {
    if ($n -match 'KIDS|NICK|POGO|CARTOON') { return 'KIDS' }
    return 'INFOTAINMENT'
  }
  if ($rlcn -ge 250 -and $rlcn -le 299) { return 'ENGLISH_MOVIES' }
  if ($rlcn -ge 200 -and $rlcn -le 249) { return 'HINDI_MOVIES' }
  if ($rlcn -ge 150 -and $rlcn -le 199) { return 'DD_REGIONAL' }
  if ($rlcn -ge 101 -and $rlcn -le 149) {
    if ($n -match 'KIDS|NICK|POGO|HUNGAMA|CARTOON') { return 'KIDS' }
    if ($n -match 'BHAKTI') { return 'DEVOTIONAL' }
    if ($n -match 'MARATHI') { return 'MARATHI' }
    if ($n -match 'DISCOVERY|ANIMAL|FOOD') { return 'INFOTAINMENT' }
    if ($n -match 'DD\b') { return 'DD_REGIONAL' }
    return 'HINDI_GEC'
  }
  return $null
}

# Name-only fallback when no main-server match
function Get-BlockByName([string]$name) {
  $n = $name.ToUpperInvariant()
  if ($n -match 'SPORT|EURO SPORT|SS KHEL|\bSS1\b|\bSS2\b|STAR SPORTS') {
    if ($n -match '\bHD\b') { return 'HD_SPORTS' }
    return 'SPORTS'
  }
  if ($n -match 'REPUBLIC' -and $n -match 'BHARAT') { return 'HINDI_NEWS' }
  if ($n -match 'REPUBLIC\s*TV' -and $n -notmatch 'BHARAT|KANNADA') { return 'ENGLISH_NEWS' }
  if ($n -match '^DD\b' -or $n -match '\bDD\b' -or $n -match 'SANSAD') { return 'DD_REGIONAL' }
  if ($n -match '\bHD\b' -or $n -match ' HD') {
    if ($n -match 'KANNADA|SUVARNA|UDAYA|PICCHAR|PICHURE') { return 'HD_KANNADA' }
    if ($n -match 'MARATHI|DISCOVERY|ANIMAL|TRAVEL|MN\+|MNX|MOVIES NOW') { return 'HD_OTHER' }
    return 'HD_HINDI'
  }
  if ($n -match 'KANNADA|UDAYA|SUVARNA|SIRIKANNADA|AYUSH|PUBLIC MUSIC|PUBLIC TV|PUBLIC MOV|DINKI|NKR|PICCHAR|R\.?\s*KANNADA') {
    return (Get-KannadaSubBlock $name)
  }
  if ($n -match 'TELUGU|GEMINI|MAA\b|ETV|SAKSHI|10\s*TV|HMTV') { return 'TELUGU' }
  if ($n -match 'TAMIL|POLIMER|SUN\s*TV|KALAIGNAR|JAYA|VIJAY|ZEE TAMIL') { return 'TAMIL' }
  if ($n -match 'MALAYALAM|KERALA|KAIRALI|ASIANET|SURYA|MAZHAVIL') { return 'MALAYALAM' }
  if (($n -match 'TIMES NOW' -and $n -notmatch 'NAVABHARAT') -or $n -match 'NDTV 24|CNN NEWS|WION|MIRROR NOW|CNBC TV18|BBC WORLD|AL JAZEERA') {
    return 'ENGLISH_NEWS'
  }
  if ($n -match 'TIMES NOW NAV|NAVABHARAT') { return 'HINDI_NEWS' }
  if ($n -match 'STAR MOVIES|SONY PIX|ROMEDY|MOVIES NOW|MNX|HBO') { return 'ENGLISH_MOVIES' }
  if ($n -match 'NICK|POGO|CARTOON|DISNEY|HUNGAMA|SONIC|CHINTO|CHINTU|SONY YAY') { return 'KIDS' }
  if ($n -match 'DISCOVERY|ANIMAL|TLC|NGC|NATIONAL GEO|BBC EARTH|HISTORY|TURBO|SCIENCE|FOOD FOOD|EPIC TV') {
    return 'INFOTAINMENT'
  }
  if ($n -match 'ARY|QTV|SALAM|URDU|MUNSIF|MERCY|GULISTAN|CHANNEL WIN|TEHZEEB|4TV NEWS|SAUDI') { return 'URDU' }
  if ($n -match 'MARATHI|SAHYADRI|JHAKAAS|MAIBOLI|PRAVAH|TALKIES|MAJHA|LOKMAT|24 TAAS|SAAM|PUDHARI|FAKT|ABP MAJHA') {
    return 'MARATHI'
  }
  if ($n -match 'BHAKTI|SHIRDI|PEACE OF MIND|SANSKAR|AASTHA|SANKARA|ANGEL|SHUBH|JINVANI|SARV DHARM|SVBC|AWAKENING|HOPE CHANNEL|SANT WANI') {
    return 'DEVOTIONAL'
  }
  if ($n -match 'AAJ TAK|ABP NEWS|ZEE NEWS|INDIA TV|NEWS NATION|TV9 BHARATVARSH|NEWS24|CNBC AWAZ|NDTV INDIA|HINDI KHABAR|BHARAT SAMACHAR|IBC|IND24|KHABRAIN|NEWS 11|ZEE BHARAT|BHARAT EXPRESS') {
    return 'HINDI_NEWS'
  }
  if ($n -match 'CINEMA|MAX\b|STAR GOLD|&PICTURES|ZEE BOLLYWOOD|ZEE ACTION|CINEPLEX|GOLDMINES|KADAK|MANORANJAN|B4U|BHOJPURI|DABANGG|PITAARA|FILMY|SHOWBOX') {
    return 'HINDI_MOVIES'
  }
  if ($n -match '9XM|MTV|MUSIC INDIA|9X JALWA|ZING|ZOOM|TASHAN|PUNJABI|INSYNC|E 24|7S MUSIC') { return 'HINDI_MUSIC' }
  if ($n -match 'COLORS|ZEE TV|SONY|STAR PLUS|STAR BHARAT|AND TV|UTSAV|ANMOL|SAB\b|PAL\b|RISHTEY') { return 'HINDI_GEC' }
  return 'OTHER'
}

# --- remote ranking + LCN by exact name ---
$remote = Import-Csv "$epgDir\REMOTE_WEBHOOK_CHANNELS.csv"
$rankByName = @{}
$remoteLcnByName = @{}
$remoteNameByNorm = @{}
foreach ($rc in $remote) {
  $key = Norm $rc.channel_name
  if (-not $key) { continue }
  $rk = [int]$rc.ranking
  $rl = [int]$rc.lcn
  if (-not $rankByName.ContainsKey($key) -or $rk -lt $rankByName[$key]) {
    $rankByName[$key] = $rk
    $remoteLcnByName[$key] = $rl
    $remoteNameByNorm[$key] = [string]$rc.channel_name
  }
}

function Find-RemoteKey([string]$localNorm) {
  if (-not $localNorm) { return $null }
  if ($rankByName.ContainsKey($localNorm)) { return $localNorm }
  # Strict prefix only when both sides look like the same channel (min 10 chars)
  $best = $null; $bestRank = 999999
  foreach ($k in $rankByName.Keys) {
    if ($localNorm.Length -lt 10 -or $k.Length -lt 10) { continue }
    if ($localNorm -eq $k -or $localNorm.StartsWith($k) -or $k.StartsWith($localNorm)) {
      $rk = [int]$rankByName[$k]
      if ($rk -lt $bestRank) { $bestRank = $rk; $best = $k }
    }
  }
  return $best
}

# --- programs ---
$cs = 'Server=localhost;Database=EPG2;Integrated Security=True'
$conn = New-Object System.Data.SqlClient.SqlConnection $cs
$conn.Open()
$nameMap = @{}; $meta = @{}
$cmd = $conn.CreateCommand()
$cmd.CommandText = 'SELECT p.service_id, p.program_name, t.ts_name, t.ts_id, t.id AS ts_table_id FROM program_info p JOIN ts_info t ON t.id=p.ts_table_id'
$r = $cmd.ExecuteReader()
while ($r.Read()) {
  $sid = [int]$r[0]
  if (-not $nameMap.ContainsKey($sid)) {
    $nameMap[$sid] = [string]$r[1]
    $meta[$sid] = @{ ts_name=[string]$r[2]; ts_id=[int]$r[3]; ts_table_id=[int64]$r[4] }
  }
}
$r.Close()

# PREVIOUS LCNs = backup before reshuffle (authoritative "old")
$prevLcn = @{}
$cmd.CommandText = 'SELECT des_value FROM ts_des_values_lcn_backup_gapped_20260916'
$r = $cmd.ExecuteReader()
while ($r.Read()) {
  $b = [byte[]]$r[0]
  for ($i = 0; $i -le $b.Length - 4; $i += 4) {
    $sid = ($b[$i] * 256) + $b[$i + 1]
    $lcn = (($b[$i + 2] -band 0x3F) * 256) + $b[$i + 3]
    if ($sid -gt 0 -and $lcn -gt 0) { $prevLcn[$sid] = $lcn }
  }
}
$r.Close()

# live (for change report only)
$curLcn = @{}
$cmd.CommandText = @"
SELECT val.des_value FROM ts_des_values val
INNER JOIN descriptor_type typ ON typ.id = val.des_type_table_id
WHERE typ.des_tag=131 AND typ.des_location='NIT_TS'
"@
$r = $cmd.ExecuteReader()
while ($r.Read()) {
  $b = [byte[]]$r[0]
  for ($i = 0; $i -le $b.Length - 4; $i += 4) {
    $sid = ($b[$i] * 256) + $b[$i + 1]
    $lcn = (($b[$i + 2] -band 0x3F) * 256) + $b[$i + 3]
    if ($sid -gt 0 -and $lcn -gt 0) { $curLcn[$sid] = $lcn }
  }
}
$r.Close()
$conn.Close()

$rows = New-Object System.Collections.Generic.List[object]
$matchStats = @{ exact = 0; fuzzy = 0; name_only = 0 }
foreach ($sid in $nameMap.Keys) {
  $nm = $nameMap[$sid]
  $key = Norm $nm
  $rkey = Find-RemoteKey $key
  $rk = 900000
  $rlcn = 0
  $matchHow = 'name_only'
  if ($rkey) {
    $rk = [int]$rankByName[$rkey]
    $rlcn = [int]$remoteLcnByName[$rkey]
    $matchHow = if ($rkey -eq $key) { 'exact' } else { 'fuzzy' }
    $matchStats[$matchHow]++
    $fromRemote = Get-BlockFromRemoteLcn $rlcn $nm
    if ($fromRemote) { $block = $fromRemote }
    else { $block = Get-BlockByName $nm }
  } else {
    $block = Get-BlockByName $nm
    $matchStats.name_only++
  }  if ($rk -ge 900000) { $rk = 900000 + $sid }
  $prev = if ($prevLcn.ContainsKey($sid)) { [int]$prevLcn[$sid] } else { 0 }
  $rows.Add([PSCustomObject]@{
    service_id = $sid
    channel_name = $nm
    block = $block
    remote_rank = $rk
    remote_lcn = $rlcn
    match_how = $matchHow
    previous_lcn = $prev
    current_lcn = if ($curLcn.ContainsKey($sid)) { [int]$curLcn[$sid] } else { 0 }
    ts_name = $meta[$sid].ts_name
    ts_id = $meta[$sid].ts_id
    ts_table_id = $meta[$sid].ts_table_id
  })
}
Write-Host ("Remote genre map: exact={0} fuzzy={1} name_fallback={2}" -f $matchStats.exact, $matchStats.fuzzy, $matchStats.name_only)

function Get-SeriesStart([int[]]$prevVals) {
  # Ignore illegal 4-digit LCNs; use cluster around median so outliers (e.g. HD at 451) don't pull series
  $vals = @($prevVals | Where-Object { $_ -ge 1 -and $_ -le 999 } | Sort-Object)
  if ($vals.Count -eq 0) { return 900 }
  $med = [int]$vals[[Math]::Floor(($vals.Count - 1) / 2)]
  $cluster = @($vals | Where-Object { $_ -ge ($med - 100) -and $_ -le ($med + 100) })
  if ($cluster.Count -eq 0) { $cluster = $vals }
  return [int]($cluster | Measure-Object -Minimum).Minimum
}

# Genre order = previous series order (robust cluster start, not raw min)
$blockStats = @{}
foreach ($g in ($rows | Group-Object block)) {
  $prevVals = @($g.Group | ForEach-Object { [int]$_.previous_lcn })
  $oldMin = Get-SeriesStart $prevVals
  $blockStats[$g.Name] = @{ old_min = [int]$oldMin; count = $g.Count }
}
$blockOrder = @($blockStats.Keys | Sort-Object { $blockStats[$_].old_min }, { $_ })
$final = New-Object System.Collections.Generic.List[object]
$summary = New-Object System.Collections.Generic.List[object]
$prevEnd = 0

foreach ($bName in $blockOrder) {
  $grp = @($rows | Where-Object { $_.block -eq $bName } | Sort-Object remote_rank, channel_name)
  if ($grp.Count -eq 0) { continue }

  # Keep previous series start; only push forward if needed for 3-gap after prior genre
  $oldStart = [int]$blockStats[$bName].old_min
  $start = $oldStart
  if ($prevEnd -gt 0) {
    $minAllowed = $prevEnd + $gapBetweenGenres + 1
    if ($start -lt $minAllowed) { $start = $minAllowed }
  }
  if ($start -lt 1) { $start = 1 }

  $i = 0
  foreach ($c in $grp) {
    $lcn = $start + $i
    if ($lcn -gt $maxLcnAllowed) {
      throw ("LCN {0} > {1} for {2} in {3}." -f $lcn, $maxLcnAllowed, $c.channel_name, $bName)
    }
    $final.Add([PSCustomObject]@{
      proposed_lcn = $lcn
      block = $bName
      rank_in_block = ($i + 1)
      remote_rank = $c.remote_rank
      remote_lcn = $c.remote_lcn
      match_how = $c.match_how
      previous_lcn = $c.previous_lcn
      current_lcn = $c.current_lcn
      lcn_change = if ($c.previous_lcn -gt 0 -and $c.previous_lcn -ne $lcn) { 'YES' } else { 'NO' }
      service_id = $c.service_id
      channel_name = $c.channel_name
      ts_name = $c.ts_name
      ts_id = $c.ts_id
      ts_table_id = $c.ts_table_id
      series_kept = $oldStart
    })
    $i++
  }
  $endUsed = $start + $grp.Count - 1
  $summary.Add([PSCustomObject]@{
    block = $bName
    previous_series_start = $oldStart
    start = $start
    end_used = $endUsed
    used = $grp.Count
    used_range = "$start-$endUsed"
    free_gap_after = "$($endUsed+1)-$($endUsed+$gapBetweenGenres)"
    gap_after = $gapBetweenGenres
    shifted_from_old = if ($start -ne $oldStart) { 'YES' } else { 'NO' }
  })
  $prevEnd = $endUsed
}

$out = "$epgDir\GAPPED_RANKED_LINEUP_PROPOSAL.csv"
$final | Sort-Object proposed_lcn | Export-Csv $out -NoTypeInformation -Encoding UTF8
$sumOut = "$epgDir\GAPPED_BLOCK_SUMMARY.csv"
$summary | Export-Csv $sumOut -NoTypeInformation -Encoding UTF8

$staged = @{
  generated_at = (Get-Date).ToString('o')
  rule = 'Genre from main-server remote LCN bands; ranking within genre from main-server; keep previous local LCN series; 3 free LCNs between genres; no global reshuffle'
  max_lcn = $maxLcnAllowed
  gap_between_genres = $gapBetweenGenres
  channels = @($final | ForEach-Object {
    @{ service_id=$_.service_id; channel_name=$_.channel_name; ranking=$_.remote_rank; lcn=$_.proposed_lcn; visible=$true; block=$_.block }
  })
}
$staged | ConvertTo-Json -Depth 4 | Set-Content "$epgDir\STAGED_GAPPED_LINEUP.json" -Encoding UTF8

$minL = ($final | ForEach-Object { [int]$_.proposed_lcn } | Measure-Object -Minimum).Minimum
$maxL = ($final | ForEach-Object { [int]$_.proposed_lcn } | Measure-Object -Maximum).Maximum
$chgPrev = @($final | Where-Object { $_.lcn_change -eq 'YES' }).Count
$unchanged = @($final | Where-Object { $_.lcn_change -eq 'NO' }).Count

Write-Host "`n=== KEEP PREVIOUS SERIES + 3-GAP + RANK WITHIN GENRE ==="
$summary | Format-Table -AutoSize
Write-Host "Wrote $out"
Write-Host "channels=$($final.Count) vs_previous: unchanged=$unchanged changed=$chgPrev range=$minL..$maxL"
Write-Host "`nKannada GEC:"
$final | Where-Object block -eq 'KANNADA_GEC' | Sort-Object proposed_lcn | Format-Table proposed_lcn,previous_lcn,remote_rank,remote_lcn,channel_name -AutoSize -ErrorAction SilentlyContinue
# remote_lcn may not be on final - it's on rows only. Show from proposal after export uses final fields.

Write-Host "Kannada NEWS (main order):"
$final | Where-Object block -eq 'KANNADA_NEWS' | Sort-Object proposed_lcn | Format-Table proposed_lcn,previous_lcn,remote_rank,channel_name -AutoSize
Write-Host "Sports:"
$final | Where-Object { $_.block -eq 'SPORTS' } | Sort-Object proposed_lcn | Format-Table proposed_lcn,previous_lcn,remote_rank,channel_name -AutoSize
Write-Host "Urdu:"
$final | Where-Object block -eq 'URDU' | Sort-Object proposed_lcn | Format-Table proposed_lcn,previous_lcn,remote_rank,channel_name -AutoSize
Write-Host "Hindi News sample:"
$final | Where-Object block -eq 'HINDI_NEWS' | Sort-Object proposed_lcn | Select-Object -First 12 | Format-Table proposed_lcn,previous_lcn,remote_rank,channel_name -AutoSize
Write-Host "Genre counts:"
$final | Group-Object block | Sort-Object Name | Format-Table Name, Count -AutoSize
