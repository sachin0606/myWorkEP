using System;
using System.Threading;
using System.Windows.Forms;

namespace TsSenders
{
	internal static class Program
	{
		[STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.ThreadException += delegate(object obj, ThreadExceptionEventArgs e)
			{
				if (!string.IsNullOrEmpty(e.Exception.Message))
				{
					MessageBox.Show(e.Exception.Message);
				}
			};
			Application.Run(new FormSender());
		}
	}
}
