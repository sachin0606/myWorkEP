using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.IDAL;
using DeXin.EPG.Model;

namespace DeXin.EPG.DAL
{
	public class LogDAL : ILogDAL
	{
		public IList<LogInfo> GetLogDataByPaging(PagingSearch pagingSearch)
		{
			IList<LogInfo> list = new List<LogInfo>();
			LogInfo logInfo = null;
			try
			{
				string pKName = pagingSearch.PKName;
				string columsNames = pagingSearch.ColumsNames;
				string tableName = pagingSearch.TableName;
				string condition = pagingSearch.Condition;
				string orderBy = pagingSearch.OrderBy;
				int pageIndex = pagingSearch.PageIndex;
				int pageSize = pagingSearch.PageSize;
				int TotalCount = pagingSearch.TotalCount;
				DataSet dataSet = SqlHelper.PageDataSet(pKName, columsNames, tableName, condition, orderBy, pageIndex, pageSize, out TotalCount);
				pagingSearch.TotalCount = TotalCount;
				DataTable dataTable = dataSet.Tables[0];
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					DataRow dataRow = dataTable.Rows[i];
					logInfo = new LogInfo();
					logInfo.LogID = Tool.GetString(dataRow["LogID"]);
					logInfo.UserID = Tool.GetString(dataRow["UserID"]);
					logInfo.UserName = Tool.GetString(dataRow["UserName"]);
					logInfo.FunctionID = Tool.GetString(dataRow["FunctionID"]);
					logInfo.OSName = Tool.GetString(dataRow["OSName"]);
					logInfo.Description = Tool.GetString(dataRow["Description"]);
					logInfo.OperateDateTime = Tool.GetDateTime1(dataRow["OperateDateTime"]);
					logInfo.IPAddress = Tool.GetString(dataRow["IPAddress"]);
					list.Add(logInfo);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return list;
		}

		public int AddLog(LogInfo logInfo)
		{
			string cmdText = "INSERT INTO [LogInfoManagement]\r\n                           ([UserID]\r\n                           ,[UserName]\r\n                           ,[FunctionID]\r\n                           ,[OSName]\r\n                           ,[Description]\r\n                           ,[OperateDateTime]\r\n                           ,[IPAddress]\r\n                           ,[ClientTime]\r\n                           )\r\n                     VALUES\r\n                           (@UserID\r\n                           ,@UserName\r\n                           ,@FunctionID\r\n                           ,@OSName\r\n                           ,@Description\r\n                           ,GETDATE()\r\n                           ,@IPAddress\r\n                           ,@ClientTime\r\n                           )";
			try
			{
				SqlParameter[] commandParameters = new SqlParameter[7]
				{
					SqlHelper.AddParameter("@UserID", logInfo.UserID, SqlDbType.NVarChar),
					SqlHelper.AddParameter("@UserName", logInfo.UserName, SqlDbType.NVarChar),
					SqlHelper.AddParameter("@FunctionID", logInfo.FunctionID, SqlDbType.NVarChar),
					SqlHelper.AddParameter("@OSName", logInfo.OSName, SqlDbType.NVarChar),
					SqlHelper.AddParameter("@Description", logInfo.Description, SqlDbType.NVarChar),
					SqlHelper.AddParameter("@IPAddress", logInfo.IPAddress, SqlDbType.NVarChar),
					SqlHelper.AddParameter("@ClientTime", DateTime.Now, SqlDbType.DateTime)
				};
				return SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public bool AddLogs(IList<LogInfo> logs)
		{
			bool flag = true;
			SqlConnection sqlConnection = null;
			SqlTransaction sqlTransaction = null;
			try
			{
				using (sqlConnection = new SqlConnection(SqlHelper.ConnectionString))
				{
					sqlConnection.Open();
					sqlTransaction = sqlConnection.BeginTransaction();
					foreach (LogInfo log in logs)
					{
						if (AddLog(log, sqlTransaction) == 0)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						sqlTransaction.Commit();
					}
					else
					{
						sqlTransaction.Rollback();
					}
				}
			}
			catch (Exception ex)
			{
				if (sqlTransaction != null)
				{
					sqlTransaction.Rollback();
				}
				throw ex;
			}
			finally
			{
				SqlHelper.CloseAll(sqlConnection, null);
			}
			return flag;
		}

		public int AddLog(LogInfo logInfo, SqlTransaction sqltrans)
		{
			string cmdText = "INSERT INTO [LogInfoManagement]\r\n                           ([UserID]\r\n                           ,[UserName]\r\n                           ,[FunctionID]\r\n                           ,[OSName]\r\n                           ,[Description]\r\n                           ,[OperateDateTime]\r\n                           ,[IPAddress]\r\n                           ,[ClientTime]\r\n                           )\r\n                     VALUES\r\n                           (@UserID\r\n                           ,@UserName\r\n                           ,@FunctionID\r\n                           ,@OSName\r\n                           ,@Description\r\n                           ,GETDATE()\r\n                           ,@IPAddress\r\n                           ,@ClientTime\r\n                            )\r\n                           ";
			try
			{
				SqlParameter[] commandParameters = new SqlParameter[7]
				{
					SqlHelper.AddParameter("@UserID", logInfo.UserID, SqlDbType.NVarChar),
					SqlHelper.AddParameter("@UserName", logInfo.UserName, SqlDbType.NVarChar),
					SqlHelper.AddParameter("@FunctionID", logInfo.FunctionID, SqlDbType.NVarChar),
					SqlHelper.AddParameter("@OSName", logInfo.OSName, SqlDbType.NVarChar),
					SqlHelper.AddParameter("@Description", logInfo.Description, SqlDbType.NVarChar),
					SqlHelper.AddParameter("@IPAddress", logInfo.IPAddress, SqlDbType.NVarChar),
					SqlHelper.AddParameter("@ClientTime", DateTime.Now, SqlDbType.DateTime)
				};
				return SqlHelper.ExecuteNonQuery(sqltrans, CommandType.Text, cmdText, commandParameters);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
