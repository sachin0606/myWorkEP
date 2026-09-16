<#
.SYNOPSIS
  EPG / QAM operator console — rule-based, no AI required.

.DESCRIPTION
  Runs the same steps we used in ops (fetch main-server ranks, backup LCNs,
  build genre+gap plan, apply via Dexin BLL, export Excel, scramble check).

  "Thinking" is encoded as fixed rules in Build-GappedRankedLineup.ps1:
    - Genre = main-server remote LCN band + name overrides
    - Order inside genre = main-server ranking
    - Keep previous local LCN series; 3 free LCNs between genres
    - Apply via ApplyGappedLcnPlan (BLL + SetAllNoticeOn)

.EXAMPLE
  powershell -NoProfile -ExecutionPolicy Bypass -File .\EPG-Ops.ps1
  powershell -NoProfile -ExecutionPolicy Bypass -File .\EPG-Ops.ps1 -Action build
  powershell -NoProfile -ExecutionPolicy Bypass -File .\EPG-Ops.ps1 -Action apply -Confirm:$false
#>
[CmdletBinding(SupportsShouldProcess = $true)]
param(
  [ValidateSet(
    'menu', 'fetch', 'backup', 'build', 'excel', 'apply', 'verify',
    'scramble-check', 'full', 'status'
  )]
  [string]$Action = 'menu',

  [string]$EpgDir = 'C:\Users\alsha\Desktop\EPG',
  [string]$ScriptsDir = 'C:\Users\alsha\source\EPG-System\scripts',
  [string]$LibDir = 'C:\Users\alsha\source\EPG-System\original\EPG2.4',
  [string]$WebhookUrl = 'http://117.215.135.147:8092',
  [int]$Gap = 3,
  [switch]$Confirm
)

$ErrorActionPreference = 'Stop'

function Write-Step([string]$msg) {
  Write-Host ""
  Write-Host "=== $msg ===" -ForegroundColor Cyan
}

function Ensure-Dir([string]$p) {
  if (-not (Test-Path -LiteralPath $p)) { New-Item -ItemType Directory -Path $p | Out-Null }
}

function Invoke-FetchMainServer {
  Write-Step "Fetch main-server ranking ($WebhookUrl)"
  $outCsv = Join-Path $EpgDir 'REMOTE_WEBHOOK_CHANNELS.csv'
  $url = "$WebhookUrl/api/webhook/channels?sort=rank"
  $resp = Invoke-WebRequest -Uri $url -TimeoutSec 30 -UseBasicParsing
  $data = $resp.Content | ConvertFrom-Json
  if (-not $data.success -and -not $data.channels) {
    # some endpoints return array / wrapped
    if ($data -is [System.Array]) {
      $channels = $data
    } else {
      throw "Webhook fetch failed or unexpected JSON"
    }
  } else {
    $channels = $data.channels
  }
  if (-not $channels -or $channels.Count -lt 1) { throw "No channels from webhook" }
  $channels | Export-Csv -LiteralPath $outCsv -NoTypeInformation -Encoding UTF8
  Write-Host "Saved $($channels.Count) channels -> $outCsv"
  return $outCsv
}

function Invoke-BackupLcn {
  Write-Step "Backup live NIT LCN blobs (SQL)"
  $stamp = Get-Date -Format 'yyyyMMdd_HHmmss'
  $table = "ts_des_values_lcn_backup_ops_$stamp"
  $sql = @"
IF OBJECT_ID('dbo.$table') IS NULL
  SELECT * INTO dbo.$table
  FROM ts_des_values
  WHERE des_type_table_id IN (
    SELECT id FROM descriptor_type WHERE des_tag=131 AND des_location='NIT_TS'
  );
SELECT COUNT(*) AS backup_rows FROM dbo.$table;
"@
  sqlcmd -S localhost -E -d EPG2 -Q $sql
  # Also refresh the "previous" table used by Build script if missing
  sqlcmd -S localhost -E -d EPG2 -Q @"
IF OBJECT_ID('dbo.ts_des_values_lcn_backup_gapped_20260916') IS NULL
BEGIN
  SELECT * INTO dbo.ts_des_values_lcn_backup_gapped_20260916
  FROM ts_des_values
  WHERE des_type_table_id IN (
    SELECT id FROM descriptor_type WHERE des_tag=131 AND des_location='NIT_TS'
  );
END
"@
  Write-Host "Backup table: $table (and ensure gapped baseline exists)"
  return $table
}

