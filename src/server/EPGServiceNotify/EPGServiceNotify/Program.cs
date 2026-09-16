using System;
using System.Windows.Forms;

namespace EPGServiceNotify
{
	internal static class Program
	{
		[STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			FormNotifyIcon formNotifyIcon = new FormNotifyIcon();
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			if (commandLineArgs.Length == 4)
			{
				formNotifyIcon.BalloonMessage = new BalloonInfo
				{
					BalloonType = (ToolTipIcon)Enum.Parse(typeof(ToolTipIcon), commandLineArgs[0]),
					Title = commandLineArgs[1].Replace('^', ' '),
					Content = commandLineArgs[2].Replace('^', ' '),
					Timeout = Convert.ToInt32(commandLineArgs[3])
				};
			}
			else if (commandLineArgs.Length == 5)
			{
				formNotifyIcon.BalloonMessage = new BalloonInfo
				{
					BalloonType = (ToolTipIcon)Enum.Parse(typeof(ToolTipIcon), commandLineArgs[1]),
					Title = commandLineArgs[2].Replace('^', ' '),
					Content = commandLineArgs[3].Replace('^', ' '),
					Timeout = Convert.ToInt32(commandLineArgs[4])
				};
			}
			Application.Run(formNotifyIcon);
		}
	}
}
