using System;
using Microsoft.Win32;

namespace DeXin.EPG.Tools.RegistryManager
{
	public class Register
	{
		private string _subkey;

		private RegDomains _domain;

		private string _regeditkey;

		public string SubKey
		{
			set
			{
				_subkey = value;
			}
		}

		public RegDomains Domain
		{
			set
			{
				_domain = value;
			}
		}

		public string RegeditKey
		{
			set
			{
				_regeditkey = value;
			}
		}

		public Register()
		{
			_subkey = "software\\";
			_domain = RegDomains.LocalMachine;
		}

		public Register(string subKey, RegDomains regDomain)
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
					RegistryKey registryKey = regDomain.CreateSubKey(_subkey);
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
					RegistryKey registryKey = regDomain.CreateSubKey(subKey);
				}
				regDomain.Close();
			}
		}

		public virtual void CreateSubKey(RegDomains regDomain)
		{
			if (!(_subkey == string.Empty) && _subkey != null)
			{
				RegistryKey regDomain2 = GetRegDomain(regDomain);
				if (!IsSubKeyExist(regDomain))
				{
					RegistryKey registryKey = regDomain2.CreateSubKey(_subkey);
				}
				regDomain2.Close();
			}
		}

		public virtual void CreateSubKey(string subKey, RegDomains regDomain)
		{
			if (!(subKey == string.Empty) && subKey != null)
			{
				RegistryKey regDomain2 = GetRegDomain(regDomain);
				if (!IsSubKeyExist(subKey, regDomain))
				{
					RegistryKey registryKey = regDomain2.CreateSubKey(subKey);
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

		public virtual bool IsSubKeyExist(RegDomains regDomain)
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

		public virtual bool IsSubKeyExist(string subKey, RegDomains regDomain)
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

		public virtual bool DeleteSubKey(string subKey, RegDomains regDomain)
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

		public virtual bool IsRegeditKeyExist(string name, string subKey, RegDomains regDomain)
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
			catch (Exception)
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

		public virtual object ReadRegeditKey(string name, string subKey, RegDomains regDomain)
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

		public virtual bool DeleteRegeditKey(string name, string subKey, RegDomains regDomain)
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

		protected RegistryKey GetRegDomain(RegDomains regDomain)
		{
			switch (regDomain)
			{
			case RegDomains.ClassesRoot:
				return Registry.ClassesRoot;
			case RegDomains.CurrentUser:
				return Registry.CurrentUser;
			case RegDomains.LocalMachine:
				return Registry.LocalMachine;
			case RegDomains.User:
				return Registry.Users;
			case RegDomains.CurrentConfig:
				return Registry.CurrentConfig;
			case RegDomains.DynDa:
				return Registry.DynData;
			case RegDomains.PerformanceData:
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
			RegistryKey regDomain = GetRegDomain(_domain);
			RegistryKey registryKey = null;
			registryKey = regDomain.OpenSubKey(_subkey);
			regDomain.Close();
			return registryKey;
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

		protected virtual RegistryKey OpenSubKey(string subKey, RegDomains regDomain)
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

		protected virtual RegistryKey OpenSubKey(string subKey, RegDomains regDomain, bool writable)
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
