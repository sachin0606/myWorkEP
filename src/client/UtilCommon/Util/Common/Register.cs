using Microsoft.Win32;

namespace Util.Common
{
	public class Register
	{
		private string _subkey;

		private RegDomain _domain;

		private string _regeditkey;

		public string SubKey
		{
			set
			{
				_subkey = value;
			}
		}

		public RegDomain Domain
		{
			private get
			{
				return _domain;
			}
			set
			{
				_domain = value;
			}
		}

		public string RegeditKey
		{
			private get
			{
				return _regeditkey;
			}
			set
			{
				_regeditkey = value;
			}
		}

		public Register()
		{
			_subkey = "software\\";
			_domain = RegDomain.LocalMachine;
		}

		public Register(string subKey, RegDomain regDomain)
		{
			_subkey = subKey;
			_domain = regDomain;
		}

		public virtual void CreateSubKey()
		{
			if (!(_subkey == string.Empty) && _subkey != null)
			{
				RegistryKey regDomain = GetRegDomain(_domain);
				if (!IsSubKeyExist())
				{
					regDomain.CreateSubKey(_subkey);
				}
				regDomain.Close();
			}
		}

		public virtual void CreateSubKey(string subKey)
		{
			if (!(subKey == string.Empty) && subKey != null)
			{
				RegistryKey regDomain = GetRegDomain(_domain);
				if (!IsSubKeyExist(subKey))
				{
					regDomain.CreateSubKey(subKey);
				}
				regDomain.Close();
			}
		}

		public virtual void CreateSubKey(RegDomain regDomain)
		{
			if (!(_subkey == string.Empty) && _subkey != null)
			{
				RegistryKey regDomain2 = GetRegDomain(regDomain);
				if (!IsSubKeyExist(regDomain))
				{
					regDomain2.CreateSubKey(_subkey);
				}
				regDomain2.Close();
			}
		}

		public virtual void CreateSubKey(string subKey, RegDomain regDomain)
		{
			if (!(subKey == string.Empty) && subKey != null)
			{
				RegistryKey regDomain2 = GetRegDomain(regDomain);
				if (!IsSubKeyExist(subKey, regDomain))
				{
					regDomain2.CreateSubKey(subKey);
				}
				regDomain2.Close();
			}
		}

		public virtual bool IsSubKeyExist()
		{
			if (_subkey == string.Empty || _subkey == null)
			{
				return false;
			}
			RegistryKey registryKey = OpenSubKey(_subkey, _domain);
			if (registryKey == null)
			{
				return false;
			}
			return true;
		}

		public virtual bool IsSubKeyExist(string subKey)
		{
			if (subKey == string.Empty || subKey == null)
			{
				return false;
			}
			RegistryKey registryKey = OpenSubKey(subKey);
			if (registryKey == null)
			{
				return false;
			}
			return true;
		}

		public virtual bool IsSubKeyExist(RegDomain regDomain)
		{
			if (_subkey == string.Empty || _subkey == null)
			{
				return false;
			}
			RegistryKey registryKey = OpenSubKey(_subkey, regDomain);
			if (registryKey == null)
			{
				return false;
			}
			return true;
		}

		public virtual bool IsSubKeyExist(string subKey, RegDomain regDomain)
		{
			if (subKey == string.Empty || subKey == null)
			{
				return false;
			}
			RegistryKey registryKey = OpenSubKey(subKey, regDomain);
			if (registryKey == null)
			{
				return false;
			}
			return true;
		}

		public virtual bool DeleteSubKey()
		{
			bool result = false;
			if (_subkey == string.Empty || _subkey == null)
			{
				return false;
			}
			RegistryKey regDomain = GetRegDomain(_domain);
			if (IsSubKeyExist())
			{
				try
				{
					regDomain.DeleteSubKey(_subkey);
					result = true;
				}
				catch
				{
					result = false;
				}
			}
			regDomain.Close();
			return result;
		}

