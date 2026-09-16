using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.IDAL;
using DeXin.EPG.Model;

namespace DeXin.EPG.DAL
{
	public class PermitGroupManagementDAL : IPermitGroupManagementDAL
	{
		public IList<PermitGroup> GetPermitGroupDataByPaging(PagingSearch pagingSearch, string groupName, string LocFunctionName)
		{
			if (pagingSearch == null)
			{
				throw new NoNullAllowedException();
			}
			IList<PermitGroup> list = new List<PermitGroup>();
			PermitGroup permitGroup = null;
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
				string text = "";
				text = (string.IsNullOrEmpty(groupName) ? "" : ("AND CHARINDEX('" + groupName + "',GroupName)>0"));
				condition = text;
				DataSet dataSet = SqlHelper.PageDataSet(pKName, columsNames, tableName, condition, orderBy, pageIndex, pageSize, out TotalCount);
				pagingSearch.TotalCount = TotalCount;
				DataTable dataTable = dataSet.Tables[0];
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					DataRow dataRow = dataTable.Rows[i];
					permitGroup = new PermitGroup();
					permitGroup.GroupID = Tool.GetInt32(dataRow["GroupID"]);
					permitGroup.FunctionID = Tool.GetString(dataRow["FunctionID"]);
					permitGroup.CanAdd = Tool.GetBoolean(dataRow["CanAdd"]);
					permitGroup.CanDelete = Tool.GetBoolean(dataRow["CanDelete"]);
					permitGroup.CanEdit = Tool.GetBoolean(dataRow["CanEdit"]);
					permitGroup.CanPrint = Tool.GetBoolean(dataRow["CanPrint"]);
					permitGroup.CanQuery = Tool.GetBoolean(dataRow["CanQuery"]);
					permitGroup.Creator = Tool.GetString(dataRow["Creator"]);
					permitGroup.CreateTime = Tool.GetDateTime1(dataRow["CreateTime"]);
					permitGroup.LastEditor = Tool.GetString(dataRow["LastEditor"]);
					permitGroup.LastEditTime = Tool.GetDateTime1(dataRow["LastEditTime"]);
					permitGroup.GroupName = Tool.GetString(dataRow["GroupName"]);
					list.Add(permitGroup);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return list.Where((PermitGroup p) => p.LocationFunctionName.Contains(LocFunctionName)).ToList();
		}

		public IList<PermitGroup> GetPermitGroupDataByPaging(PagingSearch pagingSearch)
		{
			if (pagingSearch == null)
			{
				throw new NoNullAllowedException();
			}
			IList<PermitGroup> list = new List<PermitGroup>();
			PermitGroup permitGroup = null;
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
					permitGroup = new PermitGroup();
					permitGroup.GroupID = Tool.GetInt32(dataRow["GroupID"]);
					permitGroup.FunctionID = Tool.GetString(dataRow["FunctionID"]);
					permitGroup.CanAdd = Tool.GetBoolean(dataRow["CanAdd"]);
					permitGroup.CanDelete = Tool.GetBoolean(dataRow["CanDelete"]);
					permitGroup.CanEdit = Tool.GetBoolean(dataRow["CanEdit"]);
					permitGroup.CanPrint = Tool.GetBoolean(dataRow["CanPrint"]);
					permitGroup.CanQuery = Tool.GetBoolean(dataRow["CanQuery"]);
					permitGroup.Creator = Tool.GetString(dataRow["Creator"]);
					permitGroup.CreateTime = Tool.GetDateTime1(dataRow["CreateTime"]);
					permitGroup.LastEditor = Tool.GetString(dataRow["LastEditor"]);
					permitGroup.LastEditTime = Tool.GetDateTime1(dataRow["LastEditTime"]);
					permitGroup.GroupName = Tool.GetString(dataRow["GroupName"]);
					list.Add(permitGroup);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return list;
		}

