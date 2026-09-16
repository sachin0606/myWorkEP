using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.IDAL;
using DeXin.EPG.Model;
using DeXin.EPG.Resource;
using DeXin.EPG.Tools;

namespace DeXin.EPG.DAL
{
	public class ProgramInfoDAL : IProgramInfoDAL
	{
		private SendNoticesDAL noticeDAL = new SendNoticesDAL();

		private ProgramInfo ProgramSqlDataReader(SqlDataReader sdr)
		{
			long @int = Tool.GetInt64(sdr["id"]);
			string tableName = sdr["program_name"].ToString();
			ProgramInfo programInfo = new ProgramInfo(@int, tableName);
			programInfo.TsTableId = Tool.GetInt32(sdr["ts_table_id"]);
			programInfo.ServiceId = Tool.GetInt32(sdr["service_id"]);
			programInfo.ProviderName = sdr["provider_name"].ToString();
			programInfo.ServiceType = Tool.GetInt32(sdr["service_type"]);
			programInfo.PmtId = Tool.GetInt64(sdr["pmt_pid"]);
			programInfo.PCRPID = Tool.GetInt32(sdr["PCR_PID"]);
			programInfo.PmtVersionNumber = Tool.GetInt32(sdr["pmt_version_number"]);
			programInfo.PrgSdtEitscflag = Tool.GetInt32(sdr["prg_sdt_eitscflag"]);
			programInfo.PrgSdtEitpfflag = Tool.GetInt32(sdr["prg_sdt_eitpfflag"]);
			programInfo.PrgSdtRunningstate = Tool.GetInt32(sdr["prg_sdt_runningstate"]);
			programInfo.PrgSdtFreecamod = Tool.GetInt32(sdr["prg_sdt_freecamod"]);
			programInfo.PrgManualeventid = Tool.GetInt32(sdr["prg_manualeventid"]);
			programInfo.EITFileName = sdr["eit_filename"].ToString();
			ProgramInfo programInfo2 = programInfo;
			programInfo2.ProgramName = programInfo2.BKValue;
			programInfo2.Node = programInfo2;
			programInfo2.ReadTS += prg_ReadTS;
			return programInfo2;
		}

		public IList<ProgramInfo> GetProgramInfosByTSIdentity(long TSIdentity)
		{
			IList<ProgramInfo> list = new List<ProgramInfo>();
			string cmdText = "SELECT * FROM [program_info] WHERE ts_table_id = @ts_id";
			SqlDataReader sqlDataReader = null;
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@ts_id", TSIdentity, SqlDbType.BigInt) };
			try
			{
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
				while (sqlDataReader.Read())
				{
					list.Add(ProgramSqlDataReader(sqlDataReader));
				}
			}
			catch (Exception ex)
			{
				throw new CustomException(ResourceManagement.GetLocationString("ReadDataError"), ex, MessageType.SHOWANDLOG);
			}
			finally
			{
				SqlHelper.CloseAll(null, sqlDataReader);
			}
			return list;
		}

		private void prg_ReadTS(object sender)
		{
			ProgramInfo programInfo = sender as ProgramInfo;
			programInfo.TS = new TSInfoDAL().GetTSInfoByTSIdentity(programInfo.TsTableId);
		}

		public ProgramInfo GetByIdentity(long Identity)
		{
			string cmdText = "SELECT * FROM [program_info] WHERE id = @id";
			SqlDataReader sqlDataReader = null;
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@id", Identity, SqlDbType.BigInt) };
			try
			{
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
				if (sqlDataReader.Read())
				{
					return ProgramSqlDataReader(sqlDataReader);
				}
			}
			catch (Exception ex)
			{
				throw new CustomException(ResourceManagement.GetLocationString("ReadDataError"), ex, MessageType.SHOWANDLOG);
			}
			finally
			{
				SqlHelper.CloseAll(null, sqlDataReader);
			}
			return null;
		}

		public ProgramInfo GetByTSIdentityAndServiceId(long TSIdentity, int ServiceId)
		{
			string cmdText = "SELECT * FROM [program_info] WHERE ts_table_id = @ts_table_id AND service_id = @service_id ";
			SqlDataReader sqlDataReader = null;
			SqlParameter[] commandParameters = new SqlParameter[2]
			{
				SqlHelper.AddParameter("@ts_table_id", TSIdentity),
				SqlHelper.AddParameter("@service_id", ServiceId)
			};
			try
			{
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
				if (sqlDataReader.Read())
				{
					return ProgramSqlDataReader(sqlDataReader);
				}
			}
			catch (Exception ex)
			{
				throw new CustomException(ResourceManagement.GetLocationString("ReadDataError"), ex, MessageType.SHOWANDLOG);
			}
			finally
			{
				SqlHelper.CloseAll(null, sqlDataReader);
			}
			return null;
		}

