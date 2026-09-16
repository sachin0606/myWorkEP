using System;
using System.IO;
using System.Text;

namespace Dexin.Util.Common.LogManager
{
	public class DirectLog
	{
		private static object locObject = new object();

		private static string _LogDirectory;

		private static LogLevels _LogLevel = LogLevels.Error;

		public static string LogDirectory
		{
			get
			{
				if (string.IsNullOrEmpty(_LogDirectory))
				{
					_LogDirectory = AppDomain.CurrentDomain.BaseDirectory;
				}
				return _LogDirectory;
			}
			set
			{
				_LogDirectory = value;
			}
		}

		public static LogLevels LogLevel
		{
			get
			{
				return _LogLevel;
			}
			set
			{
				_LogLevel = value;
			}
		}

		protected static string LogFilePath
		{
			get
			{
				lock (locObject)
				{
					DateTime now = DateTime.Now;
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(LogDirectory);
					if (!Directory.Exists(stringBuilder.ToString()))
					{
						Directory.CreateDirectory(stringBuilder.ToString());
					}
					stringBuilder.Append("\\");
					stringBuilder.Append(now.ToString("yyyyMM"));
					if (!Directory.Exists(stringBuilder.ToString()))
					{
						Directory.CreateDirectory(stringBuilder.ToString());
					}
					return string.Format("{0}\\{1}.txt", stringBuilder.ToString(), now.ToString("yyyy-MM-dd"));
				}
			}
		}

		public static void WriteTXTLog(string logMessage, LogLevels logLevel = LogLevels.Error)
		{
			lock (locObject)
			{
				if (LogLevel < logLevel)
				{
					return;
				}
				DateTime now = DateTime.Now;
				try
				{
					using (StreamWriter streamWriter = new StreamWriter(LogFilePath, true, Encoding.UTF8))
					{
						streamWriter.WriteLine(string.Format("[{0}][{1}] {2}", logLevel, now.ToString("yyyy-MM-dd HH:mm:ss.fff"), logMessage));
						streamWriter.AutoFlush = true;
					}
				}
				catch
				{
				}
			}
		}

		public static void WriteTXTLog(Exception ex, LogLevels logLevl = LogLevels.Debug)
		{
			string message = ex.Message;
			if (logLevl > LogLevel && LogLevel < LogLevels.Warn)
			{
				WriteTXTLog(message, LogLevel);
				return;
			}
			message += ex.StackTrace;
			WriteTXTLog(message, (logLevl > LogLevel) ? LogLevel : logLevl);
		}
	}
}
