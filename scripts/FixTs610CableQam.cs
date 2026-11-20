using System;
using System.Linq;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Tools;

internal static class FixTs610CableQam
{
	public static int Main()
	{
		try
		{
			SqlHelper.SetConnectionString("Data Source=localhost;Initial Catalog=EPG2;Integrated Security=True");
			var loc = LocationControl.Ins;

			var tsMgr = new TSInfoManager();
			var ts = tsMgr.GetTSInfoByNetWorkIdentity(new NITInfoManager().GetAllNITInfos()[0].Identity)
				.FirstOrDefault(t => t.TsName == "TS_610");
			if (ts == null)
				throw new Exception("TS_610 missing");
			if (ts.TsId == 22)
				throw new Exception("REFUSE ts_id 22");
			Console.WriteLine("TS_610 ts_id={0} table={1}", ts.TsId, ts.Identity);

			var desMgr = new DescriptorManagement();
			var cable = desMgr.GetDescriptors(EnumPossibleLoaction.NIT_TS, ts.Identity)
				.FirstOrDefault(d => d.DesTypeTag == 68);
			if (cable == null || cable.DescriptionData == null || cable.DescriptionData.Length < 7)
				throw new Exception("TS_610 cable descriptor missing");

			byte[] b = (byte[])cable.DescriptionData.Clone();
			Console.WriteLine("BEFORE hex={0} modulation=0x{1:x2}", BitConverter.ToString(b).Replace("-", ""), b[6]);
			if (b[6] == 0x05)
			{
				Console.WriteLine("Already 256-QAM");
				return 0;
			}
			b[6] = 0x05;
			cable.DescriptionData = b;
			cable.DataLength = b.Length;
			string msg;
			if (!desMgr.ModifyDescriptor(cable, out msg))
				throw new Exception("cable modify failed: " + msg);
			Console.WriteLine("AFTER  hex={0} modulation=0x05 (256-QAM) {1}", BitConverter.ToString(b).Replace("-", ""), msg);

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
}