		public ProgramInfo Get(int NetworkID, int TSID, int ServiceID)
		{
			string cmdText = "SELECT  *\r\n                                FROM    dbo.program_info p\r\n                                        INNER JOIN dbo.ts_info t ON p.ts_table_id = t.id\r\n                                        INNER JOIN dbo.nit_info n ON t.nit_table_id = n.id\r\n                                WHERE   n.network_id = @nit_id\r\n                                        AND t.ts_id = @ts_id\r\n                                        AND p.service_id = @service_id\r\n                                ";
			SqlDataReader sqlDataReader = null;
			SqlParameter[] commandParameters = new SqlParameter[3]
			{
				SqlHelper.AddParameter("@nit_id", NetworkID),
				SqlHelper.AddParameter("@ts_id", TSID),
				SqlHelper.AddParameter("@service_id", ServiceID)
			};
			try
			{
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
				if (sqlDataReader.Read())
				{
					return ProgramSqlDataReader(sqlDataReader);
				}
			}
			catch (Exception ex)
			{
				throw new CustomException(ResourceManagement.GetLocationString("ReadDataError"), ex, MessageType.SHOWANDLOG);
			}
			finally
			{
				SqlHelper.CloseAll(null, sqlDataReader);
			}
			return null;
		}

		public int DeleteProgramByNetworkIdentityWithTranscation(long NetworkIdentity)
		{
			int num = 0;
			num = new PMTPIDReMappingDAL().DeleteByNITIdentityWithTranscation(NetworkIdentity);
			if (num < 0)
			{
				throw new Exception(num.ToString());
			}
			num = new BATToProgramDAL().DeleteByNITIdentityWithTranscation(NetworkIdentity);
			if (num < 0)
			{
				throw new Exception(num.ToString());
			}
			num = new MultimediaDAL().DeltetByNITIdentityWithTranscation(NetworkIdentity);
			if (num < 0)
			{
				throw new Exception(num.ToString());
			}
			num = new EITInfoDAL().DeleteByNITWithTranscation(NetworkIdentity);
			if (num < 0)
			{
				throw new Exception(num.ToString());
			}
			string cmdText = "DELETE FROM program_info FROM nit_info n, ts_info t, program_info p WHERE n.id=@nit_table_id AND p.ts_table_id=t.id AND t.nit_table_id=n.id";
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@nit_table_id", NetworkIdentity, SqlDbType.BigInt) };
			return SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
		}

		public int DeleteProgramByTSIdentityWithTranscation(long TSIdentity)
		{
			int num = 0;
			num = new PMTPIDReMappingDAL().DeleteByTSIdentityWithTranscation(TSIdentity);
			if (num < 0)
			{
				throw new Exception(num.ToString());
			}
			num = new BATToProgramDAL().DeleteByTSIdentityWithTranscation(TSIdentity);
			if (num < 0)
			{
				throw new Exception(num.ToString());
			}
			num = new MultimediaDAL().DeleteByTSIdentityWithTranscation(TSIdentity);
			if (num < 0)
			{
				throw new Exception(num.ToString());
			}
			num = new EITInfoDAL().DeleteByTSWithTranscation(TSIdentity);
			if (num < 0)
			{
				throw new Exception(num.ToString());
			}
			string cmdText = "DELETE FROM program_info FROM ts_info t, program_info p WHERE t.id=@ts_table_id AND p.ts_table_id=t.id ";
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@ts_table_id", TSIdentity, SqlDbType.BigInt) };
			return SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
		}

		public int DeleteProgram(long ProgramIdentity)
		{
			try
			{
				int num = 0;
				SendNotices sendNotices = new SendNotices();
				sendNotices.Code = 2;
				sendNotices.Location = "program_info";
				sendNotices.Operation = 3;
				sendNotices.TableIdentity = ProgramIdentity;
				SendNotices notice = sendNotices;
				noticeDAL.SaveWithTranscation(notice);
				DescriptorService descriptorService = new DescriptorService();
				descriptorService.DeleteDescriptorByProgramID(ProgramIdentity);
				ProgramInfo byIdentity = GetByIdentity(ProgramIdentity);
				if (byIdentity != null)
				{
					descriptorService.UpdateLCNAndServiceList(byIdentity.TS.Identity, byIdentity.ServiceId);
				}
				num = new PMTPIDReMappingDAL().DeleteByPrgIdentityWithTranscation(ProgramIdentity);
				if (num < 0)
				{
					throw new Exception(num.ToString());
				}
				num = new BATToProgramDAL().DeleteByPrgWtihTranscation(ProgramIdentity);
				if (num < 0)
				{
					throw new Exception(num.ToString());
				}
				num = new MultimediaDAL().DeleteByPrgIdentityWithTransacation(ProgramIdentity);
				if (num < 0)
				{
					throw new Exception(num.ToString());
				}
				num = new EITInfoDAL().DeleteByPrgWithTranscation(ProgramIdentity);
				if (num < 0)
				{
					throw new Exception(num.ToString());
				}
				string cmdText = "DELETE FROM [program_info] WHERE id = @id";
				SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@id", ProgramIdentity) };
				num = SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
				if (num != 1)
				{
					throw new Exception(num.ToString());
				}
				SqlHelper.SaveChange();
				return num;
			}
			catch (Exception ex)
			{
				SqlHelper.CancelChange();
				throw new CustomException(ResourceManagement.GetLocationString("DeleteDataError"), ex, MessageType.SHOWANDLOG);
			}
		}