function Invoke-BuildPlan {
  Write-Step "Build gapped ranked lineup (rules engine, gap=$Gap)"
  $build = Join-Path $ScriptsDir 'Build-GappedRankedLineup.ps1'
  if (-not (Test-Path -LiteralPath $build)) {
    $build = Join-Path $EpgDir 'Build-GappedRankedLineup.ps1'
  }
  if (-not (Test-Path -LiteralPath $build)) { throw "Missing Build-GappedRankedLineup.ps1" }
  # Keep Desktop copy in sync
  Copy-Item -LiteralPath $build -Destination (Join-Path $EpgDir 'Build-GappedRankedLineup.ps1') -Force
  & powershell -NoProfile -ExecutionPolicy Bypass -File $build
  $proposal = Join-Path $EpgDir 'GAPPED_RANKED_LINEUP_PROPOSAL.csv'
  if (-not (Test-Path $proposal)) { throw "Build did not write proposal CSV" }
  Write-Host "Proposal: $proposal"
  return $proposal
}

function Invoke-ExportExcel {
  Write-Step "Export Excel"
  $export = Join-Path $EpgDir 'Export-GappedLcnExcel.ps1'
  if (Test-Path -LiteralPath $export) {
    try {
      & powershell -NoProfile -ExecutionPolicy Bypass -File $export
    } catch {
      Write-Warning $_.Exception.Message
    }
  }
  # Always write a fresh SpreadsheetML as fallback
  $csv = Import-Csv (Join-Path $EpgDir 'GAPPED_RANKED_LINEUP_PROPOSAL.csv')
  $xls = Join-Path $EpgDir ("GAPPED_LCN_LINEUP_{0}.xls" -f (Get-Date -Format 'yyyyMMdd_HHmm'))
  function Esc([string]$s) {
    if ($null -eq $s) { return '' }
    return ($s -replace '&', '&amp;' -replace '<', '&lt;' -replace '>', '&gt;' -replace '"', '&quot;')
  }
  $sb = New-Object System.Text.StringBuilder
  [void]$sb.AppendLine('<?xml version="1.0"?><?mso-application progid="Excel.Sheet"?>')
  [void]$sb.AppendLine('<Workbook xmlns="urn:schemas-microsoft-com:office:spreadsheet" xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet">')
  [void]$sb.AppendLine('<Worksheet ss:Name="LCN Lineup"><Table>')
  [void]$sb.AppendLine('<Row><Cell><Data ss:Type="String">LCN</Data></Cell><Cell><Data ss:Type="String">Block</Data></Cell><Cell><Data ss:Type="String">MainRank</Data></Cell><Cell><Data ss:Type="String">PrevLCN</Data></Cell><Cell><Data ss:Type="String">ServiceID</Data></Cell><Cell><Data ss:Type="String">Channel</Data></Cell><Cell><Data ss:Type="String">TS</Data></Cell></Row>')
  foreach ($row in ($csv | Sort-Object { [int]$_.proposed_lcn })) {
    $prev = if ($row.previous_lcn) { $row.previous_lcn } else { $row.current_lcn }
    [void]$sb.Append('<Row>')
    [void]$sb.Append('<Cell><Data ss:Type="Number">' + [int]$row.proposed_lcn + '</Data></Cell>')
    [void]$sb.Append('<Cell><Data ss:Type="String">' + (Esc $row.block) + '</Data></Cell>')
    [void]$sb.Append('<Cell><Data ss:Type="Number">' + [int]$row.remote_rank + '</Data></Cell>')
    [void]$sb.Append('<Cell><Data ss:Type="Number">' + [int]$prev + '</Data></Cell>')
    [void]$sb.Append('<Cell><Data ss:Type="Number">' + [int]$row.service_id + '</Data></Cell>')
    [void]$sb.Append('<Cell><Data ss:Type="String">' + (Esc $row.channel_name) + '</Data></Cell>')
    [void]$sb.Append('<Cell><Data ss:Type="String">' + (Esc $row.ts_name) + '</Data></Cell>')
    [void]$sb.AppendLine('</Row>')
  }
  [void]$sb.AppendLine('</Table></Worksheet></Workbook>')
  [IO.File]::WriteAllText($xls, $sb.ToString(), [Text.UTF8Encoding]::new($false))
  Write-Host "Excel: $xls"
  return $xls
}

