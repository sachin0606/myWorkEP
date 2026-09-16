$ErrorActionPreference = 'Stop'

function Parse-LcnHex([string]$hex) {
  $map = @{}
  if ([string]::IsNullOrWhiteSpace($hex)) { return $map }
  $hex = $hex.Trim()
  for ($i = 0; $i -lt $hex.Length; $i += 8) {
    if ($i + 8 -gt $hex.Length) { break }
    $chunk = $hex.Substring($i, 8)
    $sid = [Convert]::ToInt32($chunk.Substring(0, 4), 16)
    $lcn = [Convert]::ToInt32($chunk.Substring(4, 4), 16) -band 0x3FFF
    $map[$sid] = $lcn
  }
  return $map
}

$epgProgFile = 'C:\Users\alsha\AppData\Local\Temp\epg_programs.txt'
$epgLcnFile = 'C:\Users\alsha\AppData\Local\Temp\epg_lcn.txt'
$q532 = Import-Csv 'C:\Users\alsha\AppData\Local\Temp\q532_mux_map.csv'
$q632 = Import-Csv 'C:\Users\alsha\AppData\Local\Temp\q632_mux_map.csv'

$lcnByTsSid = @{}
Get-Content $epgLcnFile | ForEach-Object {
  if ($_ -match '^\d+\|TS_') {
    $p = $_ -split '\|', 3
    $tsId = [int]$p[0]
    $hex = $p[2]
    $parsed = Parse-LcnHex $hex
    foreach ($k in $parsed.Keys) { $lcnByTsSid["$tsId|$k"] = $parsed[$k] }
  }
}

$epg = @()
Get-Content $epgProgFile | ForEach-Object {
  if ($_ -match '^\d+\|TS_') {
    $p = $_ -split '\|', 4
    $tsId = [int]$p[0]
    $sid = [int]$p[2]
    $key = "$tsId|$sid"
    $lcn = if ($lcnByTsSid.ContainsKey($key)) { [int]$lcnByTsSid[$key] } else { $null }
    $freq = 0
    if ($p[1] -match '^TS_(\d+)$') { $freq = [int]$Matches[1] }
    $epg += [pscustomobject]@{
      TsId = $tsId; TS = $p[1]; Freq = $freq; SID = $sid; Name = $p[3]; Lcn = $lcn
    }
  }
}

$qam = @()
foreach ($row in @($q532 + $q632)) {
  $sid = 0
  if ($row.SID -match '^\d+$') { $sid = [int]$row.SID }
  $name = $row.Name
  if ($name -eq '(no programs)') { $name = '' }
  $qam += [pscustomobject]@{
    QAM = $row.QAM
    CH = [int]$row.CH
    Freq = [int]$row.Freq
    RF = [int]$row.RF
    SID = $sid
    Name = $name
    QamLcn = $(if ($row.QamLcn -match '^\d+$') { [int]$row.QamLcn } else { $null })
    Source = $row.Source
  }
}

# Prefer ON RF when the same frequency exists twice (6.32 CH1-4 off duplicates)
$qamOn = $qam | Where-Object { $_.RF -eq 1 -and $_.SID -gt 0 }
$qamByFreqSid = @{}
foreach ($q in $qamOn) { $qamByFreqSid["$($q.Freq)|$($q.SID)"] = $q }

$freqMeta = @{}
foreach ($q in $qam) {
  $key = [string]$q.Freq
  if (-not $freqMeta.ContainsKey($key)) {
    $freqMeta[$key] = [pscustomobject]@{ QAM = $q.QAM; CH = $q.CH; RF = $q.RF; Freq = $q.Freq }
  }
  if ($q.RF -eq 1) {
    $freqMeta[$key] = [pscustomobject]@{ QAM = $q.QAM; CH = $q.CH; RF = $q.RF; Freq = $q.Freq }
  }
}

function Status-ForEpg($e, $q) {
  $meta = $freqMeta[[string]$e.Freq]
  if (-not $meta) { return 'EPG_ONLY_NO_QAM_FREQ' }
  if ($meta.RF -ne 1) { return 'EPG_ON_RF_OFF' }
  if (-not $q) {
    $any = @($qam | Where-Object { $_.Freq -eq $e.Freq -and $_.RF -eq 1 -and $_.SID -gt 0 })
    if ($any.Count -eq 0) { return 'QAM_TS_EMPTY' }
    return 'MISSING_ON_QAM'
  }
  return 'OK_ON_QAM'
}

$map = New-Object System.Collections.Generic.List[object]
$usedQam = @{}

