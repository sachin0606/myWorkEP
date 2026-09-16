using System;
using System.Configuration;
using System.Linq;

namespace Util.Common
{
	public class AppConfigManager
	{
		private Configuration _conManger;

		private AppSettingsSection _appManger;

		private ConnectionStringsSection _csManger;

		protected Configuration conManger
		{
			get
			{
				return _conManger;
			}
			set
			{
				_conManger = value;
			}
		}

		protected AppSettingsSection appManger
		{
			get
			{
				return _appManger;
			}
		}

		protected ConnectionStringsSection csManger
		{
			get
			{
				return _csManger;
			}
		}

		public AppConfigManager()
		{
			Refresh();
		}

		public void SetConnectionString(string key, string value, bool isThrow = false)
		{
			if (csManger.ConnectionStrings[key] == null)
			{
				if (isThrow)
				{
					throw new ArgumentNullException("Key");
				}
				csManger.ConnectionStrings.Add(new ConnectionStringSettings(key, value));
			}
			else
			{
				csManger.ConnectionStrings[key].ConnectionString = value;
			}
		}

		public string GetConnectionString(string key, bool isThrow = false)
		{
			if (csManger.ConnectionStrings[key] == null)
			{
				if (isThrow)
				{
					throw new ArgumentNullException("Key");
				}
				return string.Empty;
			}
			return csManger.ConnectionStrings[key].ConnectionString;
		}

		public string GetSetting(string key, bool isThrow = false)
		{
			if (appManger.Settings.AllKeys.Contains(key))
			{
				return appManger.Settings[key].Value;
			}
			if (isThrow)
			{
				throw new ArgumentNullException("Key");
			}
			return string.Empty;
		}

		public void SetSetting(string key, string value, bool isThrow = false)
		{
			if (appManger.Settings.AllKeys.Contains(key))
			{
				appManger.Settings[key].Value = value;
				return;
			}
			if (isThrow)
			{
				throw new ArgumentNullException("Key");
			}
			appManger.Settings.Add(new KeyValueConfigurationElement(key, value));
		}

		public void Save()
		{
			conManger.Save();
			Refresh();
		}

		public void Refresh()
		{
			try
			{
				_conManger = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
				_appManger = conManger.AppSettings;
				_csManger = conManger.ConnectionStrings;
			}
			catch
			{
			}
		}
	}
}