function Invoke-CompileApply {
  $csc = 'C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe'
  if (-not (Test-Path $csc)) { throw "csc.exe not found (need .NET Framework 4)" }
  if (-not (Test-Path $LibDir)) { throw "Missing Dexin lib dir: $LibDir" }
  $src = Join-Path $ScriptsDir 'ApplyGappedLcnPlan.cs'
  if (-not (Test-Path $src)) { throw "Missing ApplyGappedLcnPlan.cs" }
  $exe = Join-Path $LibDir 'ApplyGappedLcnPlan.exe'
  $cfgSrc = Join-Path $ScriptsDir 'AddMissingEpgTs.exe.config'
  Copy-Item $cfgSrc (Join-Path $LibDir 'ApplyGappedLcnPlan.exe.config') -Force
  $refs = @(
    'EPGBLL.dll', 'EPGModel.dll', 'EPGTools.dll', 'EPGDAL.dll', 'EPGDALFactory.dll',
    'EPGIDAL.dll', 'EPGResource.dll', 'DXTool.dll', 'Dexin.Util.Common.dll',
    'UtilCommon.dll', 'CryptManage.dll'
  ) | ForEach-Object { '/r:"{0}"' -f (Join-Path $LibDir $_) }
  & $csc /nologo /platform:x86 /target:exe /out:$exe @refs /r:System.Configuration.dll $src
  if ($LASTEXITCODE -ne 0) { throw "Compile failed" }
  return $exe
}

function Invoke-ApplyPlan {
  Write-Step "Apply LCN plan via EPGBLL + notices"
  $csvPath = Join-Path $EpgDir 'GAPPED_RANKED_LINEUP_PROPOSAL.csv'
  if (-not (Test-Path $csvPath)) { throw "Run build first ($csvPath missing)" }
  $csv = Import-Csv $csvPath
  $tsv = Join-Path $EpgDir 'GAPPED_APPLY_MAP.tsv'
  @(
    "ts_table_id`tts_id`tts_name`tservice_id`tproposed_lcn`tchannel_name`tblock`tcurrent_lcn"
  ) + @(
    $csv | ForEach-Object {
      $prev = if ($_.previous_lcn) { $_.previous_lcn } else { $_.current_lcn }
      "{0}`t{1}`t{2}`t{3}`t{4}`t{5}`t{6}`t{7}" -f $_.ts_table_id, $_.ts_id, $_.ts_name, $_.service_id, $_.proposed_lcn, ($_.channel_name -replace "`t", ' '), $_.block, $prev
    }
  ) | Set-Content $tsv -Encoding UTF8

  $chg = @($csv | Where-Object { $_.lcn_change -eq 'YES' }).Count
  Write-Host "Channels in plan: $($csv.Count); vs previous change flag: $chg"
  if (-not $Confirm) {
    $ans = Read-Host "Apply to LIVE EPG2 now? Type YES to continue"
    if ($ans -ne 'YES') { Write-Host "Cancelled."; return }
  }

  $exe = Invoke-CompileApply
  $report = Join-Path $EpgDir 'GAPPED_LCN_APPLY_REPORT.csv'
  Push-Location $LibDir
  try {
    & $exe $tsv $report
    if ($LASTEXITCODE -ne 0) { throw "ApplyGappedLcnPlan failed exit=$LASTEXITCODE" }
  } finally {
    Pop-Location
    Remove-Item (Join-Path $LibDir 'ApplyGappedLcnPlan.exe') -ErrorAction SilentlyContinue
    Remove-Item (Join-Path $LibDir 'ApplyGappedLcnPlan.exe.config') -ErrorAction SilentlyContinue
  }
  Write-Host "Apply OK. Report: $report"
}

function Invoke-Verify {
  Write-Step "Verify live NIT vs proposal"
  $csv = Import-Csv (Join-Path $EpgDir 'GAPPED_RANKED_LINEUP_PROPOSAL.csv')
  $want = @{}
  foreach ($r in $csv) { $want["$($r.ts_table_id)|$($r.service_id)"] = [int]$r.proposed_lcn }
  $got = @{}
  sqlcmd -S localhost -E -d EPG2 -s '|' -W -h -1 -Q @"
SET NOCOUNT ON;
SELECT t.id, CONVERT(varchar(max), d.des_value, 2)
FROM ts_des_values d
JOIN descriptor_type typ ON typ.id=d.des_type_table_id
JOIN ts_info t ON t.id=d.ts_table_id
WHERE typ.des_tag=131 AND typ.des_location='NIT_TS'
"@ | ForEach-Object {
    if ($_ -notmatch '\S') { return }
    $p = $_.Split('|'); if ($p.Count -lt 2) { return }
    $tid = [int64]$p[0]; $hex = $p[1].Trim()
    for ($i = 0; $i + 7 -lt $hex.Length; $i += 8) {
      $sid = [Convert]::ToInt32($hex.Substring($i, 4), 16)
      $b2 = [Convert]::ToInt32($hex.Substring($i + 4, 2), 16)
      $b3 = [Convert]::ToInt32($hex.Substring($i + 6, 2), 16)
      $got["$tid|$sid"] = (($b2 -band 0x3F) * 256) + $b3
    }
  }
  $ok = 0; $bad = 0; $four = 0
  foreach ($k in $want.Keys) {
    if ($got.ContainsKey($k) -and $got[$k] -eq $want[$k]) { $ok++ } else { $bad++ }
    if ($got.ContainsKey($k) -and $got[$k] -ge 1000) { $four++ }
  }
  $all = @($got.Values | ForEach-Object { [int]$_ })
  $min = ($all | Measure-Object -Minimum).Minimum
  $max = ($all | Measure-Object -Maximum).Maximum
  Write-Host "match=$ok / $($csv.Count)  mismatch=$bad  four_digit=$four  live_span=$min..$max"
}

