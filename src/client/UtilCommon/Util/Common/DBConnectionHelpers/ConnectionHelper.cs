using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using Util.Common.ExtensionHelper;

namespace Util.Common.DBConnectionHelpers
{
	public static class ConnectionHelper
	{
		public static IDictionary<PropertyInfo, string> MapWith<T>(params string[] names)
		{
			IDictionary<PropertyInfo, string> maps = new Dictionary<PropertyInfo, string>();
			if (names == null || names.Length == 0)
			{
				return maps;
			}
			typeof(T).GetProperties().ToList().ForEach(delegate(PropertyInfo p)
			{
				if (p.CanWrite && names.Contains(p.Name))
				{
					maps.Add(new KeyValuePair<PropertyInfo, string>(p, p.Name));
				}
			});
			return maps;
		}

		public static IDictionary<PropertyInfo, string> MapWithout<T>(params string[] names)
		{
			IDictionary<PropertyInfo, string> maps = new Dictionary<PropertyInfo, string>();
			typeof(T).GetProperties().ToList().ForEach(delegate(PropertyInfo p)
			{
				if (p.CanWrite && !names.Contains(p.Name))
				{
					maps.Add(new KeyValuePair<PropertyInfo, string>(p, p.Name));
				}
			});
			return maps;
		}

		public static void EnEntity<T>(T temp, IDataReader reader, IDictionary<PropertyInfo, string> maps)
		{
			PropertyInfo[] ps = temp.GetType().GetProperties();
			maps.Keys.ToList().ForEach(delegate(PropertyInfo key)
			{
				if (ps.Contains(key))
				{
					SetValue(temp, key, reader[maps[key]]);
				}
			});
		}

		private static void SetValue<T>(T temp, PropertyInfo property, object value)
		{
			switch (property.PropertyType.Name)
			{
			case "Byte":
				property.SetValue(temp, value.ToStr().ToInt8(0), null);
				break;
			case "Byte[]":
				property.SetValue(temp, value.ToBytes(), null);
				break;
			case "Int16":
				property.SetValue(temp, value.ToStr().ToInt16(0), null);
				break;
			case "Int32":
				property.SetValue(temp, value.ToStr().ToInt32(), null);
				break;
			case "Int64":
				property.SetValue(temp, value.ToStr().ToInt64(0L), null);
				break;
			case "Boolean":
				property.SetValue(temp, value.ToStr().ToBoolean(), null);
				break;
			case "DateTime":
				property.SetValue(temp, value.ToStr().ToDateTime(), null);
				break;
			case "String":
				property.SetValue(temp, value.ToStr(), null);
				break;
			case "IPAddress":
				property.SetValue(temp, value.ToStr().ToIPAdress(), null);
				break;
			default:
				property.SetValue(temp, value, null);
				break;
			}
		}

		public static T ToDbParameter<T>(this object obj, string name) where T : DbParameter, new()
		{
			return new T
			{
				ParameterName = name,
				Value = SetValue(obj)
			};
		}

		public static T ToDbParameter<T>(this object obj, string name, DbType dbtype) where T : DbParameter, new()
		{
			return new T
			{
				ParameterName = name,
				Value = SetValue(obj),
				DbType = dbtype
			};
		}

		public static T ToDbParameter<T>(this object obj, string name, DbType dbtype, ParameterDirection direction) where T : DbParameter, new()
		{
			return new T
			{
				ParameterName = name,
				Value = SetValue(obj),
				DbType = dbtype,
				Direction = direction
			};
		}

		public static string GetColumnName(this IDataReader reader, int index = 0, string defaultName = "NULL")
		{
			string name = reader.GetName(index);
			if (!string.IsNullOrEmpty(name))
			{
				return name;
			}
			return defaultName;
		}

		private static object SetValue(object obj)
		{
			if (obj != null)
			{
				return obj;
			}
			return DBNull.Value;
		}
	}
}
