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
	public class MultiIPOutputDAL : IMultiIPOutputDAL
	{
		private SendNoticesDAL noticeDAL = new SendNoticesDAL();

		private IList<MultipleipSettings> DataReaderToData(SqlDataReader sdr)
		{
			IList<MultipleipSettings> list = new List<MultipleipSettings>();
			MultipleipSettings multipleipSettings = null;
			while (sdr.Read())
			{
				multipleipSettings = new MultipleipSettings();
				multipleipSettings.Id = Tool.GetInt64(sdr["id"]);
				multipleipSettings.Ip = Tool.GetString(sdr["ip"]);
				multipleipSettings.TsTableId = Tool.GetInt32(sdr["ts_table_id"]);
				multipleipSettings.Port = Tool.GetUInt16(sdr["port"]);
				list.Add(multipleipSettings);
			}
			return list;
		}

		public IList<MultipleipSettings> GetData()
		{
			try
			{
				IList<MultipleipSettings> list = new List<MultipleipSettings>();
				string cmdText = "SELECT * FROM multipleip_settings";
				SqlDataReader sdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, (SqlParameter[])null);
				return DataReaderToData(sdr);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public IList<MultipleipSettings> GetDataByID(long id)
		{
			MultipleipSettings multipleipSettings = new MultipleipSettings();
			try
			{
				string cmdText = "SELECT * FROM multipleip_settings WHERE id=@id";
				SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@id", id) };
				SqlDataReader sdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
				return DataReaderToData(sdr);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public IList<MultipleipSettings> GetDataByNetworkTableID(long id)
		{
			MultipleipSettings multipleipSettings = new MultipleipSettings();
			try
			{
				string cmdText = "SELECT * FROM V_MultipleIPSetting WHERE nit_table_id=@nit_table_id";
				SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@nit_table_id", id) };
				SqlDataReader sdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
				return DataReaderToData(sdr);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Add(MultipleipSettings multiIP)
		{
			int result = 0;
			try
			{
				string cmdText = "INSERT INTO  multipleip_settings(ts_table_id,ip,port) VALUES(@ts_table_id,@ip,@port); select @@IDENTITY";
				SqlParameter[] commandParameters = new SqlParameter[3]
				{
					SqlHelper.AddParameter("@ts_table_id", multiIP.TsTableId),
					SqlHelper.AddParameter("@Ip", multiIP.Ip),
					SqlHelper.AddParameter("@Port", multiIP.Port)
				};
				using (SqlDataReader sqlDataReader = SqlHelper.ExecuteReader(CommandType.Text, cmdText, commandParameters))
				{
					if (sqlDataReader.Read())
					{
						multiIP.Id = Tool.GetInt64(sqlDataReader[0]);
					}
				}
				SendNotices sendNotices = new SendNotices();
				sendNotices.Code = 21;
				sendNotices.Location = "multipleip_settings";
				sendNotices.Operation = 1;
				sendNotices.TableIdentity = multiIP.Id;
				SendNotices notice = sendNotices;
				noticeDAL.SaveWithTranscation(notice);
				SqlHelper.SaveChange();
				return result;
			}
			catch (Exception ex)
			{
				SqlHelper.CancelChange();
				throw ex;
			}
		}

		public int Update(MultipleipSettings multiIP)
		{
			int num = 0;
			try
			{
				string cmdText = "UPDATE  multipleip_settings SET ts_table_id=@ts_table_id,ip=@ip,port=@port WHERE id=@id";
				SqlParameter[] commandParameters = new SqlParameter[4]
				{
					SqlHelper.AddParameter("@id", multiIP.Id),
					SqlHelper.AddParameter("@ts_table_id", multiIP.TsTableId),
					SqlHelper.AddParameter("@Ip", multiIP.Ip),
					SqlHelper.AddParameter("@Port", multiIP.Port)
				};
				num = SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
				if (num < 0)
				{
					throw new Exception(num.ToString());
				}
				SendNotices sendNotices = new SendNotices();
				sendNotices.Code = 21;
				sendNotices.Location = "multipleip_settings";
				sendNotices.Operation = 2;
				sendNotices.TableIdentity = multiIP.Id;
				SendNotices notice = sendNotices;
				noticeDAL.SaveWithTranscation(notice);
				SqlHelper.SaveChange();
				return num;
			}
			catch (Exception ex)
			{
				SqlHelper.CancelChange();
				throw ex;
			}
		}

		public int Delete(MultipleipSettings multiIP)
		{
			try
			{
				string cmdText = "DELETE  multipleip_settings  WHERE id=@id";
				SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@id", multiIP.Id) };
				return SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Delete(MultipleipSettings multipleIP, SqlTransaction sqltrans)
		{
			try
			{
				string cmdText = "DELETE  multipleip_settings  WHERE id=@id";
				SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@id", multipleIP.Id) };
				return SqlHelper.ExecuteNonQuery(sqltrans, CommandType.Text, cmdText, commandParameters);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Delete(IList<MultipleipSettings> multipleIPs)
		{
			int num = 0;
			using (SqlConnection sqlConnection = new SqlConnection(SqlHelper.ConnectionString))
			{
				sqlConnection.Open();
				SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
				try
				{
					foreach (MultipleipSettings multipleIP in multipleIPs)
					{
						SendNotices sendNotices = new SendNotices();
						sendNotices.Code = 21;
						sendNotices.Location = "multipleip_settings";
						sendNotices.Operation = 3;
						sendNotices.TableIdentity = multipleIP.Id;
						SendNotices notice = sendNotices;
						noticeDAL.SaveWithTranscation(sqlTransaction, notice);
						Delete(multipleIP, sqlTransaction);
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

		public int DeleteByTSIdentityWithTranscation(long TSIdentity)
		{
			string cmdText = "DELETE FROM [dbo].[multipleip_settings] WHERE ts_table_id = @ts_table_id";
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@ts_table_id", TSIdentity) };
			return SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
		}

		public int DeleteByNITIdentityWithTranscation(long NITIdentity)
		{
			string cmdText = "DELETE FROM [dbo].[multipleip_settings] FROM ts_info a, multipleip_settings b WHERE a.nit_table_id = @nit_table_id and a.id = b.ts_table_id";
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@nit_table_id", NITIdentity) };
			return SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
		}
	}
}
