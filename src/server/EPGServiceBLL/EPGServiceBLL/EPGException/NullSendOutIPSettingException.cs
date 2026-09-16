using DeXin.EPG.Resource;

namespace EPGServiceBLL.EPGException
{
	public class NullSendOutIPSettingException : SettingErrorException
	{
		public NullSendOutIPSettingException()
			: base(ResourceManagement.GetLocationString("SendPointNotSet"))
		{
		}

		public NullSendOutIPSettingException(string message)
			: base(message)
		{
		}
	}
}
