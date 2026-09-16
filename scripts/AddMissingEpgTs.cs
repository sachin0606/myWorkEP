using System;
using System.Collections.Generic;
using System.Linq;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Model;
using DeXin.EPG.Tools;

internal static class AddMissingEpgTs
{
	private class Mux
	{
		public int Freq;
		public int TsId;
		public int Port;
		public Prg[] Programs;
	}

	private class Prg
	{
		public int Sid;
		public int Lcn;
		public string Name;
		public int ServiceType;
		public string Provider;
	}

	public static int Main(string[] args)
	{
		try
		{
			SqlHelper.SetConnectionString("Data Source=localhost;Initial Catalog=EPG2;Integrated Security=True");
			var loc = LocationControl.Ins;

			var muxes = new Mux[]
			{
				new Mux { Freq = 378, TsId = 33, Port = 1033, Programs = new[] {
					P(137, "DD TAMIL", 1, ""),
					P(151, "Sarv Dharm Sangam", 1, ""),
					P(196, "STUDIO YUVA", 1, ""),
					P(199, "TV9 Bangla", 1, ""),
					P(266, "NATION 27", 1, ""),
					P(343, "ANB NEWS", 1, ""),
					P(358, "ZEE BIHAR JHARKHAND", 1, ""),
					P(365, "MANORANJAN PRIME", 22, ""),
					P(411, "WIN TV", 1, "")
				}},
				new Mux { Freq = 386, TsId = 34, Port = 1034, Programs = new[] {
					P(108, "9X JALWA", 1, "PMSL"),
					P(218, "News State Uttarakhand/Uttar Pr", 1, ""),
					P(221, "Mango TV", 1, ""),
					P(263, "Shree Disha Bhakti", 1, ""),
					P(275, "DD Yadagiri", 1, ""),
					P(368, "TV9 Telugu", 1, ""),
					P(392, "Sakshi TV", 1, ""),
					P(432, "HMTV", 1, ""),
					P(485, "I NEWS", 1, ""),
					P(488, "10 TV", 1, "")
				}},
				new Mux { Freq = 394, TsId = 35, Port = 1035, Programs = new[] {
					P(50, "DD URDU", 22, ""),
					P(181, "Goldmines Bollywood", 1, ""),
					P(386, "ARYQTV", 1, ""),
					P(411, "WIN TV", 1, ""),
					P(413, "ASOM LIVE 24", 22, ""),
					P(475, "MERCY TV", 1, "")
				}},
				new Mux { Freq = 402, TsId = 36, Port = 1036, Programs = new[] {
					P(84, "Jinvani", 1, ""),
					P(166, "Goldmines 2", 1, ""),
					P(202, "IBC-24", 1, ""),
					P(205, "9X TASHAN", 1, ""),
					P(423, "7S MUSIC", 1, "")
				}}
			};

			var nitMgr = new NITInfoManager();
			var tsMgr = new TSInfoManager();
			var prgMgr = new ProgramInfoManager();
			var desMgr = new DescriptorManagement();
			var ipMgr = new MultiIPOutputBLL();
			var sendMgr = new SendOrNotInfoBLL();
			var speedMgr = new SendSpeedInfoManager();
			var patMgr = new PATManager();

			var nits = nitMgr.GetAllNITInfos();
			if (nits == null || nits.Count == 0)
				throw new Exception("No NIT in EPG2");
			var nit = nits[0];
			Console.WriteLine("NIT identity={0} network={1} {2}", nit.Identity, nit.NetworkId, nit.NetworkName);

			var speedTemplate = speedMgr.GetByTSIdentity(21);
			if (speedTemplate == null)
				speedTemplate = speedMgr.GetByTSIdentity(1);

			var newProgramIds = new List<long>();

			foreach (var mux in muxes)
			{
				string tsName = "TS_" + mux.Freq;
				var existing = tsMgr.GetTSInfoByNetWorkIdentity(nit.Identity)
					.FirstOrDefault(t => t.TsName == tsName || t.TsId == mux.TsId);
				TsInfo ts;
				if (existing != null)
				{
					ts = existing;
					Console.WriteLine("TS already present {0} id={1} table={2}", tsName, ts.TsId, ts.Identity);
				}
				else
				{
					ts = new TsInfo();
					ts.NitTableId = nit.Identity;
					ts.TsId = mux.TsId;
					ts.TsName = tsName;
					ts.OriginalNetworkId = nit.NetworkId;
					ts.PatVersionNumber = -1;
					ts.CatVersionNumber = -1;
					ts.SdtVersionNumber = -1;
					int saved = tsMgr.SaveTSInfo(ts);
					ts = tsMgr.GetTSInfoByTSId(mux.TsId).First();
					Console.WriteLine("Created {0} ts_id={1} table={2} save={3}", tsName, ts.TsId, ts.Identity, saved);
				}

				foreach (var p in mux.Programs)
				{
					var already = prgMgr.GetByTSIdentity(ts.Identity)
						.FirstOrDefault(x => x.ServiceId == p.Sid);
					if (already != null)
					{
						newProgramIds.Add(already.Identity);
						Console.WriteLine("  program exists sid={0} {1} table={2}", p.Sid, p.Name, already.Identity);
						continue;
					}
					var prg = new ProgramInfo();
					prg.TsTableId = ts.Identity;
					prg.ServiceId = p.Sid;
					prg.ProgramName = p.Name;
					prg.ServiceType = p.ServiceType;
					prg.ProviderName = string.IsNullOrEmpty(p.Provider) ? "HCN" : p.Provider;
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
					var savedPrg = prgMgr.GetByTSIdentity(ts.Identity).First(x => x.ServiceId == p.Sid);
					newProgramIds.Add(savedPrg.Identity);
					Console.WriteLine("  added sid={0} lcn={1} {2} table={3}", p.Sid, p.Lcn, p.Name, savedPrg.Identity);
				}

				if (!ipMgr.GetData().Any(x => x.TsTableId == ts.Identity))
				{
					var ip = new MultipleipSettings();
					ip.TsTableId = ts.Identity;
					ip.Ip = "224.2.2.30";
					ip.Port = mux.Port;
					ipMgr.Add(ip);
					Console.WriteLine("  multi-ip 224.2.2.30:{0}", mux.Port);
				}

				if (sendMgr.GetDataByTSTableID(ts.Identity).Count == 0)
				{
					var so = new SendOrNotInfo();
					so.TsTableId = ts.Identity;
					so.Pat = 0;
					so.Cat = 0;
					so.Pmt = 0;
					so.SdtAct = 1;
					so.SdtOth = 1;
					so.NitAct = 1;
					so.NitOth = 1;
					so.Tdt = 1;
					so.Tot = 1;
					so.EitActPf = 1;
					so.EitActSc = 1;
					so.EitOthPf = 0;
					so.EitOthSc = 0;
					so.Bat = 1;
					sendMgr.Add(so);
					Console.WriteLine("  send-or-not SI overlay");
				}

				if (speedMgr.GetByTSIdentity(ts.Identity) == null && speedTemplate != null)
				{
					var sp = new SendSpeedInfo();
					sp.TsTableId = ts.Identity;
					sp.Pat = speedTemplate.Pat;
					sp.Cat = speedTemplate.Cat;
					sp.Pmt = speedTemplate.Pmt;
					sp.SdtAct = speedTemplate.SdtAct;
					sp.SdtOth = speedTemplate.SdtOth;
					sp.NitAct = speedTemplate.NitAct;
					sp.NitOth = speedTemplate.NitOth;
					sp.Tdt = speedTemplate.Tdt;
					sp.Tot = speedTemplate.Tot;
					sp.EitActPf = speedTemplate.EitActPf;
					sp.EitActSc = speedTemplate.EitActSc;
					sp.EitOthPf = speedTemplate.EitOthPf;
					sp.EitOthSc = speedTemplate.EitOthSc;
					sp.Bat = speedTemplate.Bat;
					speedMgr.Save(sp);
					Console.WriteLine("  send-speed copied");
				}
			}

			var patIds = patMgr.GetAll().Select(p => p.ProgramTableId).ToList();
			foreach (var id in newProgramIds)
			{
				if (!patIds.Contains(id))
					patIds.Add(id);
			}
			patMgr.Save(patIds, "admin");
			Console.WriteLine("PAT now has {0} programs (added {1} new)", patIds.Count, newProgramIds.Count);

			foreach (var mux in muxes)
			{
				var ts = tsMgr.GetTSInfoByTSId(mux.TsId).First();
				var existingDes = desMgr.GetDescriptors(EnumPossibleLoaction.NIT_TS, ts.Identity);
				if (!existingDes.Any(d => d.DesTypeTag == 68))
				{
					var cable = new DescriptorSigleBase(EnumPossibleLoaction.NIT_TS, ts.Identity);
					cable.DesTypeID = 21;
					cable.DesTypeTag = 68;
					cable.DescriptionData = CableBytes(mux.Freq);
					cable.DataLength = 11;
					string msg;
					if (!desMgr.AddDescriptor(cable, out msg))
						throw new Exception("cable descriptor failed: " + msg);
					Console.WriteLine("  {0} cable delivery {1} MHz", ts.TsName, mux.Freq);
				}
				else
				{
					Console.WriteLine("  {0} cable already present", ts.TsName);
				}

				if (!existingDes.Any(d => d.DesTypeTag == 131))
				{
					var lcn = new DescriptorSigleBase(EnumPossibleLoaction.NIT_TS, ts.Identity);
					lcn.DesTypeID = 130;
					lcn.DesTypeTag = 131;
					lcn.DescriptionData = LcnBytes(mux.Programs);
					lcn.DataLength = lcn.DescriptionData.Length;
					string msg;
					if (!desMgr.AddDescriptor(lcn, out msg))
						throw new Exception("LCN descriptor failed: " + msg);
					Console.WriteLine("  {0} LCN {1} services", ts.TsName, mux.Programs.Length);
				}
				else
				{
					Console.WriteLine("  {0} LCN already present", ts.TsName);
				}
			}

			int n = new SendNoticesManager().SetAllNoticeOn();
			Console.WriteLine("Update sending data: {0} notices armed", n);
			return 0;
		}
		catch (Exception ex)
		{
			Console.WriteLine("ERROR: " + ex);
			return 1;
		}
	}

	private static Prg P(int sid, string name, int type, string provider)
	{
		return new Prg { Sid = sid, Lcn = sid, Name = name, ServiceType = type, Provider = provider };
	}

	private static byte[] CableBytes(int mhz)
	{
		string s = mhz.ToString("D4") + "0000";
		byte[] b = new byte[11];
		for (int i = 0; i < 4; i++)
			b[i] = (byte)(((s[i * 2] - '0') << 4) | (s[i * 2 + 1] - '0'));
		b[4] = 0x00;
		b[5] = 0x00;
		b[6] = 0x03;
		b[7] = 0x00;
		b[8] = 0x68;
		b[9] = 0x75;
		b[10] = 0x00;
		return b;
	}

	private static byte[] LcnBytes(Prg[] programs)
	{
		byte[] b = new byte[programs.Length * 4];
		int i = 0;
		foreach (var p in programs)
		{
			b[i++] = (byte)(p.Sid >> 8);
			b[i++] = (byte)(p.Sid & 0xFF);
			b[i++] = (byte)(0xC0 | ((p.Lcn >> 8) & 0x3F));
			b[i++] = (byte)(p.Lcn & 0xFF);
		}
		return b;
	}
}
