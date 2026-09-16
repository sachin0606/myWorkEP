using System.ServiceProcess;

namespace EPGWindowsService
{
	internal static class Program
	{
		private static void Main()
		{
			ServiceBase[] services = new ServiceBase[1]
			{
				new ServiceMain()
			};
			ServiceBase.Run(services);
		}
	}
}
