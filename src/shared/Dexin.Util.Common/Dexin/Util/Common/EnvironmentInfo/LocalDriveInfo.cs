using System.IO;
using System.Linq;

namespace Dexin.Util.Common.EnvironmentInfo
{
	public static class LocalDriveInfo
	{
		public static DriveInfo GetMaxFreeDriver()
		{
			DriveInfo driveInfo = DriveInfo.GetDrives().First((DriveInfo x) => x.IsReady);
			foreach (DriveInfo item in from x in DriveInfo.GetDrives()
				where x.IsReady
				select x)
			{
				if (item.AvailableFreeSpace > driveInfo.AvailableFreeSpace)
				{
					driveInfo = item;
				}
			}
			return driveInfo;
		}

		public static DriveInfo GetDriver(string directorName)
		{
			foreach (DriveInfo item in from x in DriveInfo.GetDrives()
				where x.IsReady
				select x)
			{
				if (Directory.Exists(item.Name + directorName))
				{
					return item;
				}
			}
			return null;
		}
	}
}
