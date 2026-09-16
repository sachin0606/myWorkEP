using System;
using DeXin.EPG.Resource;

namespace EPGServiceBLL.EPGException
{
	public class SettingErrorException : Exception
	{
		public SettingErrorException()
			: base(ResourceManagement.GetLocationString("SettingError"))
		{
		}

		public SettingErrorException(string message)
			: base(message)
		{
		}
	}
}
