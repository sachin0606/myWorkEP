# EPG Complete Working Guide

How this plant’s Dexin EPG system is organized, how to change live settings safely, and how to take this repository to another PC (cloud sync).

---

## 1. Three places — do not confuse them

| Location | What it is | Change it for live on-air? |
|----------|------------|----------------------------|
| `C:\Users\alsha\source\EPG-System` | **Source repository** — scripts, decompiled C#, Dexin DLLs used to apply changes | **Yes** — this is the toolset that writes to the DB |
| `C:\Users\alsha\Desktop\EPG` | **Ops / reports folder** — CSVs, Excel, audit dumps, local web sync copies | No — outputs and working files only |
| `C:\Program Files (x86)\EPG System` | **Installed Dexin app + Windows service** | Only when deploying a rebuilt EXE/DLL (rare) |

**Live truth is the SQL Server database `EPG2` on the EPG PC**, not any folder.  
Scripts compile against DLLs under `original\EPG2.4`, call Dexin BLL, write `EPG2`, then `SetAllNoticeOn` so the service regenerates PSI (NIT/SDT/BAT/EIT…).

```
[You / Cursor]
    │
    ▼
scripts\*.cs  →  compile with original\EPG2.4\*.dll
    │
    ▼
Dexin BLL  →  SQL Server database EPG2
    │
    ▼
send_notices (state=1)  →  EPG Windows Service regenerates tables
    │
    ▼
IP multicast / ASI → QAM modulators → cable RF → STBs
```

---

## 2. What QAM owns vs what EPG owns

| Tables / data | Owner | Notes |
|---------------|--------|------|
| PAT, PMT, CAT | **QAM** | EPG `send_or_not` should keep PAT/PMT/CAT = **OFF** |
| SDT, NIT, BAT, TDT/TOT, EIT (as configured) | **EPG** | LCN lives in NIT descriptor tag **131** |
| Video / audio PIDs | **QAM input / encoder** | EPG stores PMT PID placeholders; real PIDs are on QAM |
| Channel name in guide / LCN zap map | **EPG NIT + SDT** | Must match QAM PAT **service_id** on that frequency |

Blank until auto-search almost always means: **NIT frequency / SID / constellation ≠ live QAM**.

---

## 3. Repository layout

```
EPG-System/
├── README.md                 # Decompile / rebuild notes for Dexin product
├── EPG.sln                   # Decompiled client + server projects
├── docs/
│   └── EPG-COMPLETE-GUIDE.md # This guide
├── scripts/                  # ★ Day-to-day ops (use this)
│   ├── EPG-Ops.ps1           # Menu: fetch / build / apply LCN plan
│   ├── OPS-README.md         # Short ops menu help
│   ├── Build-GappedRankedLineup.ps1
│   ├── ApplyGappedLcnPlan.cs
│   ├── AddMissingEpgTs.exe.config  # Required when compiling *.cs helpers
│   ├── *_tmp_*.ps1           # One-off audits (safe to ignore / delete)
│   └── …Fix*.cs / Set*.cs / Update*.cs / Remove*.cs
├── original/EPG2.4/          # Reference Dexin client DLLs (compile target)
├── original/EPG-Service/     # Reference service binaries
├── src/client/               # Decompiled EPG 2.4 UI
└── src/server/               # Decompiled EPG Windows Service
```

Desktop folder (not in git by default):

```
Desktop\EPG\
├── EPG-Ops.ps1               # Often a copy — real apply still uses source\scripts
├── GAPPED_*.csv / .xlsx      # Plans and reports
├── TS_SETTINGS_AUDIT_*.csv   # Audit outputs
└── EPG-Sync-Service.ps1      # Local channel web UI (port 8093) if used
```

---

## 4. Golden rules before any live change

1. **Never edit Program Files** for LCN/bouquet/settings — use BLL scripts.
2. **Always submit notices** after DB changes: `SendNoticesManager.SetAllNoticeOn()`.
3. **One logical channel number (LCN) per network** — no duplicates.
4. **SID on EPG must exist in QAM PAT** on the NIT frequency for that TS.
5. **NIT cable_delivery** (freq / QAM mode / symbol rate) must match the modulator.
6. Prefer **small targeted scripts** (one TS / one LCN move) over full re-rank unless you intend a plant-wide renumber.
7. Keep a CSV dump of LCNs before big applies (Desktop `\EPG` backups).

---

## 5. How to apply a change the proper way

### 5.1 Pattern used for all live LCN / bouquet / TS edits

1. Write or reuse a C# helper under `scripts\` that:
   - Sets connection: `EPG2` on `localhost` (Integrated Security)
   - Uses `TSInfoManager` / `ProgramInfoManager` / `DescriptorManagement` / `BATInfoManager` etc.
   - Writes LCN descriptor tag **131** on `NIT_TS`
   - Calls `new SendNoticesManager().SetAllNoticeOn()`
2. Compile **x86** with .NET Framework `csc` against `original\EPG2.4\*.dll`
3. Run from that folder (so `exe.config` resolves)
4. Delete the temporary `.exe` after run (scripts usually do this)

