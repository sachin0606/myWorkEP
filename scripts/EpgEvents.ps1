#Requires -Version 5.1
<#
.SYNOPSIS
  Discover EPG NIT/TS/programs and add EIT events using the same BLL as EPG 2.4.

.DESCRIPTION
  Loads the installed Dexin assemblies (EPGBLL / EPGDAL / CryptManage) and calls
  NITInfoManager, TSInfoManager, ProgramInfoManager, EITInfoManager.Save —
  the same path as 节目事件管理 → 新增 → 提交.

  Connection string is taken from the live EPG Service config and decrypted
  with CryptionManage, not typed in by hand.

.EXAMPLE
  powershell -NoProfile -File .\EpgEvents.ps1 -List

.EXAMPLE
  powershell -NoProfile -File .\EpgEvents.ps1 -EnsureDays 7 -WhatIf

.EXAMPLE
  powershell -NoProfile -File .\EpgEvents.ps1 -EnsureDays 7
#>
[CmdletBinding()]
param(
    [switch]$List,
    [int]$EnsureDays = 0,
    [int]$DurationMinutes = 30,
    [switch]$WhatIf,
    [switch]$OutputOnly,
    [string]$BinDir = "",
    [string]$ServiceConfig = ""
)

$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $PSScriptRoot
if (-not $BinDir) { $BinDir = Join-Path $root "original\EPG2.4" }
if (-not $ServiceConfig) { $ServiceConfig = Join-Path $root "original\EPG-Service\EPGWindowsService.exe.Config" }

function Initialize-EpgRuntime {
    if (-not (Test-Path $BinDir)) { throw "EPG client binaries not found: $BinDir" }
    $script:bin = (Resolve-Path $BinDir).Path

    [AppDomain]::CurrentDomain.add_AssemblyResolve({
        param($sender, $args)
        $name = ([Reflection.AssemblyName]$args.Name).Name
        if ($name -match '\.resources$') { return $null }
        $path = Join-Path $script:bin ($name + ".dll")
        if (Test-Path $path) { return [Reflection.Assembly]::LoadFrom($path) }
        return $null
    })

    foreach ($name in @(
            "CryptManage", "Dexin.Util.Common", "DXTool", "UtilCommon",
            "EPGModel", "EPGTools", "EPGResource", "EPGIDAL",
            "EPGDALFactory", "EPGDAL", "EPGBLL"
        )) {
        $path = Join-Path $script:bin ($name + ".dll")
        if (Test-Path $path) { [void][Reflection.Assembly]::LoadFrom($path) }
    }
}

function Get-EncryptedConnectionString {
    if (-not (Test-Path $ServiceConfig)) { throw "Service config not found: $ServiceConfig" }
    [xml]$xml = Get-Content -LiteralPath $ServiceConfig
    $node = $xml.configuration.appSettings.add | Where-Object { $_.key -eq "DBConnectionString" }
    if (-not $node -or -not $node.value) { throw "DBConnectionString missing in $ServiceConfig" }
    return @{ Encrypted = $node.value; EventDays = (($xml.configuration.appSettings.add | Where-Object { $_.key -eq "EventLoadeDays" }).value) }
}

function Connect-EpgDatabase {
    $cfg = Get-EncryptedConnectionString
    $inst = [CryptManage.CryptionManage]::Instance
    if (-not $inst) { throw "CryptManage.CryptionManage.Instance is null" }
    $plain = $inst.DeCrypt($cfg.Encrypted)
    if ([string]::IsNullOrWhiteSpace($plain) -or $plain -eq "ERROR") {
        throw "CryptionManage.DeCrypt failed. The service config key could not be opened."
    }
    [Com.Cn.Tool.Model.SqlHelper]::SetConnectionString($plain)
    $script:EventLoadeDays = 7
    if ($cfg.EventDays) { [void][int]::TryParse($cfg.EventDays, [ref]$script:EventLoadeDays) }
    $builder = New-Object System.Data.SqlClient.SqlConnectionStringBuilder $plain
    Write-Host ("Connected to SQL {0} / {1}  (EventLoadeDays={2})" -f $builder.DataSource, $builder.InitialCatalog, $script:EventLoadeDays)
}

function Add-EpgBridge {
    if ([type]::GetType("EpgBridge")) { return }
    $cs = Join-Path $PSScriptRoot "EpgBridge.cs"
    $refs = @(
        (Join-Path $script:bin "EPGModel.dll"),
        (Join-Path $script:bin "EPGBLL.dll"),
        (Join-Path $script:bin "EPGTools.dll"),
        (Join-Path $script:bin "DXTool.dll"),
        (Join-Path $script:bin "EPGIDAL.dll"),
        (Join-Path $script:bin "EPGDALFactory.dll"),
        (Join-Path $script:bin "EPGDAL.dll"),
        "System.dll",
        "System.Core.dll",
        "System.Data.dll",
        "System.Configuration.dll"
    )
    Add-Type -Path $cs -ReferencedAssemblies $refs
}

function Show-EpgCatalog($rows) {
    $rows | Select-Object Network, @{N = "NID"; E = { "0x{0:X4}" -f $_.NetworkId } },
        TsName, @{N = "TSID"; E = { "0x{0:X4}" -f $_.TsId } },
        Program, @{N = "SID"; E = { "0x{0:X4}" -f $_.ServiceId } },
        InPat, EventCount, LastEnd, NextEventId |
        Format-Table -AutoSize
    $sum = 0
    $pat = 0
    foreach ($r in $rows) { $sum += $r.EventCount; if ($r.InPat) { $pat++ } }
    Write-Host ("Programs: {0}  In PAT (on-air): {1}  Events already stored: {2}" -f @($rows).Count, $pat, $sum)
}

try {
    Initialize-EpgRuntime
    Connect-EpgDatabase
    Add-EpgBridge
    $catalog = [EpgBridge]::GetCatalog()

    if ($List -or $EnsureDays -le 0) {
        Show-EpgCatalog $catalog
    }

    if ($EnsureDays -gt 0) {
        $days = $EnsureDays
        if ($days -le 0) { $days = $script:EventLoadeDays }
        Write-Host ("Ensuring EIT coverage for {0} day(s), slot={1} min  WhatIf={2}" -f $days, $DurationMinutes, [bool]$WhatIf)
        $total = [EpgBridge]::EnsureCoverage($days, $DurationMinutes, [bool]$WhatIf, [bool]$OutputOnly)
        Write-Host ("Finished. Events {0}: {1}" -f ($(if ($WhatIf) { "planned" } else { "saved via EITInfoManager.Save" }), $total))
    }
} catch {
    Write-Host $_
    Write-Host $_.ScriptStackTrace
    if ($_.Exception.InnerException) { Write-Host $_.Exception.InnerException.ToString() }
    exit 1
}
