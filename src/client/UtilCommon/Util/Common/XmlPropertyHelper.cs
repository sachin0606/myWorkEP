using System.Collections.Generic;
using System.Xml.Linq;
using Util.Common.ExtensionHelper;

namespace Util.Common
{
	public static class XmlPropertyHelper
	{
		public static void InitElement(this XElement item, string key, object value, string comment, Dictionary<string, string> attrs)
		{
			item.SetAttributeValue("Key", key);
			item.SetAttributeValue("Value", value.ToStr());
			if (!comment.IsNullOrEmpty())
			{
				item.SetAttributeValue("Comment", comment);
			}
			if (attrs == null)
			{
				return;
			}
			foreach (string key2 in attrs.Keys)
			{
				item.SetAttributeValue(key2, attrs[key2]);
			}
		}

		public static List<XElement> GetNodesByKey(this XElement parent, string key)
		{
			List<XElement> list = new List<XElement>();
			foreach (XElement item in parent.Elements())
			{
				if (item.IsContainKey(key))
				{
					list.Add(item);
				}
			}
			return list;
		}

		public static XElement GetNodeByKey(this XElement parent, string key)
		{
			foreach (XElement item in parent.Elements())
			{
				if (item.IsContainKey(key))
				{
					return item;
				}
			}
			return null;
		}

		public static XAttribute GetAttribute(this XElement node, string name)
		{
			return node.Attribute(name);
		}

		public static string GetAttributeValue(this XElement node)
		{
			XAttribute attribute = node.GetAttribute("Value");
			if (attribute != null)
			{
				return attribute.Value;
			}
			return "";
		}

		public static bool IsContainKey(this XElement node, string value)
		{
			XAttribute attribute = node.GetAttribute("Key");
			if (attribute == null)
			{
				return false;
			}
			return attribute.Value.Equals(value);
		}
	}
}
