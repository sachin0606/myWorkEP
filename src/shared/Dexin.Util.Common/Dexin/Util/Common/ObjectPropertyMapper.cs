using System;
using System.Linq;
using System.Reflection;

namespace Dexin.Util.Common
{
	public static class ObjectPropertyMapper
	{
		public static void Map<T1, T2>(T1 entitySource, T2 entityTarget, params string[] excudeProperties)
		{
			PropertyInfo[] properties = entityTarget.GetType().GetProperties();
			foreach (PropertyInfo propertyInfo in properties)
			{
				if (!excudeProperties.Contains(propertyInfo.Name))
				{
					propertyInfo.SetValue(entityTarget, entitySource.GetType().GetProperty(propertyInfo.Name).GetValue(entitySource, null), null);
				}
			}
		}

		public static void Map<T1, T2>(T1 entitySource, T2 entityTarget, bool ignoreError, params string[] excudeProperties)
		{
			PropertyInfo[] properties = entityTarget.GetType().GetProperties();
			foreach (PropertyInfo propertyInfo in properties)
			{
				try
				{
					if (!excudeProperties.Contains(propertyInfo.Name))
					{
						propertyInfo.SetValue(entityTarget, entitySource.GetType().GetProperty(propertyInfo.Name).GetValue(entitySource, null), null);
					}
				}
				catch (Exception ex)
				{
					if (!ignoreError)
					{
						throw ex;
					}
				}
			}
		}
	}
}
