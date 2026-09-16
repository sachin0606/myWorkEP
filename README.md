# EPG System source (decompiled)

Writable working copy of the installed Dexin EPG product so later changes can be made in C# and rebuilt. Do **not** edit the live files under `C:\Program Files (x86)\EPG System`.

| Installed app | Role | Binary | Decompiled project |
|---|---|---|---|
| EPG 2.4 | Client (WinForms) | `EPG2.0.exe` 2.6.11.26 | `src/client/EPG2.0` |
| EPG Service | Processor / TS playout | `EPGWindowsService.exe` 2.6.10.28 | `src/server/EPGWindowsService` |
| EPG Service Management | Processor UI | `EPGServiceManagement.exe` | `src/server/EPGServiceManagement` |

Both apps target **.NET Framework 4.0**, x86, namespaces `DeXin.EPG.*`. Assemblies are **not obfuscated**.

## Layout

```
original/EPG2.4/          copied client binaries (reference assemblies)
original/EPG-Service/     copied processor binaries
src/client/               C# for the EPG 2.4 client + its DLLs
src/server/               C# for EPGWindowsService + management + its DLLs
src/shared/               identical shared managed lib (Dexin.Util.Common)
tools/decompile.ps1       re-run ILSpy project export
EPG.sln                   all decompiled projects
```

Client and server copies of `EPGBLL`, `EPGDAL`, `EPGModel`, `EPGTools`, and several other DLLs are **not identical** — edit the tree that matches the binary you will replace.

## Add events without touching the DB or source

Use `scripts/Add-EpgUiEvents.ps1`. It clicks EPG 2.4 (EIT Add / Submit) so the client saves the event itself.

## How modifications will be done

1. Change the C# under `src/client` or `src/server`.
2. Rebuild that project (needs .NET Framework 4.0 targeting pack / Visual Studio).
3. Copy the new EXE/DLL over the matching file in `original\...` first, test there, then deploy to Program Files only when ready.
4. Leave Microsoft.ReportViewer, `Win32dll.dll`, `StreamDevice.dll`, `AlgSet.dll`, and `HTBDog\*` as binary references. Those are native or third-party.

Processor start path: `EPGWindowsService` → `TSOutputManager.StartSendTransprotStream()` in `src/server/EPGServiceBLL` (PAT/PMT/NIT/SDT/EIT/BAT/TDT/TOT generators + UDP/TCP output).

Client UI path: `src/client/EPG2.0/DeXin/EPG/Views` (`FormMain`, program/EIT/send-control forms).

## Re-decompile

Portable SDK: `C:\Users\alsha\source\tools\dotnet`  
ILSpy CLI: `C:\Users\alsha\source\tools\ilspycmd\ilspycmd.exe`

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File .\tools\decompile.ps1
```

Native binaries are skipped automatically.
