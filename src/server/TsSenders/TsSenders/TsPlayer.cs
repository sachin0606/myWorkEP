using System;

namespace TsSenders
{
	public class TsPlayer
	{
		private static long MinDeviceHandle = -1L;

		private static long _DeviceHandle = -1L;

		private static object lockRePreparTSPlayer = new object();

		public static long DeviceHandle
		{
			get
			{
				return _DeviceHandle;
			}
			private set
			{
				_DeviceHandle = value;
			}
		}

		public static bool IsOpen
		{
			get
			{
				return DeviceHandle != MinDeviceHandle;
			}
		}

		public static bool OpenDevice(bool isthrowexception = false)
		{
			long phDeviceHandle = MinDeviceHandle;
			ITsPlayer.OpenDevice(ref phDeviceHandle, true);
			DeviceHandle = phDeviceHandle;
			if (!IsOpen)
			{
				Console.WriteLine(string.Format("0x{0:X}", phDeviceHandle));
				DeviceHandle = MinDeviceHandle;
				if (isthrowexception)
				{
					throw new MessageException("open ts player error!{0}", phDeviceHandle);
				}
			}
			return IsOpen;
		}

		public static bool CloseDevice()
		{
			bool result = false;
			if (IsOpen && ITsPlayer.CloseDevice(DeviceHandle))
			{
				result = true;
				DeviceHandle = 0L;
			}
			return result;
		}

		public static bool GetState()
		{
			return ITsPlayer.GetState();
		}

		public static bool SetRate(double rate)
		{
			return ITsPlayer.SetRate(rate);
		}

		public static bool WriteData(byte[] buff)
		{
			lock (lockRePreparTSPlayer)
			{
				bool flag = false;
				if (IsOpen)
				{
					flag = ITsPlayer.WriteData(buff, buff.Length);
					if (!flag)
					{
						CloseDevice();
					}
				}
				return flag;
			}
		}
	}
}
