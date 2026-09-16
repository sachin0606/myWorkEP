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
	public class PMTPIDReMappingDAL : IPMTPIDReMappingDAL
	{
		private SendNoticesDAL noticeDAL = new SendNoticesDAL();

		private IList<PmtPidOutputInfo> DataReaderToData(SqlDataReader sdr)
		{
			IList<PmtPidOutputInfo> list = new List<PmtPidOutputInfo>();
			PmtPidOutputInfo pmtPidOutputInfo = null;
			while (sdr.Read())
			{
				pmtPidOutputInfo = new PmtPidOutputInfo();
				pmtPidOutputInfo.Id = Tool.GetInt64(sdr["id"]);
				pmtPidOutputInfo.ProgramTableId = Tool.GetInt64(sdr["program_table_id"]);
				pmtPidOutputInfo.OutputPid = Tool.GetInt32(sdr["output_pid"]);
				pmtPidOutputInfo.Program = new ProgramInfoDAL().GetByIdentity(pmtPidOutputInfo.ProgramTableId);
				list.Add(pmtPidOutputInfo);
			}
			return list;
		}

		public IList<PmtPidOutputInfo> GetData()
		{
			try
			{
				IList<PmtPidOutputInfo> list = new List<PmtPidOutputInfo>();
				string cmdText = "SELECT * FROM pmt_pid_output_info";
				SqlDataReader sdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, (SqlParameter[])null);
				return DataReaderToData(sdr);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public IList<PmtPidOutputInfo> GetDataByID(long id)
		{
			PmtPidOutputInfo pmtPidOutputInfo = new PmtPidOutputInfo();
			try
			{
				string cmdText = "SELECT * FROM pmt_pid_output_info WHERE id=@id";
				SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@id", id) };
				SqlDataReader sdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
				return DataReaderToData(sdr);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public IList<PmtPidOutputInfo> GetDataByProgramTableID(long program_table_id)
		{
			PmtPidOutputInfo pmtPidOutputInfo = new PmtPidOutputInfo();
			try
			{
				string cmdText = "SELECT * FROM pmt_pid_output_info WHERE program_table_id=@program_table_id";
				SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@program_table_id", program_table_id) };
				SqlDataReader sdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
				return DataReaderToData(sdr);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public IList<PmtPidOutputInfo> GetDataByTSTableID(long tsTableId)
		{
			PmtPidOutputInfo pmtPidOutputInfo = new PmtPidOutputInfo();
			try
			{
				string cmdText = "SELECT * FROM V_PMT_PIDOutputInfo WHERE ts_table_id=@ts_table_id";
				SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@ts_table_id", tsTableId) };
				SqlDataReader sdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
				return DataReaderToData(sdr);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Add(PmtPidOutputInfo multiIP)
		{
			int result = 0;
			try
			{
				string cmdText = "INSERT INTO  pmt_pid_output_info(program_table_id,output_pid) VALUES(@program_table_id,@output_pid) SELECT @@IDENTITY ";
				SqlParameter[] commandParameters = new SqlParameter[2]
				{
					SqlHelper.AddParameter("@program_table_id", multiIP.ProgramTableId),
					SqlHelper.AddParameter("@output_pid", multiIP.OutputPid)
				};
				using (SqlDataReader sqlDataReader = SqlHelper.ExecuteReader(CommandType.Text, cmdText, commandParameters))
				{
					if (sqlDataReader.Read())
					{
						multiIP.Id = Tool.GetInt64(sqlDataReader[0]);
					}
				}
				SendNotices sendNotices = new SendNotices();
				sendNotices.Code = 19;
				sendNotices.Location = "pmt_pid_output_info";
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

		public int Update(PmtPidOutputInfo multiIP)
		{
			int num = 0;
			try
			{
				string cmdText = "UPDATE  pmt_pid_output_info SET program_table_id=@program_table_id,output_pid=@output_pid WHERE id=@id";
				SqlParameter[] commandParameters = new SqlParameter[3]
				{
					SqlHelper.AddParameter("@id", multiIP.Id),
					SqlHelper.AddParameter("@program_table_id", multiIP.ProgramTableId),
					SqlHelper.AddParameter("@output_pid", multiIP.OutputPid)
				};
				num = SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
				if (num < 0)
				{
					throw new Exception(num.ToString());
				}
				SendNotices sendNotices = new SendNotices();
				sendNotices.Code = 19;
				sendNotices.Location = "pmt_pid_output_info";
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

		public int Delete(PmtPidOutputInfo multiIP)
		{
			try
			{
				string cmdText = "DELETE  pmt_pid_output_info  WHERE id=@id";
				SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@id", multiIP.Id) };
				return SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Delete(PmtPidOutputInfo multipleIP, SqlTransaction sqltrans)
		{
			try
			{
				string cmdText = "DELETE  pmt_pid_output_info  WHERE id=@id";
				SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@id", multipleIP.Id) };
				return SqlHelper.ExecuteNonQuery(sqltrans, CommandType.Text, cmdText, commandParameters);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Delete(IList<PmtPidOutputInfo> multipleIPs)
		{
			int num = 0;
			using (SqlConnection sqlConnection = new SqlConnection(SqlHelper.ConnectionString))
			{
				sqlConnection.Open();
				SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
				try
				{
					foreach (PmtPidOutputInfo multipleIP in multipleIPs)
					{
						SendNotices sendNotices = new SendNotices();
						sendNotices.Code = 19;
						sendNotices.Location = "pmt_pid_output_info";
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

		public int DeleteByPrgIdentityWithTranscation(long PrgIdentity)
		{
			string cmdText = "DELETE FROM [dbo].[pmt_pid_output_info] WHERE program_table_id = @program_table_id";
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@program_table_id", PrgIdentity) };
			return SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
		}

		public int DeleteByTSIdentityWithTranscation(long TSIdentity)
		{
			string cmdText = "DELETE FROM pmt_pid_output_info FROM pmt_pid_output_info a, program_info b, ts_info c WHERE c.id = @ts_table_id AND c.id = b.ts_table_id AND b.id = a.program_table_id";
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@ts_table_id", TSIdentity) };
			return SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
		}

		public int DeleteByNITIdentityWithTranscation(long NetworkIdentity)
		{
			string cmdText = "DELETE FROM pmt_pid_output_info FROM pmt_pid_output_info a, program_info b, ts_info c, nit_info d WHERE d.id = @nit_table_id AND d.id = c.nit_table_id AND c.id = b.ts_table_id AND b.id = a.program_table_id";
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@nit_table_id", NetworkIdentity) };
			return SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
		}
	}
}
