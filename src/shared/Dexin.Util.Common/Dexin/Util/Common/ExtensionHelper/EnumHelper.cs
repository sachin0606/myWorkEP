using System;
using System.Collections.Generic;
using System.Linq;

namespace Dexin.Util.Common.ExtensionHelper
{
	public static class EnumHelper
	{
		public static IEnumerable<T> EnumToList<T>() where T : struct
		{
			return Enum.GetValues(typeof(T)).Cast<T>();
		}

		public static IList<KeyValuePair<string, string>> EnumToList<T>(Func<string, string> localizeFun) where T : struct
		{
			IList<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
			foreach (T value in Enum.GetValues(typeof(T)))
			{
				list.Add(new KeyValuePair<string, string>(value.ToString(), localizeFun(value.ToString())));
			}
			return list;
		}

		public static IList<KeyValuePair<TKey, TValue>> EnumToList<T, TKey, TValue>(Func<T, TKey> funKey, Func<T, TValue> funValue) where T : struct
		{
			IList<KeyValuePair<TKey, TValue>> list = new List<KeyValuePair<TKey, TValue>>();
			foreach (T item in EnumToList<T>())
			{
				list.Add(new KeyValuePair<TKey, TValue>(funKey(item), funValue(item)));
			}
			return list;
		}
	}
}
