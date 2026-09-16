using System;
using DeXin.EPG.Resource;

namespace EPGServiceBLL.EPGException
{
	public class NullOrErrorAccreditException : Exception
	{
		public NullOrErrorAccreditException()
			: base(ResourceManagement.GetLocationString("SoftOutOfDate"))
		{
		}

		public NullOrErrorAccreditException(string message)
			: base(message)
		{
		}
	}
}
