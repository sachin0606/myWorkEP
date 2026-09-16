using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace Util.Common.ExtensionHelper
{
	public static class ObjectHelper
	{
		public static string ToStr(this string obj, string defaultValue = "")
		{
			if (obj != null)
			{
				return obj;
			}
			return defaultValue;
		}

		public static string ToStr(this object obj, string defaultValue = "")
		{
			if (obj != null)
			{
				return obj.ToString();
			}
			return defaultValue;
		}

		public static bool IsNullOrEmpty(this string obj)
		{
			return string.IsNullOrEmpty(obj);
		}

		public static DateTime ToDateTime(this string obj)
		{
			return DateTime.Parse(obj);
		}

		public static DateTime ToDateTime(this object obj)
		{
			return Convert.ToDateTime(obj);
		}

		public static DateTime ToDateTime(this string obj, DateTime defaultVal)
		{
			DateTime result = DateTime.Now;
			if (!DateTime.TryParse(obj, out result))
			{
				return defaultVal;
			}
			return result;
		}

		public static DateTime ToDateTime(this object obj, DateTime defaultVal)
		{
			try
			{
				return Convert.ToDateTime(obj);
			}
			catch
			{
				return defaultVal;
			}
		}

		public static bool ToBoolean(this object obj, bool defaultValue = false)
		{
			bool flag = defaultValue;
			try
			{
				return Convert.ToBoolean(obj);
			}
			catch
			{
				return defaultValue;
			}
		}

		public static byte[] ToBytes(this object obj)
		{
			return (byte[])obj;
		}

		public static byte ToInt8(this string obj, byte defaultValue = 0, NumberStyle numberStyle = NumberStyle.Dec)
		{
			byte result = defaultValue;
			if (!obj.IsNullOrEmpty())
			{
				try
				{
					result = Convert.ToByte(obj, (int)numberStyle);
				}
				catch
				{
					result = defaultValue;
				}
			}
			return result;
		}

		public static short ToInt16(this string obj, short defaultValue = 0, NumberStyle numberStyle = NumberStyle.Dec)
		{
			short result = defaultValue;
			if (!obj.IsNullOrEmpty())
			{
				try
				{
					result = Convert.ToInt16(obj, (int)numberStyle);
				}
				catch
				{
					result = defaultValue;
				}
			}
			return result;
		}

		public static short ToInt16(this object obj, short defaultValue = 0)
		{
			short result = defaultValue;
			if (obj != null)
			{
				try
				{
					result = Convert.ToInt16(obj);
				}
				catch
				{
					result = defaultValue;
				}
			}
			return result;
		}

		public static int ToInt32(this string obj, int defaultValue = 0, NumberStyle numberStyle = NumberStyle.Dec)
		{
			int result = defaultValue;
			if (!obj.IsNullOrEmpty())
			{
				try
				{
					result = Convert.ToInt32(obj, (int)numberStyle);
				}
				catch
				{
					result = defaultValue;
				}
			}
			return result;
		}

		public static int ToInt32(this object obj, int defaultValue = 0)
		{
			int result = defaultValue;
			if (obj != null)
			{
				try
				{
					result = Convert.ToInt32(obj);
				}
				catch
				{
					result = defaultValue;
				}
			}
			return result;
		}

		public static long ToInt64(this string obj, long defaultValue = 0L, NumberStyle numberStyle = NumberStyle.Dec)
		{
			long result = defaultValue;
			if (!obj.IsNullOrEmpty())
			{
				try
				{
					result = Convert.ToInt64(obj, (int)numberStyle);
				}
				catch
				{
					result = defaultValue;
				}
			}
			return result;
		}

		public static long ToInt64(this object obj, long defaultValue = 0L)
		{
			long result = defaultValue;
			if (obj != null)
			{
				try
				{
					result = Convert.ToInt64(obj);
				}
				catch
				{
					result = defaultValue;
				}
			}
			return result;
		}

		public static decimal ToDecimal(this object obj, long defaultValue = 0L)
		{
			decimal result = defaultValue;
			if (obj != null)
			{
				try
				{
					result = Convert.ToDecimal(obj);
				}
				catch
				{
					result = defaultValue;
				}
			}
			return result;
		}

		public static ulong FormatToUInt64(this string str)
		{
			return (ulong)str.FormatToInt64();
		}

		public static long FormatToInt64(this string str)
		{
			NumberStyle numberStyle = NumberStyle.Dec;
			if (str.ToUpper().StartsWith("0X"))
			{
				numberStyle = NumberStyle.Hex;
			}
			return str.ToInt64(0L, numberStyle);
		}

		public static uint FormatToUInt32(this string str)
		{
			return (uint)str.FormatToInt32();
		}

		public static int FormatToInt32(this string str)
		{
			NumberStyle numberStyle = NumberStyle.Dec;
			if (str.ToUpper().StartsWith("0X"))
			{
				numberStyle = NumberStyle.Hex;
			}
			return str.ToInt32(0, numberStyle);
		}

		public static ushort FormatToUInt16(this string str)
		{
			return (ushort)str.FormatToInt64();
		}

		public static short FormatToInt16(this string str)
		{
			NumberStyle numberStyle = NumberStyle.Dec;
			if (str.ToUpper().StartsWith("0X"))
			{
				numberStyle = NumberStyle.Hex;
			}
			return str.ToInt16(0, numberStyle);
		}

		public static byte FormatToInt8(this string str)
		{
			NumberStyle numberStyle = NumberStyle.Dec;
			if (str.ToUpper().StartsWith("0X"))
			{
				numberStyle = NumberStyle.Hex;
			}
			return str.ToInt8(0, numberStyle);
		}

		public static byte ToInt8(this object obj)
		{
			return Convert.ToByte(obj);
		}

		public static ushort ToUInt16(this object obj)
		{
			return Convert.ToUInt16(obj);
		}

		public static uint ToUInt32(this object obj)
		{
			return Convert.ToUInt32(obj);
		}

		public static ulong ToUInt64(this object obj)
		{
			return Convert.ToUInt64(obj);
		}

		public static void AP<T>(this StringBuilder sb, T obj, int count = 1)
		{
			for (int i = 0; i < count; i++)
			{
				sb.Append(obj);
			}
		}

		public static IPAddress ToIPAdress(this string obj, string defaultValue = "127.0.0.1")
		{
			IPAddress address = IPAddress.Parse(defaultValue);
			if (!obj.IsNullOrEmpty() && !IPAddress.TryParse(obj, out address))
			{
				address = IPAddress.Parse(defaultValue);
			}
			return address;
		}

		public static void CopyWith<T, TResource>(this T obj, TResource resource, params string[] names)
		{
			if (resource == null)
			{
				throw new ArgumentNullException("resource");
			}
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (names == null || names.Length == 0)
			{
				throw new ArgumentNullException("obj");
			}
			PropertyInfo[] properties = typeof(T).GetProperties();
			PropertyInfo[] properties2 = typeof(TResource).GetProperties();
			string name;
			for (int i = 0; i < names.Length; i++)
			{
				name = names[i];
				PropertyInfo propertyInfo = properties.FirstOrDefault((PropertyInfo p) => p.Name.Equals(name));
				if (!(propertyInfo == null))
				{
					PropertyInfo propertyInfo2 = properties2.FirstOrDefault((PropertyInfo p) => p.Name.Equals(name));
					if (!(propertyInfo2 == null))
					{
						propertyInfo.SetValue(obj, propertyInfo2.GetValue(resource, null), null);
					}
				}
			}
		}

		public static void CopyWithout<T, TResource>(this T obj, TResource resource, params string[] names)
		{
			if (resource == null)
			{
				throw new ArgumentNullException("resource");
			}
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			PropertyInfo[] properties = typeof(TResource).GetProperties();
			PropertyInfo[] properties2 = typeof(T).GetProperties();
			PropertyInfo p1;
			for (int i = 0; i < properties2.Length; i++)
			{
				p1 = properties2[i];
				if (names != null && names.Length != 0 && !names.Contains(p1.Name))
				{
					PropertyInfo propertyInfo = properties.FirstOrDefault((PropertyInfo propertyInfo2) => propertyInfo2.Name.Equals(p1.Name));
					if (!(propertyInfo == null))
					{
						p1.SetValue(obj, propertyInfo.GetValue(resource, null), null);
					}
				}
			}
		}

		public static T Clone<T>(T t) where T : class, new()
		{
			T val = new T();
			PropertyInfo[] properties = typeof(T).GetProperties();
			foreach (PropertyInfo propertyInfo in properties)
			{
				propertyInfo.SetValue(val, propertyInfo.GetValue(t, null), null);
			}
			return val;
		}

		public static string ArrayToString<T>(this IEnumerable<T> items, string splitChar = "", Func<T, string> fun = null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			int num2 = items.Count();
			foreach (T item in items)
			{
				if (fun == null)
				{
					stringBuilder.Append(item);
				}
				else
				{
					stringBuilder.Append(fun(item));
				}
				if (++num < num2)
				{
					stringBuilder.Append(splitChar);
				}
			}
			return stringBuilder.ToString();
		}

		public static string ShowString(this byte[] items, string splitChar = "")
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			int num2 = items.Count();
			foreach (byte b in items)
			{
				stringBuilder.Append(b.ToString("X2"));
				if (++num < num2)
				{
					stringBuilder.Append(splitChar);
				}
			}
			return stringBuilder.ToString();
		}

		public static string Fmt(this string formatString, params object[] args)
		{
			return string.Format(formatString, args);
		}

		public static bool Eq(this byte[] p1, byte[] p2)
		{
			if (p1 == null && p2 == null)
			{
				return true;
			}
			if (p1 == null || p2 == null)
			{
				return false;
			}
			if (p1.Length == 0 && p2.Length == 0)
			{
				return true;
			}
			if (p1.Length != p2.Length)
			{
				return false;
			}
			for (int i = 0; i < p1.Length; i++)
			{
				if (p1[i] != p2[i])
				{
					return false;
				}
			}
			return true;
		}
	}
}
