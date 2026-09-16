using System;
using System.Runtime.InteropServices;

namespace EPGServiceNotify.WinAPI
{
	public static class Kernel32DLL
	{
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern int WTSGetActiveConsoleSessionId();

		[DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool CloseHandle(IntPtr handle);
	}
}
