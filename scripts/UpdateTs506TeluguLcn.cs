using System;
using System.Collections.Generic;
using System.Linq;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Model;
using DeXin.EPG.Tools;

// TS_506 new Telugu GEC placement:
//   MAA(140) -> Z TELAGU(141) -> ETV TELAGU(142)
//   -> GEMINI MOVIES(143) -> ZEE CINEMALU(144) -> ETV CINEMA(145)
//   -> GEMINI MUSIC(146) -> SVBC(147 end of Telugu GEC)
//   RAJ NEWS/MUSIX shift to 148/149
//   ETV BAL BHARAT -> Kids end LCN 491
internal static class UpdateTs506TeluguLcn
{
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

			// SID -> new LCN overrides (only listed SIDs change / get set)
			var want = new Dictionary<int, int>
			{
				// TS_506 new
				{ 49005, 141 }, // Z TELAGU after MAA 140
				{ 49001, 142 }, // ETV TELAGU after Z TELAGU
				{ 49003, 144 }, // ZEE CINEMALU after Gemini Movies
				{ 49004, 145 }, // ETV CINEMA after Z Cinemalu
				{ 49002, 491 }, // ETV BAL BHARAT kids
				{ 49006, 147 }, // SVBC end of Telugu GEC
				// shifts to make room
				{ 1005, 143 },  // GEMINI MOVIES-PAY was 141
				{ 1003, 146 },  // GEMINI MUSIC-PAY was 142
				{ 133, 148 },   // RAJ NEWS TELUGU was 144
				{ 143, 149 },   // RAJ MUSIX TELUGU was 147
			};

			var bySidTs = new Dictionary<int, string>();
			foreach (var t in allTs)
			{
				foreach (var p in prgMgr.GetByTSIdentity(t.Identity))
					bySidTs[p.ServiceId] = t.TsName;
			}

			foreach (var kv in want.OrderBy(x => x.Value))
			{
				string tsName;
				if (!bySidTs.TryGetValue(kv.Key, out tsName))
					throw new Exception("SID " + kv.Key + " not found in program_info");
				Console.WriteLine("PLAN sid={0} -> LCN {1} on {2}", kv.Key, kv.Value, tsName);
			}

			// Collision check among target LCNs
			var targetLcns = want.Values.ToList();
			if (targetLcns.Distinct().Count() != targetLcns.Count)
				throw new Exception("Duplicate target LCN in plan");

			// Apply per TS: read full map, overlay wants for SIDs on that TS, write
			var touched = new HashSet<string>();
			foreach (var t in allTs)
			{
				var live = prgMgr.GetByTSIdentity(t.Identity).ToList();
				if (live.Count == 0) continue;

				bool any = live.Any(p => want.ContainsKey(p.ServiceId));
				if (!any) continue;

				var map = ReadLcnMap(desMgr, t.Identity);
				Console.WriteLine("\n=== {0} ts_id={1} ===", t.TsName, t.TsId);
				foreach (var p in live)
				{
					int oldLcn;
					map.TryGetValue(p.ServiceId, out oldLcn);
					int newLcn;
					if (want.TryGetValue(p.ServiceId, out newLcn))
					{
						Console.WriteLine("  sid={0} '{1}' LCN {2} -> {3}", p.ServiceId, p.ProgramName, oldLcn, newLcn);
						map[p.ServiceId] = newLcn;
					}
					else if (!map.ContainsKey(p.ServiceId))
					{
						// keep missing as-is only if not in want; TS_506 should have all in want
						Console.WriteLine("  WARN sid={0} '{1}' has no LCN (unchanged)", p.ServiceId, p.ProgramName);
					}
				}

				var pairs = new List<KeyValuePair<int, int>>();
				foreach (var p in live)
				{
					int lcn;
					if (!map.TryGetValue(p.ServiceId, out lcn))
						lcn = p.ServiceId; // fallback should not happen for TS_506
					pairs.Add(new KeyValuePair<int, int>(p.ServiceId, lcn));
				}
				WriteLcn(desMgr, t.Identity, pairs);
				touched.Add(t.TsName);
			}

			// Verify network: no duplicate among assigned LCNs for our targets
			var occupied = new Dictionary<int, string>();
			foreach (var t in allTs)
			{
				var m = ReadLcnMap(desMgr, t.Identity);
				var live = prgMgr.GetByTSIdentity(t.Identity).ToList();
				foreach (var p in live)
				{
					int lcn;
					if (!m.TryGetValue(p.ServiceId, out lcn)) continue;
					string who = string.Format("{0} SID={1} {2}", t.TsName, p.ServiceId, p.ProgramName);
					if (occupied.ContainsKey(lcn) && want.ContainsValue(lcn))
					{
						if (occupied[lcn] != who)
							Console.WriteLine("CLASH LCN {0}: {1} | {2}", lcn, occupied[lcn], who);
					}
					occupied[lcn] = who;
				}
			}

			Console.WriteLine("\n=== Telugu GEC window 138-149 ===");
			for (int i = 138; i <= 149; i++)
			{
				if (occupied.ContainsKey(i))
					Console.WriteLine("  {0} {1}", i, occupied[i]);
				else
					Console.WriteLine("  {0} FREE", i);
			}
			Console.WriteLine("Kids 491: {0}", occupied.ContainsKey(491) ? occupied[491] : "FREE");

			int n = new SendNoticesManager().SetAllNoticeOn();
			Console.WriteLine("\nTouched TS: {0}", string.Join(", ", touched.OrderBy(x => x)));
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
