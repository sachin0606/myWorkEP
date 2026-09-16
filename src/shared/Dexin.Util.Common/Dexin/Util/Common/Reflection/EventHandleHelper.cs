using System;
using System.ComponentModel;
using System.Reflection;

namespace Dexin.Util.Common.Reflection
{
	public class EventHandleHelper
	{
		public static Delegate[] GetObjectEventList(object p_Object, string p_EventName, Type p_EventType)
		{
			PropertyInfo property = p_Object.GetType().GetProperty("Events", BindingFlags.Instance | BindingFlags.NonPublic);
			if (property != null)
			{
				object value = property.GetValue(p_Object, null);
				if (value != null && value is EventHandlerList)
				{
					EventHandlerList eventHandlerList = (EventHandlerList)value;
					FieldInfo field = p_EventType.GetField(p_EventName, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.NonPublic);
					if (field == null)
					{
						return null;
					}
					Delegate obj = eventHandlerList[field.GetValue(p_Object)];
					if ((object)obj == null)
					{
						return null;
					}
					return obj.GetInvocationList();
				}
			}
			return null;
		}
	}
}
