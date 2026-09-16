using System;
using System.Configuration;
using System.IO;
using Com.Cn.Tool.Model;

namespace Com.Cn.Tool
{
	public class ConfigManager
	{
		public Configuration conManger;

		protected AppSettingsSection appManger;

		protected ConnectionStringsSection csSection;

		private static ConfigManager _Ins = new ConfigManager();

		private LogLevel _logLevel = LogLevel.DEBUG;

		private string _DefaultMessage = "ERROR";

		private string _SystemTitle = "System";

		private string _LogPath = "";

		public static ConfigManager Ins
		{
			get
			{
				if (_Ins == null)
				{
					_Ins = new ConfigManager();
				}
				return _Ins;
			}
		}

		public LogLevel logLevel
		{
			get
			{
				return _logLevel;
			}
		}

		public string DefaultMessage
		{
			get
			{
				if (_DefaultMessage.Length == 0)
				{
					_DefaultMessage = "ERROR";
				}
				return _DefaultMessage;
			}
			set
			{
				_DefaultMessage = value;
			}
		}

		public string SystemTitle
		{
			get
			{
				if (_SystemTitle.Length == 0)
				{
					_SystemTitle = "System";
				}
				return _SystemTitle;
			}
		}

		public string LogPath
		{
			get
			{
				return _LogPath;
			}
		}

		protected ConfigManager()
		{
			try
			{
				conManger = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
				appManger = conManger.AppSettings;
				csSection = conManger.ConnectionStrings;
				Read();
			}
			catch (Exception ex)
			{
				Tool.ShowMessage("ConfigManager " + ex.Message + ex.StackTrace);
			}
		}

		protected void Read()
		{
			_DefaultMessage = Tool.GetLocalizedString("Com.Cn.Tool.ResourceMsg", "Error");
			_SystemTitle = Read(AppConfigKey.SystemTitle);
			CreateLogPath();
			try
			{
				_logLevel = (LogLevel)Enum.Parse(typeof(LogLevel), Read(AppConfigKey.LogLevel), true);
			}
			catch
			{
				_logLevel = LogLevel.ERROR;
			}
		}

		public virtual void Save()
		{
			conManger.Save();
			ConfigurationManager.RefreshSection("appSettings");
			ConfigurationManager.RefreshSection("connectionStrings");
		}

		public string Read(string key)
		{
			try
			{
				return Tool.GetString(appManger.Settings[key].Value);
			}
			catch
			{
				return "";
			}
		}

		public string Read(AppConfigKey key)
		{
			return Read(key.ToString());
		}

		public void Write(AppConfigKey key, string value)
		{
			Write(key.ToString(), value);
		}

		public void Write(string key, string value)
		{
			appManger.Settings[key].Value = value;
		}

		public void CreateLogPath()
		{
			DateTime now = DateTime.Now;
			_LogPath = Read(AppConfigKey.LogPath);
			if (_LogPath.Length == 0)
			{
				_LogPath = Tool.AppPath + "\\log\\" + now.ToString("yyyyMM") + "\\";
			}
			else
			{
				_LogPath = _LogPath + "\\" + now.ToString("yyyyMM") + "\\";
			}
			DirectoryInfo directoryInfo = new DirectoryInfo(_LogPath);
			if (directoryInfo.Exists)
			{
				return;
			}
			try
			{
				directoryInfo.Create();
			}
			catch
			{
				_LogPath = Tool.AppPath + "\\log\\" + now.ToString("yyyyMM") + "\\";
				directoryInfo = new DirectoryInfo(_LogPath);
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
			}
		}
	}
}
