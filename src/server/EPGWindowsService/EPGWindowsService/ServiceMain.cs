using System;
using System.ComponentModel;
using System.Globalization;
using System.ServiceProcess;
using System.Threading;
using Com.Cn.Tool;
using CryptManage;
using DeXin.EPG.Tools;
using EPGServiceBLL.SendOut;

namespace EPGWindowsService
{
	internal class ServiceMain : ServiceBase
	{
		private IContainer components = null;

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			components = new Container();
			base.ServiceName = "ServiceMain";
		}

		public ServiceMain()
		{
			InitializeComponent();
			base.CanHandleSessionChangeEvent = true;
			base.CanShutdown = true;
		}

		protected override void OnStart(string[] args)
		{
			Tool.WriteTXTLog(string.Format("Service is starting with {0} args......", args.Length));
			try
			{
				if (args.Length > 0)
				{
					ConfigManager.Ins.Write("DBConnectionString", CryptionManage.Instance.EnCrpty(args[0]));
					if (args.Length >= 8)
					{
						ConfigManager.Ins.Write("EventLoadeDays", args[1]);
						ConfigManager.Ins.Write("SendTSType", args[2]);
						ConfigManager.Ins.Write("TSPlayerRate", args[3]);
						ConfigManager.Ins.Write("Language", args[4]);
						ConfigManager.Ins.Write("LanguageUI", args[5]);
						ConfigManager.Ins.Write("IsAddEmptyPackage", args[6]);
						ConfigManager.Ins.Write("TargetRate", args[7]);
					}
					int package_Count = 7;
					if (args.Length >= 9)
					{
						package_Count = Tool.GetInt32(args[8]);
					}
					ConfigManager.Ins.Write("Pkg_Cnt", package_Count.ToString());
					EPGSettings.Instatce.Package_Count = package_Count;
					ConfigManager.Ins.Save();
				}
				EPGSettings.Instatce.DBConnectionString = CryptionManage.Instance.DeCrypt(ConfigManager.Ins.Read("DBConnectionString"));
				EPGSettings.Instatce.EventLoadeDays = Tool.GetInt32(ConfigManager.Ins.Read("EventLoadeDays"));
				EPGSettings.Instatce.SendTSType = (EnumSendType)Tool.GetInt32(ConfigManager.Ins.Read("SendTSType"));
				EPGSettings.Instatce.TSPlayerRate = Tool.GetInt32(ConfigManager.Ins.Read("TSPlayerRate"));
				EPGSettings.Instatce.IsAddEmptyPackage = bool.Parse(ConfigManager.Ins.Read("IsAddEmptyPackage"));
				EPGSettings.Instatce.TargetRate = decimal.Parse(ConfigManager.Ins.Read("TargetRate"));
				try
				{
					EPGSettings.Instatce.NullPkgPID = ushort.Parse(ConfigManager.Ins.Read("NullPkgPID"));
				}
				catch
				{
				}
				EPGSettings.Instatce.Save();
				Thread.CurrentThread.CurrentCulture = new CultureInfo(ConfigManager.Ins.Read("Language"));
				Thread.CurrentThread.CurrentUICulture = new CultureInfo(ConfigManager.Ins.Read("LanguageUI"));
				TSOutputManager.ServiceExiting += delegate
				{
					Stop();
				};
				TSOutputManager.StartSendTransprotStream();
			}
			catch (Exception ex)
			{
				Tool.WriteTXTLog(ex);
				throw ex;
			}
		}

		protected override void OnStop()
		{
			Tool.WriteTXTLog("Service is stopping......");
			try
			{
				TSOutputManager.StopSendTransprotStream();
			}
			catch
			{
			}
		}

		protected override void OnSessionChange(SessionChangeDescription changeDescription)
		{
			Tool.WriteTXTLog(string.Format("System {0}", changeDescription.Reason));
		}

		protected override void OnShutdown()
		{
			Tool.WriteTXTLog("System Shutdown");
		}
	}
}
