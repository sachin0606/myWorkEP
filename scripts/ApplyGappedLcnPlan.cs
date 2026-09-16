using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Model;
using DeXin.EPG.Tools;

internal static class ApplyGappedLcnPlan
{
	private class Wanted
	{
		public int Sid;
		public int Lcn;
		public string Name;
	}

	private class PlanRow
	{
		public long TsTableId;
		public int TsId;
		public string TsName;
		public int ServiceId;
		public int ProposedLcn;
		public string ChannelName;
		public string Block;
		public int CurrentLcn;
	}

	public static int Main(string[] args)
	{
		try
		{
			string mapPath = args != null && args.Length > 0
				? args[0]
				: Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
					"EPG", "GAPPED_APPLY_MAP.tsv");
			string reportPath = args != null && args.Length > 1
				? args[1]
				: Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
					"EPG", "GAPPED_LCN_APPLY_REPORT.csv");

			if (!File.Exists(mapPath))
				throw new Exception("Missing map: " + mapPath);

			var plan = new List<PlanRow>();
			foreach (var line in File.ReadAllLines(mapPath, Encoding.UTF8))
			{
				if (string.IsNullOrWhiteSpace(line) || line.StartsWith("ts_table_id", StringComparison.OrdinalIgnoreCase))
					continue;
				var p = line.Split('\t');
				if (p.Length < 8)
					throw new Exception("Bad map line: " + line);
				var row = new PlanRow
				{
					TsTableId = long.Parse(p[0]),
					TsId = int.Parse(p[1]),
					TsName = p[2],
					ServiceId = int.Parse(p[3]),
					ProposedLcn = int.Parse(p[4]),
					ChannelName = p[5],
					Block = p[6],
					CurrentLcn = int.Parse(p[7])
				};
				if (row.ProposedLcn < 1 || row.ProposedLcn > 999)
					throw new Exception("Refuse 4-digit/out-of-range LCN " + row.ProposedLcn + " for SID " + row.ServiceId);
				plan.Add(row);
			}
			Console.WriteLine("plan_rows={0}", plan.Count);

			SqlHelper.SetConnectionString("Data Source=localhost;Initial Catalog=EPG2;Integrated Security=True");
			var loc = LocationControl.Ins;

			var tsMgr = new TSInfoManager();
			var prgMgr = new ProgramInfoManager();
			var desMgr = new DescriptorManagement();
			var nit = new NITInfoManager().GetAllNITInfos()[0];
			var allTs = tsMgr.GetTSInfoByNetWorkIdentity(nit.Identity).ToList();
			var tsByTable = allTs.ToDictionary(t => t.Identity);

			var byTs = plan.GroupBy(x => x.TsTableId).OrderBy(g => g.Key).ToList();
			int tsUpdated = 0;
			int slotsChanged = 0;
			var report = new List<string>();
			report.Add("ts_name,ts_id,service_id,channel_name,block,old_lcn,new_lcn,status");