### 5.2 Compile + run template (PowerShell)

```powershell
$LibDir     = 'C:\Users\alsha\source\EPG-System\original\EPG2.4'
$ScriptsDir = 'C:\Users\alsha\source\EPG-System\scripts'
$csc        = 'C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe'
$name       = 'YourScript'   # without .cs
$src        = Join-Path $ScriptsDir "$name.cs"
$exe        = Join-Path $LibDir "$name.exe"

Copy-Item (Join-Path $ScriptsDir 'AddMissingEpgTs.exe.config') `
          (Join-Path $LibDir "$name.exe.config") -Force

$refs = @(
  'EPGBLL.dll','EPGModel.dll','EPGTools.dll','EPGDAL.dll','EPGDALFactory.dll',
  'EPGIDAL.dll','EPGResource.dll','DXTool.dll','Dexin.Util.Common.dll',
  'UtilCommon.dll','CryptManage.dll'
) | ForEach-Object { '/r:"{0}"' -f (Join-Path $LibDir $_) }

& $csc /nologo /platform:x86 /target:exe /out:$exe @refs /r:System.Configuration.dll $src
if ($LASTEXITCODE -ne 0) { throw 'Compile failed' }

Push-Location $LibDir
try { & $exe }
finally {
  Pop-Location
  Remove-Item $exe -ErrorAction SilentlyContinue
  Remove-Item (Join-Path $LibDir "$name.exe.config") -ErrorAction SilentlyContinue
}
```

### 5.3 Useful existing helpers

| Script | Purpose |
|--------|---------|
| `EPG-Ops.ps1` / `Start-EPG-Ops.bat` | Fetch main-server ranks → build gapped plan → apply |
| `ApplyGappedLcnPlan.cs` | Plant-wide LCN apply from TSV via BLL |
| `UpdateTs506TeluguLcn.cs` | Telugu GEC insert on TS_506 + shifts |
| `SetTv9TeluguLcn150.cs` | Move TV9 Telugu (news) to LCN 150 |
| `FixLcn387Clash.cs` | Resolve duplicate LCN |
| `RemoveAllBouquets.cs` | Delete all BAT bouquets + submit |
| `FixTs618SendOrNot.cs` / send_speed helpers | PSI send flags / intervals |
| `_tmp_ts_full_audit.ps1` | Read-only plant settings audit |

---

## 6. Weekly / plant LCN workflow (gapped ranking)

Short version lives in `scripts/OPS-README.md`.

```bat
cd C:\Users\alsha\source\EPG-System\scripts
Start-EPG-Ops.bat
```

Typical order:

1. **Fetch** ranking from main webhook (`http://117.215.135.147:8092` — plant-specific)
2. **Backup** live LCNs to Desktop `\EPG`
3. **Build** plan (`Build-GappedRankedLineup.ps1`) — genre bands + 3-LCN gaps
4. Spot-check Excel / CSV on Desktop
5. **Apply** with confirm (`ApplyGappedLcnPlan.cs` + notices)
6. **Verify**

Genre rules are encoded in `Build-GappedRankedLineup.ps1` (remote LCN bands + name overrides). AI is not required for weekly runs.

---

## 7. LCN placement conventions (this plant)

- Keep **GEC blocks contiguous**; put **news** after the GEC block (example: TV9 Telugu @ **150**).
- Insert new GEC channels by **shifting** neighbours — do not leave holes that break “after X” remote order.
- **Kids** cluster is roughly LCN **476–491** (ETV Bal Bharat @ **491** as of last update).
- **Telugu GEC window** (after last Telugu work):

| LCN | Channel |
|-----|---------|
| 139 | Gemini |
| 140 | MAA TV |
| 141 | Z Telugu |
| 142 | ETV Telugu |
| 143 | Gemini Movies |
| 144 | Zee Cinemalu |
| 145 | ETV Cinema |
| 146 | Gemini Music |
| 147 | SVBC Telugu (end of Telugu GEC) |
| 148–149 | Raj News / Raj Musix |
| 150 | TV9 Telugu (news) |

Re-check with a read-only LCN dump before every new insert.

---

## 8. Bouquets (BAT)

- Stored in `bat_info`, `bat_to_program`, `bat_des_values`, `bat_ts_des_values`.
- UI path: Program Management → Bouquet — delete uses `BATInfoManager.Delete` + notices.
- Script: `RemoveAllBouquets.cs` (same BLL path). After remove, BAT send may still be ON in `send_or_not_info`; empty BAT is OK, or turn BAT send off per TS if desired.

---

## 9. Read-only checks (safe anytime)

```powershell
# List programs on a TS
sqlcmd -S localhost -E -d EPG2 -W -Q "SELECT p.service_id, p.program_name FROM program_info p JOIN ts_info t ON t.id=p.ts_table_id WHERE t.ts_name='TS_506' ORDER BY 1"

# Full settings audit (writes Desktop\EPG CSVs)
powershell -NoProfile -ExecutionPolicy Bypass -File C:\Users\alsha\source\EPG-System\scripts\_tmp_ts_full_audit.ps1
```

