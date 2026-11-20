using System;
using System.Collections.Generic;
using System.Linq;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Model;
using DeXin.EPG.Tools;

// TV9 Telugu is news — move out of Telugu GEC head to LCN 150.
internal static class SetTv9TeluguLcn150
{
	private const int Sid = 11009;
	private const int NewLcn = 150;

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

			var ts = allTs.FirstOrDefault(t => t.TsName == "TS_178");
			if (ts == null)
				throw new Exception("TS_178 missing");

			// Ensure 150 free network-wide
			foreach (var t in allTs)
			{
				var m = ReadLcnMap(desMgr, t.Identity);
				foreach (var kv in m)
				{
					if (kv.Value == NewLcn && kv.Key != Sid)
					{
						var name = prgMgr.GetByTSIdentity(t.Identity)
							.Where(p => p.ServiceId == kv.Key)
							.Select(p => p.ProgramName)
							.FirstOrDefault() ?? "?";
						throw new Exception(string.Format("LCN {0} already used by {1} SID={2} {3}", NewLcn, t.TsName, kv.Key, name));
					}
				}
			}

			var live = prgMgr.GetByTSIdentity(ts.Identity).ToList();
			var prg = live.FirstOrDefault(p => p.ServiceId == Sid);
			if (prg == null)
				throw new Exception("SID 11009 TV9 Telugu missing on TS_178");

			var map = ReadLcnMap(desMgr, ts.Identity);
			int old;
			map.TryGetValue(Sid, out old);
			Console.WriteLine("TS_178 SID {0} '{1}' LCN {2} -> {3}", Sid, prg.ProgramName, old, NewLcn);
			map[Sid] = NewLcn;

			var pairs = new List<KeyValuePair<int, int>>();
			foreach (var p in live)
			{
				int lcn;
				if (!map.TryGetValue(p.ServiceId, out lcn))
					lcn = p.ServiceId;
				pairs.Add(new KeyValuePair<int, int>(p.ServiceId, lcn));
			}
			WriteLcn(desMgr, ts.Identity, pairs);

			int n = new SendNoticesManager().SetAllNoticeOn();
			Console.WriteLine("SetAllNoticeOn {0}", n);

			Console.WriteLine("\n=== Telugu window 138-150 ===");
			var occ = new Dictionary<int, string>();
			foreach (var t in allTs)
			{
				var m = ReadLcnMap(desMgr, t.Identity);
				foreach (var p in prgMgr.GetByTSIdentity(t.Identity))
				{
					int lcn;
					if (m.TryGetValue(p.ServiceId, out lcn) && lcn >= 138 && lcn <= 150)
						occ[lcn] = string.Format("{0} SID={1} {2}", t.TsName, p.ServiceId, p.ProgramName);
				}
			}
			for (int i = 138; i <= 150; i++)
			{
				if (occ.ContainsKey(i))
					Console.WriteLine("  {0} {1}", i, occ[i]);
				else
					Console.WriteLine("  {0} FREE", i);
			}

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
