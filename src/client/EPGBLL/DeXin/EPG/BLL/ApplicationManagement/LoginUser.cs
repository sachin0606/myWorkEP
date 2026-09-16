using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Sockets;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.Model;

namespace DeXin.EPG.BLL.ApplicationManagement
{
	public class LoginUser
	{
		private UserInfo _user;

		private static LoginUser _ins;

		private DateTime dt = DateTime.MinValue;

		public UserInfo UserIns
		{
			get
			{
				return _user;
			}
			set
			{
				_user = value;
			}
		}

		public static LoginUser Ins
		{
			get
			{
				if (_ins == null)
				{
					_ins = new LoginUser();
				}
				return _ins;
			}
		}

		public DateTime GetshortTime
		{
			get
			{
				DateTime minValue = DateTime.MinValue;
				try
				{
					DateTime nowTime = GetNowTime();
					return new DateTime(nowTime.Year, nowTime.Month, nowTime.Day);
				}
				catch
				{
					return DateTime.MinValue;
				}
			}
		}

		private LoginUser()
		{
		}

		public DateTime GetNowTime()
		{
			return Tool.GetDate(SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.StoredProcedure, "sp_GetNowTime", (SqlParameter[])null));
		}

		public string GetClientIP()
		{
			string result = "";
			IPHostEntry hostEntry = Dns.GetHostEntry("");
			IPAddress[] addressList = hostEntry.AddressList;
			foreach (IPAddress iPAddress in addressList)
			{
				if (iPAddress.AddressFamily == AddressFamily.InterNetwork)
				{
					result = iPAddress.ToString();
					break;
				}
			}
			return result;
		}
	}
}
