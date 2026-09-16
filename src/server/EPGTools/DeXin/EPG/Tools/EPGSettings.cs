using System.Configuration;
using Com.Cn.Tool.Model;
using CryptManage;

namespace DeXin.EPG.Tools
{
	public class EPGSettings : ApplicationSettingsBase
	{
		private static EPGSettings _instance = null;

		[DefaultSettingValue("05FFFFFFFFFF73FB9216552E94971A252E494D0F5FF13BFA8114511EB8D94D2B658986116C3669FA5DDA6033BBA3")]
		[UserScopedSetting]
		public string DBConnectionString
		{
			get
			{
				return CryptionManage.Instance.DeCrypt(this["DBConnectionString"].ToString());
			}
			set
			{
				this["DBConnectionString"] = CryptionManage.Instance.EnCrpty(value);
			}
		}

		[UserScopedSetting]
		[DefaultSettingValue("7")]
		public int EventLoadeDays
		{
			get
			{
				return (int)this["EventLoadeDays"];
			}
			set
			{
				this["EventLoadeDays"] = value;
			}
		}

		[UserScopedSetting]
		[DefaultSettingValue("10")]
		public int TSPlayerRate
		{
			get
			{
				return (int)this["TSPlayerRate"];
			}
			set
			{
				this["TSPlayerRate"] = value;
			}
		}

		[UserScopedSetting]
		[DefaultSettingValue("NetworkUDP")]
		public EnumSendType SendTSType
		{
			get
			{
				return (EnumSendType)this["SendTSType"];
			}
			set
			{
				this["SendTSType"] = value;
			}
		}

		[DefaultSettingValue("Other")]
		[UserScopedSetting]
		public EnumUseRegions UseRegion
		{
			get
			{
				return (EnumUseRegions)this["UseRegion"];
			}
			set
			{
				this["UseRegion"] = value;
			}
		}

		[DefaultSettingValue("False")]
		[UserScopedSetting]
		public bool IsAddEmptyPackage
		{
			get
			{
				return (bool)this["IsAddEmptyPackage"];
			}
			set
			{
				this["IsAddEmptyPackage"] = value;
			}
		}

		[UserScopedSetting]
		[DefaultSettingValue("1.0000")]
		public decimal TargetRate
		{
			get
			{
				return (decimal)this["TargetRate"];
			}
			set
			{
				this["TargetRate"] = value;
			}
		}

		[DefaultSettingValue("1")]
		[UserScopedSetting]
		public int Package_Count
		{
			get
			{
				return (int)this["Package_Count"];
			}
			set
			{
				this["Package_Count"] = value;
			}
		}

		[DefaultSettingValue("8191")]
		[UserScopedSetting]
		public ushort NullPkgPID
		{
			get
			{
				return (ushort)this["NullPkgPID"];
			}
			set
			{
				this["NullPkgPID"] = value;
			}
		}

		public static EPGSettings Instatce
		{
			get
			{
				if (_instance == null)
				{
					_instance = new EPGSettings();
				}
				return _instance;
			}
		}

		protected EPGSettings()
		{
			SqlHelper.SetConnectionString(DBConnectionString);
		}

		public override void Save()
		{
			base.Save();
			SqlHelper.SetConnectionString(Instatce.DBConnectionString);
		}
	}
}
