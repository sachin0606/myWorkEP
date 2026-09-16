using System;
using System.Linq;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Model;
using DeXin.EPG.Tools;

internal static class FixTs618SendOrNot
{
	public static int Main()
	{
		try
		{
			SqlHelper.SetConnectionString("Data Source=localhost;Initial Catalog=EPG2;Integrated Security=True");
			var loc = LocationControl.Ins;

			var tsMgr = new TSInfoManager();
			var ts = tsMgr.GetTSInfoByNetWorkIdentity(new NITInfoManager().GetAllNITInfos()[0].Identity)
				.FirstOrDefault(t => t.TsName == "TS_618");
			if (ts == null)
				throw new Exception("TS_618 missing");
			Console.WriteLine("TS_618 ts_id={0} table={1}", ts.TsId, ts.Identity);

			var bll = new SendOrNotInfoBLL();
			var rows = bll.GetDataByTSTableID(ts.Identity);
			if (rows == null || rows.Count == 0)
				throw new Exception("send_or_not row missing for TS_618");
			var row = rows[0];
			Console.WriteLine("BEFORE pat={0} cat={1} pmt={2} sdt_act={3} nit_act={4} eit_pf={5} bat={6}",
				row.Pat, row.Cat, row.Pmt, row.SdtAct, row.NitAct, row.EitActPf, row.Bat);

			// Match other Data2 TS: QAM owns PAT/PMT/CAT; EPG sends SDT/NIT/EIT/BAT
			row.Pat = 0;
			row.Cat = 0;
			row.Pmt = 0;
			row.SdtAct = 1;
			row.SdtOth = 1;
			row.NitAct = 1;
			row.NitOth = 1;
			row.Tdt = 1;
			row.Tot = 1;
			row.EitActPf = 1;
			row.EitActSc = 1;
			// Keep other-EIT off like TS_506 (0), not the previous all-on mess
			row.EitOthPf = 0;
			row.EitOthSc = 0;
			row.Bat = 1;

			int n = bll.Update(row);
			Console.WriteLine("Update returned {0}", n);

			var after = bll.GetDataByTSTableID(ts.Identity)[0];
			Console.WriteLine("AFTER pat={0} cat={1} pmt={2} sdt_act={3} nit_act={4} eit_pf={5} eit_oth_pf={6} bat={7}",
				after.Pat, after.Cat, after.Pmt, after.SdtAct, after.NitAct, after.EitActPf, after.EitOthPf, after.Bat);

			int notices = new SendNoticesManager().SetAllNoticeOn();
			Console.WriteLine("SetAllNoticeOn {0}", notices);
			return 0;
		}
		catch (Exception ex)
		{
			Console.WriteLine("ERROR: " + ex);
			return 1;
		}
	}
}
