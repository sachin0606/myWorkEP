using System;
using CryptManage;

namespace DeXin.EPG.Tools
{
	public class RegistInfo
	{
		private Version _Version;

		private DateTime _FirstRunTime;

		private bool _TrialVersion;

		private int _AccreditDays;

		public Version Version
		{
			get
			{
				return _Version;
			}
			set
			{
				_Version = value;
			}
		}

		public DateTime FirstRunTime
		{
			get
			{
				return _FirstRunTime;
			}
			set
			{
				_FirstRunTime = value;
			}
		}

		public bool TrialVersion
		{
			get
			{
				return _TrialVersion;
			}
			set
			{
				_TrialVersion = value;
			}
		}

		public int AccreditDays
		{
			get
			{
				return _AccreditDays;
			}
			set
			{
				_AccreditDays = value;
			}
		}

		public RegistInfo()
		{
		}

		public RegistInfo(string regString)
		{
			string[] array = CryptionManage.Instance.DeCrypt(regString).Split('|');
			Version = Version.Parse(array[0]);
			FirstRunTime = new DateTime(Convert.ToInt64(array[1]));
			TrialVersion = Convert.ToBoolean(array[2]);
			AccreditDays = Convert.ToInt32(array[3]);
		}

		public override string ToString()
		{
			string unCryptedString = string.Format("{0}|{1}|{2}|{3}|", Version, FirstRunTime.Ticks, TrialVersion, AccreditDays);
			return CryptionManage.Instance.EnCrpty(unCryptedString);
		}

		public override bool Equals(object obj)
		{
			if (obj is RegistInfo)
			{
				RegistInfo registInfo = obj as RegistInfo;
				if (_Version != registInfo.Version)
				{
					return false;
				}
				if (_FirstRunTime != registInfo.FirstRunTime)
				{
					return false;
				}
				if (_TrialVersion != registInfo.TrialVersion)
				{
					return false;
				}
				if (_AccreditDays != registInfo.AccreditDays)
				{
					return false;
				}
				return true;
			}
			return base.Equals(obj);
		}
	}
}
