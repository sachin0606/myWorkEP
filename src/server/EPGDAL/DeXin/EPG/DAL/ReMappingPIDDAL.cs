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
	public class ReMappingPIDDAL : IReMappingPIDDAL
	{
		private SendNoticesDAL noticeDAL = new SendNoticesDAL();

		private IList<PidOutputInfo> DataReaderToData(SqlDataReader sdr)
		{
			IList<PidOutputInfo> list = new List<PidOutputInfo>();
			PidOutputInfo pidOutputInfo = null;
			while (sdr.Read())
			{
				pidOutputInfo = new PidOutputInfo();
				pidOutputInfo.Id = Tool.GetInt64(sdr["id"]);
				pidOutputInfo.TsTableId = Tool.GetInt64(sdr["ts_table_id"]);
				pidOutputInfo.Pat = Tool.GetInt32(sdr["pat"]);
				pidOutputInfo.Cat = Tool.GetInt32(sdr["cat"]);
				pidOutputInfo.Eit = Tool.GetInt32(sdr["eit"]);
				pidOutputInfo.Nit = Tool.GetInt32(sdr["nit"]);
				pidOutputInfo.SdtOrBat = Tool.GetInt32(sdr["sdt_or_bat"]);
				pidOutputInfo.TotOrTdt = Tool.GetInt32(sdr["tot_or_tdt"]);
				pidOutputInfo.ReadTS += pidOutputInfo_ReadTS;
				list.Add(pidOutputInfo);
			}
			return list;
		}

		private void pidOutputInfo_ReadTS(object sender)
		{
			PidOutputInfo pidOutputInfo = sender as PidOutputInfo;
			pidOutputInfo.TS = new TSInfoDAL().GetTSInfoByTSIdentity(pidOutputInfo.TsTableId);
		}

		public IList<PidOutputInfo> GetData()
		{
			try
			{
				IList<PidOutputInfo> list = new List<PidOutputInfo>();
				string cmdText = "SELECT * FROM pid_output_info";
				SqlDataReader sdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, (SqlParameter[])null);
				return DataReaderToData(sdr);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public IList<PidOutputInfo> GetDataByID(long id)
		{
			PidOutputInfo pidOutputInfo = new PidOutputInfo();
			try
			{
				string cmdText = "SELECT * FROM pid_output_info WHERE id=@id";
				SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@id", id) };
				SqlDataReader sdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
				return DataReaderToData(sdr);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public IList<PidOutputInfo> GetDataByNetworkTableID(long networkTableId)
		{
			PidOutputInfo pidOutputInfo = new PidOutputInfo();
			try
			{
				string cmdText = "SELECT * FROM V_pidoutput WHERE nit_table_id=@nit_table_id";
				SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@nit_table_id", networkTableId) };
				SqlDataReader sdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
				return DataReaderToData(sdr);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public IList<PidOutputInfo> GetDataByTSTableID(long tsTableId)
		{
			PidOutputInfo pidOutputInfo = new PidOutputInfo();
			try
			{
				string cmdText = "SELECT * FROM pid_output_info WHERE ts_table_id=@ts_table_id";
				SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@ts_table_id", tsTableId) };
				SqlDataReader sdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
				return DataReaderToData(sdr);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Add(PidOutputInfo PidOutputInfo)
		{
			int result = 0;
			try
			{
				string cmdText = " INSERT INTO [dbo].[pid_output_info]\r\n                                ([ts_table_id]\r\n                                ,[pat]\r\n                                ,[cat]\r\n                                ,[sdt_or_bat]\r\n                                ,[nit]\r\n                                ,[tot_or_tdt]\r\n                                ,[eit])\r\n                            VALUES\r\n                                (@ts_table_id\r\n                                ,@pat\r\n                                ,@cat\r\n                                ,@sdt_or_bat\r\n                                ,@nit\r\n                                ,@tot_or_tdt\r\n                                ,@eit) ;SELECT @@IDENTITY";
				SqlParameter[] commandParameters = new SqlParameter[7]
				{
					SqlHelper.AddParameter("@ts_table_id", PidOutputInfo.TsTableId),
					SqlHelper.AddParameter("@pat", PidOutputInfo.Pat),
					SqlHelper.AddParameter("@cat", PidOutputInfo.Cat),
					SqlHelper.AddParameter("@sdt_or_bat", PidOutputInfo.SdtOrBat),
					SqlHelper.AddParameter("@nit", PidOutputInfo.Nit),
					SqlHelper.AddParameter("@tot_or_tdt", PidOutputInfo.TotOrTdt),
					SqlHelper.AddParameter("@eit", PidOutputInfo.Eit)
				};
				using (SqlDataReader sqlDataReader = SqlHelper.ExecuteReader(CommandType.Text, cmdText, commandParameters))
				{
					if (sqlDataReader.Read())
					{
						PidOutputInfo.Id = Tool.GetInt64(sqlDataReader[0]);
					}
				}
				SendNotices sendNotices = new SendNotices();
				sendNotices.Code = 20;
				sendNotices.Location = "pid_output_info";
				sendNotices.Operation = 1;
				sendNotices.TableIdentity = PidOutputInfo.Id;
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

		public int Update(PidOutputInfo PidOutputInfo)
		{
			int num = 0;
			try
			{
				string cmdText = "UPDATE [dbo].[pid_output_info]\r\n                               SET [ts_table_id] = @ts_table_id, \r\n                                  [pat] = @pat,\r\n                                  [cat] = @cat,\r\n                                  [sdt_or_bat]=@sdt_or_bat ,\r\n                                  [nit] =@nit,\r\n                                  [tot_or_tdt]=@tot_or_tdt,\r\n                                  [eit]=@eit\r\n\r\n                             WHERE id=@id";
				SqlParameter[] commandParameters = new SqlParameter[8]
				{
					SqlHelper.AddParameter("@id", PidOutputInfo.Id),
					SqlHelper.AddParameter("@ts_table_id", PidOutputInfo.TsTableId),
					SqlHelper.AddParameter("@pat", PidOutputInfo.Pat),
					SqlHelper.AddParameter("@cat", PidOutputInfo.Cat),
					SqlHelper.AddParameter("@sdt_or_bat", PidOutputInfo.SdtOrBat),
					SqlHelper.AddParameter("@nit", PidOutputInfo.Nit),
					SqlHelper.AddParameter("@tot_or_tdt", PidOutputInfo.TotOrTdt),
					SqlHelper.AddParameter("@eit", PidOutputInfo.Eit)
				};
				num = SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
				if (num < 0)
				{
					throw new Exception(num.ToString());
				}
				SendNotices sendNotices = new SendNotices();
				sendNotices.Code = 20;
				sendNotices.Location = "pid_output_info";
				sendNotices.Operation = 2;
				sendNotices.TableIdentity = PidOutputInfo.Id;
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

		public int Delete(PidOutputInfo PidOutputInfo)
		{
			try
			{
				string cmdText = "DELETE  pid_output_info  WHERE id=@id";
				SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@id", PidOutputInfo.Id) };
				return SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Delete(PidOutputInfo PidOutputInfo, SqlTransaction sqltrans)
		{
			try
			{
				string cmdText = "DELETE  pid_output_info  WHERE id=@id";
				SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@id", PidOutputInfo.Id) };
				return SqlHelper.ExecuteNonQuery(sqltrans, CommandType.Text, cmdText, commandParameters);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Delete(IList<PidOutputInfo> multipleIPs)
		{
			int num = 0;
			using (SqlConnection sqlConnection = new SqlConnection(SqlHelper.ConnectionString))
			{
				sqlConnection.Open();
				SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
				try
				{
					foreach (PidOutputInfo multipleIP in multipleIPs)
					{
						SendNotices sendNotices = new SendNotices();
						sendNotices.Code = 20;
						sendNotices.Location = "pid_output_info";
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
			string cmdText = "DELETE FROM [dbo].[pid_output_info] WHERE ts_table_id = @ts_table_id";
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@ts_table_id", TSIdentity) };
			return SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
		}

		public int DeleteByNITIdentityWithTranscation(long NITIdentity)
		{
			string cmdText = "DELETE FROM [dbo].[pid_output_info] FROM ts_info a, pid_output_info b WHERE a.nit_table_id = @nit_table_id and a.id = b.ts_table_id";
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@nit_table_id", NITIdentity) };
			return SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
		}
	}
}