		public bool CanSaveProgramInfo(ProgramInfo Program)
		{
			string cmdText = "select count(1) from program_info where id <> @id and service_id = @service_id and ts_table_id = @ts_table_id ";
			SqlParameter[] commandParameters = new SqlParameter[3]
			{
				SqlHelper.AddParameter("@id", Program.Identity),
				SqlHelper.AddParameter("@service_id", Program.ServiceId),
				SqlHelper.AddParameter("@ts_table_id", Program.TsTableId)
			};
			object obj = SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
			if ((int)obj > 0)
			{
				return false;
			}
			return true;
		}

		public int Save(ProgramInfo Program)
		{
			try
			{
				int result = SaveWithTranscation(Program);
				SqlHelper.SaveChange();
				return result;
			}
			catch (Exception ex)
			{
				SqlHelper.CancelChange();
				Exception ex2 = ((ex is CustomException) ? ex : new CustomException(ResourceManagement.GetLocationString("ApplicationProcessError"), ex, MessageType.SHOWANDLOG));
				throw ex2;
			}
		}

		public int SaveWithTranscation(ProgramInfo Program)
		{
			if (!CanSaveProgramInfo(Program))
			{
				throw new CustomException(ResourceManagement.GetLocationString("ProgramIDExist"), MessageType.SHOW);
			}
			int result = 0;
			long num = 0L;
			if (Program.Identity != 0)
			{
				string cmdText = "UPDATE [program_info] SET [ts_table_id] = @ts_table_id ,[service_id] = @service_id ,[program_name] = @program_name ,[service_type] = @service_type ,[provider_name] = @provider_name ,[pmt_pid] = @pmt_pid ,[PCR_PID] = @PCR_PID ,[pmt_version_number] = @pmt_version_number ,[prg_sdt_eitscflag] = @prg_sdt_eitscflag ,[prg_sdt_eitpfflag] = @prg_sdt_eitpfflag ,[prg_sdt_runningstate] = @prg_sdt_runningstate ,[prg_sdt_freecamod] = @prg_sdt_freecamod ,[prg_manualeventid] = @prg_manualeventid ,[eit_filename] = @eit_filename WHERE id = @id";
				SqlParameter[] commandParameters = new SqlParameter[15]
				{
					SqlHelper.AddParameter("@ts_table_id", Program.TsTableId),
					SqlHelper.AddParameter("@service_id", Program.ServiceId),
					SqlHelper.AddParameter("@program_name", Program.ProgramName),
					SqlHelper.AddParameter("@service_type", Program.ServiceType),
					SqlHelper.AddParameter("@provider_name", Program.ProviderName),
					SqlHelper.AddParameter("@pmt_pid", Program.PmtId),
					SqlHelper.AddParameter("@PCR_PID", Program.PCRPID),
					SqlHelper.AddParameter("@pmt_version_number", Program.PmtVersionNumber),
					SqlHelper.AddParameter("@prg_sdt_eitscflag", Program.PrgSdtEitscflag),
					SqlHelper.AddParameter("@prg_sdt_eitpfflag", Program.PrgSdtEitpfflag),
					SqlHelper.AddParameter("@prg_sdt_runningstate", Program.PrgSdtRunningstate),
					SqlHelper.AddParameter("@prg_sdt_freecamod", Program.PrgSdtFreecamod),
					SqlHelper.AddParameter("@prg_manualeventid", Program.PrgManualeventid),
					SqlHelper.AddParameter("@eit_filename", Program.EITFileName),
					SqlHelper.AddParameter("@id", Program.Identity)
				};
				result = SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
				num = Program.Identity;
			}
			else
			{
				string cmdText = "INSERT INTO [program_info] ([ts_table_id] ,[service_id] ,[program_name] ,[service_type] ,[provider_name] ,[pmt_pid] ,[PCR_PID] ,[pmt_version_number] ,[prg_sdt_eitscflag] ,[prg_sdt_eitpfflag] ,[prg_sdt_runningstate] ,[prg_sdt_freecamod] ,[prg_manualeventid] ,[eit_filename]) VALUES (@ts_table_id ,@service_id ,@program_name ,@service_type ,@provider_name ,@pmt_pid ,@PCR_PID ,@pmt_version_number ,@prg_sdt_eitscflag ,@prg_sdt_eitpfflag ,@prg_sdt_runningstate ,@prg_sdt_freecamod ,@prg_manualeventid ,@eit_filename) SELECT @@IDENTITY";
				SqlParameter[] commandParameters = new SqlParameter[14]
				{
					SqlHelper.AddParameter("@ts_table_id", Program.TsTableId),
					SqlHelper.AddParameter("@service_id", Program.ServiceId),
					SqlHelper.AddParameter("@program_name", Program.ProgramName),
					SqlHelper.AddParameter("@service_type", Program.ServiceType),
					SqlHelper.AddParameter("@provider_name", Program.ProviderName),
					SqlHelper.AddParameter("@pmt_pid", Program.PmtId),
					SqlHelper.AddParameter("@PCR_PID", Program.PCRPID),
					SqlHelper.AddParameter("@pmt_version_number", Program.PmtVersionNumber),
					SqlHelper.AddParameter("@prg_sdt_eitscflag", Program.PrgSdtEitscflag),
					SqlHelper.AddParameter("@prg_sdt_eitpfflag", Program.PrgSdtEitpfflag),
					SqlHelper.AddParameter("@prg_sdt_runningstate", Program.PrgSdtRunningstate),
					SqlHelper.AddParameter("@prg_sdt_freecamod", Program.PrgSdtFreecamod),
					SqlHelper.AddParameter("@prg_manualeventid", Program.PrgManualeventid),
					SqlHelper.AddParameter("@eit_filename", Program.EITFileName)
				};
				using (SqlDataReader sqlDataReader = SqlHelper.ExecuteReader(CommandType.Text, cmdText, commandParameters))
				{
					if (sqlDataReader.Read())
					{
						num = Tool.GetInt64(sqlDataReader[0]);
						result = 1;
					}
				}
			}
			SendNotices sendNotices = new SendNotices();
			sendNotices.Code = 2;
			sendNotices.Location = "program_info";
			sendNotices.Operation = ((Program.Identity <= 0) ? 1 : 2);
			sendNotices.TableIdentity = num;
			SendNotices notice = sendNotices;
			noticeDAL.SaveWithTranscation(notice);
			DescriptorService descriptorService = new DescriptorService();
			ServiceDescriptor args = new ServiceDescriptor(num, EnumPossibleLoaction.SDT, (byte)Program.ServiceType, Program.ProgramName, Program.ProviderName, EnumDescriptorTag.service_descriptor);
			if (!descriptorService.AddDescriptor(args))
			{
				throw new Exception("save service_descriptor error");
			}
			return result;
		}

