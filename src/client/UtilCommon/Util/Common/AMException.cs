using System;
using Util.Common.ExtensionHelper;

namespace Util.Common
{
	public class AMException : Exception
	{
		private int _ErrorCode;

		public int ErrorCode
		{
			get
			{
				return _ErrorCode;
			}
			protected set
			{
				_ErrorCode = value;
			}
		}

		public AMException(string message)
			: base(message)
		{
		}

		public AMException(string message, int errorCode)
			: base(message)
		{
			ErrorCode = errorCode;
		}

		public AMException(string formatMessage, params object[] args)
			: base(formatMessage.Fmt(args))
		{
		}

		public AMException(string formatMessage, int errorCode, object[] args)
			: base(formatMessage.Fmt(args))
		{
			ErrorCode = errorCode;
		}

		public AMException(Exception ex)
			: base(ex.Message, ex)
		{
		}

		public AMException(Exception ex, int errorCode)
			: base(ex.Message, ex)
		{
			ErrorCode = errorCode;
		}

		public AMException(ErrorCode errorCode)
			: base(errorCode.ToString())
		{
			ErrorCode = (int)errorCode;
		}
	}
}