		public virtual bool DeleteSubKey(string subKey)
		{
			bool result = false;
			if (subKey == string.Empty || subKey == null)
			{
				return false;
			}
			RegistryKey regDomain = GetRegDomain(_domain);
			if (IsSubKeyExist())
			{
				try
				{
					regDomain.DeleteSubKey(subKey);
					result = true;
				}
				catch
				{
					result = false;
				}
			}
			regDomain.Close();
			return result;
		}

		public virtual bool DeleteSubKey(string subKey, RegDomain regDomain)
		{
			bool result = false;
			if (subKey == string.Empty || subKey == null)
			{
				return false;
			}
			RegistryKey regDomain2 = GetRegDomain(regDomain);
			if (IsSubKeyExist(subKey, regDomain))
			{
				try
				{
					regDomain2.DeleteSubKey(subKey);
					result = true;
				}
				catch
				{
					result = false;
				}
			}
			regDomain2.Close();
			return result;
		}

		public virtual bool IsRegeditKeyExist()
		{
			bool result = false;
			if (_regeditkey == string.Empty || _regeditkey == null)
			{
				return false;
			}
			if (IsSubKeyExist())
			{
				RegistryKey registryKey = OpenSubKey();
				string[] valueNames = registryKey.GetValueNames();
				string[] array = valueNames;
				foreach (string strA in array)
				{
					if (string.Compare(strA, _regeditkey, true) == 0)
					{
						result = true;
						break;
					}
				}
				registryKey.Close();
			}
			return result;
		}

		public virtual bool IsRegeditKeyExist(string name)
		{
			bool result = false;
			if (name == string.Empty || name == null)
			{
				return false;
			}
			if (IsSubKeyExist())
			{
				RegistryKey registryKey = OpenSubKey();
				string[] valueNames = registryKey.GetValueNames();
				string[] array = valueNames;
				foreach (string strA in array)
				{
					if (string.Compare(strA, name, true) == 0)
					{
						result = true;
						break;
					}
				}
				registryKey.Close();
			}
			return result;
		}

		public virtual bool IsRegeditKeyExist(string name, string subKey)
		{
			bool result = false;
			if (name == string.Empty || name == null)
			{
				return false;
			}
			if (IsSubKeyExist())
			{
				RegistryKey registryKey = OpenSubKey(subKey);
				string[] valueNames = registryKey.GetValueNames();
				string[] array = valueNames;
				foreach (string strA in array)
				{
					if (string.Compare(strA, name, true) == 0)
					{
						result = true;
						break;
					}
				}
				registryKey.Close();
			}
			return result;
		}

		public virtual bool IsRegeditKeyExist(string name, string subKey, RegDomain regDomain)
		{
			bool result = false;
			if (name == string.Empty || name == null)
			{
				return false;
			}
			if (IsSubKeyExist())
			{
				RegistryKey registryKey = OpenSubKey(subKey, regDomain);
				string[] valueNames = registryKey.GetValueNames();
				string[] array = valueNames;
				foreach (string strA in array)
				{
					if (string.Compare(strA, name, true) == 0)
					{
						result = true;
						break;
					}
				}
				registryKey.Close();
			}
			return result;
		}

		public virtual bool WriteRegeditKey(object content)
		{
			bool result = false;
			if (_regeditkey == string.Empty || _regeditkey == null)
			{
				return false;
			}
			if (!IsSubKeyExist(_subkey))
			{
				CreateSubKey(_subkey);
			}
			RegistryKey registryKey = OpenSubKey(true);
			if (registryKey == null)
			{
				return false;
			}
			try
			{
				registryKey.SetValue(_regeditkey, content);
				result = true;
			}
			catch
			{
				result = false;
			}
			finally
			{
				registryKey.Close();
			}
			return result;
		}

		public virtual bool WriteRegeditKey(string name, object content)
		{
			bool result = false;
			if (name == string.Empty || name == null)
			{
				return false;
			}
			if (!IsSubKeyExist(_subkey))
			{
				CreateSubKey(_subkey);
			}
			RegistryKey registryKey = OpenSubKey(true);
			if (registryKey == null)
			{
				return false;
			}
			try
			{
				registryKey.SetValue(name, content);
				result = true;
			}
			catch
			{
				result = false;
			}
			finally
			{
				registryKey.Close();
			}
			return result;
		}

