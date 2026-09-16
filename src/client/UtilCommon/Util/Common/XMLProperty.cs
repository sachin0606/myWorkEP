using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace Util.Common
{
	[Serializable]
	public abstract class XMLProperty
	{
		public const string NodeName = "Property";

		public const string KeyName = "Key";

		public const string ValueName = "Value";

		public const string CommentName = "Comment";

		public XDocument Doc { get; set; }

		public XElement Root { get; set; }

		public virtual string FilePath
		{
			get
			{
				return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "properties.xml");
			}
		}

		public void Init()
		{
			if (Validate())
			{
				Doc = XDocument.Load(FilePath);
				Root = Doc.Root;
				Read();
			}
		}

		protected abstract void Read();

		protected abstract void Write();

		public abstract bool Validate();

		public string Get(string key)
		{
			XElement nodeByKey = Root.GetNodeByKey(key);
			if (nodeByKey == null)
			{
				return null;
			}
			XAttribute attribute = nodeByKey.GetAttribute("Value");
			if (attribute == null)
			{
				return null;
			}
			return attribute.Value;
		}

		public void Set(string key, string value, string comment, Dictionary<string, string> attrs)
		{
			lock (Root)
			{
				XElement xElement = Root.GetNodeByKey(key);
				if (xElement == null)
				{
					xElement = new XElement("Property");
					xElement.SetAttributeValue("Key", key);
					Root.Add(xElement);
				}
				xElement.InitElement(key, value, comment, attrs);
			}
		}

		public void Set(string key, string value)
		{
			lock (Root)
			{
				XElement xElement = Root.GetNodeByKey(key);
				if (xElement == null)
				{
					xElement = new XElement("Property");
					xElement.SetAttributeValue("Key", key);
					xElement.SetAttributeValue("Comment", "");
					Root.Add(xElement);
				}
				xElement.SetAttributeValue("Value", value);
			}
		}

		public void Save()
		{
			Write();
			Doc.Save(FilePath);
		}

		public virtual void CheckPropertiesFile(string dllpath, string fileapath)
		{
			if (File.Exists(FilePath))
			{
				return;
			}
			Assembly assembly = Assembly.LoadFrom(dllpath);
			using (Stream stream = assembly.GetManifestResourceStream(fileapath))
			{
				byte[] array = new byte[stream.Length];
				stream.Read(array, 0, array.Length);
				using (StreamWriter streamWriter = new StreamWriter(FilePath, false))
				{
					streamWriter.Write(Encoding.UTF8.GetString(array));
				}
			}
		}
	}
}
