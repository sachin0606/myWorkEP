using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace EPGWindowsService
{
	[RunInstaller(true)]
	public class ProjectInstaller : Installer
	{
		private IContainer components = null;

		private ServiceProcessInstaller serviceProcessInstaller1;

		private ServiceInstaller serviceInstaller1;

		public ProjectInstaller()
		{
			InitializeComponent();
		}

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
			serviceProcessInstaller1 = new ServiceProcessInstaller();
			serviceInstaller1 = new ServiceInstaller();
			serviceProcessInstaller1.Account = ServiceAccount.LocalSystem;
			serviceProcessInstaller1.Password = null;
			serviceProcessInstaller1.Username = null;
			serviceInstaller1.DelayedAutoStart = true;
			serviceInstaller1.Description = "本服务提供EPG客户端录入数据的监控和采集，并根据EPG客户端指定的配置生成和发送各传输流中所有表的数据";
			serviceInstaller1.DisplayName = "EPG Service";
			serviceInstaller1.ServiceName = "EPGService";
			serviceInstaller1.StartType = ServiceStartMode.Automatic;
			base.Installers.AddRange(new Installer[2] { serviceProcessInstaller1, serviceInstaller1 });
		}
	}
}
