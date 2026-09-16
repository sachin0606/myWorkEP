using System;
using System.Collections.Generic;

namespace Util.Common.ExtensionHelper
{
	public static class EnumHelper
	{
		public static T ToEnum<T>(this byte obj) where T : struct
		{
			return (T)Enum.ToObject(typeof(T), obj);
		}

		public static T ToEnum<T>(this short obj) where T : struct
		{
			return (T)Enum.ToObject(typeof(T), obj);
		}

		public static T ToEnum<T>(this int obj) where T : struct
		{
			return (T)Enum.ToObject(typeof(T), obj);
		}

		public static T ToEnum<T>(this long obj) where T : struct
		{
			return (T)Enum.ToObject(typeof(T), obj);
		}

		public static T ToEnum<T>(this ushort obj) where T : struct
		{
			return (T)Enum.ToObject(typeof(T), obj);
		}

		public static T ToEnum<T>(this uint obj) where T : struct
		{
			return (T)Enum.ToObject(typeof(T), obj);
		}

		public static T ToEnum<T>(this ulong obj) where T : struct
		{
			return (T)Enum.ToObject(typeof(T), obj);
		}

		public static T ToEnum<T>(this string obj, T defaultValue, bool ignoreCase = true) where T : struct
		{
			T result;
			if (obj != null && Enum.TryParse<T>(obj, ignoreCase, out result))
			{
				return result;
			}
			return defaultValue;
		}

		public static IList<KeyValuePair<int, string>> ToIntStringList<T>(Func<T, string> localFun = null) where T : struct
		{
			IList<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>();
			foreach (T value in Enum.GetValues(typeof(T)))
			{
				if (localFun == null)
				{
					list.Add(new KeyValuePair<int, string>(value.ToInt32(), value.ToString()));
				}
				else
				{
					list.Add(new KeyValuePair<int, string>(value.ToInt32(), localFun(value)));
				}
			}
			return list;
		}

		public static IList<KeyValuePair<string, string>> ToStringStringList<T>(Func<string, string> localFun = null) where T : struct
		{
			IList<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
			foreach (T value in Enum.GetValues(typeof(T)))
			{
				if (localFun == null)
				{
					list.Add(new KeyValuePair<string, string>(value.ToString(), value.ToString()));
				}
				else
				{
					list.Add(new KeyValuePair<string, string>(value.ToString(), localFun(value.ToString())));
				}
			}
			return list;
		}

		public static IList<KeyValuePair<T, string>> ToTStringList<T>(Func<T, string> localFun = null) where T : struct
		{
			IList<KeyValuePair<T, string>> list = new List<KeyValuePair<T, string>>();
			foreach (T value in Enum.GetValues(typeof(T)))
			{
				if (localFun == null)
				{
					list.Add(new KeyValuePair<T, string>(value, value.ToString()));
				}
				else
				{
					list.Add(new KeyValuePair<T, string>(value, localFun(value)));
				}
			}
			return list;
		}
	}
}
