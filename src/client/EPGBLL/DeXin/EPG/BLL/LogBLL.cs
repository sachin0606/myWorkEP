using System;
using System.Collections.Generic;
using Com.Cn.Tool;
using DeXin.EPG.BLL.ApplicationManagement;
using DeXin.EPG.DALFactory;
using DeXin.EPG.Model;
using DeXin.EPG.Tools;

namespace DeXin.EPG.BLL
{
	public class LogBLL
	{
		private AbstractFactory factory = AbstractFactory.CreateFactory();

		public IList<LogInfo> GetLogDataByPaging(PagingSearch pagingSearch)
		{
			try
			{
				return factory.CreateLogDAL().GetLogDataByPaging(pagingSearch);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private int AddLog(LogInfo logInfo)
		{
			int num = 0;
			try
			{
				factory.CreateLogDAL().AddLog(logInfo);
				return 1;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int AddLog(EnumFunctionName enumFunctionName, EnumOSName OSName, string remark)
		{
			LogInfo logInfo = new LogInfo();
			logInfo.LogID = null;
			logInfo.UserID = LoginUser.Ins.UserIns.UserID;
			logInfo.UserName = LoginUser.Ins.UserIns.UserName;
			logInfo.FunctionID = Tool.GetValueOfEnumByName(typeof(EnumFunctionName), enumFunctionName.ToString()).ToString();
			logInfo.OSName = OSName.ToString();
			logInfo.Description = remark;
			logInfo.OperateDateTime = null;
			logInfo.IPAddress = LoginUser.Ins.GetClientIP();
			return AddLog(logInfo);
		}

		public int AddLog(EnumOSName OSName, string remark)
		{
			return AddLog(EnumFunctionName.SysFuncAllFunctions, OSName, remark);
		}

		private bool AddLog(IList<LogInfo> logs)
		{
			try
			{
				return factory.CreateLogDAL().AddLogs(logs);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
