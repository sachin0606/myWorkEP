using System;
using DeXin.EPG.Resource;

namespace EPGServiceBLL.EPGException
{
	public class HTBDogErrorException : Exception
	{
		public HTBDogErrorException()
			: base(ResourceManagement.GetLocationString("FailedToCheckSoftdog"))
		{
		}

		public HTBDogErrorException(string message)
			: base(message)
		{
		}
	}
}
