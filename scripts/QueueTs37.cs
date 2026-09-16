using System;
using System.Linq;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Tools;

internal static class QueueTs37
{
	public static int Main()
	{
		try
		{
			SqlHelper.SetConnectionString("Data Source=localhost;Initial Catalog=EPG2;Integrated Security=True");
			var loc = LocationControl.Ins;
			var tsMgr = new TSInfoManager();
			var ipMgr = new MultiIPOutputBLL();
			var desMgr = new DescriptorManagement();
			var ts = tsMgr.GetTSInfoByTSId(37).First();
			if (ts.TsId == 22) throw new Exception("refusing ts 22");
			tsMgr.SaveTSInfo(ts);
			foreach (var ip in ipMgr.GetData().Where(x => x.TsTableId == ts.Identity))
				ipMgr.Update(ip);
			foreach (var des in desMgr.GetDescriptors(EnumPossibleLoaction.NIT_TS, ts.Identity))
			{
				string msg;
				desMgr.ModifyDescriptor(des, out msg);
			}
			int n = new SendNoticesManager().SetAllNoticeOn();
			Console.WriteLine("queued TS_410 and armed {0} notices", n);
			return 0;
		}
		catch (Exception ex)
		{
			Console.WriteLine("ERROR: " + ex);
			return 1;
		}
	}
}
