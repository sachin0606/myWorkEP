namespace DeXin.EPG.Tools
{
	public class StreamPlayerManagement
	{
		private static StreamPlayerManagement _Ins = null;

		private long phDeviceHandle = 0L;

		public static StreamPlayerManagement Ins
		{
			get
			{
				if (_Ins == null)
				{
					_Ins = new StreamPlayerManagement();
				}
				return _Ins;
			}
		}

		private StreamPlayerManagement()
		{
		}

		public bool OpenDevice()
		{
			StreamPlayerInterface.OpenDevice(ref phDeviceHandle, true);
			return phDeviceHandle < uint.MaxValue;
		}

		public bool CloseDevice()
		{
			bool result = false;
			if (phDeviceHandle > 0 && StreamPlayerInterface.CloseDevice(phDeviceHandle))
			{
				result = true;
				phDeviceHandle = 0L;
			}
			return result;
		}

		public bool GetState()
		{
			return StreamPlayerInterface.GetState();
		}

		public bool SetRate(double rate)
		{
			return StreamPlayerInterface.SetRate(rate);
		}

		public bool WriteData(byte[] buff)
		{
			return StreamPlayerInterface.WriteData(buff, buff.Length);
		}
	}
}
