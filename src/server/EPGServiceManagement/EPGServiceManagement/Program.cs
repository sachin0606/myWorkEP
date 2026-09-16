using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using Com.Cn.Tool;
using DeXin.EPG.Resource;
using DeXin.EPG.Tools;

namespace EPGServiceManagement
{
	internal static class Program
	{
		[STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			bool createdNew;
			Mutex mutex = new Mutex(true, Application.ProductName, out createdNew);
			if (createdNew)
			{
				Application.ThreadException += delegate(object sender, ThreadExceptionEventArgs e)
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
				};
				object instatce = EPGSettings.Instatce;
				EPGConfigurationManger instance = EPGConfigurationManger.Instance;
				if (!string.IsNullOrEmpty(instance.Language))
				{
					Thread.CurrentThread.CurrentCulture = new CultureInfo(instance.Language);
				}
				if (!string.IsNullOrEmpty(instance.LanguageUI))
				{
					Thread.CurrentThread.CurrentUICulture = new CultureInfo(instance.LanguageUI);
				}
				Application.Run(new FormManagement());
				mutex.ReleaseMutex();
			}
			else
			{
				Tool.ShowMessage(ResourceManagement.GetLocationString("CannotStartupMultiClient"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				Application.Exit();
			}
		}
	}
}
