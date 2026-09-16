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
	public class GroupInfoDAL : IGroupInfoDAL
	{
		public IList<GroupInfo> GetAllData()
		{
			IList<GroupInfo> list = new List<GroupInfo>();
			string cmdText = "SELECT * FROM GroupInfo";
			GroupInfo groupInfo = null;
			SqlDataReader sqlDataReader = null;
			try
			{
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, (SqlParameter[])null);
				while (sqlDataReader.Read())
				{
					groupInfo = new GroupInfo();
					groupInfo.GroupID = Tool.GetInt32(sqlDataReader["GroupID"]);
					groupInfo.GroupName = Tool.GetString(sqlDataReader["GroupName"]);
					groupInfo.Status = Tool.GetBoolean(sqlDataReader["Status"]);
					groupInfo.Creator = Tool.GetString(sqlDataReader["Creator"]);
					groupInfo.CreateTime = Tool.GetDateTime1(sqlDataReader["CreateTime"]);
					groupInfo.LastEditor = Tool.GetString(sqlDataReader["LastEditor"]);
					groupInfo.LastEditTime = Tool.GetDateTime1(sqlDataReader["LastEditTime"]);
					list.Add(groupInfo);
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

		public IList<GroupInfo> GetDataByGroupID(int intGroupID)
		{
			IList<GroupInfo> list = new List<GroupInfo>();
			string cmdText = "SELECT * FROM GroupInfo WHERE GroupID=@GroupID ";
			GroupInfo groupInfo = null;
			SqlDataReader sqlDataReader = null;
			SqlParameter sqlParameter = new SqlParameter("@GroupID", intGroupID);
			try
			{
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, sqlParameter);
				while (sqlDataReader.Read())
				{
					groupInfo = new GroupInfo();
					groupInfo.GroupID = Tool.GetInt32(sqlDataReader["GroupID"]);
					groupInfo.GroupName = Tool.GetString(sqlDataReader["GroupName"]);
					groupInfo.Status = Tool.GetBoolean(sqlDataReader["Status"]);
					groupInfo.Creator = Tool.GetString(sqlDataReader["Creator"]);
					groupInfo.CreateTime = Tool.GetDateTime1(sqlDataReader["CreateTime"]);
					groupInfo.LastEditor = Tool.GetString(sqlDataReader["LastEditor"]);
					groupInfo.LastEditTime = Tool.GetDateTime1(sqlDataReader["LastEditTime"]);
					list.Add(groupInfo);
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

		public int Add(GroupInfo myGroupInfo)
		{
			string cmdText = "INSERT INTO  GroupInfo( GroupName ,Status ,Creator ,CreateTime ,LastEditor ,LastEditTime)VALUES \r\n               (@GroupName,@Status,@Creator,@CreateTime,@LastEditor,@LastEditTime)";
			try
			{
				SqlParameter[] commandParameters = new SqlParameter[6]
				{
					SqlHelper.AddParameter("@GroupName", myGroupInfo.GroupName, SqlDbType.NVarChar),
					SqlHelper.AddParameter("@Status", myGroupInfo.Status, SqlDbType.NVarChar),
					SqlHelper.AddParameter("@Creator", myGroupInfo.Creator, SqlDbType.NVarChar),
					SqlHelper.AddParameter("@CreateTime", myGroupInfo.CreateTime, SqlDbType.DateTime),
					SqlHelper.AddParameter("@LastEditor", myGroupInfo.LastEditor, SqlDbType.NVarChar),
					SqlHelper.AddParameter("@LastEditTime", myGroupInfo.LastEditTime, SqlDbType.DateTime)
				};
				return SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Delete(GroupInfo myGroupInfo)
		{
			string cmdText = "DELETE FROM GroupInfo WHERE  GroupID=@GroupID";
			try
			{
				SqlParameter sqlParameter = SqlHelper.AddParameter("@GroupID", myGroupInfo.GroupID, SqlDbType.Int);
				return SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, cmdText, sqlParameter);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private int Delete(GroupInfo myGroupInfo, SqlTransaction sqltrans)
		{
			string cmdText = "DELETE FROM GroupInfo WHERE  GroupID=@GroupID";
			try
			{
				SqlParameter sqlParameter = SqlHelper.AddParameter("@GroupID", myGroupInfo.GroupID, SqlDbType.Int);
				return SqlHelper.ExecuteNonQuery(sqltrans, CommandType.Text, cmdText, sqlParameter);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int DeleteWithTrans(List<GroupInfo> lstPermitInfo)
		{
			int num = 0;
			using (SqlConnection sqlConnection = new SqlConnection(SqlHelper.ConnectionString))
			{
				sqlConnection.Open();
				SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
				try
				{
					foreach (GroupInfo item in lstPermitInfo)
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

		public int Update(GroupInfo myGroupInfo)
		{
			string cmdText = "UPDATE [GroupInfo]SET [GroupName] = @GroupName,[Status] = @Status,[LastEditor] = @LastEditor,[LastEditTime] = GETDATE()\r\n                        WHERE GroupID=@GroupID";
			try
			{
				SqlParameter[] commandParameters = new SqlParameter[4]
				{
					SqlHelper.AddParameter("@GroupID", myGroupInfo.GroupID, SqlDbType.Int),
					SqlHelper.AddParameter("@GroupName", myGroupInfo.GroupName, SqlDbType.NVarChar),
					SqlHelper.AddParameter("@Status", myGroupInfo.Status, SqlDbType.Bit),
					SqlHelper.AddParameter("@LastEditor", myGroupInfo.LastEditor, SqlDbType.NVarChar)
				};
				return SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
