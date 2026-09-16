using System;
using System.Data.SqlClient;
using System.Threading;
using System.Windows.Forms;
using Com.Cn.Tool;
using DeXin.EPG.Resource;
using DeXin.EPG.Tools;

namespace DeXin.EPG.Views.BaseForm
{
	internal static class Program
	{
		[STAThread]
		private static void Main()
		{
			try
			{
				Thread.CurrentThread.CurrentUICulture = ThreadInfo.Ins.UIcullureInfo;
				Thread.CurrentThread.CurrentCulture = ThreadInfo.Ins.cullureInfo;
				Application.SetCompatibleTextRenderingDefault(false);
				bool createdNew;
				Mutex mutex = new Mutex(true, Application.ProductName, out createdNew);
				if (createdNew)
				{
					Application.ThreadException += Application_ThreadException;
					if (!EPGRegistor.CheckRegistClient())
					{
						Tool.ShowMessage(ResourceManagement.GetLocationString("SoftOutOfDate"));
						return;
					}
					Application.Run(new FormMain());
					mutex.ReleaseMutex();
				}
				else
				{
					Tool.ShowMessage(ResourceManagement.GetLocationString("CannotStartupMultiClient"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					Application.Exit();
				}
			}
			catch (Exception ex)
			{
				Application_ThreadException(ex, new ThreadExceptionEventArgs(ex));
				Application.Exit();
			}
		}

		private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			if (e.Exception is SqlException)
			{
				Tool.ShowMessage(ResourceManagement.GetLocationString("DatabaseConnectionException"));
			}
			else if (e.Exception is EPGRegistorException)
			{
				Tool.ShowMessage(e.Exception.Message);
			}
			else
			{
				Tool.ShowMessage(e.Exception);
			}
		}
	}
}