		public virtual bool WriteRegeditKey(string name, object content, RegValueKind regValueKind)
		{
			bool result = false;
			if (name == string.Empty || name == null)
			{
				return false;
			}
			if (!IsSubKeyExist(_subkey))
			{
				CreateSubKey(_subkey);
			}
			RegistryKey registryKey = OpenSubKey(true);
			if (registryKey == null)
			{
				return false;
			}
			try
			{
				registryKey.SetValue(name, content, GetRegValueKind(regValueKind));
				result = true;
			}
			catch
			{
				result = false;
			}
			finally
			{
				registryKey.Close();
			}
			return result;
		}

		public virtual object ReadRegeditKey()
		{
			object result = null;
			if (_regeditkey == string.Empty || _regeditkey == null)
			{
				return null;
			}
			if (IsRegeditKeyExist(_regeditkey))
			{
				RegistryKey registryKey = OpenSubKey();
				if (registryKey != null)
				{
					result = registryKey.GetValue(_regeditkey);
				}
				registryKey.Close();
			}
			return result;
		}

		public virtual object ReadRegeditKey(string name)
		{
			object result = null;
			if (name == string.Empty || name == null)
			{
				return null;
			}
			if (IsRegeditKeyExist(name))
			{
				RegistryKey registryKey = OpenSubKey();
				if (registryKey != null)
				{
					result = registryKey.GetValue(name);
				}
				registryKey.Close();
			}
			return result;
		}

		public virtual object ReadRegeditKey(string name, string subKey)
		{
			object result = null;
			if (name == string.Empty || name == null)
			{
				return null;
			}
			if (IsRegeditKeyExist(name))
			{
				RegistryKey registryKey = OpenSubKey(subKey);
				if (registryKey != null)
				{
					result = registryKey.GetValue(name);
				}
				registryKey.Close();
			}
			return result;
		}

		public virtual object ReadRegeditKey(string name, string subKey, RegDomain regDomain)
		{
			object result = null;
			if (name == string.Empty || name == null)
			{
				return null;
			}
			if (IsRegeditKeyExist(name))
			{
				RegistryKey registryKey = OpenSubKey(subKey, regDomain);
				if (registryKey != null)
				{
					result = registryKey.GetValue(name);
				}
				registryKey.Close();
			}
			return result;
		}

		public virtual bool DeleteRegeditKey()
		{
			bool result = false;
			if (_regeditkey == string.Empty || _regeditkey == null)
			{
				return false;
			}
			if (IsRegeditKeyExist(_regeditkey))
			{
				RegistryKey registryKey = OpenSubKey(true);
				if (registryKey != null)
				{
					try
					{
						registryKey.DeleteValue(_regeditkey);
						result = true;
					}
					catch
					{
						result = false;
					}
					finally
					{
						registryKey.Close();
					}
				}
			}
			return result;
		}

		public virtual bool DeleteRegeditKey(string name)
		{
			bool result = false;
			if (name == string.Empty || name == null)
			{
				return false;
			}
			if (IsRegeditKeyExist(name))
			{
				RegistryKey registryKey = OpenSubKey(true);
				if (registryKey != null)
				{
					try
					{
						registryKey.DeleteValue(name);
						result = true;
					}
					catch
					{
						result = false;
					}
					finally
					{
						registryKey.Close();
					}
				}
			}
			return result;
		}

		public virtual bool DeleteRegeditKey(string name, string subKey)
		{
			bool result = false;
			if (name == string.Empty || name == null || subKey == string.Empty || subKey == null)
			{
				return false;
			}
			if (IsRegeditKeyExist(name))
			{
				RegistryKey registryKey = OpenSubKey(subKey, true);
				if (registryKey != null)
				{
					try
					{
						registryKey.DeleteValue(name);
						result = true;
					}
					catch
					{
						result = false;
					}
					finally
					{
						registryKey.Close();
					}
				}
			}
			return result;
		}

