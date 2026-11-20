using System;
using System.Collections.Generic;
using System.Linq;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Model;
using DeXin.EPG.Tools;

internal static class SetTs410Lcn90
{
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
			var tsMgr = new TSInfoManager();
			var prgMgr = new ProgramInfoManager();
			var desMgr = new DescriptorManagement();
			var nit = new NITInfoManager().GetAllNITInfos()[0];
			var allTs = tsMgr.GetTSInfoByNetWorkIdentity(nit.Identity).ToList();

			// Free 99/100: move I NEWS + Valicham Plus to free LCNs 135/136
			MoveSid(desMgr, prgMgr, allTs, 135, 135, "I NEWS");
			MoveSid(desMgr, prgMgr, allTs, 209, 136, "Valicham Plus");

			// TS_410 / 6.32 CH5 -> LCN 90-100 (match QAM order; Z SALAM -> 100)
			var ts = allTs.FirstOrDefault(t => t.TsName == "TS_410");
			if (ts == null) throw new Exception("TS_410 missing");
			if (string.Equals(ts.TsName, "TS_546", StringComparison.OrdinalIgnoreCase))
				throw new Exception("REFUSE 546");

			var map = new Dictionary<int, int>
			{
				{ 18401, 90 },
				{ 18402, 91 },
				{ 18403, 92 },
				{ 18406, 93 },
				{ 18404, 94 },
				{ 18405, 95 },
				{ 18407, 96 },
				{ 18408, 97 },
				{ 18409, 98 },
				{ 18410, 99 },
				{ 18412, 100 }
			};

			var live = prgMgr.GetByTSIdentity(ts.Identity).ToList();
			var lcnMap = ReadLcnMap(desMgr, ts.Identity);
			foreach (var kv in map)
				lcnMap[kv.Key] = kv.Value;

			var pairs = new List<Wanted>();
			foreach (var p in live)
			{
				int lcn;
				if (!lcnMap.TryGetValue(p.ServiceId, out lcn))
					lcn = p.ServiceId;
				if (lcn < 1 || lcn > 999)
					throw new Exception("Bad LCN " + lcn + " for SID " + p.ServiceId);
				pairs.Add(new Wanted { Sid = p.ServiceId, Lcn = lcn, Name = p.ProgramName });
			}
			WriteLcn(desMgr, ts.Identity, pairs);
			foreach (var w in pairs.OrderBy(x => x.Lcn))
				Console.WriteLine("TS_410 sid={0} lcn={1} {2}", w.Sid, w.Lcn, w.Name);

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

	private static void MoveSid(DescriptorManagement desMgr, ProgramInfoManager prgMgr, List<TsInfo> allTs, int sid, int newLcn, string label)
	{
		foreach (var ts in allTs)
		{
			var live = prgMgr.GetByTSIdentity(ts.Identity).ToList();
			var hit = live.FirstOrDefault(p => p.ServiceId == sid);
			if (hit == null) continue;
			var lcnMap = ReadLcnMap(desMgr, ts.Identity);
			int old;
			lcnMap.TryGetValue(sid, out old);
			lcnMap[sid] = newLcn;
			var pairs = new List<Wanted>();
			foreach (var p in live)
			{
				int lcn;
				if (!lcnMap.TryGetValue(p.ServiceId, out lcn))
					lcn = p.ServiceId;
				pairs.Add(new Wanted { Sid = p.ServiceId, Lcn = lcn, Name = p.ProgramName });
			}
			WriteLcn(desMgr, ts.Identity, pairs);
			Console.WriteLine("Moved {0} sid={1} on {2}: LCN {3} -> {4}", label, sid, ts.TsName, old, newLcn);
			return;
		}
		Console.WriteLine("WARN: {0} sid={1} not found", label, sid);
	}

	private static Dictionary<int, int> ReadLcnMap(DescriptorManagement desMgr, long tsIdentity)
	{
		var map = new Dictionary<int, int>();
		var des = desMgr.GetDescriptors(EnumPossibleLoaction.NIT_TS, tsIdentity)
			.FirstOrDefault(d => d.DesTypeTag == 131);
		if (des == null || des.DescriptionData == null) return map;
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
