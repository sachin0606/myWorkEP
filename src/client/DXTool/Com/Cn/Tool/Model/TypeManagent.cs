using System;
using System.Reflection;

namespace Com.Cn.Tool.Model
{
	public class TypeManagent<T>
	{
		public static T GetEnum(string args)
		{
			return (T)Enum.Parse(typeof(T), args, true);
		}

		public static T GetEnum(int args)
		{
			return (T)Enum.ToObject(typeof(T), args);
		}

		public static T ConvertInstance(object args)
		{
			if (args is T)
			{
				return (T)args;
			}
			return default(T);
		}

		public static T LoadInstance()
		{
			Assembly assembly = Assembly.GetAssembly(typeof(T));
			return ConvertInstance(assembly.CreateInstance(typeof(T).ToString(), true));
		}

		public static Assembly LoadInstance(string path)
		{
			return Assembly.LoadFile(path);
		}
	}
}
