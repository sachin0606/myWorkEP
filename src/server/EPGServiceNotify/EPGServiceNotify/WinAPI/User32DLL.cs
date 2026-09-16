using System;
using System.Runtime.InteropServices;

namespace EPGServiceNotify.WinAPI
{
	public static class User32DLL
	{
		[DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern IntPtr GetDesktopWindow();

		[DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern IntPtr GetProcessWindowStation();

		[DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern IntPtr GetThreadDesktop(uint dwThread);

		[DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern IntPtr OpenWindowStation(string lpszWinSta, bool fInherit, WindowStationAccessRight dwDesiredAccess);

		[DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern IntPtr SetProcessWindowStation(IntPtr hWinSta);

		[DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern IntPtr CloseWindowStation(IntPtr hWinSta);

		[DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern IntPtr OpenDesktop(string lpszDesktop, OpenDesktopFlag dwFlags, bool fInherit, DesktopAccessRight dwDesiredAccess);

		[DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern IntPtr CloseDesktop(IntPtr hDesktop);

		[DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern bool SetThreadDesktop(IntPtr hDesktop);
	}
}
