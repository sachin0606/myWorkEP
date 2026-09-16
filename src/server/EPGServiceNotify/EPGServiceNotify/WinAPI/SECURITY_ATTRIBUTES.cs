using System;

namespace EPGServiceNotify.WinAPI
{
	public struct SECURITY_ATTRIBUTES
	{
		public int Length;

		public IntPtr lpSecurityDescriptor;

		public bool bInheritHandle;
	}
}