foreach ($e in $epg) {
  $q = $null
  $qk = "$($e.Freq)|$($e.SID)"
  if ($qamByFreqSid.ContainsKey($qk)) {
    $q = $qamByFreqSid[$qk]
    $usedQam[$qk] = $true
  }
  $meta = $freqMeta[[string]$e.Freq]
  $st = Status-ForEpg $e $q
  $map.Add([pscustomobject]@{
    TS = $e.TS
    TS_ID = $e.TsId
    Freq_MHz = $e.Freq
    QAM_IP = $(if ($meta) { $meta.QAM } else { '' })
    QAM_CH = $(if ($meta) { $meta.CH } else { '' })
    RF = $(if ($meta) { if ($meta.RF -eq 1) { 'ON' } else { 'OFF' } } else { 'NONE' })
    EPG_Name = $e.Name
    EPG_SID = $e.SID
    EPG_LCN = $e.Lcn
    QAM_Name = $(if ($q) { $q.Name } else { '' })
    QAM_SID = $(if ($q) { $q.SID } else { '' })
    QAM_LCN = $(if ($q) { $q.QamLcn } else { '' })
    QAM_Source = $(if ($q) { $q.Source } else { '' })
    Status = $st
    WillTune = $(if ($st -eq 'OK_ON_QAM') { 'YES' } else { 'NO' })
    Note = switch ($st) {
      'OK_ON_QAM' { 'SID present on this RF' }
      'MISSING_ON_QAM' { 'EPG advertises this SID but QAM mux does not carry it' }
      'QAM_TS_EMPTY' { 'RF is ON but mux has no programs' }
      'EPG_ON_RF_OFF' { 'EPG TS exists but this frequency is RF OFF' }
      'EPG_ONLY_NO_QAM_FREQ' { 'No QAM channel at this frequency' }
      default { $st }
    }
  })
}

foreach ($q in $qamOn) {
  $qk = "$($q.Freq)|$($q.SID)"
  if ($usedQam.ContainsKey($qk)) { continue }
  $epgTs = ($epg | Where-Object { $_.Freq -eq $q.Freq } | Select-Object -First 1)
  $map.Add([pscustomobject]@{
    TS = $(if ($epgTs) { $epgTs.TS } else { "QAM_$($q.Freq)" })
    TS_ID = $(if ($epgTs) { $epgTs.TsId } else { '' })
    Freq_MHz = $q.Freq
    QAM_IP = $q.QAM
    QAM_CH = $q.CH
    RF = 'ON'
    EPG_Name = ''
    EPG_SID = ''
    EPG_LCN = ''
    QAM_Name = $q.Name
    QAM_SID = $q.SID
    QAM_LCN = $q.QamLcn
    QAM_Source = $q.Source
    Status = 'QAM_ONLY_NO_EPG'
    WillTune = 'YES_NO_EPG_LCN'
    Note = 'On QAM but not in EPG for this frequency - box may not list it from NIT'
  })
}

$full = $map | Sort-Object { [int]$_.TS_ID }, { [int]$_.EPG_SID }, { [int]($_.QAM_SID -as [int]) }
$missing = $full | Where-Object { $_.Status -in @('MISSING_ON_QAM','QAM_TS_EMPTY') }
$extra = $full | Where-Object { $_.Status -eq 'QAM_ONLY_NO_EPG' }
$focus = $full | Where-Object { $_.TS_ID -in 9,10,12,13,14,15 }

$summary = $epg | Group-Object TS | ForEach-Object {
  $ts = $_.Name
  $rows = @($full | Where-Object { $_.TS -eq $ts })
  $epgN = @($rows | Where-Object { $_.EPG_SID -ne '' }).Count
  $ok = @($rows | Where-Object { $_.Status -eq 'OK_ON_QAM' }).Count
  $miss = @($rows | Where-Object { $_.Status -in @('MISSING_ON_QAM','QAM_TS_EMPTY') }).Count
  $xtra = @($rows | Where-Object { $_.Status -eq 'QAM_ONLY_NO_EPG' }).Count
  $rfOff = @($rows | Where-Object { $_.Status -eq 'EPG_ON_RF_OFF' }).Count
  $sample = $rows | Select-Object -First 1
  [pscustomobject]@{
    TS = $ts
    TS_ID = $sample.TS_ID
    Freq_MHz = $sample.Freq_MHz
    QAM_IP = $sample.QAM_IP
    QAM_CH = $sample.QAM_CH
    RF = $sample.RF
    EPG_Count = $epgN
    Match_OK = $ok
    Missing_on_QAM = $miss
    Extra_on_QAM = $xtra
    EPG_RF_Off = $rfOff
    BoxImpact = $(
      if ($sample.RF -eq 'OFF' -or $sample.RF -eq 'NONE') { 'Not on air' }
      elseif ($miss -eq $epgN -and $epgN -gt 0) { 'ALL EPG channels fail to tune' }
      elseif ($miss -gt 0) { "$miss of $epgN EPG channels fail to tune" }
      else { 'OK' }
    )
  }
} | Sort-Object { [int]$_.TS_ID }

