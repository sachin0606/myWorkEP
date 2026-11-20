using System;
using System.Collections.Generic;
using System.Linq;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Model;
using DeXin.EPG.Tools;

internal static class AddAlJazeera194
{
	private const int Sid = 13016;
	private const int Lcn = 387;
	private const string Name = "Al Jazeera English";

	public static int Main()
	{
		try
		{
			SqlHelper.SetConnectionString("Data Source=localhost;Initial Catalog=EPG2;Integrated Security=True");
			var loc = LocationControl.Ins;

			var tsMgr = new TSInfoManager();
			var prgMgr = new ProgramInfoManager();
			var desMgr = new DescriptorManagement();
			var patMgr = new PATManager();

			var nit = new NITInfoManager().GetAllNITInfos()[0];
			var ts = tsMgr.GetTSInfoByNetWorkIdentity(nit.Identity)
				.FirstOrDefault(t => t.TsName == "TS_194");
			if (ts == null)
				throw new Exception("TS_194 missing");
			if (ts.TsId == 22)
				throw new Exception("REFUSE ts_id 22");
			Console.WriteLine("TS_194 ts_id={0} table={1}", ts.TsId, ts.Identity);

			var existing = prgMgr.GetByTSIdentity(ts.Identity).ToList();
			var prg = existing.FirstOrDefault(p => p.ServiceId == Sid);
			if (prg == null)
			{
				prg = new ProgramInfo();
				prg.TsTableId = ts.Identity;
				prg.ServiceId = Sid;
				prg.ProgramName = Name;
				prg.ServiceType = 1;
				prg.ProviderName = "HCN";
				prg.PmtId = 8191;
				prg.PCRPID = 8191;
				prg.PmtVersionNumber = -1;
				prg.PrgSdtEitscflag = 1;
				prg.PrgSdtEitpfflag = 1;
				prg.PrgSdtRunningstate = 4;
				prg.PrgSdtFreecamod = 0; // scrambled
				prg.PrgManualeventid = 0;
				prg.EITFileName = "";
				prgMgr.Save(prg);
				prg = prgMgr.GetByTSIdentity(ts.Identity).First(x => x.ServiceId == Sid);
				Console.WriteLine("ADD sid={0} {1} table={2}", Sid, Name, prg.Identity);
			}
			else
			{
				prg.ProgramName = Name;
				prg.PrgSdtFreecamod = 0;
				prgMgr.Save(prg);
				Console.WriteLine("UPDATE sid={0} {1} table={2}", Sid, Name, prg.Identity);
			}

			var live = prgMgr.GetByTSIdentity(ts.Identity).ToList();
			var lcnMap = ReadLcnMap(desMgr, ts.Identity);
			lcnMap[Sid] = Lcn;

			var pairs = new List<KeyValuePair<int, int>>();
			foreach (var p in live)
			{
				int lcn;
				if (!lcnMap.TryGetValue(p.ServiceId, out lcn))
					lcn = p.ServiceId;
				pairs.Add(new KeyValuePair<int, int>(p.ServiceId, lcn));
			}
			WriteLcn(desMgr, ts.Identity, pairs);
			Console.WriteLine("LCN services={0}", pairs.Count);
			foreach (var kv in pairs.OrderBy(x => x.Value))
			{
				var n = live.First(p => p.ServiceId == kv.Key).ProgramName;
				Console.WriteLine("  sid={0} lcn={1} {2}", kv.Key, kv.Value, n);
			}

			var patIds = patMgr.GetAll().Select(p => p.ProgramTableId).ToList();
			if (!patIds.Contains(prg.Identity))
			{
				patIds.Add(prg.Identity);
				patMgr.Save(patIds, "admin");
				Console.WriteLine("PAT added program table {0}", prg.Identity);
			}
			else
				Console.WriteLine("PAT already has program");

			int nNotice = new SendNoticesManager().SetAllNoticeOn();
			Console.WriteLine("SetAllNoticeOn {0}", nNotice);
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