		public virtual bool DeleteRegeditKey(string name, string subKey, RegDomain regDomain)
		{
			bool result = false;
			if (name == string.Empty || name == null || subKey == string.Empty || subKey == null)
			{
				return false;
			}
			if (IsRegeditKeyExist(name))
			{
				RegistryKey registryKey = OpenSubKey(subKey, regDomain, true);
				if (registryKey != null)
				{
					try
					{
						registryKey.DeleteValue(name);
						result = true;
					}
					catch
					{
						result = false;
					}
					finally
					{
						registryKey.Close();
					}
				}
			}
			return result;
		}

		protected RegistryKey GetRegDomain(RegDomain regDomain)
		{
			switch (regDomain)
			{
			case RegDomain.ClassesRoot:
				return Registry.ClassesRoot;
			case RegDomain.CurrentUser:
				return Registry.CurrentUser;
			case RegDomain.LocalMachine:
				return Registry.LocalMachine;
			case RegDomain.User:
				return Registry.Users;
			case RegDomain.CurrentConfig:
				return Registry.CurrentConfig;
			case RegDomain.DynDa:
				return Registry.DynData;
			case RegDomain.PerformanceData:
				return Registry.PerformanceData;
			default:
				return Registry.LocalMachine;
			}
		}

		protected RegistryValueKind GetRegValueKind(RegValueKind regValueKind)
		{
			switch (regValueKind)
			{
			case RegValueKind.Unknown:
				return RegistryValueKind.Unknown;
			case RegValueKind.String:
				return RegistryValueKind.String;
			case RegValueKind.ExpandString:
				return RegistryValueKind.ExpandString;
			case RegValueKind.Binary:
				return RegistryValueKind.Binary;
			case RegValueKind.DWord:
				return RegistryValueKind.DWord;
			case RegValueKind.MultiString:
				return RegistryValueKind.MultiString;
			case RegValueKind.QWord:
				return RegistryValueKind.QWord;
			default:
				return RegistryValueKind.String;
			}
		}

		protected virtual RegistryKey OpenSubKey()
		{
			if (_subkey == string.Empty || _subkey == null)
			{
				return null;
			}
			RegistryKey registryKey = null;
			using (RegistryKey registryKey2 = GetRegDomain(_domain))
			{
				registryKey = registryKey2.OpenSubKey(_subkey);
				registryKey2.Close();
				return registryKey;
			}
		}

		protected virtual RegistryKey OpenSubKey(bool writable)
		{
			if (_subkey == string.Empty || _subkey == null)
			{
				return null;
			}
			RegistryKey regDomain = GetRegDomain(_domain);
			RegistryKey registryKey = null;
			registryKey = regDomain.OpenSubKey(_subkey, writable);
			regDomain.Close();
			return registryKey;
		}

		protected virtual RegistryKey OpenSubKey(string subKey)
		{
			if (subKey == string.Empty || subKey == null)
			{
				return null;
			}
			RegistryKey regDomain = GetRegDomain(_domain);
			RegistryKey registryKey = null;
			registryKey = regDomain.OpenSubKey(subKey);
			regDomain.Close();
			return registryKey;
		}

		protected virtual RegistryKey OpenSubKey(string subKey, bool writable)
		{
			if (subKey == string.Empty || subKey == null)
			{
				return null;
			}
			RegistryKey regDomain = GetRegDomain(_domain);
			RegistryKey registryKey = null;
			registryKey = regDomain.OpenSubKey(subKey, writable);
			regDomain.Close();
			return registryKey;
		}

		protected virtual RegistryKey OpenSubKey(string subKey, RegDomain regDomain)
		{
			if (subKey == string.Empty || subKey == null)
			{
				return null;
			}
			RegistryKey regDomain2 = GetRegDomain(regDomain);
			RegistryKey registryKey = null;
			registryKey = regDomain2.OpenSubKey(subKey);
			regDomain2.Close();
			return registryKey;
		}

		protected virtual RegistryKey OpenSubKey(string subKey, RegDomain regDomain, bool writable)
		{
			if (subKey == string.Empty || subKey == null)
			{
				return null;
			}
			RegistryKey regDomain2 = GetRegDomain(regDomain);
			RegistryKey registryKey = null;
			registryKey = regDomain2.OpenSubKey(subKey, writable);
			regDomain2.Close();
			return registryKey;
		}
	}
}
