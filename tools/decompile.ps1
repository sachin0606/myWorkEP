# Decompile first-party Dexin EPG assemblies to compilable C# projects.
# Requires portable .NET 8 SDK + ilspycmd (see tools/README).

$ErrorActionPreference = "Stop"
$env:DOTNET_ROOT = "C:\Users\alsha\source\tools\dotnet"
$env:PATH = "$env:DOTNET_ROOT;$env:PATH"
$ilspy = "C:\Users\alsha\source\tools\ilspycmd\ilspycmd.exe"

$root = Split-Path -Parent $PSScriptRoot
if (-not (Test-Path $ilspy)) { throw "ilspycmd not found: $ilspy" }

function Decompile-Assembly {
    param(
        [Parameter(Mandatory = $true)][string]$AssemblyPath,
        [Parameter(Mandatory = $true)][string]$OutputDir,
        [Parameter(Mandatory = $true)][string]$ReferencePath
    )
    if (-not (Test-Path $AssemblyPath)) {
        Write-Host "SKIP missing $AssemblyPath"
        return
    }
    if (Test-Path $OutputDir) {
        Remove-Item $OutputDir -Recurse -Force
    }
    New-Item -ItemType Directory -Force -Path $OutputDir | Out-Null
    Write-Host "DECOMPILE $(Split-Path $AssemblyPath -Leaf) -> $OutputDir"
    & $ilspy $AssemblyPath -p --nested-directories --disable-updatecheck -lv CSharp5 -r $ReferencePath -o $OutputDir
    if ($LASTEXITCODE -ne 0) {
        Write-Host "WARN: ilspycmd failed for $AssemblyPath (exit $LASTEXITCODE) — likely native/unmanaged. Skipping."
        if (Test-Path $OutputDir) { Remove-Item $OutputDir -Recurse -Force }
        return
    }
}

$clientRef = Join-Path $root "original\EPG2.4"
$serverRef = Join-Path $root "original\EPG-Service"
$src = Join-Path $root "src"

$clientAsms = @(
    "EPG2.0.exe",
    "EPGBLL.dll",
    "EPGDAL.dll",
    "EPGDALFactory.dll",
    "EPGIDAL.dll",
    "EPGModel.dll",
    "EPGResource.dll",
    "EPGTools.dll",
    "EPGSoftDog.dll",
    "CryptManage.dll",
    "DXTool.dll",
    "UtilCommon.dll"
)

$sharedAsms = @(
    "Dexin.Util.Common.dll"
)

$serverAsms = @(
    "EPGWindowsService.exe",
    "EPGServiceManagement.exe",
    "EPGServiceBLL.dll",
    "EPGServiceNotify.exe",
    "TsSenders.exe",
    "EPGBLL.dll",
    "EPGDAL.dll",
    "EPGDALFactory.dll",
    "EPGIDAL.dll",
    "EPGModel.dll",
    "EPGResource.dll",
    "EPGTools.dll",
    "EPGSoftDog.dll",
    "CryptManage.dll",
    "DXTool.dll"
)

foreach ($asm in $clientAsms) {
    $name = [IO.Path]::GetFileNameWithoutExtension($asm)
    Decompile-Assembly -AssemblyPath (Join-Path $clientRef $asm) -OutputDir (Join-Path $src "client\$name") -ReferencePath $clientRef
}

foreach ($asm in $sharedAsms) {
    $name = [IO.Path]::GetFileNameWithoutExtension($asm)
    Decompile-Assembly -AssemblyPath (Join-Path $clientRef $asm) -OutputDir (Join-Path $src "shared\$name") -ReferencePath $clientRef
}

foreach ($asm in $serverAsms) {
    $name = [IO.Path]::GetFileNameWithoutExtension($asm)
    Decompile-Assembly -AssemblyPath (Join-Path $serverRef $asm) -OutputDir (Join-Path $src "server\$name") -ReferencePath $serverRef
}

Write-Host "All first-party assemblies decompiled."
