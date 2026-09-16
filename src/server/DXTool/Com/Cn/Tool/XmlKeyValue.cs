using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Com.Cn.Tool
{
	public class XmlKeyValue
	{
		private string xmlPath = "DefaultSetting.xml";

		private XDocument xDoc;

		private Dictionary<string, string> _XmlKeyValues;

		private XmlKeyValue()
		{
		}

		public XmlKeyValue(string xmlpath)
		{
			xmlPath = xmlpath;
			Init();
		}

		private void Init()
		{
			_XmlKeyValues = new Dictionary<string, string>();
			xDoc = XDocument.Load(xmlPath);
			IEnumerable<XElement> enumerable = from el in xDoc.Elements("root").Elements()
				select (el);
			foreach (XElement item in enumerable)
			{
				_XmlKeyValues.Add(item.Attribute("name").Value, item.Attribute("value").Value);
			}
		}

		public Dictionary<string, string> GetAllXValues()
		{
			return _XmlKeyValues;
		}

		public string GetXValue(string name)
		{
			string text = "";
			try
			{
				return _XmlKeyValues[name];
			}
			catch
			{
				return "";
			}
		}

		public void AddXValue(string name, string value)
		{
			XElement content = new XElement("setting", new XAttribute("name", name), new XAttribute("value", value));
			XElement root = xDoc.Root;
			root.Add(content);
		}

		public void SetXValue(string name, string value)
		{
			IEnumerable<XElement> enumerable = from el in xDoc.Elements("root").Elements()
				select (el);
			foreach (XElement item in enumerable)
			{
				if (item.Attribute("name").Value == name)
				{
					item.Attribute("value").Value = value;
				}
			}
		}

		public void SaveAll()
		{
			xDoc.Save(xmlPath);
		}
	}
}
