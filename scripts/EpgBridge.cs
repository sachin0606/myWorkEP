using System;
using System.Collections.Generic;
using System.Linq;
using DeXin.EPG.BLL;
using DeXin.EPG.Model;

public class EpgCatalogRow
{
    public int NetworkId;
    public string Network;
    public long NitIdentity;
    public int TsId;
    public string TsName;
    public long TsIdentity;
    public int ServiceId;
    public string Program;
    public long PrgIdentity;
    public int EventCount;
    public DateTime? LastEnd;
    public int NextEventId;
    public bool InPat;
    public List<string> EventNames = new List<string>();
}

public static class EpgBridge
{
    public static List<EpgCatalogRow> GetCatalog()
    {
        var nitMgr = new NITInfoManager();
        var tsMgr = new TSInfoManager();
        var prgMgr = new ProgramInfoManager();
        var eitMgr = new EITInfoManager();
        var rows = new List<EpgCatalogRow>();
        var outputIds = new HashSet<long>(prgMgr.GetOutputProgram().Select(p => p.Identity));
        foreach (NitInfo nit in nitMgr.GetAllNITInfos())
        {
            foreach (TsInfo ts in tsMgr.GetTSInfoByNetWorkIdentity(nit.Identity))
            {
                ts.NIT = nit;
                foreach (ProgramInfo prg in prgMgr.GetByTSIdentity(ts.Identity))
                {
                    if (prg.TS == null) prg.TS = ts;
                    if (prg.TS != null && prg.TS.NIT == null) prg.TS.NIT = nit;
                    if (string.IsNullOrEmpty(prg.ProgramName)) prg.ProgramName = prg.BKValue;
                    IList<EitInfo> events = eitMgr.GetByProgramIdentity(prg.Identity);
                    DateTime? lastEnd = null;
                    var names = new List<string>();
                    if (events != null && events.Count > 0)
                    {
                        lastEnd = events.Max(e => e.EndTime);
                        names.AddRange(events.Select(e => e.EventName).Where(n => !string.IsNullOrEmpty(n)).Distinct());
                    }
                    rows.Add(new EpgCatalogRow
                    {
                        NetworkId = nit.NetworkId,
                        Network = nit.NetworkName,
                        NitIdentity = nit.Identity,
                        TsId = ts.TsId,
                        TsName = ts.TsName,
                        TsIdentity = ts.Identity,
                        ServiceId = prg.ServiceId,
                        Program = prg.ProgramName,
                        PrgIdentity = prg.Identity,
                        EventCount = events == null ? 0 : events.Count,
                        LastEnd = lastEnd,
                        NextEventId = prg.PrgManualeventid + 1,
                        InPat = outputIds.Contains(prg.Identity),
                        EventNames = names
                    });
                }
            }
        }
        return rows;
    }

    public static int EnsureCoverage(int days, int durationMinutes, bool whatIf, bool outputOnly)
    {
        var nitMgr = new NITInfoManager();
        var tsMgr = new TSInfoManager();
        var prgMgr = new ProgramInfoManager();
        var eitMgr = new EITInfoManager();
        int created = 0;
        DateTime horizon = DateTime.Now.AddDays(days);
        HashSet<long> outputIds = outputOnly
            ? new HashSet<long>(prgMgr.GetOutputProgram().Select(p => p.Identity))
            : null;
        foreach (NitInfo nit in nitMgr.GetAllNITInfos())
        {
            foreach (TsInfo ts in tsMgr.GetTSInfoByNetWorkIdentity(nit.Identity))
            {
                ts.NIT = nit;
                foreach (ProgramInfo prg in prgMgr.GetByTSIdentity(ts.Identity))
                {
                    if (outputIds != null && !outputIds.Contains(prg.Identity)) continue;
                    if (prg.TS == null) prg.TS = ts;
                    if (prg.TS != null && prg.TS.NIT == null) prg.TS.NIT = nit;
                    if (string.IsNullOrEmpty(prg.ProgramName)) prg.ProgramName = prg.BKValue;
                    List<EitInfo> existing = new List<EitInfo>(eitMgr.GetByProgramIdentity(prg.Identity) ?? new List<EitInfo>());
                    DateTime cursor = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
                    if (existing.Count > 0)
                    {
                        DateTime last = existing.Max(e => e.EndTime);
                        if (last > cursor) cursor = last;
                    }
                    List<string> templates = existing.Select(e => e.EventName).Where(n => !string.IsNullOrEmpty(n)).Distinct().ToList();
                    if (templates.Count == 0) templates.Add(prg.ProgramName);
                    int i = 0;
                    int createdForProgram = 0;
                    DateTime firstNew = cursor;
                    while (cursor < horizon)
                    {
                        DateTime start = cursor;
                        DateTime end = start.AddMinutes(durationMinutes);
                        var overlaps = existing.Where(e => e.StartTime < end && e.EndTime > start).ToList();
                        if (overlaps.Count > 0)
                        {
                            cursor = overlaps.Max(e => e.EndTime);
                            continue;
                        }
                        string name = templates[i % templates.Count];
                        i++;
                        if (whatIf)
                        {
                            created++;
                            createdForProgram++;
                            cursor = end;
                            continue;
                        }
                        var eit = new EitInfo
                        {
                            Program = prg,
                            ProgramTableId = prg.Identity,
                            EventId = -1,
                            EventName = name,
                            EventRemark = name,
                            EventLongRemark = "",
                            StartTime = start,
                            EndTime = end
                        };
                        int saved = eitMgr.Save(eit);
                        if (saved != 1)
                            throw new Exception("EITInfoManager.Save returned " + saved + " for " + prg.ProgramName);
                        Console.WriteLine("  SAVED   {0}  {1:yyyy-MM-dd HH:mm} -> {2:HH:mm}  [{3}]", prg.ProgramName, start, end, name);
                        existing.Add(eit);
                        created++;
                        createdForProgram++;
                        cursor = end;
                    }
                    if (createdForProgram > 0)
                    {
                        Console.WriteLine("  {0}  {1} slots  from {2:yyyy-MM-dd HH:mm}  ({3})",
                            prg.ProgramName, createdForProgram, firstNew, whatIf ? "planned" : "saved");
                    }
                }
            }
        }
        return created;
    }
}
