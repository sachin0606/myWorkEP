using DeXin.EPG.Resource;

namespace EPGServiceBLL.EPGException
{
	public class VersionLimitedException : NullOrErrorAccreditException
	{
		public VersionLimitedException()
			: base(ResourceManagement.GetLocationString("VersionLimited"))
		{
		}

		public VersionLimitedException(string message)
			: base(message)
		{
		}
	}
}
