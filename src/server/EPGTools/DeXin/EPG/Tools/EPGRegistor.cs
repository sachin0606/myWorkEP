using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using DeXin.EPG.Tools.RegistryManager;

namespace DeXin.EPG.Tools
{
	public class EPGRegistor
	{
		protected const bool ISTRAILVERSION = false;

		protected const int TRAILVERSIONDAYS = 30;

		public const bool ISNOEITVERSION = false;

		public static Version VERSION_CLIENT
		{
			get
			{
				try
				{
					return Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory + "EPG2.0.exe").GetName().Version;
				}
				catch
				{
					throw new EPGRegistorException("Obtain client software version failed, Please contact for more information");
				}
			}
		}

		public static Version VERSION_SERVER
		{
			get
			{
				try
				{
					return Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory + "EPGWindowsService.exe").GetName().Version;
				}
				catch
				{
					throw new EPGRegistorException("Obtain server software version failed, Please contact for more information");
				}
			}
		}

		private static string clientRegistPath
		{
			get
			{
				return string.Format("{0}\\CDXBoot{1}.ini", Environment.SystemDirectory, VERSION_CLIENT);
			}
		}

		private static string serverRegistPath
		{
			get
			{
				return string.Format("{0}\\SDXBoot{1}.ini", Environment.SystemDirectory, VERSION_SERVER);
			}
		}

		protected static bool CheckRegist(RegistInfo regInfo)
		{
			if (regInfo.TrialVersion && (regInfo.FirstRunTime > DateTime.UtcNow || regInfo.FirstRunTime.AddDays(30.0) < DateTime.UtcNow))
			{
				return false;
			}
			return true;
		}

		private static void SetRegist(RegistInfo regInfo, string regPath)
		{
			try
			{
				using (StreamWriter streamWriter = new StreamWriter(regPath, false, Encoding.UTF8))
				{
					streamWriter.WriteLine(regInfo.ToString());
					streamWriter.AutoFlush = true;
				}
			}
			catch (UnauthorizedAccessException)
			{
				throw new EPGRegistorException("write regist information error, please run application as administrator");
			}
			catch
			{
				throw new EPGRegistorException();
			}
		}

		private static RegistInfo GetRegist(string regPath)
		{
			try
			{
				if (!File.Exists(regPath))
				{
					return null;
				}
				string regString = File.ReadLines(regPath, Encoding.UTF8).First();
				return new RegistInfo(regString);
			}
			catch (UnauthorizedAccessException)
			{
				throw new EPGRegistorException("read regist information error, please run application as administrator");
			}
			catch
			{
				throw new EPGRegistorException();
			}
		}

		private static RegistInfo GetRegist(Register regedit, string regFilePaht, Version softVer)
		{
			RegistInfo registInfo = GetRegist(regFilePaht);
			RegistInfo registInfo2 = null;
			if (regedit.IsRegeditKeyExist("sub"))
			{
				registInfo2 = new RegistInfo(regedit.ReadRegeditKey("sub").ToString());
				if (registInfo == null)
				{
					registInfo = registInfo2;
					SetRegist(registInfo, regFilePaht);
				}
			}
			else if (registInfo != null)
			{
				registInfo2 = registInfo;
				regedit.WriteRegeditKey("sub", registInfo2.ToString());
				regedit.WriteRegeditKey("FIRST_RUN", registInfo2.FirstRunTime.ToString());
			}
			else
			{
				RegistInfo registInfo3 = new RegistInfo();
				registInfo3.Version = softVer;
				registInfo3.FirstRunTime = DateTime.UtcNow;
				registInfo3.TrialVersion = false;
				registInfo3.AccreditDays = 30;
				RegistInfo regInfo = registInfo3;
				SetRegist(regInfo, regFilePaht);
				registInfo2 = (registInfo = GetRegist(regFilePaht));
				regedit.WriteRegeditKey("sub", registInfo2.ToString());
				regedit.WriteRegeditKey("FIRST_RUN", registInfo2.FirstRunTime.ToString());
			}
			if (!registInfo.Equals(registInfo2))
			{
				throw new EPGRegistorException("regist infomation is destoried!");
			}
			if (registInfo2.TrialVersion)
			{
				registInfo.TrialVersion = false;
				SetRegist(registInfo, regFilePaht);
				registInfo2 = GetRegist(regFilePaht);
				regedit.WriteRegeditKey("sub", registInfo2.ToString());
				if (!regedit.IsRegeditKeyExist("FIRST_RUN"))
				{
					regedit.WriteRegeditKey("FIRST_RUN", registInfo2.FirstRunTime.ToString());
				}
			}
			return registInfo2;
		}

		public static RegistInfo GetRegistC()
		{
			Register regedit = new Register("SOFTWARE\\epg2\\client\\" + VERSION_CLIENT.ToString(), RegDomains.LocalMachine);
			return GetRegist(regedit, clientRegistPath, VERSION_CLIENT);
		}

		public static RegistInfo GetRegistS()
		{
			Register regedit = new Register("SOFTWARE\\epg2\\server\\" + VERSION_SERVER.ToString(), RegDomains.LocalMachine);
			return GetRegist(regedit, serverRegistPath, VERSION_SERVER);
		}

		public static bool CheckRegistClient()
		{
			RegistInfo registC = GetRegistC();
			return CheckRegist(registC);
		}

		public static bool CheckRegistServer()
		{
			RegistInfo registS = GetRegistS();
			return CheckRegist(registS);
		}
	}
}
