using System;
using System.Runtime.Serialization;

namespace Com.Cn.Tool.Model
{
	public class CustomException : Exception, ISerializable
	{
		private Exception _EX = new Exception();

		private string _CusMessage = string.Empty;

		private MessageType _messageType = MessageType.SHOW;

		public Exception EX
		{
			get
			{
				return _EX;
			}
		}

		public string CusMessage
		{
			get
			{
				return _CusMessage;
			}
		}

		public MessageType messageType
		{
			get
			{
				return _messageType;
			}
			set
			{
				_messageType = value;
			}
		}

		public CustomException()
		{
		}

		public CustomException(string cusMessage)
			: base(cusMessage)
		{
			_CusMessage = cusMessage;
			messageType = MessageType.SHOW;
			_EX = new Exception(cusMessage);
		}

		public CustomException(string cusMessage, MessageType messageType)
			: base(cusMessage)
		{
			_CusMessage = cusMessage;
			this.messageType = messageType;
			_EX = new Exception(cusMessage);
		}

		public CustomException(string cusMessage, Exception ex, MessageType messageType)
			: base(ex.Message)
		{
			_CusMessage = cusMessage;
			this.messageType = messageType;
			_EX = ex;
		}

		protected CustomException(string message, Exception inner)
		{
		}

		protected CustomException(SerializationInfo info, StreamingContext context)
		{
		}
	}
}
