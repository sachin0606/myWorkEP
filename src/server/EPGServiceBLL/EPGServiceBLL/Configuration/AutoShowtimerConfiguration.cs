using DeXin.EPG.BLL.EventFileHelper;

namespace EPGServiceBLL.Configuration
{
	public class AutoShowtimerConfiguration
	{
		public bool IsAuto { get; set; }

		public string ShowtimPath { get; set; }

		public EnumDateFormat DateFormat { get; set; }
	}
}
