using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using Util.Common.ExtensionHelper;

namespace Util.Common
{
	public static class DiskManager
	{
		public static DriveInfo GetMaxFreeDriver()
		{
			DriveInfo driveInfo = DriveInfo.GetDrives().First();
			foreach (DriveInfo item in from p in DriveInfo.GetDrives()
				where p.DriveType == DriveType.Fixed
				select p)
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
			DriveInfo[] drives = DriveInfo.GetDrives();
			foreach (DriveInfo driveInfo in drives)
			{
				if (Directory.Exists(driveInfo.Name + directorName))
				{
					return driveInfo;
				}
			}
			return null;
		}

		public static List<string> GetDiskVolumeSerialNumber()
		{
			List<string> list = new List<string>();
			using (ManagementClass managementClass = new ManagementClass("win32_diskdrive"))
			{
				using (ManagementObjectCollection managementObjectCollection = managementClass.GetInstances())
				{
					foreach (ManagementBaseObject item in managementObjectCollection)
					{
						list.Add(item.Properties["Model"].Value.ToStr());
					}
					return list;
				}
			}
		}

		public static List<string> GetCpuNumber()
		{
			List<string> list = new List<string>();
			using (ManagementClass managementClass = new ManagementClass("win32_processor"))
			{
				using (ManagementObjectCollection managementObjectCollection = managementClass.GetInstances())
				{
					foreach (ManagementBaseObject item in managementObjectCollection)
					{
						list.Add(item.Properties["Processorid"].Value.ToStr());
					}
					return list;
				}
			}
		}

		public static List<string> GetNetworkNumber()
		{
			List<string> list = new List<string>();
			using (ManagementClass managementClass = new ManagementClass("win32_networkadapterconfiguration"))
			{
				using (ManagementObjectCollection managementObjectCollection = managementClass.GetInstances())
				{
					foreach (ManagementBaseObject item in managementObjectCollection)
					{
						if (item.Properties["IPEnabled"].Value.ToBoolean())
						{
							list.Add(item.Properties["MacAddress"].Value.ToStr());
						}
					}
					return list;
				}
			}
		}

		public static string GetDefaultNumber()
		{
			List<string> cpuNumber = GetCpuNumber();
			string text = cpuNumber.ArrayToString();
			return text.Replace(" ", "").Replace(":", "").ToUpper();
		}
	}
}