			foreach (var grp in byTs)
			{
				long tid = grp.Key;
				TsInfo ts;
				if (!tsByTable.TryGetValue(tid, out ts))
					throw new Exception("TS table missing: " + tid);
				// Never touch the physical 546 remux.
				if (string.Equals(ts.TsName, "TS_546", StringComparison.OrdinalIgnoreCase))
					throw new Exception("REFUSE 546 remux: " + ts.TsName);

				var live = prgMgr.GetByTSIdentity(ts.Identity).ToList();
				var lcnMap = ReadLcnMap(desMgr, ts.Identity);
				var wantBySid = grp.ToDictionary(x => x.ServiceId);

				foreach (var w in grp)
				{
					var prg = live.FirstOrDefault(p => p.ServiceId == w.ServiceId);
					if (prg == null)
					{
						report.Add(string.Format("{0},{1},{2},\"{3}\",{4},{5},{6},MISSING_PROGRAM",
							ts.TsName, ts.TsId, w.ServiceId, Esc(w.ChannelName), w.Block, w.CurrentLcn, w.ProposedLcn));
						continue;
					}
					int old;
					if (!lcnMap.TryGetValue(w.ServiceId, out old))
						old = w.CurrentLcn;
					if (old != w.ProposedLcn)
						slotsChanged++;
					lcnMap[w.ServiceId] = w.ProposedLcn;
					report.Add(string.Format("{0},{1},{2},\"{3}\",{4},{5},{6},{7}",
						ts.TsName, ts.TsId, w.ServiceId, Esc(prg.ProgramName ?? w.ChannelName), w.Block, old, w.ProposedLcn,
						old == w.ProposedLcn ? "UNCHANGED" : "UPDATED"));
				}

				// Keep any live programs not in plan at their existing LCN (or SID fallback)
				var pairs = new List<Wanted>();
				foreach (var p in live)
				{
					int lcn;
					if (!lcnMap.TryGetValue(p.ServiceId, out lcn))
						lcn = p.ServiceId > 999 ? (p.ServiceId % 1000) : p.ServiceId;
					if (lcn < 1 || lcn > 999)
						throw new Exception(string.Format("Unplanned SID {0} on {1} would keep LCN {2} (>999)", p.ServiceId, ts.TsName, lcn));
					pairs.Add(new Wanted { Sid = p.ServiceId, Lcn = lcn, Name = p.ProgramName });
				}

				// Detect duplicate LCN inside TS
				var dups = pairs.GroupBy(x => x.Lcn).Where(g => g.Count() > 1).ToList();
				if (dups.Count > 0)
				{
					foreach (var d in dups)
						Console.WriteLine("WARN dup LCN {0} on {1}: {2}", d.Key, ts.TsName,
							string.Join(", ", d.Select(x => x.Sid.ToString()).ToArray()));
				}

				WriteLcn(desMgr, ts.Identity, pairs);
				tsUpdated++;
				Console.WriteLine("LCN written {0} services={1} planned={2}", ts.TsName, pairs.Count, grp.Count());
			}

			int n = new SendNoticesManager().SetAllNoticeOn();
			Console.WriteLine("SetAllNoticeOn {0}", n);
			Console.WriteLine("ts_updated={0} slots_changed={1}", tsUpdated, slotsChanged);

			File.WriteAllLines(reportPath, report.ToArray(), Encoding.UTF8);
			Console.WriteLine("report={0}", reportPath);
			return 0;
		}
		catch (Exception ex)
		{
			Console.WriteLine("ERROR: " + ex);
			return 1;
		}
	}

	private static string Esc(string s)
	{
		if (s == null) return "";
		return s.Replace("\"", "\"\"");
	}

	private static Dictionary<int, int> ReadLcnMap(DescriptorManagement desMgr, long tsIdentity)
	{
		var map = new Dictionary<int, int>();
		var des = desMgr.GetDescriptors(EnumPossibleLoaction.NIT_TS, tsIdentity)
			.FirstOrDefault(d => d.DesTypeTag == 131);
		if (des == null || des.DescriptionData == null)
			return map;
		byte[] b = des.DescriptionData;
		for (int i = 0; i + 3 < b.Length; i += 4)
		{
			int sid = (b[i] << 8) | b[i + 1];
			int lcn = ((b[i + 2] & 0x3F) << 8) | b[i + 3];
			map[sid] = lcn;
		}
		return map;
	}

	private static void WriteLcn(DescriptorManagement desMgr, long tsIdentity, List<Wanted> pairs)
	{
		byte[] b = new byte[pairs.Count * 4];
		int i = 0;
		foreach (var p in pairs.OrderBy(x => x.Lcn).ThenBy(x => x.Sid))
		{
			b[i++] = (byte)(p.Sid >> 8);
			b[i++] = (byte)(p.Sid & 0xFF);
			b[i++] = (byte)(0xC0 | ((p.Lcn >> 8) & 0x3F));
			b[i++] = (byte)(p.Lcn & 0xFF);
		}
		var existing = desMgr.GetDescriptors(EnumPossibleLoaction.NIT_TS, tsIdentity)
			.FirstOrDefault(d => d.DesTypeTag == 131);
		string msg;
		if (existing == null)
		{
			var lcn = new DescriptorSigleBase(EnumPossibleLoaction.NIT_TS, tsIdentity);
			lcn.DesTypeID = 130;
			lcn.DesTypeTag = 131;
			lcn.DescriptionData = b;
			lcn.DataLength = b.Length;
			if (!desMgr.AddDescriptor(lcn, out msg))
				throw new Exception("LCN add failed: " + msg);
		}
		else
		{
			existing.DescriptionData = b;
			existing.DataLength = b.Length;
			if (!desMgr.ModifyDescriptor(existing, out msg))
				throw new Exception("LCN modify failed: " + msg);
		}
	}
}
