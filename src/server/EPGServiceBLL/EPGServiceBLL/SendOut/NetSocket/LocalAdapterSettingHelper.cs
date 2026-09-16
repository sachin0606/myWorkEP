using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace EPGServiceBLL.SendOut.NetSocket
{
	public class LocalAdapterSettingHelper
	{
		private static string settingFile = AppDomain.CurrentDomain.BaseDirectory + "NetAdapters.xml";

		public static List<string> GetLocalAdapterSettings()
		{
			List<string> result = new List<string>();
			if (File.Exists(settingFile))
			{
				XElement xElement = XElement.Load(settingFile);
				result = (from xElement2 in xElement.Elements("Add")
					select xElement2.Attribute("IP").Value).ToList();
			}
			return result;
		}

		public static void SaveLocalAdapterSettings(List<string> adapterIps)
		{
			XDocument xDoc = XDocument.Load(settingFile);
			List<string> localAdapterSettings = GetLocalAdapterSettings();
			xDoc.Root.RemoveAll();
			adapterIps.ForEach(delegate(string itemIp)
			{
				xDoc.Root.Add(new XElement("Add", new XAttribute("IP", itemIp)));
			});
			xDoc.Save(settingFile);
		}
	}
}
