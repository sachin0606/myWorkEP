using System;
using System.Collections.Generic;
using System.Linq;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Model;
using DeXin.EPG.Tools;

internal static class AddPublicMovies570
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

			var extra = new Wanted { Sid = 12012, Lcn = 13, Name = "PUBLIC MOVIES" };

			var tsMgr = new TSInfoManager();
			var prgMgr = new ProgramInfoManager();
			var desMgr = new DescriptorManagement();
			var patMgr = new PATManager();

			var nit = new NITInfoManager().GetAllNITInfos()[0];
			var ts = tsMgr.GetTSInfoByNetWorkIdentity(nit.Identity)
				.FirstOrDefault(t => t.TsName == "TS_570");
			if (ts == null)
				throw new Exception("TS_570 missing");
			Console.WriteLine("TS_570 ts_id={0} table={1}", ts.TsId, ts.Identity);

			var existing = prgMgr.GetByTSIdentity(ts.Identity).ToList();
			var have = existing.FirstOrDefault(p => p.ServiceId == extra.Sid);
			long newId;
			if (have != null)
			{
				if (!string.Equals((have.ProgramName ?? "").Trim(), extra.Name, StringComparison.OrdinalIgnoreCase))
				{
					have.ProgramName = extra.Name;
					prgMgr.Save(have);
					Console.WriteLine("  REN sid={0} {1}", extra.Sid, extra.Name);
				}
				else
					Console.WriteLine("  exists sid={0} {1} table={2}", extra.Sid, extra.Name, have.Identity);
				newId = have.Identity;
			}
			else
			{
				var prg = new ProgramInfo();
				prg.TsTableId = ts.Identity;
				prg.ServiceId = extra.Sid;
				prg.ProgramName = extra.Name;
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
				var saved = prgMgr.GetByTSIdentity(ts.Identity).First(x => x.ServiceId == extra.Sid);
				newId = saved.Identity;
				Console.WriteLine("  ADD sid={0} lcn={1} {2} table={3}", extra.Sid, extra.Lcn, extra.Name, saved.Identity);
			}

			var live = prgMgr.GetByTSIdentity(ts.Identity).ToList();
			var lcnMap = ReadLcnMap(desMgr, ts.Identity);
			lcnMap[extra.Sid] = extra.Lcn;
			if (!lcnMap.ContainsKey(22008)) lcnMap[22008] = 9;
			if (!lcnMap.ContainsKey(22007)) lcnMap[22007] = 24;

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

			var patIds = patMgr.GetAll().Select(p => p.ProgramTableId).ToList();
			if (!patIds.Contains(newId))
				patIds.Add(newId);
			patMgr.Save(patIds, "admin");
			Console.WriteLine("PAT now {0} programs", patIds.Count);

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
