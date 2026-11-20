using System;
using System.Collections.Generic;
using System.Linq;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Model;
using DeXin.EPG.Tools;

// Keep Al Jazeera English (SID 13016 @ TS_194) on LCN 387.
// Move REPUBLIC TV -FREE (SID 22003 @ TS_570) to LCN 62 (gapped plan; currently free).
internal static class FixLcn387Clash
{
	private const int RepublicSid = 22003;
	private const int RepublicLcn = 62;
	private const int AlJazeeraSid = 13016;
	private const int AlJazeeraLcn = 387;

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

			var ts570 = allTs.FirstOrDefault(t => t.TsName == "TS_570");
			if (ts570 == null)
				throw new Exception("TS_570 missing");
			// TS_570 legitimately uses DVB transport_stream_id=22 in this plant
			Console.WriteLine("TS_570 ts_id={0} table={1}", ts570.TsId, ts570.Identity);

			var live570 = prgMgr.GetByTSIdentity(ts570.Identity).ToList();
			var rep = live570.FirstOrDefault(p => p.ServiceId == RepublicSid);
			if (rep == null)
				throw new Exception("SID 22003 missing on TS_570");

			var lcn570 = ReadLcnMap(desMgr, ts570.Identity);
			int oldRep;
			lcn570.TryGetValue(RepublicSid, out oldRep);
			Console.WriteLine("TS_570 SID {0} '{1}' LCN {2} -> {3}", RepublicSid, rep.ProgramName, oldRep, RepublicLcn);
			lcn570[RepublicSid] = RepublicLcn;

			var pairs570 = new List<KeyValuePair<int, int>>();
			foreach (var p in live570)
			{
				int lcn;
				if (!lcn570.TryGetValue(p.ServiceId, out lcn))
					lcn = p.ServiceId;
				pairs570.Add(new KeyValuePair<int, int>(p.ServiceId, lcn));
			}
			WriteLcn(desMgr, ts570.Identity, pairs570);

			var ts194 = allTs.FirstOrDefault(t => t.TsName == "TS_194");
			if (ts194 == null)
				throw new Exception("TS_194 missing");
			var lcn194 = ReadLcnMap(desMgr, ts194.Identity);
			int aj;
			if (!lcn194.TryGetValue(AlJazeeraSid, out aj) || aj != AlJazeeraLcn)
				Console.WriteLine("WARN TS_194 Al Jazeera LCN={0} (expected {1})", aj, AlJazeeraLcn);
			else
				Console.WriteLine("KEEP TS_194 SID {0} Al Jazeera English LCN {1}", AlJazeeraSid, AlJazeeraLcn);

			// Network duplicate check for 387 and 62
			foreach (var lv in new[] { 387, 62 })
			{
				var hits = new List<string>();
				foreach (var t in allTs)
				{
					var m = ReadLcnMap(desMgr, t.Identity);
					foreach (var kv in m)
					{
						if (kv.Value == lv)
						{
							var name = prgMgr.GetByTSIdentity(t.Identity)
								.Where(p => p.ServiceId == kv.Key)
								.Select(p => p.ProgramName)
								.FirstOrDefault() ?? "?";
							hits.Add(string.Format("{0} SID={1} {2}", t.TsName, kv.Key, name));
						}
					}
				}
				Console.WriteLine("LCN {0} now: {1}", lv, hits.Count == 0 ? "FREE" : string.Join(" | ", hits));
			}

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

	private static void WriteLcn(DescriptorManagement desMgr, long tsIdentity, List<KeyValuePair<int, int>> pairs)
	{
		byte[] b = new byte[pairs.Count * 4];
		int i = 0;
		foreach (var p in pairs)
		{
			b[i++] = (byte)(p.Key >> 8);
			b[i++] = (byte)(p.Key & 0xFF);
			b[i++] = (byte)(0xC0 | ((p.Value >> 8) & 0x3F));
			b[i++] = (byte)(p.Value & 0xFF);
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
