using System;
using System.Collections.Generic;
using System.Linq;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Model;
using DeXin.EPG.Tools;

internal static class RemoveDuplicateTsPrograms
{
	private class Drop
	{
		public string TsName;
		public int Sid;
		public string Name;
	}

	private class Wanted
	{
		public int Sid;
		public int Lcn;
		public string Name;
	}

	public static int Main()
	{
		try
		{
			SqlHelper.SetConnectionString("Data Source=localhost;Initial Catalog=EPG2;Integrated Security=True");
			var loc = LocationControl.Ins;

			// Keep home TS; drop remux duplicates (same SPTS on two RFs)
			var drops = new[]
			{
				new Drop { TsName = "TS_114", Sid = 3018, Name = "9X JALWA" },
				new Drop { TsName = "TS_106", Sid = 2015, Name = "9X TASHAN" },
				new Drop { TsName = "TS_106", Sid = 2007, Name = "B4U BHOJPURI" },
				new Drop { TsName = "TS_106", Sid = 2006, Name = "Bharat Samachar" },
				new Drop { TsName = "TS_618", Sid = 35008, Name = "DD TAMIL" },
				new Drop { TsName = "TS_506", Sid = 275, Name = "DD Yadagiri" },
				new Drop { TsName = "TS_618", Sid = 35002, Name = "Goldmines Bollywood" },
				new Drop { TsName = "TS_106", Sid = 2014, Name = "IND24" },
				new Drop { TsName = "TS_618", Sid = 35012, Name = "NATION 27" },
				new Drop { TsName = "TS_106", Sid = 2004, Name = "Sarv Dharm Sangam" },
				new Drop { TsName = "TS_506", Sid = 368, Name = "TV9 Telugu" },
			};

			var tsMgr = new TSInfoManager();
			var prgMgr = new ProgramInfoManager();
			var desMgr = new DescriptorManagement();
			var patMgr = new PATManager();
			var nit = new NITInfoManager().GetAllNITInfos()[0];
			var allTs = tsMgr.GetTSInfoByNetWorkIdentity(nit.Identity).ToList();

			var deletedIds = new HashSet<long>();
			var touchedTs = new HashSet<long>();

			foreach (var d in drops)
			{
				var ts = allTs.FirstOrDefault(t => t.TsName == d.TsName);
				if (ts == null)
				{
					Console.WriteLine("SKIP missing TS {0}", d.TsName);
					continue;
				}
				if (ts.TsId == 22)
					throw new Exception("REFUSE touch ts_id 22");

				var prg = prgMgr.GetByTSIdentityAndServiceId(ts.Identity, d.Sid);
				if (prg == null)
				{
					Console.WriteLine("SKIP not in EPG {0} SID {1} {2}", d.TsName, d.Sid, d.Name);
					continue;
				}
				Console.WriteLine("DELETE {0} SID={1} table={2} [{3}]", d.TsName, d.Sid, prg.Identity, prg.ProgramName);
				prgMgr.DeleteProgram(prg.Identity);
				deletedIds.Add(prg.Identity);
				touchedTs.Add(ts.Identity);
			}

			// Fix SID collision: DD SAHYADRI on TS_562 shares SID 143 with RAJ MUSIX TELUGU on TS_170
			var ts562 = allTs.First(t => t.TsName == "TS_562");
			var sahyadri = prgMgr.GetByTSIdentityAndServiceId(ts562.Identity, 143);
			if (sahyadri != null && (sahyadri.ProgramName ?? "").IndexOf("SAHYADRI", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				int newSid = 21014;
				var clash = prgMgr.GetByTSIdentityAndServiceId(ts562.Identity, newSid);
				if (clash != null)
					throw new Exception("SID 21014 already used on TS_562");
				Console.WriteLine("REMAP TS_562 DD SAHYADRI SID 143 -> {0}", newSid);
				var lcnMap562 = ReadLcnMap(desMgr, ts562.Identity);
				int oldLcn = 143;
				if (lcnMap562.ContainsKey(143))
					oldLcn = lcnMap562[143];
				sahyadri.ServiceId = newSid;
				prgMgr.Save(sahyadri);
				lcnMap562.Remove(143);
				lcnMap562[newSid] = oldLcn;
				RewriteLcnFromMap(desMgr, prgMgr, ts562.Identity, lcnMap562);
				touchedTs.Add(ts562.Identity);
			}

			// Rebuild LCN blobs for touched TS (strip deleted SIDs if DeleteProgram left orphans)
			foreach (long tid in touchedTs)
			{
				var live = prgMgr.GetByTSIdentity(tid).ToList();
				var map = ReadLcnMap(desMgr, tid);
				var pairs = new List<Wanted>();
				foreach (var p in live)
				{
					int lcn;
					if (!map.TryGetValue(p.ServiceId, out lcn))
						lcn = p.ServiceId;
					pairs.Add(new Wanted { Sid = p.ServiceId, Lcn = lcn, Name = p.ProgramName });
				}
				WriteLcn(desMgr, tid, pairs);
				Console.WriteLine("LCN rewritten {0} services={1}", allTs.First(t => t.Identity == tid).TsName, pairs.Count);
			}

			// PAT: remove deleted program table ids
			var patIds = patMgr.GetAll().Select(p => p.ProgramTableId).Where(id => !deletedIds.Contains(id)).ToList();
			patMgr.Save(patIds, "admin");
			Console.WriteLine("PAT now {0} (removed {1})", patIds.Count, deletedIds.Count);

			int n = new SendNoticesManager().SetAllNoticeOn();
			Console.WriteLine("SetAllNoticeOn {0}", n);
			return 0;
		}
		catch (Exception ex)
		{
			Console.WriteLine("ERROR: " + ex);
			return 1;
		}
	}

	private static void RewriteLcnFromMap(DescriptorManagement desMgr, ProgramInfoManager prgMgr, long tsIdentity, Dictionary<int, int> map)
	{
		var live = prgMgr.GetByTSIdentity(tsIdentity).ToList();
		var pairs = new List<Wanted>();
		foreach (var p in live)
		{
			int lcn;
			if (!map.TryGetValue(p.ServiceId, out lcn))
				lcn = p.ServiceId;
			pairs.Add(new Wanted { Sid = p.ServiceId, Lcn = lcn, Name = p.ProgramName });
		}
		WriteLcn(desMgr, tsIdentity, pairs);
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
		foreach (var p in pairs)
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
			if (pairs.Count == 0) return;
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