		public IList<ProgramInfo> GetOutputProgram()
		{
			IList<ProgramInfo> list = new List<ProgramInfo>();
			string cmdText = "SELECT * FROM program_info p WHERE EXISTS ( \tSELECT 1 FROM PAT pat \tWHERE pat.program_table_id = p.id )";
			using (SqlDataReader sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, (SqlParameter[])null))
			{
				while (sqlDataReader.Read())
				{
					ProgramInfo programInfo = ProgramSqlDataReader(sqlDataReader);
					programInfo.ProgramName = programInfo.BKValue;
					programInfo.Node = programInfo;
					programInfo.ReadTS += prg_ReadTS;
					list.Add(programInfo);
				}
			}
			return list;
		}

		public IList<ProgramInfo> GetHasEITFileProgram()
		{
			IList<ProgramInfo> list = new List<ProgramInfo>();
			string cmdText = "SELECT p.* FROM program_info p, ts_info t WHERE ISNULL(eit_filename, '') <> '' AND p.ts_table_id = t.id ORDER BY t.nit_table_id, t.id";
			using (SqlDataReader sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, (SqlParameter[])null))
			{
				while (sqlDataReader.Read())
				{
					list.Add(ProgramSqlDataReader(sqlDataReader));
				}
			}
			return list;
		}

		public IList<ProgramInfo> GetByEitFile(string fileName)
		{
			IList<ProgramInfo> list = new List<ProgramInfo>();
			string cmdText = "SELECT p.* FROM program_info p, ts_info t WHERE ISNULL(eit_filename, '') = @fileName AND p.ts_table_id = t.id ORDER BY t.id";
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@fileName", fileName, SqlDbType.NVarChar) };
			using (SqlDataReader sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters))
			{
				while (sqlDataReader.Read())
				{
					list.Add(ProgramSqlDataReader(sqlDataReader));
				}
			}
			return list;
		}
	}
}
