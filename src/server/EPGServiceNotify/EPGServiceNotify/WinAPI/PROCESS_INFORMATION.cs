using System;

namespace EPGServiceNotify.WinAPI
{
	public struct PROCESS_INFORMATION
	{
		public IntPtr hProcess;

		public IntPtr hThread;

		public int dwProcessID;

		public int dwThreadID;
	}
}
