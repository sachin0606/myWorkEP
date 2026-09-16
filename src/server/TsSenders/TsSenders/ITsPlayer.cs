using System.Runtime.InteropServices;

namespace TsSenders
{
	public class ITsPlayer
	{
		[DllImport("StreamDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool OpenDevice(ref long phDeviceHandle, bool bDirect);

		[DllImport("StreamDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool CloseDevice(long phDeviceHandle);

		[DllImport("StreamDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool GetState();

		[DllImport("StreamDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool WriteData(byte[] pBuffer, int bufLength);

		[DllImport("StreamDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool SetRate(double rate);
	}
}