		public IList<PermitGroup> GetDataByGroupID(int groupID)
		{
			IList<PermitGroup> list = new List<PermitGroup>();
			string cmdText = "SELECT * FROM V_PermitGroupManagement WHERE GroupID=@GroupID ";
			PermitGroup permitGroup = null;
			SqlDataReader sqlDataReader = null;
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@GroupID", groupID, SqlDbType.Int) };
			try
			{
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
				while (sqlDataReader.Read())
				{
					permitGroup = new PermitGroup();
					permitGroup.GroupID = Tool.GetInt32(sqlDataReader["GroupID"]);
					permitGroup.FunctionID = Tool.GetString(sqlDataReader["FunctionID"]);
					permitGroup.CanAdd = Tool.GetBoolean(sqlDataReader["CanAdd"]);
					permitGroup.CanDelete = Tool.GetBoolean(sqlDataReader["CanDelete"]);
					permitGroup.CanEdit = Tool.GetBoolean(sqlDataReader["CanEdit"]);
					permitGroup.CanPrint = Tool.GetBoolean(sqlDataReader["CanPrint"]);
					permitGroup.CanQuery = Tool.GetBoolean(sqlDataReader["CanQuery"]);
					permitGroup.Creator = Tool.GetString(sqlDataReader["Creator"]);
					permitGroup.CreateTime = Tool.GetDateTime1(sqlDataReader["CreateTime"]);
					permitGroup.LastEditor = Tool.GetString(sqlDataReader["LastEditor"]);
					permitGroup.LastEditTime = Tool.GetDateTime1(sqlDataReader["LastEditTime"]);
					permitGroup.GroupName = Tool.GetString(sqlDataReader["GroupName"]);
					permitGroup.GroupStatus = Tool.GetBoolean(sqlDataReader["Status"]);
					list.Add(permitGroup);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				SqlHelper.CloseAll(null, sqlDataReader);
			}
			return list;
		}

		public IList<PermitGroup> GetDataByGroupIDAndGroupStatus(int groupID, bool status)
		{
			IList<PermitGroup> list = new List<PermitGroup>();
			string cmdText = "SELECT * FROM V_PermitGroupManagement WHERE GroupID=@GroupID and Status=@Status ";
			PermitGroup permitGroup = null;
			SqlDataReader sqlDataReader = null;
			SqlParameter[] commandParameters = new SqlParameter[2]
			{
				SqlHelper.AddParameter("@GroupID", groupID, SqlDbType.Int),
				SqlHelper.AddParameter("@Status", status, SqlDbType.Bit)
			};
			try
			{
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
				while (sqlDataReader.Read())
				{
					permitGroup = new PermitGroup();
					permitGroup.GroupID = Tool.GetInt32(sqlDataReader["GroupID"]);
					permitGroup.FunctionID = Tool.GetString(sqlDataReader["FunctionID"]);
					permitGroup.CanAdd = Tool.GetBoolean(sqlDataReader["CanAdd"]);
					permitGroup.CanDelete = Tool.GetBoolean(sqlDataReader["CanDelete"]);
					permitGroup.CanEdit = Tool.GetBoolean(sqlDataReader["CanEdit"]);
					permitGroup.CanPrint = Tool.GetBoolean(sqlDataReader["CanPrint"]);
					permitGroup.CanQuery = Tool.GetBoolean(sqlDataReader["CanQuery"]);
					permitGroup.Creator = Tool.GetString(sqlDataReader["Creator"]);
					permitGroup.CreateTime = Tool.GetDateTime1(sqlDataReader["CreateTime"]);
					permitGroup.LastEditor = Tool.GetString(sqlDataReader["LastEditor"]);
					permitGroup.LastEditTime = Tool.GetDateTime1(sqlDataReader["LastEditTime"]);
					permitGroup.GroupName = Tool.GetString(sqlDataReader["GroupName"]);
					list.Add(permitGroup);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				SqlHelper.CloseAll(null, sqlDataReader);
			}
			return list;
		}

		public IList<PermitGroup> GetAllData()
		{
			IList<PermitGroup> list = new List<PermitGroup>();
			string cmdText = "SELECT * FROM V_PermitGroupManagement";
			PermitGroup permitGroup = null;
			SqlDataReader sqlDataReader = null;
			try
			{
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, (SqlParameter[])null);
				while (sqlDataReader.Read())
				{
					permitGroup = new PermitGroup();
					permitGroup.GroupID = Tool.GetInt32(sqlDataReader["GroupID"]);
					permitGroup.FunctionID = Tool.GetString(sqlDataReader["FunctionID"]);
					permitGroup.CanAdd = Tool.GetBoolean(sqlDataReader["CanAdd"]);
					permitGroup.CanDelete = Tool.GetBoolean(sqlDataReader["CanDelete"]);
					permitGroup.CanEdit = Tool.GetBoolean(sqlDataReader["CanEdit"]);
					permitGroup.CanPrint = Tool.GetBoolean(sqlDataReader["CanPrint"]);
					permitGroup.CanQuery = Tool.GetBoolean(sqlDataReader["CanQuery"]);
					permitGroup.Creator = Tool.GetString(sqlDataReader["Creator"]);
					permitGroup.CreateTime = Tool.GetDateTime1(sqlDataReader["CreateTime"]);
					permitGroup.LastEditor = Tool.GetString(sqlDataReader["LastEditor"]);
					permitGroup.LastEditTime = Tool.GetDateTime1(sqlDataReader["LastEditTime"]);
					permitGroup.GroupName = Tool.GetString(sqlDataReader["GroupName"]);
					list.Add(permitGroup);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				SqlHelper.CloseAll(null, sqlDataReader);
			}
			return list;
		}

		public DataSet GetDataSet()
		{
			string commandText = "SELECT * FROM V_PermitGroupManagement ";
			return SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, commandText);
		}

		public IList<PermitGroup> GetDataByGroupIDAndFunctionID(int groupID, string functionID)
		{
			IList<PermitGroup> list = new List<PermitGroup>();
			string cmdText = "SELECT * FROM PermitGroupManagement WHERE GroupID=@GroupID AND FunctionID=@FunctionID ";
			PermitGroup permitGroup = null;
			SqlDataReader sqlDataReader = null;
			SqlParameter[] commandParameters = new SqlParameter[2]
			{
				SqlHelper.AddParameter("@GroupID", groupID, SqlDbType.Int),
				SqlHelper.AddParameter("@FunctionID", functionID, SqlDbType.NVarChar)
			};
			try
			{
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
				while (sqlDataReader.Read())
				{
					permitGroup = new PermitGroup();
					permitGroup.GroupID = Tool.GetInt32(sqlDataReader["GroupID"]);
					permitGroup.FunctionID = Tool.GetString(sqlDataReader["FunctionID"]);
					permitGroup.CanAdd = Tool.GetBoolean(sqlDataReader["CanAdd"]);
					permitGroup.CanDelete = Tool.GetBoolean(sqlDataReader["CanDelete"]);
					permitGroup.CanEdit = Tool.GetBoolean(sqlDataReader["CanEdit"]);
					permitGroup.CanPrint = Tool.GetBoolean(sqlDataReader["CanPrint"]);
					permitGroup.CanQuery = Tool.GetBoolean(sqlDataReader["CanQuery"]);
					permitGroup.Creator = Tool.GetString(sqlDataReader["Creator"]);
					permitGroup.CreateTime = Tool.GetDateTime1(sqlDataReader["CreateTime"]);
					permitGroup.LastEditor = Tool.GetString(sqlDataReader["LastEditor"]);
					permitGroup.LastEditTime = Tool.GetDateTime1(sqlDataReader["LastEditTime"]);
					list.Add(permitGroup);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				SqlHelper.CloseAll(null, sqlDataReader);
			}
			return list;
		}

		public int Add(PermitGroup permitInfo)
		{
			string cmdText = "INSERT INTO [PermitGroupManagement]\r\n                           ([GroupID]\r\n                           ,[FunctionID]\r\n                           ,[CanAdd]\r\n                           ,[CanDelete]\r\n                           ,[CanEdit]\r\n                           ,[CanQuery]\r\n                           ,[CanPrint]\r\n                           ,[Creator]\r\n                           ,[CreateTime]\r\n                           ,[LastEditor]\r\n                           ,[LastEditTime])\r\n                     VALUES\r\n                           (@GroupID,\r\n                           ,@FunctionID,\r\n                           ,@CanAdd,\r\n                           ,@CanDelete,\r\n                           ,@CanEdit,\r\n                           ,@CanQuery, \r\n                           ,@CanPrint, \r\n                           ,@Creator, \r\n                           ,@CreateTime,\r\n                           ,@LastEditor,\r\n                           ,@LastEditTime)";
			try
			{
				SqlParameter[] commandParameters = new SqlParameter[10]
				{
					SqlHelper.AddParameter("@FunctionID", permitInfo.FunctionID, SqlDbType.Int),
					SqlHelper.AddParameter("@CanAdd", permitInfo.CanAdd, SqlDbType.Bit),
					SqlHelper.AddParameter("@CanDelete", permitInfo.CanDelete, SqlDbType.Bit),
					SqlHelper.AddParameter("@CanEdit", permitInfo.CanEdit, SqlDbType.Bit),
					SqlHelper.AddParameter("@CanQuery", permitInfo.CanQuery, SqlDbType.Bit),
					SqlHelper.AddParameter("@CanPrint", permitInfo.CanPrint, SqlDbType.Bit),
					SqlHelper.AddParameter("@Creator", permitInfo.Creator, SqlDbType.NVarChar),
					SqlHelper.AddParameter("@CreateTime", permitInfo.CreateTime, SqlDbType.DateTime),
					SqlHelper.AddParameter("@LastEditor", permitInfo.LastEditor, SqlDbType.NVarChar),
					SqlHelper.AddParameter("@LastEditTime", permitInfo.LastEditTime, SqlDbType.DateTime)
				};
				return SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private int Add(PermitGroup permitInfo, SqlTransaction sqltrans)
		{
			string cmdText = "INSERT INTO [PermitGroupManagement]\r\n                           ([GroupID]\r\n                            ,[FunctionID]\r\n                           ,[CanAdd]\r\n                           ,[CanDelete]\r\n                           ,[CanEdit]\r\n                           ,[CanQuery]\r\n                           ,[CanPrint]\r\n                           ,[Creator]\r\n                           ,[CreateTime]\r\n                           ,[LastEditor]\r\n                           ,[LastEditTime])\r\n                     VALUES\r\n                           (\r\n                            @GroupID\r\n                            ,@FunctionID\r\n                           ,@CanAdd\r\n                           ,@CanDelete\r\n                           ,@CanEdit\r\n                           ,@CanQuery\r\n                           ,@CanPrint \r\n                           ,@Creator \r\n                           ,@CreateTime\r\n                           ,@LastEditor\r\n                           ,@LastEditTime)";
			SqlParameter[] commandParameters = new SqlParameter[11]
			{
				SqlHelper.AddParameter("@GroupID", permitInfo.GroupID, SqlDbType.Int),
				SqlHelper.AddParameter("@FunctionID", permitInfo.FunctionID, SqlDbType.NVarChar),
				SqlHelper.AddParameter("@CanAdd", permitInfo.CanAdd, SqlDbType.Bit),
				SqlHelper.AddParameter("@CanDelete", permitInfo.CanDelete, SqlDbType.Bit),
				SqlHelper.AddParameter("@CanEdit", permitInfo.CanEdit, SqlDbType.Bit),
				SqlHelper.AddParameter("@CanQuery", permitInfo.CanQuery, SqlDbType.Bit),
				SqlHelper.AddParameter("@CanPrint", permitInfo.CanPrint, SqlDbType.Bit),
				SqlHelper.AddParameter("@Creator", permitInfo.Creator, SqlDbType.NVarChar),
				SqlHelper.AddParameter("@CreateTime", permitInfo.CreateTime, SqlDbType.DateTime),
				SqlHelper.AddParameter("@LastEditor", permitInfo.LastEditor, SqlDbType.NVarChar),
				SqlHelper.AddParameter("@LastEditTime", permitInfo.LastEditTime, SqlDbType.DateTime)
			};
			return SqlHelper.ExecuteNonQuery(sqltrans, CommandType.Text, cmdText, commandParameters);
		}

		public int AddWithSqlTrans(List<PermitGroup> lstPermitInfo)
		{
			int num = 0;
			using (SqlConnection sqlConnection = new SqlConnection(SqlHelper.ConnectionString))
			{
				sqlConnection.Open();
				SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
				try
				{
					foreach (PermitGroup item in lstPermitInfo)
					{
						Add(item, sqlTransaction);
						num++;
					}
					sqlTransaction.Commit();
					return num;
				}
				catch (Exception ex)
				{
					sqlTransaction.Rollback();
					throw ex;
				}
				finally
				{
					if (sqlConnection.State == ConnectionState.Open)
					{
						sqlConnection.Close();
					}
				}
			}
		}

		public int AddWithSqlTrans(IList<PermitGroup> lstPermitInfo, IList<PermitGroup> lstPermitInfoOriginal)
		{
			int num = 0;
			using (SqlConnection sqlConnection = new SqlConnection(SqlHelper.ConnectionString))
			{
				sqlConnection.Open();
				SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
				try
				{
					foreach (PermitGroup item in lstPermitInfoOriginal)
					{
						Delete(item, sqlTransaction);
					}
					foreach (PermitGroup item2 in lstPermitInfo)
					{
						Add(item2, sqlTransaction);
					}
					sqlTransaction.Commit();
					return 1;
				}
				catch (Exception ex)
				{
					sqlTransaction.Rollback();
					throw ex;
				}
				finally
				{
					if (sqlConnection.State == ConnectionState.Open)
					{
						sqlConnection.Close();
					}
				}
			}
		}

		public int Update(PermitGroup permitInfo)
		{
			string cmdText = "UPDATE [PermitGroupManagement]\r\n                           SET [GroupID] = @GroupID, \r\n                              [FunctionID] = @FunctionID, \r\n                              [CanAdd] = @CanAdd,\r\n                              [CanDelete] = @CanDelete,\r\n                              [CanEdit] = @CanEdit,\r\n                              [CanQuery] = @CanQuery,\r\n                              [CanPrint] = @CanPrint, \r\n                            \r\n                              [LastEditor] = @LastEditor, \r\n                              [LastEditTime] = GETDATE()\r\n                         WHERE GroupID=@GroupID AND FunctionID=@FunctionID";
			SqlParameter[] commandParameters = new SqlParameter[8]
			{
				SqlHelper.AddParameter("@GroupID", permitInfo.GroupID, SqlDbType.Int),
				SqlHelper.AddParameter("@FunctionID", permitInfo.FunctionID, SqlDbType.Int),
				SqlHelper.AddParameter("@CanAdd", permitInfo.CanAdd, SqlDbType.Bit),
				SqlHelper.AddParameter("@CanDelete", permitInfo.CanDelete, SqlDbType.Bit),
				SqlHelper.AddParameter("@CanEdit", permitInfo.CanEdit, SqlDbType.Bit),
				SqlHelper.AddParameter("@CanQuery", permitInfo.CanQuery, SqlDbType.Bit),
				SqlHelper.AddParameter("@CanPrint", permitInfo.CanPrint, SqlDbType.Bit),
				SqlHelper.AddParameter("@LastEditor", permitInfo.LastEditor, SqlDbType.NVarChar)
			};
			return SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
		}

		public int Delete(PermitGroup permitInfo)
		{
			string cmdText = "DELETE FROM PermitGroupManagement WHERE  GroupID=@GroupID AND FunctionID=@FunctionID";
			try
			{
				SqlParameter[] commandParameters = new SqlParameter[2]
				{
					SqlHelper.AddParameter("@GroupID", permitInfo.GroupID, SqlDbType.Int),
					SqlHelper.AddParameter("@FunctionID", permitInfo.FunctionID, SqlDbType.NVarChar)
				};
				return SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private int Delete(PermitGroup permitInfo, SqlTransaction sqltrans)
		{
			string cmdText = "DELETE FROM PermitGroupManagement WHERE  GroupID=@GroupID AND FunctionID=@FunctionID";
			SqlParameter[] commandParameters = new SqlParameter[2]
			{
				SqlHelper.AddParameter("@GroupID", permitInfo.GroupID, SqlDbType.Int),
				SqlHelper.AddParameter("@FunctionID", permitInfo.FunctionID, SqlDbType.NVarChar)
			};
			return SqlHelper.ExecuteNonQuery(sqltrans, CommandType.Text, cmdText, commandParameters);
		}

		public int DeleteWithSqlTrans(List<PermitGroup> lstPermitInfo)
		{
			int num = 0;
			using (SqlConnection sqlConnection = new SqlConnection(SqlHelper.ConnectionString))
			{
				sqlConnection.Open();
				SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
				try
				{
					foreach (PermitGroup item in lstPermitInfo)
					{
						Delete(item, sqlTransaction);
						num++;
					}
					sqlTransaction.Commit();
					return num;
				}
				catch (Exception ex)
				{
					sqlTransaction.Rollback();
					throw ex;
				}
				finally
				{
					if (sqlConnection.State == ConnectionState.Open)
					{
						sqlConnection.Close();
					}
				}
			}
		}
	}
}
