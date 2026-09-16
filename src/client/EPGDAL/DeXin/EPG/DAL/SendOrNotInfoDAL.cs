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
	public class SendOrNotInfoDAL : ISendOrNotInfoDAL
	{
		private SendNoticesDAL noticeDAL = new SendNoticesDAL();

		private IList<SendOrNotInfo> DataReaderToData(SqlDataReader sdr)
		{
			IList<SendOrNotInfo> list = new List<SendOrNotInfo>();
			SendOrNotInfo sendOrNotInfo = null;
			while (sdr.Read())
			{
				sendOrNotInfo = new SendOrNotInfo();
				sendOrNotInfo.Id = Tool.GetInt64(sdr["id"]);
				sendOrNotInfo.TsTableId = Tool.GetInt64(sdr["ts_table_id"]);
				sendOrNotInfo.Bat = Tool.GetInt32(sdr["bat"]);
				sendOrNotInfo.Pat = Tool.GetInt32(sdr["pat"]);
				sendOrNotInfo.Cat = Tool.GetInt32(sdr["cat"]);
				sendOrNotInfo.Pmt = Tool.GetInt32(sdr["pmt"]);
				sendOrNotInfo.SdtAct = Tool.GetInt32(sdr["sdt_act"]);
				sendOrNotInfo.SdtOth = Tool.GetInt32(sdr["sdt_oth"]);
				sendOrNotInfo.NitAct = Tool.GetInt32(sdr["nit_act"]);
				sendOrNotInfo.NitOth = Tool.GetInt32(sdr["nit_oth"]);
				sendOrNotInfo.Tdt = Tool.GetInt32(sdr["tdt"]);
				sendOrNotInfo.Tot = Tool.GetInt32(sdr["tot"]);
				sendOrNotInfo.EitActPf = Tool.GetInt32(sdr["eit_act_pf"]);
				sendOrNotInfo.EitActSc = Tool.GetInt32(sdr["eit_act_sc"]);
				sendOrNotInfo.EitOthPf = Tool.GetInt32(sdr["eit_oth_pf"]);
				sendOrNotInfo.EitOthSc = Tool.GetInt32(sdr["eit_oth_sc"]);
				list.Add(sendOrNotInfo);
			}
			return list;
		}

		public IList<SendOrNotInfo> GetData()
		{
			try
			{
				IList<SendOrNotInfo> list = new List<SendOrNotInfo>();
				string cmdText = "SELECT * FROM send_or_not_info";
				SqlDataReader sdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, (SqlParameter[])null);
				return DataReaderToData(sdr);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public IList<SendOrNotInfo> GetDataByID(long id)
		{
			SendOrNotInfo sendOrNotInfo = new SendOrNotInfo();
			try
			{
				string cmdText = "SELECT * FROM send_or_not_info WHERE id=@id";
				SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@id", id) };
				SqlDataReader sdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
				return DataReaderToData(sdr);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public IList<SendOrNotInfo> GetDataByNetworkTableID(long networkTableId)
		{
			SendOrNotInfo sendOrNotInfo = new SendOrNotInfo();
			try
			{
				string cmdText = "SELECT * FROM V_SendOrNot WHERE nit_table_id=@nit_table_id";
				SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@nit_table_id", networkTableId) };
				SqlDataReader sdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
				return DataReaderToData(sdr);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public IList<SendOrNotInfo> GetDataByTSTableID(long tsTableId)
		{
			SendOrNotInfo sendOrNotInfo = new SendOrNotInfo();
			try
			{
				string cmdText = "SELECT * FROM send_or_not_info WHERE ts_table_id=@ts_table_id";
				SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@ts_table_id", tsTableId) };
				SqlDataReader sdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
				return DataReaderToData(sdr);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Add(SendOrNotInfo sendOrNotInfo)
		{
			int result = 0;
			try
			{
				string cmdText = " INSERT INTO [send_or_not_info]\r\n                            ([ts_table_id]\r\n                            ,[bat]\r\n                            ,[pat]\r\n                            ,[cat]\r\n                            ,[pmt]\r\n                            ,[sdt_act]\r\n                            ,[sdt_oth]\r\n                            ,[nit_act]\r\n                            ,[nit_oth]\r\n                            ,[tdt]\r\n                            ,[tot]\r\n                            ,[eit_act_pf]\r\n                            ,[eit_act_sc]\r\n                            ,[eit_oth_pf]\r\n                            ,[eit_oth_sc])\r\n                        VALUES\r\n                            (@ts_table_id,\r\n                            @bat,\r\n                            @pat,\r\n                            @cat,\r\n                            @pmt, \r\n                            @sdt_act,\r\n                            @sdt_oth, \r\n                            @nit_act,\r\n                            @nit_oth,\r\n                            @tdt,\r\n                            @tot, \r\n                            @eit_act_pf,\r\n                            @eit_act_sc,\r\n                            @eit_oth_pf,\r\n                            @eit_oth_sc) SELECT @@IDENTITY";
				SqlParameter[] commandParameters = new SqlParameter[15]
				{
					SqlHelper.AddParameter("@ts_table_id", sendOrNotInfo.TsTableId),
					SqlHelper.AddParameter("@bat", sendOrNotInfo.Bat),
					SqlHelper.AddParameter("@pat", sendOrNotInfo.Pat),
					SqlHelper.AddParameter("@cat", sendOrNotInfo.Cat),
					SqlHelper.AddParameter("@pmt", sendOrNotInfo.Pmt),
					SqlHelper.AddParameter("@sdt_act", sendOrNotInfo.SdtAct),
					SqlHelper.AddParameter("@sdt_oth", sendOrNotInfo.SdtOth),
					SqlHelper.AddParameter("@nit_act", sendOrNotInfo.NitAct),
					SqlHelper.AddParameter("@nit_oth", sendOrNotInfo.NitOth),
					SqlHelper.AddParameter("@tdt", sendOrNotInfo.Tdt),
					SqlHelper.AddParameter("@tot", sendOrNotInfo.Tot),
					SqlHelper.AddParameter("@eit_act_pf", sendOrNotInfo.EitActPf),
					SqlHelper.AddParameter("@eit_act_sc", sendOrNotInfo.EitActSc),
					SqlHelper.AddParameter("@eit_oth_pf", sendOrNotInfo.EitOthPf),
					SqlHelper.AddParameter("@eit_oth_sc", sendOrNotInfo.EitOthSc)
				};
				using (SqlDataReader sqlDataReader = SqlHelper.ExecuteReader(CommandType.Text, cmdText, commandParameters))
				{
					if (sqlDataReader.Read())
					{
						sendOrNotInfo.Id = Tool.GetInt64(sqlDataReader[0]);
					}
				}
				SendNotices sendNotices = new SendNotices();
				sendNotices.Code = 18;
				sendNotices.Location = "send_or_not_info";
				sendNotices.Operation = 1;
				sendNotices.TableIdentity = sendOrNotInfo.Id;
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

		public int Update(SendOrNotInfo sendOrNotInfo)
		{
			int num = 0;
			try
			{
				string cmdText = "UPDATE [dbo].[send_or_not_info]\r\n                               SET [ts_table_id] = @ts_table_id, \r\n                                  [bat]=@bat,\r\n                                  [pat] = @pat,\r\n                                  [cat] = @cat,\r\n                                  [pmt] = @pmt, \r\n                                  [sdt_act] = @sdt_act, \r\n                                  [sdt_oth] = @sdt_oth, \r\n                                  [nit_act] = @nit_act, \r\n                                  [nit_oth] = @nit_oth, \r\n                                  [tdt] = @tdt,\r\n                                  [tot] = @tot, \r\n                                  [eit_act_pf] = @eit_act_pf,\r\n                                  [eit_act_sc] = @eit_act_sc, \r\n                                  [eit_oth_pf] = @eit_oth_pf, \r\n                                  [eit_oth_sc] = @eit_oth_sc\r\n                             WHERE id=@id";
				SqlParameter[] commandParameters = new SqlParameter[16]
				{
					SqlHelper.AddParameter("@id", sendOrNotInfo.Id),
					SqlHelper.AddParameter("@ts_table_id", sendOrNotInfo.TsTableId),
					SqlHelper.AddParameter("@bat", sendOrNotInfo.Bat),
					SqlHelper.AddParameter("@pat", sendOrNotInfo.Pat),
					SqlHelper.AddParameter("@cat", sendOrNotInfo.Cat),
					SqlHelper.AddParameter("@pmt", sendOrNotInfo.Pmt),
					SqlHelper.AddParameter("@sdt_act", sendOrNotInfo.SdtAct),
					SqlHelper.AddParameter("@sdt_oth", sendOrNotInfo.SdtOth),
					SqlHelper.AddParameter("@nit_act", sendOrNotInfo.NitAct),
					SqlHelper.AddParameter("@nit_oth", sendOrNotInfo.NitOth),
					SqlHelper.AddParameter("@tdt", sendOrNotInfo.Tdt),
					SqlHelper.AddParameter("@tot", sendOrNotInfo.Tot),
					SqlHelper.AddParameter("@eit_act_pf", sendOrNotInfo.EitActPf),
					SqlHelper.AddParameter("@eit_act_sc", sendOrNotInfo.EitActSc),
					SqlHelper.AddParameter("@eit_oth_pf", sendOrNotInfo.EitOthPf),
					SqlHelper.AddParameter("@eit_oth_sc", sendOrNotInfo.EitOthSc)
				};
				num = SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
				if (num < 0)
				{
					throw new Exception(num.ToString());
				}
				SendNotices sendNotices = new SendNotices();
				sendNotices.Code = 18;
				sendNotices.Location = "send_or_not_info";
				sendNotices.Operation = 2;
				sendNotices.TableIdentity = sendOrNotInfo.Id;
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

		public int Delete(SendOrNotInfo sendOrNotInfo)
		{
			try
			{
				string cmdText = "DELETE  send_or_not_info  WHERE id=@id";
				SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@id", sendOrNotInfo.Id) };
				return SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Delete(SendOrNotInfo sendOrNotInfo, SqlTransaction sqltrans)
		{
			try
			{
				string cmdText = "DELETE  send_or_not_info  WHERE id=@id";
				SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@id", sendOrNotInfo.Id) };
				return SqlHelper.ExecuteNonQuery(sqltrans, CommandType.Text, cmdText, commandParameters);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Delete(IList<SendOrNotInfo> multipleIPs)
		{
			int num = 0;
			using (SqlConnection sqlConnection = new SqlConnection(SqlHelper.ConnectionString))
			{
				sqlConnection.Open();
				SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
				try
				{
					foreach (SendOrNotInfo multipleIP in multipleIPs)
					{
						SendNotices sendNotices = new SendNotices();
						sendNotices.Code = 18;
						sendNotices.Location = "send_or_not_info";
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
			string cmdText = "DELETE FROM [dbo].[send_or_not_info] WHERE ts_table_id = @ts_table_id";
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@ts_table_id", TSIdentity) };
			return SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
		}

		public int DeleteByNITIdentityWithTranscation(long NITIdentity)
		{
			string cmdText = "DELETE FROM [dbo].[send_or_not_info] FROM ts_info a, send_or_not_info b WHERE a.nit_table_id = @nit_table_id and a.id = b.ts_table_id";
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@nit_table_id", NITIdentity) };
			return SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
		}
	}
}
