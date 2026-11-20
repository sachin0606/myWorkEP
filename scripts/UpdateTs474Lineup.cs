using System;
using System.Collections.Generic;
using System.Linq;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Model;
using DeXin.EPG.Tools;

internal static class UpdateTs474Lineup
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

			var add = new[]
			{
				new Wanted { Sid = 28003, Lcn = 300, Name = "EPIC PARIWAR- PAY" },
				new Wanted { Sid = 28001, Lcn = 322, Name = "CINEPLEX-PAY" },
				new Wanted { Sid = 28004, Lcn = 407, Name = "EPIC MUSIC" },
				new Wanted { Sid = 28002, Lcn = 475, Name = "EPIC KIDS-PAY" },
				new Wanted { Sid = 28005, Lcn = 801, Name = "COLORS KANNADA HD - PAY" }
			};

			var tsMgr = new TSInfoManager();
			var prgMgr = new ProgramInfoManager();
			var desMgr = new DescriptorManagement();
			var patMgr = new PATManager();
			var nit = new NITInfoManager().GetAllNITInfos()[0];
			var allTs = tsMgr.GetTSInfoByNetWorkIdentity(nit.Identity).ToList();

			var ts474 = allTs.FirstOrDefault(t => t.TsName == "TS_474");
			if (ts474 == null)
				throw new Exception("TS_474 missing");
			if (ts474.TsId == 22)
				throw new Exception("REFUSE ts_id 22");
			Console.WriteLine("TS_474 ts_id={0} table={1}", ts474.TsId, ts474.Identity);

			var newIds = new List<long>();
			foreach (var w in add)
			{
				var have = prgMgr.GetByTSIdentity(ts474.Identity).FirstOrDefault(p => p.ServiceId == w.Sid);
				if (have != null)
				{
					if (!string.Equals((have.ProgramName ?? "").Trim(), w.Name, StringComparison.OrdinalIgnoreCase))
					{
						have.ProgramName = w.Name;
						prgMgr.Save(have);
						Console.WriteLine("  REN sid={0} {1}", w.Sid, w.Name);
					}
					else
						Console.WriteLine("  exists sid={0} {1}", w.Sid, w.Name);
					newIds.Add(have.Identity);
					continue;
				}
				var prg = new ProgramInfo();
				prg.TsTableId = ts474.Identity;
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
				var saved = prgMgr.GetByTSIdentity(ts474.Identity).First(x => x.ServiceId == w.Sid);
				newIds.Add(saved.Identity);
				Console.WriteLine("  ADD sid={0} lcn={1} {2}", w.Sid, w.Lcn, w.Name);
			}

			WriteLcn(desMgr, ts474.Identity, add.ToList());
			Console.WriteLine("TS_474 LCN written {0}", add.Length);
			foreach (var w in add.OrderBy(x => x.Lcn))
				Console.WriteLine("  LCN {0} sid={1} {2}", w.Lcn, w.Sid, w.Name);

			// Move EPIC MUSIC off TS_178 (Showbox on 474 is the same channel)
			var ts178 = allTs.FirstOrDefault(t => t.TsName == "TS_178");
			var deletedIds = new HashSet<long>();
			if (ts178 == null)
				Console.WriteLine("SKIP TS_178 missing");
			else if (ts178.TsId == 22)
				throw new Exception("REFUSE ts_id 22");
			else
			{
				var epic = prgMgr.GetByTSIdentity(ts178.Identity).FirstOrDefault(p => p.ServiceId == 11013);
				if (epic == null)
					Console.WriteLine("TS_178 SID 11013 already gone");
				else
				{
					Console.WriteLine("DELETE TS_178 SID=11013 table={0} [{1}]", epic.Identity, epic.ProgramName);
					prgMgr.DeleteProgram(epic.Identity);
					deletedIds.Add(epic.Identity);
					var live178 = prgMgr.GetByTSIdentity(ts178.Identity).ToList();
					var map178 = ReadLcnMap(desMgr, ts178.Identity);
					map178.Remove(11013);
					var pairs178 = new List<Wanted>();
					foreach (var p in live178)
					{
						int lcn;
						if (!map178.TryGetValue(p.ServiceId, out lcn))
							lcn = p.ServiceId;
						pairs178.Add(new Wanted { Sid = p.ServiceId, Lcn = lcn, Name = p.ProgramName });
					}
					WriteLcn(desMgr, ts178.Identity, pairs178);
					Console.WriteLine("TS_178 LCN rewritten services={0}", pairs178.Count);
				}
			}

			var patIds = patMgr.GetAll().Select(p => p.ProgramTableId).Where(id => !deletedIds.Contains(id)).ToList();
			foreach (var id in newIds)
			{
				if (!patIds.Contains(id))
					patIds.Add(id);
			}
			patMgr.Save(patIds, "admin");
			Console.WriteLine("PAT now {0}", patIds.Count);

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
