using System;
using System.Runtime.InteropServices;

namespace EPGServiceNotify.WinAPI
{
	public static class WtsAPI32DLL
	{
		[DllImport("wtsapi32.dll", SetLastError = true)]
		public static extern bool WTSQueryUserToken(int sessionId, out IntPtr Token);
	}
}
