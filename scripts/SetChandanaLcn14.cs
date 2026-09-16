using System;
using System.Collections.Generic;
using System.Linq;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Model;
using DeXin.EPG.Tools;

internal static class SetChandanaLcn14
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
			var ts = tsMgr.GetTSInfoByNetWorkIdentity(nit.Identity)
				.FirstOrDefault(t => t.TsName == "TS_562");
			if (ts == null)
				throw new Exception("TS_562 missing");
			if (ts.TsId == 22)
				throw new Exception("REFUSE ts_id 22");
			Console.WriteLine("TS_562 ts_id={0} table={1}", ts.TsId, ts.Identity);

			var live = prgMgr.GetByTSIdentity(ts.Identity).ToList();
			var chandana = live.FirstOrDefault(p => p.ServiceId == 308);
			if (chandana == null)
				throw new Exception("SID 308 DD Chandana missing on TS_562");
			Console.WriteLine("found SID 308 {0}", chandana.ProgramName);

			var lcnMap = ReadLcnMap(desMgr, ts.Identity);
			int old;
			if (lcnMap.TryGetValue(308, out old))
				Console.WriteLine("SID 308 LCN {0} -> 14", old);
			else
				Console.WriteLine("SID 308 had no LCN; setting 14");
			lcnMap[308] = 14;

			var pairs = new List<Wanted>();
			foreach (var p in live)
			{
				int lcn;
				if (!lcnMap.TryGetValue(p.ServiceId, out lcn))
					lcn = p.ServiceId;
				pairs.Add(new Wanted { Sid = p.ServiceId, Lcn = lcn, Name = p.ProgramName });
			}
			WriteLcn(desMgr, ts.Identity, pairs);
			foreach (var w in pairs)
				Console.WriteLine("  sid={0} lcn={1} {2}", w.Sid, w.Lcn, w.Name);

			int n = new SendNoticesManager().SetAllNoticeOn();
			Console.WriteLine("SetAllNoticeOn {0}", n);
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
