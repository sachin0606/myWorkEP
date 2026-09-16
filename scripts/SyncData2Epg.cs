using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Model;
using DeXin.EPG.Tools;

internal static class SyncData2Epg
{
	public class Wanted
	{
		public int Sid { get; set; }
		public string Name { get; set; }
		public int QamLcn { get; set; }
	}

	public class Target
	{
		public int Freq { get; set; }
		public string TS { get; set; }
		public List<Wanted> Programs { get; set; }
	}

	private static readonly HashSet<string> Data1Ts = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
	{
		"TS_98", "TS_138", "TS_146", "TS_154", "TS_162", "TS_194", "TS_202",
		"TS_218", "TS_226", "TS_234", "TS_242"
	};

	public static int Main(string[] args)
	{
		try
		{
			SqlHelper.SetConnectionString("Data Source=localhost;Initial Catalog=EPG2;Integrated Security=True");
			var loc = LocationControl.Ins;
			string jsonPath = @"C:\Users\alsha\source\EPG-System\scripts\data2_qam_targets.json";
			var targets = new JavaScriptSerializer().Deserialize<List<Target>>(File.ReadAllText(jsonPath));

			var tsMgr = new TSInfoManager();
			var prgMgr = new ProgramInfoManager();
			var desMgr = new DescriptorManagement();
			var patMgr = new PATManager();

			var nit = new NITInfoManager().GetAllNITInfos()[0];
			var allTs = tsMgr.GetTSInfoByNetWorkIdentity(nit.Identity);

			int added = 0, updated = 0, deleted = 0, lcnWrites = 0;

			foreach (var tgt in targets)
			{
				if (tgt == null || Data1Ts.Contains(tgt.TS))
				{
					Console.WriteLine("SKIP data1 {0}", tgt == null ? "?" : tgt.TS);
					continue;
				}
				var ts = allTs.FirstOrDefault(t => t.TsName == tgt.TS);
				if (ts == null)
				{
					Console.WriteLine("SKIP no EPG TS {0}", tgt.TS);
					continue;
				}
				if (ts.TsId == 22)
				{
					Console.WriteLine("SKIP ts_id 22 {0}", ts.TsName);
					continue;
				}

				var existing = prgMgr.GetByTSIdentity(ts.Identity).ToList();
				var oldLcn = ReadLcnMap(desMgr, ts.Identity);
				var wantedSids = new HashSet<int>(tgt.Programs.Select(p => p.Sid));

				foreach (var w in tgt.Programs)
				{
					var have = existing.FirstOrDefault(p => p.ServiceId == w.Sid);
					if (have == null)
					{
						var prg = new ProgramInfo();
						prg.TsTableId = ts.Identity;
						prg.ServiceId = w.Sid;
						prg.ProgramName = w.Name;
						prg.ServiceType = 1;
						prg.ProviderName = "HCN";
						prg.PmtId = 8191;
						prg.PCRPID = 8191;
						prg.PmtVersionNumber = -1;
						prg.PrgSdtEitscflag = 1;
						prg.PrgSdtEitpfflag = 1;
						prg.PrgSdtRunningstate = 4;
						prg.PrgSdtFreecamod = 1;
						prg.PrgManualeventid = 0;
						prg.EITFileName = "";
						prgMgr.Save(prg);
						added++;
						Console.WriteLine("  ADD {0} sid={1} {2}", tgt.TS, w.Sid, w.Name);
					}
					else if (!string.Equals((have.ProgramName ?? "").Trim(), (w.Name ?? "").Trim(), StringComparison.OrdinalIgnoreCase))
					{
						have.ProgramName = w.Name;
						prgMgr.Save(have);
						updated++;
						Console.WriteLine("  REN {0} sid={1} -> {2}", tgt.TS, w.Sid, w.Name);
					}
				}

				existing = prgMgr.GetByTSIdentity(ts.Identity).ToList();
				foreach (var have in existing.ToList())
				{
					if (!wantedSids.Contains(have.ServiceId))
					{
						prgMgr.DeleteProgram(have.Identity);
						deleted++;
						Console.WriteLine("  DEL {0} sid={1} {2}", tgt.TS, have.ServiceId, have.ProgramName);
					}
				}

				var live = prgMgr.GetByTSIdentity(ts.Identity).ToList();
				var lcnPairs = new List<Wanted>();
				foreach (var w in tgt.Programs)
				{
					int lcn = w.QamLcn;
					if (oldLcn.ContainsKey(w.Sid))
						lcn = oldLcn[w.Sid];
					lcnPairs.Add(new Wanted { Sid = w.Sid, Name = w.Name, QamLcn = lcn });
				}
				WriteLcn(desMgr, ts.Identity, lcnPairs, ref lcnWrites);
				Console.WriteLine("SYNC {0} live={1} lcn={2}", tgt.TS, live.Count, lcnPairs.Count);
			}

			var patIds = new List<long>();
			foreach (var ts in tsMgr.GetTSInfoByNetWorkIdentity(nit.Identity))
			{
				foreach (var p in prgMgr.GetByTSIdentity(ts.Identity))
					patIds.Add(p.Identity);
			}
			patMgr.Save(patIds.Distinct().ToList(), "admin");
			int n = new SendNoticesManager().SetAllNoticeOn();
			Console.WriteLine("DONE added={0} renamed={1} deleted={2} lcnWrites={3} pat={4} notices={5}",
				added, updated, deleted, lcnWrites, patIds.Count, n);
			var still22 = tsMgr.GetTSInfoByTSId(22).First();
			Console.WriteLine("UNTOUCHED ts_id=22 {0}", still22.TsName);
			return 0;
		}
		catch (Exception ex)
		{
			Console.WriteLine("ERROR: " + ex);
			return 1;
		}
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

	private static void WriteLcn(DescriptorManagement desMgr, long tsIdentity, List<Wanted> pairs, ref int writes)
	{
		byte[] b = new byte[pairs.Count * 4];
		int i = 0;
		foreach (var p in pairs)
		{
			b[i++] = (byte)(p.Sid >> 8);
			b[i++] = (byte)(p.Sid & 0xFF);
			b[i++] = (byte)(0xC0 | ((p.QamLcn >> 8) & 0x3F));
			b[i++] = (byte)(p.QamLcn & 0xFF);
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
		writes++;
	}
}