$deskXlsx = 'C:\Users\alsha\Desktop\EPG_QAM_Channel_Map.xlsx'
$deskCsv = 'C:\Users\alsha\Desktop\EPG_QAM_Channel_Map.csv'
$srcCsv = 'C:\Users\alsha\source\EPG-System\scripts\EPG_QAM_Channel_Map.csv'
$full | Export-Csv $srcCsv -NoTypeInformation -Encoding UTF8
$full | Export-Csv $deskCsv -NoTypeInformation -Encoding UTF8

$excelOk = $false
try {
  $xl = New-Object -ComObject Excel.Application
  $xl.Visible = $false
  $xl.DisplayAlerts = $false
  $wb = $xl.Workbooks.Add()
  while ($wb.Worksheets.Count -lt 5) { [void]$wb.Worksheets.Add() }
  $wb.Worksheets.Item(1).Name = 'Full_Map'
  $wb.Worksheets.Item(2).Name = 'Missing_on_QAM'
  $wb.Worksheets.Item(3).Name = 'Extra_on_QAM'
  $wb.Worksheets.Item(4).Name = 'Focus_TS9_15'
  $wb.Worksheets.Item(5).Name = 'Summary'

  function Write-Sheet($ws, $rows) {
    $arr = @($rows)
    if ($arr.Count -eq 0) {
      $ws.Cells.Item(1, 1) = 'No rows'
      return
    }
    $props = @($arr[0].PSObject.Properties.Name)
    for ($c = 0; $c -lt $props.Count; $c++) { $ws.Cells.Item(1, $c + 1) = $props[$c] }
    for ($r = 0; $r -lt $arr.Count; $r++) {
      for ($c = 0; $c -lt $props.Count; $c++) {
        $ws.Cells.Item($r + 2, $c + 1) = [string]$arr[$r].($props[$c])
      }
    }
    $used = $ws.Range($ws.Cells.Item(1, 1), $ws.Cells.Item($arr.Count + 1, $props.Count))
    $used.AutoFilter() | Out-Null
    $ws.Range($ws.Cells.Item(1, 1), $ws.Cells.Item(1, $props.Count)).Font.Bold = $true
    $ws.Columns.AutoFit() | Out-Null
  }

  Write-Sheet $wb.Worksheets.Item('Full_Map') $full
  Write-Sheet $wb.Worksheets.Item('Missing_on_QAM') $missing
  Write-Sheet $wb.Worksheets.Item('Extra_on_QAM') $extra
  Write-Sheet $wb.Worksheets.Item('Focus_TS9_15') $focus
  Write-Sheet $wb.Worksheets.Item('Summary') $summary

  if (Test-Path $deskXlsx) { Remove-Item $deskXlsx -Force }
  $wb.SaveAs($deskXlsx, 51)
  $wb.Close($false)
  $xl.Quit()
  [void][Runtime.InteropServices.Marshal]::ReleaseComObject($wb)
  [void][Runtime.InteropServices.Marshal]::ReleaseComObject($xl)
  $excelOk = $true
} catch {
  "Excel COM failed: $($_.Exception.Message)"
}

"`nMAP rows=$($full.Count)"
"Missing on QAM (will not tune): $($missing.Count)"
"Extra on QAM (no EPG): $($extra.Count)"
"OK match: $((@($full | Where-Object Status -eq 'OK_ON_QAM')).Count)"
"xlsx=$excelOk $deskXlsx"
"csv=$deskCsv"

"`n=== FOCUS TS 12-15 ==="
$summary | Where-Object { $_.TS_ID -in 12,13,14,15 } | Format-List
"`n=== OTHER BAD TS ==="
$summary | Where-Object { $_.BoxImpact -notmatch '^OK$' } | Format-Table TS, QAM_CH, RF, EPG_Count, Match_OK, Missing_on_QAM, Extra_on_QAM, BoxImpact -AutoSize
