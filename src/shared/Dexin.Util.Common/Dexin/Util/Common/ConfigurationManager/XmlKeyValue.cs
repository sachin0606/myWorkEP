using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Dexin.Util.Common.ConfigurationManager
{
	public class XmlKeyValue
	{
		private static FileInfo fileInfo;

		private string _ConfigurationFilePaht;

		private string _DefaultConfigurationFilePath;

		private string _DefaultDocumentRootName;

		private XDocument _XDoc;

		private Dictionary<string, string> _XmlKeyValues;

		public Dictionary<string, string> XmlKeyValues
		{
			get
			{
				return _XmlKeyValues;
			}
		}

		public XmlKeyValue(string configurationFilePath, string defaultConfigurationFilePath, string defaultDocumentRootName = "root")
		{
			_ConfigurationFilePaht = configurationFilePath;
			_DefaultConfigurationFilePath = defaultConfigurationFilePath;
			_DefaultDocumentRootName = defaultDocumentRootName;
			if (!File.Exists(_DefaultConfigurationFilePath))
			{
				throw new FileNotFoundException("defaultConfigurationFilePath is not exists!");
			}
			Init();
		}

		private void Init()
		{
			if (!File.Exists(_ConfigurationFilePaht))
			{
				File.Copy(_DefaultConfigurationFilePath, _ConfigurationFilePaht);
			}
			_XmlKeyValues = Load(_ConfigurationFilePaht, _DefaultDocumentRootName);
			_XDoc = XDocument.Load(_ConfigurationFilePaht);
			fileInfo = new FileInfo(_ConfigurationFilePaht);
		}

		public string GetXValue(string name)
		{
			try
			{
				FileInfo fileInfo = new FileInfo(_ConfigurationFilePaht);
				if (!fileInfo.Exists || XmlKeyValue.fileInfo.LastWriteTimeUtc < fileInfo.LastWriteTimeUtc)
				{
					Init();
				}
				return _XmlKeyValues[name];
			}
			catch (KeyNotFoundException ex)
			{
				if (string.IsNullOrEmpty(_DefaultConfigurationFilePath))
				{
					throw ex;
				}
				XDocument xDocument = XDocument.Load(_DefaultConfigurationFilePath);
				XElement xElement = xDocument.Elements(_DefaultDocumentRootName).Elements().SingleOrDefault((XElement x) => x.Attribute("name").Value == name);
				if (xElement != null)
				{
					SetOrAddXValue(name, xElement.Attribute("value").Value);
					SaveAll();
					return GetXValue(name);
				}
				try
				{
					XDocument xDocument2 = XDocument.Load(Assembly.GetEntryAssembly().GetManifestResourceStream(string.Format("{0}{1}", Assembly.GetEntryAssembly().GetName().Name, ".DefaultSetting.xml")));
					XElement xElement2 = xDocument2.Elements(_DefaultDocumentRootName).Elements().SingleOrDefault((XElement x) => x.Attribute("name").Value == name);
					if (xElement2 != null)
					{
						SetOrAddXValue(name, xElement2.Attribute("value").Value);
						SaveAll();
						return GetXValue(name);
					}
					throw new InvalidConfigurationException("", ex);
				}
				catch (Exception innerException)
				{
					throw new InvalidConfigurationException("", innerException);
				}
			}
			catch (Exception innerException2)
			{
				throw new InvalidConfigurationException("", innerException2);
			}
		}

		public void AddXValue(string name, string value)
		{
			XElement content = new XElement("setting", new XAttribute("name", name), new XAttribute("value", value));
			XElement root = _XDoc.Root;
			root.Add(content);
		}

		public void SetXValue(string name, string value)
		{
			XElement xElement = _XDoc.Elements(_DefaultDocumentRootName).Elements().FirstOrDefault((XElement p) => p.Attribute("name").Value == name);
			if (xElement != null)
			{
				xElement.Attribute("value").Value = value;
				return;
			}
			throw new ArgumentException("the given name is not exist", "name");
		}

		public void SetOrAddXValue(string name, string value)
		{
			XElement xElement = _XDoc.Elements(_DefaultDocumentRootName).Elements().FirstOrDefault((XElement p) => p.Attribute("name").Value == name);
			if (xElement != null)
			{
				xElement.Attribute("value").Value = value;
			}
			else
			{
				AddXValue(name, value);
			}
		}

		public void SaveAll()
		{
			_XDoc.Save(_ConfigurationFilePaht);
			Init();
		}

		public static Dictionary<string, string> Load(string file, string defaultDocumentRootName = "root")
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			XDocument xDocument = XDocument.Load(file);
			IEnumerable<XElement> enumerable = from el in xDocument.Elements(defaultDocumentRootName).Elements()
				select (el);
			foreach (XElement item in enumerable)
			{
				dictionary.Add(item.Attribute("name").Value, item.Attribute("value").Value);
			}
			return dictionary;
		}

		public static Dictionary<string, string> Load(Stream stream, string defaultDocumentRootName = "root")
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			XDocument xDocument = XDocument.Load(stream);
			IEnumerable<XElement> enumerable = from el in xDocument.Elements(defaultDocumentRootName).Elements()
				select (el);
			foreach (XElement item in enumerable)
			{
				dictionary.Add(item.Attribute("name").Value, item.Attribute("value").Value);
			}
			return dictionary;
		}
	}
}
