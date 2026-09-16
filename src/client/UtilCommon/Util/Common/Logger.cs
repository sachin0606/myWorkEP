using System;
using System.IO;
using System.Text;
using System.Threading;

namespace Util.Common
{
	public class Logger
	{
		private string _ExtendName = "log";

		private string _BaseDirectory;

		private LogLevels _CurrentLevel = LogLevels.Info;

		private object lockObject = new object();

		public string ExtendName
		{
			get
			{
				return _ExtendName;
			}
			set
			{
				_ExtendName = value;
			}
		}

		public string BaseDirectory
		{
			get
			{
				if (string.IsNullOrEmpty(_BaseDirectory))
				{
					_BaseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
				}
				return _BaseDirectory;
			}
			private set
			{
				_BaseDirectory = value;
			}
		}

		public string FullPath
		{
			get
			{
				DateTime now = DateTime.Now;
				string text = Path.Combine(BaseDirectory, now.Year.ToString(), now.Month.ToString());
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				return Path.Combine(text, string.Format("{0}.{1}", now.Day, ExtendName));
			}
		}

		public LogLevels CurrentLevel
		{
			get
			{
				return _CurrentLevel;
			}
			set
			{
				_CurrentLevel = value;
			}
		}

		public Logger()
		{
		}

		public Logger(string basedirectory)
		{
			BaseDirectory = basedirectory;
		}

		public void Write(LogLevels level, string message)
		{
			if (level > CurrentLevel)
			{
				return;
			}
			DoWrite(delegate
			{
				try
				{
					string text = (string.IsNullOrEmpty(Thread.CurrentThread.Name) ? Thread.CurrentThread.ManagedThreadId.ToString() : Thread.CurrentThread.Name);
					using (StreamWriter streamWriter = new StreamWriter(FullPath, true, Encoding.UTF8))
					{
						streamWriter.WriteLine(string.Format("[{0}][{1}][{2}] {3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), text, level, message));
						streamWriter.AutoFlush = true;
					}
				}
				catch (Exception)
				{
				}
			});
		}

		public void Write(LogLevels level, string formatMessage, params object[] paramaters)
		{
			Write(level, string.Format(formatMessage, paramaters));
		}

		public void Info(string message)
		{
			Write(LogLevels.Info, message);
		}

		public void Info(string message, params object[] paramaters)
		{
			Write(LogLevels.Info, message, paramaters);
		}

		public void Debug(string message)
		{
			Write(LogLevels.Debug, message);
		}

		public void Debug(string message, params object[] paramaters)
		{
			Write(LogLevels.Debug, message, paramaters);
		}

		public void Trace(string message)
		{
			Write(LogLevels.Trace, message);
		}

		public void Trace(string message, params object[] paramaters)
		{
			Write(LogLevels.Trace, message, paramaters);
		}

		public void Warn(string message)
		{
			Write(LogLevels.Warn, message);
		}

		public void Warn(string message, params object[] paramaters)
		{
			Write(LogLevels.Warn, message, paramaters);
		}

		public void Write(LogLevels level, Exception ex)
		{
			if (ex == null || level < LogLevels.Warn)
			{
				return;
			}
			try
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (ex != null)
				{
					if (level < LogLevels.Trace)
					{
						stringBuilder.Append(ex.Message);
					}
					else
					{
						stringBuilder.Append(string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace));
					}
				}
				Write(level, stringBuilder.ToString());
			}
			catch (Exception)
			{
			}
		}

		public void Write(LogLevels level, Exception ex, string message)
		{
			if (level < LogLevels.Warn)
			{
				return;
			}
			try
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(message);
				stringBuilder.Append("\r\n");
				if (ex != null)
				{
					if (level < LogLevels.Trace)
					{
						stringBuilder.Append(ex.Message);
					}
					else
					{
						stringBuilder.Append(string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace));
					}
				}
				Write(level, stringBuilder.ToString());
			}
			catch (Exception)
			{
			}
		}

		public void Write(LogLevels level, Exception ex, string formatString, params object[] paramaters)
		{
			if (level < LogLevels.Warn)
			{
				return;
			}
			try
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(string.Format(formatString, paramaters));
				stringBuilder.Append("\r\n");
				if (ex != null)
				{
					if (level < LogLevels.Trace)
					{
						stringBuilder.Append(ex.Message);
					}
					else
					{
						stringBuilder.Append(string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace));
					}
				}
				Write(level, stringBuilder.ToString());
			}
			catch (Exception)
			{
			}
		}

		private void DoWrite(Action func)
		{
			lock (lockObject)
			{
				func();
			}
		}
	}
}
