using System;
using System.Globalization;
using System.Threading;

namespace Util.Common
{
	public static class ThreadHelpers
	{
		public static Thread CreateThread(ThreadStart action, string threadName, CultureInfo currentUICulture = null, CultureInfo currentCulture = null)
		{
			Thread thread = new Thread(action);
			thread.CurrentUICulture = ((currentUICulture == null) ? Thread.CurrentThread.CurrentUICulture : currentUICulture);
			thread.CurrentCulture = ((currentCulture == null) ? Thread.CurrentThread.CurrentCulture : currentCulture);
			if (!string.IsNullOrEmpty(threadName))
			{
				thread.Name = threadName;
			}
			return thread;
		}

		public static Thread CreateThread(ParameterizedThreadStart action, string threadName, CultureInfo currentUICulture = null, CultureInfo currentCulture = null)
		{
			Thread thread = new Thread(action);
			thread.CurrentUICulture = ((currentUICulture == null) ? Thread.CurrentThread.CurrentUICulture : currentUICulture);
			thread.CurrentCulture = ((currentCulture == null) ? Thread.CurrentThread.CurrentCulture : currentCulture);
			if (!string.IsNullOrEmpty(threadName))
			{
				thread.Name = threadName;
			}
			return thread;
		}

		public static void DoAbort(this Thread thread, ActionWithParametersHandler<Exception> action)
		{
			try
			{
				thread.Abort();
			}
			catch (Exception ex)
			{
				if (action != null)
				{
					action(ex);
				}
			}
		}

		public static void DoAbort(this Thread thread)
		{
			thread.DoAbort(null);
		}
	}
}
