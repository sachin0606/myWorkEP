using System.Diagnostics;
using System.Windows.Forms;

namespace EPGServiceNotify
{
	public class NotifyFormManage
	{
		private static object lockObject = new object();

		public static void StartShowMessageUI(BalloonInfo message)
		{
			lock (lockObject)
			{
				try
				{
					Process[] processesByName = Process.GetProcessesByName("EPGServiceNotify");
					foreach (Process process in processesByName)
					{
						process.WaitForExit();
					}
					if (!Desktop.CreateProcess(Application.StartupPath + "/EPGServiceNotify.exe", Application.StartupPath, string.Format("{0} {1} {2} {3}", message.BalloonType.ToString(), message.Title.Replace(' ', '^'), message.Content.Replace(' ', '^'), message.Timeout)))
					{
						Process.Start(Application.StartupPath + "/EPGServiceNotify.exe", string.Format("{0} {1} {2} {3}", message.BalloonType.ToString(), message.Title.Replace(' ', '^'), message.Content.Replace(' ', '^'), message.Timeout));
					}
				}
				catch
				{
				}
			}
		}
	}
}
