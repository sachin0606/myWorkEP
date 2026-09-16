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
	public class PATDAL : IPATDAL
	{
		public IList<PAT> GetAll()
		{
			IList<PAT> list = new List<PAT>();
			string cmdText = "SELECT * FROM [PAT]";
			SqlDataReader sqlDataReader = null;
			try
			{
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText);
				while (sqlDataReader.Read())
				{
					PAT pAT = new PAT();
					pAT.Id = Tool.GetInt64(sqlDataReader["id"]);
					pAT.ProgramTableId = Tool.GetInt64(sqlDataReader["program_table_id"]);
					pAT.UserId = sqlDataReader["user_id"].ToString();
					pAT.Version = sqlDataReader["version"].ToString();
					PAT item = pAT;
					list.Add(item);
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

		public void Save(List<long> ProgramIdentityList, string UserId)
		{
			try
			{
				string cmdText = "INSERT  INTO PAT_history ( program_table_id , create_time , user_id , version ) SELECT  program_table_id , create_time , user_id , version FROM    PAT";
				SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, (SqlParameter[])null);
				cmdText = "delete from PAT";
				SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, (SqlParameter[])null);
				string value = Guid.NewGuid().ToString();
				foreach (long ProgramIdentity in ProgramIdentityList)
				{
					cmdText = "INSERT INTO [PAT] ([program_table_id] ,[create_time] ,[user_id] ,[version]) VALUES (@program_table_id ,GETDATE() ,@user_id ,@version)";
					SqlParameter[] commandParameters = new SqlParameter[3]
					{
						SqlHelper.AddParameter("program_table_id", ProgramIdentity),
						SqlHelper.AddParameter("user_id", UserId),
						SqlHelper.AddParameter("version", value)
					};
					SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
				}
				SendNotices sendNotices = new SendNotices();
				sendNotices.Code = 3;
				sendNotices.Location = "PAT";
				sendNotices.Operation = 2;
				sendNotices.TableIdentity = 0L;
				SendNotices notice = sendNotices;
				new SendNoticesDAL().SaveWithTranscation(notice);
				SqlHelper.SaveChange();
			}
			catch (Exception ex)
			{
				SqlHelper.CancelChange();
				throw ex;
			}
		}
	}
}
