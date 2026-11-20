# EPG / QAM Ops — no AI required

**Full plant guide (architecture, LCN rules, cloud sync):** [`../docs/EPG-COMPLETE-GUIDE.md`](../docs/EPG-COMPLETE-GUIDE.md)

This folder’s **rules engine** already “thinks” for LCN genre + ranking. You only run the menu.

## Why AI is not needed

| Step | Who decides | Where the logic lives |
|------|-------------|------------------------|
| Genre / language | Fixed rules from main-server **remote LCN bands** + name overrides | `Build-GappedRankedLineup.ps1` |
| Order inside genre | Main-server **ranking** (CSV) | same script |
| Keep old series + 3-gap | Arithmetic on previous LCN clusters | same script |
| Write live NIT | Dexin **BLL** + `SetAllNoticeOn` | `ApplyGappedLcnPlan.cs` |

AI was only used to *discover* those rules once. After that, the scripts are a normal ops tool.

## Start

```bat
Start-EPG-Ops.bat
```

Or:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File .\EPG-Ops.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File .\EPG-Ops.ps1 -Action full
powershell -NoProfile -ExecutionPolicy Bypass -File .\EPG-Ops.ps1 -Action apply -Confirm
```

## Typical weekly flow

1. **Fetch** main-server ranking  
2. **Backup** live LCNs  
3. **Build** plan  
4. Open Excel under `Desktop\EPG` and spot-check  
5. **Apply** (type `YES`)  
6. **Verify**

## Paths

- Working files: `C:\Users\alsha\Desktop\EPG\`
- Scripts / BLL helpers: `C:\Users\alsha\source\EPG-System\scripts\`
- Dexin DLLs: `C:\Users\alsha\source\EPG-System\original\EPG2.4\`
