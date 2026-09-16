using System;
using Util.Common.ExtensionHelper;

namespace Util.Common
{
	public class MyMessage
	{
		private byte[] _data = new byte[0];

		private int _Priority;

		private bool _State = true;

		private DateTime _BuildTime = DateTime.Now;

		public byte[] data
		{
			get
			{
				return _data;
			}
			set
			{
				_data = value;
			}
		}

		public int Priority
		{
			get
			{
				return _Priority;
			}
			set
			{
				_Priority = value;
			}
		}

		public bool State
		{
			get
			{
				return _State;
			}
			set
			{
				_State = value;
			}
		}

		public DateTime BuildTime
		{
			get
			{
				return _BuildTime;
			}
			private set
			{
				_BuildTime = value;
			}
		}

		public MyMessage(byte[] buff)
		{
			int index = 0;
			BuildTime = buff.GetDateTime(ref index);
			Priority = buff.GetInt32(ref index);
			int @int = buff.GetInt32(ref index);
			data = buff.GetBytes(@int, ref index);
		}

		public MyMessage(byte[] buff, int priority = 0)
		{
			data = buff;
			Priority = priority;
			BuildTime = DateTime.Now;
		}

		public byte[] ToBuff()
		{
			byte[] array = new byte[15 + data.Length];
			int index = 0;
			array.CopyDateTime(BuildTime, ref index);
			array.Copy(Priority, ref index);
			array.Copy(data.Length, ref index);
			array.CopyBytes(data, ref index);
			return array;
		}
	}
}
