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
	public class NITInfoDAL : INITInfoDAL
	{
		private SendNoticesDAL noticeDAL = new SendNoticesDAL();

		public IList<NitInfo> GetAllNITInfos()
		{
			IList<NitInfo> list = new List<NitInfo>();
			string cmdText = "SELECT [id],[network_id],[network_name],[version_number],[LanguageCode]  FROM [nit_info]";
			SqlDataReader sqlDataReader = null;
			try
			{
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText);
				while (sqlDataReader.Read())
				{
					long @int = Tool.GetInt64(sqlDataReader["id"]);
					string tableName = sqlDataReader["network_name"].ToString();
					NitInfo nitInfo = new NitInfo(@int, tableName);
					nitInfo.NetworkId = Tool.GetInt32(sqlDataReader["network_id"]);
					nitInfo.NetworkName = sqlDataReader["network_name"].ToString();
					nitInfo.VersionNumber = Tool.GetInt32(sqlDataReader["version_number"]);
					nitInfo.LanguageCode = sqlDataReader["LanguageCode"].ToString();
					NitInfo nitInfo2 = nitInfo;
					nitInfo2.Node = nitInfo2;
					list.Add(nitInfo2);
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

		public IList<NitInfo> GetOutputNIT()
		{
			IList<NitInfo> list = new List<NitInfo>();
			string cmdText = "SELECT * FROM nit_info n WHERE EXISTS ( \tSELECT 1 FROM ts_info t, program_info p, PAT pat \tWHERE pat.program_table_id = p.id \tAND p.ts_table_id = t.id \tAND t.nit_table_id = n.id )";
			using (SqlDataReader sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, (SqlParameter[])null))
			{
				while (sqlDataReader.Read())
				{
					long @int = Tool.GetInt64(sqlDataReader["id"]);
					string tableName = sqlDataReader["network_name"].ToString();
					NitInfo nitInfo = new NitInfo(@int, tableName);
					nitInfo.NetworkId = Tool.GetInt32(sqlDataReader["network_id"]);
					nitInfo.NetworkName = sqlDataReader["network_name"].ToString();
					nitInfo.VersionNumber = Tool.GetInt32(sqlDataReader["version_number"]);
					nitInfo.LanguageCode = sqlDataReader["LanguageCode"].ToString();
					NitInfo nitInfo2 = nitInfo;
					nitInfo2.Node = nitInfo2;
					list.Add(nitInfo2);
				}
			}
			return list;
		}

		public IList<NitInfo> GetNITInfoByNetworkID(int NetworkID)
		{
			IList<NitInfo> list = new List<NitInfo>();
			string cmdText = "SELECT * FROM [nit_info] WHERE network_id = @network_id";
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@network_id", NetworkID) };
			SqlDataReader sqlDataReader = null;
			try
			{
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
				while (sqlDataReader.Read())
				{
					long @int = Tool.GetInt64(sqlDataReader["id"]);
					string tableName = sqlDataReader["network_name"].ToString();
					NitInfo nitInfo = new NitInfo(@int, tableName);
					nitInfo.NetworkId = NetworkID;
					nitInfo.NetworkName = sqlDataReader["network_name"].ToString();
					nitInfo.VersionNumber = Tool.GetInt32(sqlDataReader["version_number"]);
					nitInfo.LanguageCode = sqlDataReader["LanguageCode"].ToString();
					NitInfo nitInfo2 = nitInfo;
					nitInfo2.Node = nitInfo2;
					list.Add(nitInfo2);
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

		public NitInfo GetNITInfoByNetworkIdentity(long NetworkIdentity)
		{
			string cmdText = "SELECT * FROM [nit_info] WHERE id = @id";
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@id", NetworkIdentity) };
			SqlDataReader sqlDataReader = null;
			try
			{
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
				if (sqlDataReader.Read())
				{
					string tableName = sqlDataReader["network_name"].ToString();
					NitInfo nitInfo = new NitInfo(NetworkIdentity, tableName);
					nitInfo.NetworkId = Tool.GetInt32(sqlDataReader["network_id"]);
					nitInfo.NetworkName = sqlDataReader["network_name"].ToString();
					nitInfo.VersionNumber = Tool.GetInt32(sqlDataReader["version_number"]);
					nitInfo.LanguageCode = sqlDataReader["LanguageCode"].ToString();
					NitInfo nitInfo2 = nitInfo;
					nitInfo2.Node = nitInfo2;
					return nitInfo2;
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

		public bool CanSaveNITInfo(NitInfo NIT)
		{
			string cmdText = "select count(1) from nit_info where id <> @id and network_id = @network_id";
			SqlParameter[] commandParameters = new SqlParameter[2]
			{
				SqlHelper.AddParameter("@id", NIT.Identity),
				SqlHelper.AddParameter("@network_id", NIT.NetworkId)
			};
			object obj = SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
			if ((int)obj > 0)
			{
				return false;
			}
			return true;
		}

		public int SaveNITInfo(NitInfo NIT)
		{
			if (!CanSaveNITInfo(NIT))
			{
				throw new CustomException(ResourceManagement.GetLocationString("NetworkIDExists"), MessageType.SHOW);
			}
			try
			{
				int num = 0;
				long num2 = 0L;
				if (NIT.Identity != 0)
				{
					string cmdText = "UPDATE [nit_info]  SET [network_id] = @network_id  ,[network_name] = @network_name  ,[version_number] = @version_number  ,[LanguageCode] = @language_code  WHERE id = @id";
					SqlParameter[] commandParameters = new SqlParameter[5]
					{
						SqlHelper.AddParameter("@network_id", NIT.NetworkId),
						SqlHelper.AddParameter("@network_name", NIT.NetworkName),
						SqlHelper.AddParameter("@version_number", NIT.VersionNumber),
						SqlHelper.AddParameter("@language_code", NIT.LanguageCode),
						SqlHelper.AddParameter("@id", NIT.Identity)
					};
					num = SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
					num2 = NIT.Identity;
				}
				else
				{
					string cmdText = "INSERT INTO [nit_info] ([network_id] ,[network_name] ,[version_number] ,[LanguageCode]) VALUES (@network_id ,@network_name ,@version_number ,@language_code) SELECT @@IDENTITY";
					SqlParameter[] commandParameters = new SqlParameter[4]
					{
						SqlHelper.AddParameter("@network_id", NIT.NetworkId),
						SqlHelper.AddParameter("@network_name", NIT.NetworkName),
						SqlHelper.AddParameter("@version_number", NIT.VersionNumber),
						SqlHelper.AddParameter("@language_code", NIT.LanguageCode)
					};
					using (SqlDataReader sqlDataReader = SqlHelper.ExecuteReader(CommandType.Text, cmdText, commandParameters))
					{
						if (sqlDataReader.Read())
						{
							num2 = Tool.GetInt64(sqlDataReader[0]);
							num = 1;
						}
					}
				}
				SendNotices sendNotices = new SendNotices();
				sendNotices.Code = 7;
				sendNotices.Location = "nit_info";
				sendNotices.Operation = ((NIT.Identity <= 0) ? 1 : 2);
				sendNotices.TableIdentity = num2;
				SendNotices sendNotices2 = sendNotices;
				DescriptorService descriptorService = new DescriptorService();
				StringNameDescriptor args = new StringNameDescriptor(num2, EnumPossibleLoaction.NIT, NIT.NetworkName, EnumDescriptorTag.network_name_name_descriptor);
				if (!descriptorService.AddDescriptor(args))
				{
					throw new CustomException(ResourceManagement.GetLocationString("SaveDescriptorError"), MessageType.SHOW);
				}
				if (num > 0)
				{
					SqlHelper.SaveChange();
				}
				return num;
			}
			catch (Exception ex)
			{
				SqlHelper.CancelChange();
				Exception ex2 = ((ex is CustomException) ? ex : new CustomException(ResourceManagement.GetLocationString("ApplicationProcessError"), ex, MessageType.SHOWANDLOG));
				throw ex2;
			}
		}

		public int DeleteNITInfo(long NetworkIdentity)
		{
			try
			{
				int num = 0;
				new DescriptorService().DeleteDescriptorByNetWorkID(NetworkIdentity);
				num = new BATInfoDAL().DeleteByNITIdentityWithTranscation(NetworkIdentity);
				if (num < 0)
				{
					throw new Exception(num.ToString());
				}
				num = new TSInfoDAL().DeleteTSInfoByNITInfoWithTranscation(NetworkIdentity);
				if (num < 0)
				{
					throw new Exception(num.ToString());
				}
				string cmdText = "DELETE FROM [nit_info] WHERE id = @id";
				SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@id", NetworkIdentity) };
				num = SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
				if (num != 1)
				{
					throw new Exception(num.ToString());
				}
				SendNotices sendNotices = new SendNotices();
				sendNotices.Code = 7;
				sendNotices.Location = "nit_info";
				sendNotices.Operation = 2;
				sendNotices.TableIdentity = NetworkIdentity;
				SendNotices sendNotices2 = sendNotices;
				SqlHelper.SaveChange();
				return num;
			}
			catch (Exception ex)
			{
				SqlHelper.CancelChange();
				throw new CustomException(ResourceManagement.GetLocationString("DeleteDataError"), ex, MessageType.SHOWANDLOG);
			}
		}
	}
}
