using System;

namespace TsSenders
{
	public class MessageException : Exception
	{
		public MessageException(string message)
			: base(message)
		{
		}

		public MessageException(string formatstring, params object[] args)
			: base(string.Format(formatstring, args))
		{
		}
	}
}
