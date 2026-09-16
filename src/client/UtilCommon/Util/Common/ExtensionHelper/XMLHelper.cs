using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Util.Common.ExtensionHelper
{
	public static class XMLHelper
	{
		public static XElement CreateXElement(this XName xname, object value)
		{
			return xname.CreateXElement(value, null);
		}

		public static XElement CreateXElement(this XName xname, object value, params KeyValuePair<string, string>[] attributes)
		{
			XElement xElement = new XElement(xname);
			xElement.SetValue(value);
			if (attributes != null && attributes.Length > 0)
			{
				for (int i = 0; i < attributes.Length; i++)
				{
					KeyValuePair<string, string> keyValuePair = attributes[i];
					xElement.SetAttributeValue(keyValuePair.Key, keyValuePair.Value);
				}
			}
			return xElement;
		}

		public static void AddChild(this XElement element, XName xname, object value, bool isCheckExists = false)
		{
			XElement xElement = null;
			if (isCheckExists)
			{
				xElement = element.Element(xname);
				if (xElement == null)
				{
					xElement = xname.CreateXElement(value);
					element.Add(xElement);
				}
				else
				{
					xElement.SetValue(value);
				}
			}
			else
			{
				xElement = xname.CreateXElement(value);
				element.Add(xElement);
			}
		}

		public static void AddChild(this XElement element, XName xname, object value, bool isCheckExists = false, params KeyValuePair<string, string>[] attributes)
		{
			XElement xElement = null;
			if (isCheckExists)
			{
				xElement = element.Element(xname);
				if (xElement == null)
				{
					xElement = xname.CreateXElement(value, attributes);
					element.Add(xElement);
					return;
				}
				xElement.SetValue(value);
				for (int i = 0; i < attributes.Length; i++)
				{
					KeyValuePair<string, string> keyValuePair = attributes[i];
					if (xElement.Attribute(keyValuePair.Key) == null)
					{
						xElement.SetAttributeValue(keyValuePair.Key, keyValuePair.Value);
					}
					else
					{
						xElement.Attribute(keyValuePair.Key).SetValue(keyValuePair.Value);
					}
				}
			}
			else
			{
				xElement = xname.CreateXElement(value, attributes);
				element.Add(xElement);
			}
		}

		public static XElement GetChildByName(this XContainer element, XName name)
		{
			return element.Elements().FirstOrDefault((XElement p) => p.Name == name);
		}

		public static string GetChildValueByName(this XContainer element, XName name)
		{
			XElement childByName = element.GetChildByName(name);
			if (childByName != null)
			{
				return childByName.Value;
			}
			return "";
		}

		public static IEnumerable<XElement> GetChildrenByName(this XContainer element, XName name)
		{
			return from p in element.Elements()
				where p.Name == name
				select p;
		}

		public static string GetValueByName(this XElement element, XName name, bool isThrow = false)
		{
			string result = string.Empty;
			XAttribute xAttribute = element.Attributes().FirstOrDefault((XAttribute p) => p.Name == name);
			if (xAttribute == null)
			{
				if (isThrow)
				{
					throw new ArgumentNullException("name");
				}
			}
			else
			{
				result = xAttribute.Value;
			}
			return result;
		}

		public static IEnumerable<XElement> RecurrenceFistXContainersByName(this XContainer element, XName name)
		{
			IEnumerable<XElement> enumerable = from p in element.Elements()
				where p.Name == name
				select p;
			if (enumerable.Count() == 0)
			{
				using (IEnumerator<XElement> enumerator = (from p in element.Elements()
					where p.Name != name
					select p).GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						XContainer current = enumerator.Current;
						return current.RecurrenceFistXContainersByName(name);
					}
				}
			}
			return enumerable;
		}

		public static void ToXML<T>(this T demo, string filePath, Encoding encoding)
		{
			if (File.Exists(filePath))
			{
				File.Delete(filePath);
			}
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
			using (StreamWriter textWriter = new StreamWriter(filePath, false, encoding))
			{
				xmlSerializer.Serialize(textWriter, demo);
			}
		}

		public static T ToEntity<T>(string filePath, Encoding encoding)
		{
			if (File.Exists(filePath))
			{
				using (StreamReader textReader = new StreamReader(filePath, encoding))
				{
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
					return (T)xmlSerializer.Deserialize(textReader);
				}
			}
			return default(T);
		}

		public static string GetChildElementValueByName(this XElement root, XName name)
		{
			string result = "";
			XElement childByName = root.GetChildByName(name);
			if (childByName != null)
			{
				result = childByName.Value;
			}
			return result;
		}
	}
}
