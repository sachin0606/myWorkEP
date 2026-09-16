using System;
using System.Xml.Linq;
using Com.Cn.Tool;
using DeXin.EPG.BLL.EventFileHelper;

namespace EPGServiceBLL.Configuration
{
	public class AutoShowtimer
	{
		private static string settingFile = AppDomain.CurrentDomain.BaseDirectory + "AutoShowtimer.config";

		public static AutoShowtimerConfiguration Get()
		{
			XElement xElement = XElement.Load(settingFile).Element("AutoShowtimer");
			AutoShowtimerConfiguration autoShowtimerConfiguration = new AutoShowtimerConfiguration();
			autoShowtimerConfiguration.IsAuto = bool.Parse(xElement.Attribute("Auto").Value);
			autoShowtimerConfiguration.ShowtimPath = xElement.Element("ShowtimePath").Value;
			autoShowtimerConfiguration.DateFormat = ((!string.IsNullOrEmpty(xElement.Element("DateFormat").Value)) ? ((EnumDateFormat)(object)Tool.GetEnum(typeof(EnumDateFormat), xElement.Element("DateFormat").Value)) : ((EnumDateFormat)0));
			return autoShowtimerConfiguration;
		}

		public static void Set(AutoShowtimerConfiguration cfg)
		{
			XDocument xDocument = XDocument.Load(settingFile);
			XElement xElement = xDocument.Element("configuration").Element("AutoShowtimer");
			xElement.Attribute("Auto").Value = cfg.IsAuto.ToString();
			xElement.Element("ShowtimePath").Value = cfg.ShowtimPath;
			xElement.Element("DateFormat").Value = cfg.DateFormat.ToString();
			xDocument.Save(settingFile);
		}
	}
}
