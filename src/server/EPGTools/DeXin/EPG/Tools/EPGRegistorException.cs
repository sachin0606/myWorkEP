using System;

namespace DeXin.EPG.Tools
{
	public class EPGRegistorException : Exception
	{
		public EPGRegistorException()
			: base("regist infomation error")
		{
		}

		public EPGRegistorException(string message)
			: base(message)
		{
		}
	}
}