function Invoke-ScrambleCheck {
  Write-Step "Data2 / SDT unscramble check (QAM 5.32 + 6.32)"
  $checker = Join-Path $EpgDir 'Check-Data2Unscramble.ps1'
  if (-not (Test-Path $checker)) {
    Write-Host "Optional: add Check-Data2Unscramble.ps1 later. Skipping dedicated script."
    Write-Host "Tip: look for clear programs on Data2 SPTS muxes with SDT on (see DATA2_UNSCRAMBLE_CHECK.txt)."
    return
  }
  & powershell -NoProfile -ExecutionPolicy Bypass -File $checker
}

function Invoke-Status {
  Write-Step "Status"
  $files = @(
    'REMOTE_WEBHOOK_CHANNELS.csv',
    'GAPPED_RANKED_LINEUP_PROPOSAL.csv',
    'GAPPED_BLOCK_SUMMARY.csv',
    'GAPPED_LCN_APPLY_REPORT.csv',
    'STAGED_GAPPED_LINEUP.json'
  )
  foreach ($f in $files) {
    $p = Join-Path $EpgDir $f
    if (Test-Path $p) {
      $i = Get-Item $p
      Write-Host ("  OK  {0,-40} {1,8} bytes  {2}" -f $f, $i.Length, $i.LastWriteTime)
    } else {
      Write-Host ("  --  {0}" -f $f) -ForegroundColor DarkGray
    }
  }
  try {
    $n = (sqlcmd -S localhost -E -d EPG2 -h -1 -Q "SET NOCOUNT ON; SELECT COUNT(*) FROM program_info").Trim()
    Write-Host "  EPG2 programs: $n"
  } catch {
    Write-Host "  EPG2 SQL: unavailable ($($_.Exception.Message))"
  }
}

function Invoke-Full {
  Invoke-FetchMainServer | Out-Null
  Invoke-BackupLcn | Out-Null
  Invoke-BuildPlan | Out-Null
  Invoke-ExportExcel | Out-Null
  Write-Host ""
  Write-Host "Plan ready. Review Excel / CSV, then run:  EPG-Ops.ps1 -Action apply" -ForegroundColor Yellow
}

function Show-Menu {
  Ensure-Dir $EpgDir
  while ($true) {
    Write-Host ""
    Write-Host "======== EPG / QAM OPS (no AI) ========" -ForegroundColor Green
    Write-Host " 1) Fetch main-server ranking"
    Write-Host " 2) Backup live LCNs"
    Write-Host " 3) Build plan (genre from main LCN bands + rank + 3-gap)"
    Write-Host " 4) Export Excel"
    Write-Host " 5) Apply plan to LIVE (BLL + notices)  [asks YES]"
    Write-Host " 6) Verify live vs plan"
    Write-Host " 7) Data2 unscramble check"
    Write-Host " 8) Full: fetch + backup + build + excel"
    Write-Host " 9) Status"
    Write-Host " 0) Exit"
    Write-Host "--------------------------------------"
    $c = Read-Host "Choose"
    switch ($c) {
      '1' { Invoke-FetchMainServer }
      '2' { Invoke-BackupLcn }
      '3' { Invoke-BuildPlan }
      '4' { Invoke-ExportExcel }
      '5' { Invoke-ApplyPlan }
      '6' { Invoke-Verify }
      '7' { Invoke-ScrambleCheck }
      '8' { Invoke-Full }
      '9' { Invoke-Status }
      '0' { return }
      default { Write-Host "Unknown choice" }
    }
  }
}

Ensure-Dir $EpgDir

switch ($Action) {
  'menu' { Show-Menu }
  'fetch' { Invoke-FetchMainServer }
  'backup' { Invoke-BackupLcn }
  'build' { Invoke-BuildPlan }
  'excel' { Invoke-ExportExcel }
  'apply' { Invoke-ApplyPlan }
  'verify' { Invoke-Verify }
  'scramble-check' { Invoke-ScrambleCheck }
  'full' { Invoke-Full }
  'status' { Invoke-Status }
}
