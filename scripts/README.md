# EPG event tool (uses the Dexin BLL, not raw SQL)

`EpgEvents.ps1` loads the same client DLLs as EPG 2.4, decrypts the live service connection with `CryptionManage`, then calls:

- `NITInfoManager` / `TSInfoManager` / `ProgramInfoManager` to discover the bouquet
- `EITInfoManager.Save` to add events (same method as 节目事件管理 → 新增 → 提交)

It also writes `send_notices` and EIT descriptors, which is what the processor watches.

## List everything (no names needed)

```powershell
powershell -NoProfile -File .\EpgEvents.ps1 -List
```

## Fill missing EIT for the playout window

Processor setting `EventLoadeDays` is 7. This creates non-overlapping slots from now until that horizon. Event IDs are auto-assigned (`PrgManualeventid + 1`). Existing titles are reused when present.

Dry run (PAT programs only — the ones actually on-air):

```powershell
powershell -NoProfile -File .\EpgEvents.ps1 -EnsureDays 7 -OutputOnly -WhatIf
```

Write for real:

```powershell
powershell -NoProfile -File .\EpgEvents.ps1 -EnsureDays 7 -OutputOnly
```
