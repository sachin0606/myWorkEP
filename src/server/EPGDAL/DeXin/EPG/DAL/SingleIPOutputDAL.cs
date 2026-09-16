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
	public class SingleIPOutputDAL : ISingleIPOutputDAL
	{
		private SendNoticesDAL noticeDAL = new SendNoticesDAL();

		private IList<SimpleipSettings> DataReaderToData(SqlDataReader sdr)
		{
			IList<SimpleipSettings> list = new List<SimpleipSettings>();
			SimpleipSettings simpleipSettings = null;
			while (sdr.Read())
			{
				simpleipSettings = new SimpleipSettings();
				simpleipSettings.Ip = Tool.GetString(sdr["ip"]);
				simpleipSettings.Port = Tool.GetInt32(sdr["port"]);
				list.Add(simpleipSettings);
			}
			return list;
		}

		public IList<SimpleipSettings> GetData()
		{
			IList<SimpleipSettings> list = new List<SimpleipSettings>();
			string cmdText = "SELECT [ip],[port] FROM [simpleip_settings]";
			SqlDataReader sdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText);
			return DataReaderToData(sdr);
		}

		public int Add(SimpleipSettings singleIP)
		{
			int num = 0;
			string cmdText = "DELETE FROM simpleip_settings; INSERT INTO simpleip_settings VALUES(@ip,@port)";
			SqlParameter[] commandParameters = new SqlParameter[2]
			{
				SqlHelper.AddParameter("@ip", singleIP.Ip),
				SqlHelper.AddParameter("@port", singleIP.Port)
			};
			num = SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
			if (num < 0)
			{
				throw new Exception(num.ToString());
			}
			SendNotices sendNotices = new SendNotices();
			sendNotices.Code = 22;
			sendNotices.Location = "simpleip_settings";
			sendNotices.Operation = 2;
			sendNotices.TableIdentity = 0L;
			SendNotices notice = sendNotices;
			noticeDAL.SaveWithTranscation(notice);
			SqlHelper.SaveChange();
			return num;
		}

		public int Update(SimpleipSettings singleIP)
		{
			string cmdText = "UPDATE simpleip_settings SET ip=@ip,port=@port";
			SqlParameter[] commandParameters = new SqlParameter[2]
			{
				SqlHelper.AddParameter("@ip", singleIP.Ip),
				SqlHelper.AddParameter("@port", singleIP.Port)
			};
			return SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
		}

		public int Delete(SimpleipSettings singleIP)
		{
			string cmdText = "DELETE simpleip_settings WHERE ip=@ip";
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@ip", singleIP.Ip) };
			return SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
		}

		public int Delete(SimpleipSettings singleIP, SqlTransaction sqltrans)
		{
			string cmdText = "DELETE simpleip_settings WHERE ip=@ip";
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@ip", singleIP.Ip) };
			return SqlHelper.ExecuteNonQuery(sqltrans, CommandType.Text, cmdText, commandParameters);
		}

		public int DeleteWithTrans(IList<SimpleipSettings> lstPermitInfo)
		{
			int num = 0;
			using (SqlConnection sqlConnection = new SqlConnection(SqlHelper.ConnectionString))
			{
				sqlConnection.Open();
				SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
				try
				{
					foreach (SimpleipSettings item in lstPermitInfo)
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
