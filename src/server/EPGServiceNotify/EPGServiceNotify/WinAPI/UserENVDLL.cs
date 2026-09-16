using System;
using System.Runtime.InteropServices;

namespace EPGServiceNotify.WinAPI
{
	public static class UserENVDLL
	{
		[DllImport("userenv.dll", SetLastError = true)]
		public static extern bool CreateEnvironmentBlock(out IntPtr lpEnvironment, IntPtr hToken, bool bInherit);
	}
}
