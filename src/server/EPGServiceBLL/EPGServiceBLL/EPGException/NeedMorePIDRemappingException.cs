using DeXin.EPG.Resource;

namespace EPGServiceBLL.EPGException
{
	public class NeedMorePIDRemappingException : SettingErrorException
	{
		public NeedMorePIDRemappingException()
			: base(ResourceManagement.GetLocationString("PIDRemappingError"))
		{
		}

		public NeedMorePIDRemappingException(string message)
			: base(message)
		{
		}
	}
}
