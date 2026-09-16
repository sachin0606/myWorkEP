using System;
using System.Runtime.InteropServices;

namespace EPGServiceNotify.WinAPI
{
	public static class AdvAPI32DLL
	{
		[DllImport("advapi32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern bool CreateProcessAsUser(IntPtr hToken, string lpApplicationName, string lpCommandLine, ref SECURITY_ATTRIBUTES lpProcessAttributes, ref SECURITY_ATTRIBUTES lpThreadAttributes, bool bInheritHandle, int dwCreationFlags, IntPtr lpEnvrionment, string lpCurrentDirectory, ref STARTUPINFO lpStartupInfo, ref PROCESS_INFORMATION lpProcessInformation);

		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern bool DuplicateTokenEx(IntPtr hExistingToken, int dwDesiredAccess, ref SECURITY_ATTRIBUTES lpThreadAttributes, int ImpersonationLevel, int dwTokenType, ref IntPtr phNewToken);
	}
}
