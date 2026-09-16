using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using EPGServiceNotify.WinAPI;

namespace EPGServiceNotify
{
	internal class Desktop
	{
		private const int GENERIC_ALL_ACCESS = 268435456;

		private IntPtr m_hCurWinsta = IntPtr.Zero;

		private IntPtr m_hCurDesktop = IntPtr.Zero;

		private IntPtr m_hWinsta = IntPtr.Zero;

		private IntPtr m_hDesk = IntPtr.Zero;

		internal bool BeginInteraction()
		{
			EndInteraction();
			m_hCurWinsta = User32DLL.GetProcessWindowStation();
			if (m_hCurWinsta == IntPtr.Zero)
			{
				return false;
			}
			m_hCurDesktop = User32DLL.GetDesktopWindow();
			if (m_hCurDesktop == IntPtr.Zero)
			{
				return false;
			}
			m_hWinsta = User32DLL.OpenWindowStation("winsta0", false, WindowStationAccessRight.WINSTA_ALL_ACCESS);
			if (m_hWinsta == IntPtr.Zero)
			{
				return false;
			}
			User32DLL.SetProcessWindowStation(m_hWinsta);
			m_hDesk = User32DLL.OpenDesktop("default", OpenDesktopFlag.DF_NONE, false, DesktopAccessRight.DESKTOP_CREATEMENU | DesktopAccessRight.DESKTOP_CREATEWINDOW | DesktopAccessRight.DESKTOP_ENUMERATE | DesktopAccessRight.DESKTOP_HOOKCONTROL | DesktopAccessRight.DESKTOP_JOURNALPLAYBACK | DesktopAccessRight.DESKTOP_JOURNALRECORD | DesktopAccessRight.DESKTOP_READOBJECTS | DesktopAccessRight.DESKTOP_SWITCHDESKTOP | DesktopAccessRight.DESKTOP_WRITEOBJECTS);
			if (m_hDesk == IntPtr.Zero)
			{
				return false;
			}
			User32DLL.SetThreadDesktop(m_hDesk);
			return true;
		}

		internal void EndInteraction()
		{
			if (m_hCurWinsta != IntPtr.Zero)
			{
				User32DLL.SetProcessWindowStation(m_hCurWinsta);
			}
			if (m_hCurDesktop != IntPtr.Zero)
			{
				User32DLL.SetThreadDesktop(m_hCurDesktop);
			}
			if (m_hWinsta != IntPtr.Zero)
			{
				User32DLL.CloseWindowStation(m_hWinsta);
			}
			if (m_hDesk != IntPtr.Zero)
			{
				User32DLL.CloseDesktop(m_hDesk);
			}
		}

		public static bool CreateProcess(string app, string path, string parameter)
		{
			IntPtr Token = WindowsIdentity.GetCurrent().Token;
			IntPtr phNewToken = IntPtr.Zero;
			PROCESS_INFORMATION lpProcessInformation = default(PROCESS_INFORMATION);
			SECURITY_ATTRIBUTES lpThreadAttributes = default(SECURITY_ATTRIBUTES);
			lpThreadAttributes.Length = Marshal.SizeOf((object)lpThreadAttributes);
			STARTUPINFO lpStartupInfo = default(STARTUPINFO);
			lpStartupInfo.cb = Marshal.SizeOf((object)lpStartupInfo);
			int sessionId = Kernel32DLL.WTSGetActiveConsoleSessionId();
			if (!WtsAPI32DLL.WTSQueryUserToken(sessionId, out Token))
			{
				return false;
			}
			if (!AdvAPI32DLL.DuplicateTokenEx(Token, 268435456, ref lpThreadAttributes, 1, 1, ref phNewToken))
			{
				return false;
			}
			IntPtr lpEnvironment = IntPtr.Zero;
			if (!UserENVDLL.CreateEnvironmentBlock(out lpEnvironment, phNewToken, false))
			{
				return false;
			}
			if (!AdvAPI32DLL.CreateProcessAsUser(phNewToken, app, parameter, ref lpThreadAttributes, ref lpThreadAttributes, false, 0, IntPtr.Zero, path, ref lpStartupInfo, ref lpProcessInformation))
			{
				return false;
			}
			if (lpProcessInformation.hProcess != IntPtr.Zero)
			{
				Kernel32DLL.CloseHandle(lpProcessInformation.hProcess);
			}
			if (lpProcessInformation.hThread != IntPtr.Zero)
			{
				Kernel32DLL.CloseHandle(lpProcessInformation.hThread);
			}
			if (phNewToken != IntPtr.Zero)
			{
				Kernel32DLL.CloseHandle(phNewToken);
			}
			return true;
		}
	}
}
