using System;

namespace EPGServiceBLL
{
	public static class DateTimeHelper
	{
		public static DateTime GetUTCDateTimeNow()
		{
			return DateTime.UtcNow;
		}
	}
}
