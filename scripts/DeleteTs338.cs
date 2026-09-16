using System;
using System.Linq;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Tools;

internal static class DeleteTs338
{
	public static int Main()
	{
		try
		{
			SqlHelper.SetConnectionString("Data Source=localhost;Initial Catalog=EPG2;Integrated Security=True");
			var loc = LocationControl.Ins;
			var tsMgr = new TSInfoManager();
			var nit = new NITInfoManager().GetAllNITInfos()[0];
			var all = tsMgr.GetTSInfoByNetWorkIdentity(nit.Identity);
			var ts = all.FirstOrDefault(t => t.TsName == "TS_338" || t.TsId == 31);
			if (ts == null)
			{
				Console.WriteLine("TS_338 already gone");
				return 0;
			}
			if (ts.TsId == 22)
			{
				Console.WriteLine("REFUSE ts_id 22");
				return 2;
			}
			Console.WriteLine("Deleting {0} ts_id={1} table={2}", ts.TsName, ts.TsId, ts.Identity);
			int n = tsMgr.DeleteTSInfo(ts.Identity);
			Console.WriteLine("DeleteTSInfo returned {0}", n);
			int on = new SendNoticesManager().SetAllNoticeOn();
			Console.WriteLine("SetAllNoticeOn {0}", on);
			var after = tsMgr.GetTSInfoByNetWorkIdentity(nit.Identity);
			Console.WriteLine("remaining TS_338 count={0}", after.Count(t => t.TsName == "TS_338" || t.TsId == 31));
			return 0;
		}
		catch (Exception ex)
		{
			Console.WriteLine("ERROR: " + ex);
			return 1;
		}
	}
}
