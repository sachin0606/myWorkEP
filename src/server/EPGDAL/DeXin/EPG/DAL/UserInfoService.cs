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
	public class UserInfoService : IUserInfo
	{
		public UserInfo GetUserInfoByLoginName(string loginName)
		{
			throw new NotImplementedException();
		}

		public UserInfo GetUserByID(string userId)
		{
			UserInfo userInfo = null;
			SqlDataReader sqlDataReader = null;
			try
			{
				string cmdText = "SELECT UserID, UserName, Password, GroupID, Sex, PhoneNumber, Creator, CreateTime, LastEditor,LastEditTime, Status FROM UserInfo where UserID = @UserID";
				SqlParameter sqlParameter = SqlHelper.AddParameter("@UserID", userId);
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, sqlParameter);
				while (sqlDataReader.Read())
				{
					userInfo = new UserInfo();
					userInfo.UserID = Tool.GetString(sqlDataReader["UserID"]);
					userInfo.UserName = Tool.GetString(sqlDataReader["UserName"]);
					userInfo.Password = Tool.GetString(sqlDataReader["Password"]);
					userInfo.GroupID = Tool.GetString(sqlDataReader["GroupID"]);
					userInfo.Sex = Tool.GetBoolean(sqlDataReader["Sex"]);
					userInfo.PhoneNumber = Tool.GetString(sqlDataReader["PhoneNumber"]);
					userInfo.Creator = Tool.GetString(sqlDataReader["Creator"]);
					userInfo.CreateTime = Tool.GetDateTime1(sqlDataReader["CreateTime"]);
					userInfo.LastEditor = Tool.GetString(sqlDataReader["LastEditor"]);
					userInfo.LastEditTime = Tool.GetDateTime1(sqlDataReader["LastEditTime"]);
					userInfo.Status = Tool.GetBoolean(sqlDataReader["Status"]);
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
			return userInfo;
		}

		public IList<UserInfo> GetUserByGroupID(string groupId)
		{
			IList<UserInfo> list = new List<UserInfo>();
			UserInfo userInfo = null;
			SqlDataReader sqlDataReader = null;
			try
			{
				string cmdText = "SELECT UserID, UserName, Password, GroupID, Sex, PhoneNumber, Creator, CreateTime, LastEditor,LastEditTime, Status FROM UserInfo where GroupID = @GroupID";
				SqlParameter sqlParameter = SqlHelper.AddParameter("@GroupID", groupId);
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, sqlParameter);
				while (sqlDataReader.Read())
				{
					userInfo = new UserInfo();
					userInfo.UserID = Tool.GetString(sqlDataReader["UserID"]);
					userInfo.UserName = Tool.GetString(sqlDataReader["UserName"]);
					userInfo.Password = Tool.GetString(sqlDataReader["Password"]);
					userInfo.GroupID = Tool.GetString(sqlDataReader["GroupID"]);
					userInfo.Sex = Tool.GetBoolean(sqlDataReader["Sex"]);
					userInfo.PhoneNumber = Tool.GetString(sqlDataReader["PhoneNumber"]);
					userInfo.Creator = Tool.GetString(sqlDataReader["Creator"]);
					userInfo.CreateTime = Tool.GetDateTime1(sqlDataReader["CreateTime"]);
					userInfo.LastEditor = Tool.GetString(sqlDataReader["LastEditor"]);
					userInfo.LastEditTime = Tool.GetDateTime1(sqlDataReader["LastEditTime"]);
					userInfo.Status = Tool.GetBoolean(sqlDataReader["Status"]);
					list.Add(userInfo);
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

		public int Add_UpdateUserInfo(UserInfo user)
		{
			int num = 0;
			try
			{
				string cmdText = "sp_MangementUserInfo";
				SqlParameter[] commandParameters = new SqlParameter[10]
				{
					SqlHelper.AddParameter("@UserID", user.UserID, SqlDbType.NVarChar, 64),
					SqlHelper.AddParameter("@UserName", user.UserName, SqlDbType.NVarChar, 64),
					SqlHelper.AddParameter("@Password", user.Password, SqlDbType.NVarChar, 64),
					SqlHelper.AddParameter("@GroupID", user.GroupID, SqlDbType.NVarChar, 64),
					SqlHelper.AddParameter("@Sex", user.Sex, SqlDbType.Bit),
					SqlHelper.AddParameter("@PhoneNumber", user.PhoneNumber, SqlDbType.NVarChar, 64),
					SqlHelper.AddParameter("@Creator", user.Creator, SqlDbType.NVarChar, 64),
					SqlHelper.AddParameter("@CreateTime", user.CreateTime),
					SqlHelper.AddParameter("@LastEditor", user.LastEditor, SqlDbType.NVarChar, 64),
					SqlHelper.AddParameter("@Status", user.Status, SqlDbType.Bit)
				};
				return SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.StoredProcedure, cmdText, commandParameters);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public IList<UserInfo> SearchUserInfosByCondiction(string userID, string userName, string sex, string groupID, int PageIndex, int PageSize)
		{
			string pKName = "UserID";
			string clolumnNames = " UserID, UserName, Password, GroupID, Sex, PhoneNumber, Creator, CreateTime, LastEditor, LastEditTime, Status";
			string tblName = " UserInfo";
			string condiction = ((userID.Length == 0) ? "" : ("  AND CHARINDEX('" + userID + "',UserID) >= 1")) + ((userName.Length == 0) ? "" : ("  AND CHARINDEX('" + userName + "',UserName) >= 1")) + ((sex.Length == 0) ? "" : ("  AND Sex = " + sex)) + ((groupID.Length == 0) ? "" : ("  AND GroupID = '" + groupID + "'"));
			string orderBy = " UserName";
			IList<UserInfo> list = new List<UserInfo>();
			SqlDataReader sqlDataReader = null;
			UserInfo userInfo = null;
			try
			{
				sqlDataReader = SqlHelper.ExecuteReader(pKName, clolumnNames, tblName, condiction, orderBy, PageIndex, PageSize);
				while (sqlDataReader.Read())
				{
					userInfo = new UserInfo();
					userInfo.UserID = Tool.GetString(sqlDataReader["UserID"]);
					userInfo.UserName = Tool.GetString(sqlDataReader["UserName"]);
					userInfo.Password = Tool.GetString(sqlDataReader["Password"]);
					userInfo.GroupID = Tool.GetString(sqlDataReader["GroupID"]);
					userInfo.Sex = Tool.GetBoolean(sqlDataReader["Sex"]);
					userInfo.PhoneNumber = Tool.GetString(sqlDataReader["PhoneNumber"]);
					userInfo.Creator = Tool.GetString(sqlDataReader["Creator"]);
					userInfo.CreateTime = Tool.GetDateTime1(sqlDataReader["CreateTime"]);
					userInfo.LastEditor = Tool.GetString(sqlDataReader["LastEditor"]);
					userInfo.LastEditTime = Tool.GetDateTime1(sqlDataReader["LastEditTime"]);
					userInfo.Status = Tool.GetBoolean(sqlDataReader["Status"]);
					list.Add(userInfo);
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

		public int DeleteUserInfo(params string[] users)
		{
			int num = 0;
			try
			{
				string text = "";
				for (int i = 0; i < users.Length; i++)
				{
					string text2 = users[i];
					text = ((i != users.Length - 1) ? (text + "'" + text2 + "',") : (text + "'" + text2 + "'"));
				}
				string cmdText = "DELETE FROM UserInfo WHERE UserID IN (" + text + ")";
				return SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, cmdText, (SqlParameter[])null);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int DeleteStatusUserInfo(string lastEditor, params string[] users)
		{
			int num = 0;
			try
			{
				string text = "";
				for (int i = 0; i < users.Length; i++)
				{
					string text2 = users[i];
					text = ((i != users.Length - 1) ? (text + "'" + text2 + "',") : (text + "'" + text2 + "'"));
				}
				string cmdText = "UPDATE UserInfo SET [Status] = 0,LastEditor = @LastEditor,LastEditTime = GETDATE() WHERE UserID IN (" + text + ")";
				SqlParameter sqlParameter = new SqlParameter("@LastEditor", lastEditor);
				return SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, cmdText, sqlParameter);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