Compare EPG NIT frequencies to live QAM dumps (`scripts\live_qam_programs.csv` / `Build-EpgQamMap.ps1`) when diagnosing blank channels.

---

## 10. Sync this repository to the cloud (use on another PC)

### 10.1 What gets synced vs what does not

| Synced in git (this repo) | Stays on the EPG PC only |
|---------------------------|---------------------------|
| Scripts, docs, decompiled `src`, `original` DLLs (if committed) | Live database **EPG2** |
| Ops history in scripts | QAM modulator configs |
| | Desktop `\EPG` reports (unless you copy them in) |
| | Program Files install |

**Cloning the repo on another PC does not clone the live channel list.**  
To work on-air you still need access to an EPG PC with `EPG2` + Dexin service, or a restored DB backup.

### 10.2 Cursor Cloud repo (this project)

| Item | Value |
|------|--------|
| Cloud page | https://cursor.com/codebase/sachin-t/epg-system |
| Clone URL | `https://origin.cursor.com/sachin-t/epg-system.git` |
| Visibility | **Private** |
| Local remote | `origin` → that URL |

Local commits for the ops guide + LCN/bouquet scripts are already on `main`.

**Finish the first push from this Windows PC** (git needs your Cursor login once):

1. In Cursor: **Source Control** → **Push** / **Publish Branch**, **or** in a terminal:

```powershell
cd C:\Users\alsha\source\EPG-System
git push -u origin main
```

2. Complete the browser / credential prompt when Windows asks for `origin.cursor.com`.
3. On the **other Cursor IDE**: open https://cursor.com/codebase/sachin-t/epg-system → Clone / Open, signed in as the same Cursor account (`sachin-t`).

If push keeps hanging on Windows, install **WSL + Ubuntu**, then:

```bash
curl -fsSL https://downloads.cursor.com/origin/install.sh | sh
~/.local/bin/origin auth login
cd /mnt/c/Users/alsha/source/EPG-System
git push -u origin main
```

### 10.3 Later updates (either PC)

```powershell
cd C:\Users\alsha\source\EPG-System
git add -A
git status
git commit -m "Describe the change."
git push
```

On the other machine: `git pull`.

### 10.4 Cursor Cloud Agents

Cloud agents see whatever is **pushed** to the Origin remote.  
They **cannot** talk to plant `localhost` SQL or QAM (do not expose EPG DB to the internet).

### 10.5 USB / DB backup (plant only)

```powershell
Compress-Archive -Path C:\Users\alsha\source\EPG-System\* `
  -DestinationPath D:\Backup\EPG-System-$(Get-Date -Format yyyyMMdd).zip
```

```bat
sqlcmd -S localhost -E -Q "BACKUP DATABASE EPG2 TO DISK='D:\Backup\EPG2.bak' WITH INIT"
```

Keep backups offline — they contain the full channel map.

---

## 11. Using Cursor on this vs another system

| Goal | Do this |
|------|---------|
| Edit scripts / docs | Open folder `C:\Users\alsha\source\EPG-System` in Cursor |
| Apply live LCN / bouquet | Run scripts on the **EPG PC** (needs `EPG2` + Dexin DLLs) |
| Continue work elsewhere | `git pull` the private repo; read this guide |
| Run audits / applies from another office PC | Only if that PC can reach SQL `EPG2` (usually VPN) **or** RDP to the EPG PC |
| Local channel web UI | Desktop `\EPG\EPG-Sync-Service.ps1` (port 8093) — plant-local |

This chat’s agent may be rooted on Program Files or on the source repo. **Always open `source\EPG-System` for script work.**

---

## 12. Quick troubleshooting

| Symptom | Likely cause | Check |
|---------|--------------|--------|
| Blank channel, OK after auto search | NIT freq/SID/QAM ≠ RF | NIT tag 0x44 vs spectrum / live QAM |
| Two channels same number | Duplicate LCN | Network LCN dump |
| New channel missing from zap list | No LCN descriptor / notices not submitted | Tag 131 + `SetAllNoticeOn` |
| Guide empty, video OK | EIT send off | `send_or_not_info` eit_* flags |
| Script compile fails | Wrong platform / missing DLL | `/platform:x86` + `original\EPG2.4` |
| Desktop script “does nothing” | Old copy | Run from `source\EPG-System\scripts` |

---

## 13. Checklist — new channel on an existing TS

1. Confirm service exists in `program_info` (and on QAM PAT).
2. Decide LCN (after which neighbour? news vs GEC vs kids?).
3. Shift any neighbours that would collide.
4. Write LCN map for that TS (+ any shifted TS).
5. `SetAllNoticeOn`.
6. Verify LCN window dump.
7. Commit script to git and **push** so the other PC/cloud stays in sync.

---

## 14. Related short docs

- `README.md` — decompile / rebuild Dexin product code  
- `scripts/OPS-README.md` — gapped LCN menu only  
- `scripts/EPG-Ops.ps1` — operator console entry point  

---

*Last aligned with plant work including: Telugu GEC on TS_506, TV9 Telugu @ 150, bouquets removed, LCN 387 fix, PSI/QAM overlay model.*
