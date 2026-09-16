using System;
using System.Linq;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Tools;

internal static class QueueEpgNotices
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
			int n = 0;
			foreach (int tsId in new[] { 33, 34, 35, 36 })
			{
				var ts = tsMgr.GetTSInfoByTSId(tsId).First();
				tsMgr.SaveTSInfo(ts);
				n++;
				Console.WriteLine("queued TS {0} table={1}", ts.TsName, ts.Identity);
				foreach (var ip in ipMgr.GetData().Where(x => x.TsTableId == ts.Identity))
				{
					ipMgr.Update(ip);
					n++;
					Console.WriteLine("  queued multi-ip {0}:{1}", ip.Ip, ip.Port);
				}
				foreach (var des in desMgr.GetDescriptors(EnumPossibleLoaction.NIT_TS, ts.Identity))
				{
					string msg;
					desMgr.ModifyDescriptor(des, out msg);
					n++;
					Console.WriteLine("  queued NIT_TS tag={0} {1}", des.DesTypeTag, msg);
				}
			}
			Console.WriteLine("queued {0} BLL saves; notices left pending (state=0)", n);
			return 0;
		}
		catch (Exception ex)
		{
			Console.WriteLine("ERROR: " + ex);
			return 1;
		}
	}
}
