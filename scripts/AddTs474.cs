using System;
using System.Collections.Generic;
using System.Linq;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Model;
using DeXin.EPG.Tools;

internal static class AddTs474
{
	private class Prg
	{
		public int Sid;
		public int Lcn;
		public string Name;
		public int ServiceType;
		public string Provider;
	}

	public static int Main()
	{
		try
		{
			SqlHelper.SetConnectionString("Data Source=localhost;Initial Catalog=EPG2;Integrated Security=True");
			var loc = LocationControl.Ins;

			const int freq = 474;
			const int tsId = 31;
			const int port = 1038;
			const string tsName = "TS_474";

			// Main-headend rank 270 / remote LCN 911 — HD_KANNADA before ZEE KANNADA HD (802)
			var programs = new[]
			{
				P(28005, 801, "COLORS KANNADA HD - PAY", 1, "HCN")
			};

			var nitMgr = new NITInfoManager();
			var tsMgr = new TSInfoManager();
			var prgMgr = new ProgramInfoManager();
			var desMgr = new DescriptorManagement();
			var ipMgr = new MultiIPOutputBLL();
			var sendMgr = new SendOrNotInfoBLL();
			var speedMgr = new SendSpeedInfoManager();
			var patMgr = new PATManager();

			if (tsMgr.GetTSInfoByTSId(22).Count == 0)
				throw new Exception("Safety check failed: ts_id 22 missing");
			if (tsId == 22)
				throw new Exception("REFUSE ts_id 22");

			var nit = nitMgr.GetAllNITInfos()[0];
			var speedTemplate = speedMgr.GetByTSIdentity(21) ?? speedMgr.GetByTSIdentity(1);

			var existing = tsMgr.GetTSInfoByNetWorkIdentity(nit.Identity)
				.FirstOrDefault(t => t.TsName == tsName || t.TsId == tsId);
			TsInfo ts;
			if (existing != null)
			{
				if (existing.TsId == 22)
					throw new Exception("Refusing to write ts_id 22");
				ts = existing;
				Console.WriteLine("TS already present {0} id={1} table={2}", tsName, ts.TsId, ts.Identity);
			}
			else
			{
				ts = new TsInfo();
				ts.NitTableId = nit.Identity;
				ts.TsId = tsId;
				ts.TsName = tsName;
				ts.OriginalNetworkId = nit.NetworkId;
				ts.PatVersionNumber = -1;
				ts.CatVersionNumber = -1;
				ts.SdtVersionNumber = -1;
				tsMgr.SaveTSInfo(ts);
				ts = tsMgr.GetTSInfoByTSId(tsId).First();
				if (ts.TsId == 22)
					throw new Exception("Refusing to write ts_id 22");
				Console.WriteLine("Created {0} ts_id={1} table={2}", tsName, ts.TsId, ts.Identity);
			}

			var newProgramIds = new List<long>();
			foreach (var p in programs)
			{
				var already = prgMgr.GetByTSIdentity(ts.Identity)
					.FirstOrDefault(x => x.ServiceId == p.Sid);
				if (already != null)
				{
					already.ProgramName = p.Name;
					prgMgr.Save(already);
					newProgramIds.Add(already.Identity);
					Console.WriteLine("  exists sid={0} {1}", p.Sid, p.Name);
					continue;
				}
				var prg = new ProgramInfo();
				prg.TsTableId = ts.Identity;
				prg.ServiceId = p.Sid;
				prg.ProgramName = p.Name;
				prg.ServiceType = p.ServiceType;
				prg.ProviderName = p.Provider;
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
				Console.WriteLine("  added sid={0} lcn={1} {2}", p.Sid, p.Lcn, p.Name);
			}

			if (!ipMgr.GetData().Any(x => x.TsTableId == ts.Identity))
			{
				var ip = new MultipleipSettings();
				ip.TsTableId = ts.Identity;
				ip.Ip = "224.2.2.30";
				ip.Port = port;
				ipMgr.Add(ip);
				Console.WriteLine("  multi-ip 224.2.2.30:{0}", port);
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

			var patIds = patMgr.GetAll().Select(p => p.ProgramTableId).ToList();
			foreach (var id in newProgramIds)
			{
				if (!patIds.Contains(id))
					patIds.Add(id);
			}
			patMgr.Save(patIds, "admin");
			Console.WriteLine("PAT now has {0} programs", patIds.Count);

			var existingDes = desMgr.GetDescriptors(EnumPossibleLoaction.NIT_TS, ts.Identity);
			if (!existingDes.Any(d => d.DesTypeTag == 68))
			{
				var cable = new DescriptorSigleBase(EnumPossibleLoaction.NIT_TS, ts.Identity);
				cable.DesTypeID = 21;
				cable.DesTypeTag = 68;
				cable.DescriptionData = CableBytes(freq);
				cable.DataLength = 11;
				string msg;
				if (!desMgr.AddDescriptor(cable, out msg))
					throw new Exception("cable descriptor failed: " + msg);
				Console.WriteLine("  cable 474 MHz 64-QAM");
			}

			var lcnDes = existingDes.FirstOrDefault(d => d.DesTypeTag == 131);
			byte[] lcnBytes = LcnBytes(programs);
			string lcnMsg;
			if (lcnDes == null)
			{
				var lcn = new DescriptorSigleBase(EnumPossibleLoaction.NIT_TS, ts.Identity);
				lcn.DesTypeID = 130;
				lcn.DesTypeTag = 131;
				lcn.DescriptionData = lcnBytes;
				lcn.DataLength = lcnBytes.Length;
				if (!desMgr.AddDescriptor(lcn, out lcnMsg))
					throw new Exception("LCN add failed: " + lcnMsg);
			}
			else
			{
				lcnDes.DescriptionData = lcnBytes;
				lcnDes.DataLength = lcnBytes.Length;
				if (!desMgr.ModifyDescriptor(lcnDes, out lcnMsg))
					throw new Exception("LCN modify failed: " + lcnMsg);
			}
			Console.WriteLine("  LCN 801 COLORS KANNADA HD");

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

	private static Prg P(int sid, int lcn, string name, int type, string provider)
	{
		return new Prg { Sid = sid, Lcn = lcn, Name = name, ServiceType = type, Provider = provider };
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
