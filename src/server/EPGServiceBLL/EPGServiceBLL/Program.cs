using System;
using System.Threading;
using EPGServiceBLL.Configuration;

namespace EPGServiceBLL
{
	internal static class Program
	{
		[STAThread]
		private static void Main()
		{
			using (EPGWindowsFolderWatcher ePGWindowsFolderWatcher = new EPGWindowsFolderWatcher())
			{
				string folderpath = "F:\\d";
				ePGWindowsFolderWatcher.Start(folderpath);
				int num = 0;
				while (num++ < 20000)
				{
					Console.WriteLine("fffffffffffff :{0}", num);
					EPGWindowsFolderWatcher.WatcherFile watcherFile = ePGWindowsFolderWatcher.Get();
					Console.WriteLine("file :{0}--{1}", watcherFile.Name, watcherFile.Path);
					Thread.Sleep(50);
				}
			}
			Console.ReadLine();
		}
	}
}
