using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.IDAL;
using DeXin.EPG.Model;
using DeXin.EPG.Resource;
using DeXin.EPG.Resource.ResourcesSpecialFiles;

namespace DeXin.EPG.DAL
{
	public class TSInfoDAL : ITSInfoDAL
	{
		private SendNoticesDAL noticeDAL = new SendNoticesDAL();

		public IList<TsInfo> GetTSInfoByNetWorkIdentity(long NetWorkIdentity)
		{
			IList<TsInfo> list = new List<TsInfo>();
			string cmdText = "SELECT [id],[ts_id],[ts_name],[original_network_id],[pat_version_number],[cat_version_number],[sdt_version_number] FROM [ts_info] WHERE nit_table_id=@nit_id";
			SqlDataReader sqlDataReader = null;
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@nit_id", NetWorkIdentity, SqlDbType.BigInt) };
			try
			{
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
				while (sqlDataReader.Read())
				{
					long @int = Tool.GetInt64(sqlDataReader["id"]);
					string tableName = sqlDataReader["ts_name"].ToString();
					TsInfo tsInfo = new TsInfo(@int, tableName);
					tsInfo.NitTableId = NetWorkIdentity;
					tsInfo.TsId = Tool.GetInt32(sqlDataReader["ts_id"]);
					tsInfo.OriginalNetworkId = Tool.GetInt32(sqlDataReader["original_network_id"]);
					tsInfo.PatVersionNumber = Tool.GetInt32(sqlDataReader["pat_version_number"]);
					tsInfo.CatVersionNumber = Tool.GetInt32(sqlDataReader["cat_version_number"]);
					tsInfo.SdtVersionNumber = Tool.GetInt32(sqlDataReader["sdt_version_number"]);
					TsInfo tsInfo2 = tsInfo;
					tsInfo2.TsName = tsInfo2.BKValue;
					tsInfo2.Node = tsInfo2;
					tsInfo2.ReadNIT += ts_ReadNIT;
					list.Add(tsInfo2);
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

		private void ts_ReadNIT(object sender)
		{
			TsInfo tsInfo = sender as TsInfo;
			tsInfo.NIT = new NITInfoDAL().GetNITInfoByNetworkIdentity(tsInfo.NitTableId);
		}

		public IList<TsInfo> GetOutputTS()
		{
			IList<TsInfo> list = new List<TsInfo>();
			string cmdText = "SELECT  t.* FROM ts_info t WHERE EXISTS ( SELECT 1 FROM program_info p, PAT pat WHERE pat.program_table_id = p.id AND p.ts_table_id = t.id )";
			using (SqlDataReader sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, (SqlParameter[])null))
			{
				while (sqlDataReader.Read())
				{
					long @int = Tool.GetInt64(sqlDataReader["id"]);
					string tableName = sqlDataReader["ts_name"].ToString();
					TsInfo tsInfo = new TsInfo(@int, tableName);
					tsInfo.NitTableId = Tool.GetInt32(sqlDataReader["nit_table_id"]);
					tsInfo.TsId = Tool.GetInt32(sqlDataReader["ts_id"]);
					tsInfo.OriginalNetworkId = Tool.GetInt32(sqlDataReader["original_network_id"]);
					tsInfo.PatVersionNumber = Tool.GetInt32(sqlDataReader["pat_version_number"]);
					tsInfo.CatVersionNumber = Tool.GetInt32(sqlDataReader["cat_version_number"]);
					tsInfo.SdtVersionNumber = Tool.GetInt32(sqlDataReader["sdt_version_number"]);
					TsInfo tsInfo2 = tsInfo;
					tsInfo2.TsName = tsInfo2.BKValue;
					tsInfo2.Node = tsInfo2;
					tsInfo2.ReadNIT += ts_ReadNIT;
					list.Add(tsInfo2);
				}
			}
			return list;
		}

		public IList<TsInfo> GetTSInfoByTSId(int TSId)
		{
			IList<TsInfo> list = new List<TsInfo>();
			string cmdText = "SELECT [id],[nit_table_id],[ts_id],[ts_name],[original_network_id],[pat_version_number],[cat_version_number],[sdt_version_number] FROM [ts_info] WHERE ts_id = @ts_id";
			SqlDataReader sqlDataReader = null;
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@ts_id", TSId) };
			try
			{
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
				while (sqlDataReader.Read())
				{
					long @int = Tool.GetInt64(sqlDataReader["id"]);
					string tableName = sqlDataReader["ts_name"].ToString();
					TsInfo tsInfo = new TsInfo(@int, tableName);
					tsInfo.NitTableId = Tool.GetInt32(sqlDataReader["nit_table_id"]);
					tsInfo.TsId = Tool.GetInt32(sqlDataReader["ts_id"]);
					tsInfo.OriginalNetworkId = Tool.GetInt32(sqlDataReader["original_network_id"]);
					tsInfo.PatVersionNumber = Tool.GetInt32(sqlDataReader["pat_version_number"]);
					tsInfo.CatVersionNumber = Tool.GetInt32(sqlDataReader["cat_version_number"]);
					tsInfo.SdtVersionNumber = Tool.GetInt32(sqlDataReader["sdt_version_number"]);
					TsInfo tsInfo2 = tsInfo;
					tsInfo2.TsName = tsInfo2.BKValue;
					tsInfo2.Node = tsInfo2;
					tsInfo2.ReadNIT += ts_ReadNIT;
					list.Add(tsInfo2);
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

		public TsInfo GetTSInfoByTSIdentity(long TSIdentity)
		{
			string cmdText = "SELECT [id],[nit_table_id],[ts_id],[ts_name],[original_network_id],[pat_version_number],[cat_version_number],[sdt_version_number] FROM [ts_info] WHERE id = @id";
			SqlDataReader sqlDataReader = null;
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@id", TSIdentity) };
			try
			{
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
				if (sqlDataReader.Read())
				{
					string tableName = sqlDataReader["ts_name"].ToString();
					TsInfo tsInfo = new TsInfo(TSIdentity, tableName);
					tsInfo.NitTableId = Tool.GetInt32(sqlDataReader["nit_table_id"]);
					tsInfo.TsId = Tool.GetInt32(sqlDataReader["ts_id"]);
					tsInfo.OriginalNetworkId = Tool.GetInt32(sqlDataReader["original_network_id"]);
					tsInfo.PatVersionNumber = Tool.GetInt32(sqlDataReader["pat_version_number"]);
					tsInfo.CatVersionNumber = Tool.GetInt32(sqlDataReader["cat_version_number"]);
					tsInfo.SdtVersionNumber = Tool.GetInt32(sqlDataReader["sdt_version_number"]);
					TsInfo tsInfo2 = tsInfo;
					tsInfo2.TsName = tsInfo2.BKValue;
					tsInfo2.Node = tsInfo2;
					tsInfo2.ReadNIT += ts_ReadNIT;
					return tsInfo2;
				}
				return null;
			}
			catch (Exception ex)
			{
				throw new CustomException(ResourceManagement.GetLocationString("ReadDataError"), ex, MessageType.SHOWANDLOG);
			}
			finally
			{
				SqlHelper.CloseAll(null, sqlDataReader);
			}
		}

		public int DeleteTSInfo(long TSInfoIdentity)
		{
			try
			{
				int num = 0;
				SendNotices sendNotices = new SendNotices();
				sendNotices.Code = 1;
				sendNotices.Location = "ts_info";
				sendNotices.Operation = 3;
				sendNotices.TableIdentity = TSInfoIdentity;
				SendNotices notice = sendNotices;
				noticeDAL.SaveWithTranscation(notice);
				num = new MultiIPOutputDAL().DeleteByTSIdentityWithTranscation(TSInfoIdentity);
				if (num < 0)
				{
					throw new Exception(num.ToString());
				}
				num = new ReMappingPIDDAL().DeleteByTSIdentityWithTranscation(TSInfoIdentity);
				if (num < 0)
				{
					throw new Exception(num.ToString());
				}
				num = new SendOrNotInfoDAL().DeleteByTSIdentityWithTranscation(TSInfoIdentity);
				if (num < 0)
				{
					throw new Exception(num.ToString());
				}
				num = new SendSpeedInfoDAL().DeleteByTSIdentityWithTranscation(TSInfoIdentity);
				if (num < 0)
				{
					throw new Exception(num.ToString());
				}
				new DescriptorService().DeleteDescriptorByTSID(TSInfoIdentity);
				num = new ProgramInfoDAL().DeleteProgramByTSIdentityWithTranscation(TSInfoIdentity);
				if (num < 0)
				{
					throw new Exception(num.ToString());
				}
				string cmdText = "DELETE FROM [ts_info] WHERE id = @id";
				SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@id", TSInfoIdentity) };
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

		public int DeleteTSInfoByNITInfoWithTranscation(long NetworkIdentity)
		{
			int num = 0;
			num = new MultiIPOutputDAL().DeleteByNITIdentityWithTranscation(NetworkIdentity);
			if (num < 0)
			{
				throw new Exception(num.ToString());
			}
			num = new ReMappingPIDDAL().DeleteByNITIdentityWithTranscation(NetworkIdentity);
			if (num < 0)
			{
				throw new Exception(num.ToString());
			}
			num = new SendOrNotInfoDAL().DeleteByNITIdentityWithTranscation(NetworkIdentity);
			if (num < 0)
			{
				throw new Exception(num.ToString());
			}
			num = new SendSpeedInfoDAL().DeleteByNITIdentityWithTranscation(NetworkIdentity);
			if (num < 0)
			{
				throw new Exception(num.ToString());
			}
			num = new ProgramInfoDAL().DeleteProgramByNetworkIdentityWithTranscation(NetworkIdentity);
			if (num < 0)
			{
				throw new Exception(num.ToString());
			}
			string cmdText = "DELETE FROM [ts_info] WHERE nit_table_id = @nit_table_id";
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@nit_table_id", NetworkIdentity) };
			return SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
		}

		public bool CanSaveTSInfo(TsInfo TS)
		{
			string cmdText = "select count(1) from ts_info where id <> @id and ts_id = @ts_id and nit_table_id = @nit_table_id ";
			SqlParameter[] commandParameters = new SqlParameter[3]
			{
				SqlHelper.AddParameter("@id", TS.Identity),
				SqlHelper.AddParameter("@ts_id", TS.TsId),
				SqlHelper.AddParameter("@nit_table_id", TS.NitTableId)
			};
			object obj = SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
			if ((int)obj > 0)
			{
				return false;
			}
			return true;
		}

		public int SaveTSInfo(TsInfo TS)
		{
			if (!CanSaveTSInfo(TS))
			{
				throw new CustomException(ResourceSpecial.TSIDExists, MessageType.SHOW);
			}
			int num = 0;
			long tableIdentity = 0L;
			try
			{
				if (TS.Identity != 0)
				{
					string cmdText = "UPDATE [ts_info] SET [nit_table_id] = @nit_table_id    ,[ts_id] = @ts_id   ,[ts_name] = @ts_name    ,[original_network_id] = @original_network_id    ,[pat_version_number] = @pat_version_number    ,[cat_version_number] = @cat_version_number    ,[sdt_version_number] = @sdt_version_number WHERE id = @id ";
					SqlParameter[] commandParameters = new SqlParameter[8]
					{
						SqlHelper.AddParameter("@nit_table_id", TS.NitTableId),
						SqlHelper.AddParameter("@ts_id", TS.TsId),
						SqlHelper.AddParameter("@ts_name", TS.TsName),
						SqlHelper.AddParameter("@original_network_id", TS.OriginalNetworkId),
						SqlHelper.AddParameter("@pat_version_number", TS.PatVersionNumber),
						SqlHelper.AddParameter("@cat_version_number", TS.CatVersionNumber),
						SqlHelper.AddParameter("@sdt_version_number", TS.SdtVersionNumber),
						SqlHelper.AddParameter("@id", TS.Identity)
					};
					num = SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
					tableIdentity = TS.Identity;
				}
				else
				{
					string cmdText = "INSERT INTO [ts_info] ([nit_table_id] ,[ts_id] ,[ts_name] ,[original_network_id] ,[pat_version_number] ,[cat_version_number] ,[sdt_version_number]) VALUES (@nit_table_id ,@ts_id ,@ts_name ,@original_network_id ,@pat_version_number ,@cat_version_number ,@sdt_version_number) SELECT @@IDENTITY";
					SqlParameter[] commandParameters = new SqlParameter[7]
					{
						SqlHelper.AddParameter("@nit_table_id", TS.NitTableId),
						SqlHelper.AddParameter("@ts_id", TS.TsId),
						SqlHelper.AddParameter("@ts_name", TS.TsName),
						SqlHelper.AddParameter("@original_network_id", TS.OriginalNetworkId),
						SqlHelper.AddParameter("@pat_version_number", TS.PatVersionNumber),
						SqlHelper.AddParameter("@cat_version_number", TS.CatVersionNumber),
						SqlHelper.AddParameter("@sdt_version_number", TS.SdtVersionNumber)
					};
					using (SqlDataReader sqlDataReader = SqlHelper.ExecuteReader(CommandType.Text, cmdText, commandParameters))
					{
						if (sqlDataReader.Read())
						{
							tableIdentity = Tool.GetInt64(sqlDataReader[0]);
							num = 1;
						}
					}
				}
				SendNotices sendNotices = new SendNotices();
				sendNotices.Code = 1;
				sendNotices.Location = "ts_info";
				sendNotices.Operation = ((TS.Identity <= 0) ? 1 : 2);
				sendNotices.TableIdentity = tableIdentity;
				SendNotices notice = sendNotices;
				noticeDAL.SaveWithTranscation(notice);
				if (num > 0)
				{
					SqlHelper.SaveChange();
				}
				return num;
			}
			catch (Exception ex)
			{
				SqlHelper.CancelChange();
				throw new CustomException(ResourceManagement.GetLocationString("SaveDataError"), ex, MessageType.SHOWANDLOG);
			}
		}
	}
}
