using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.IDAL;
using DeXin.EPG.Model;
using DeXin.EPG.Resource;
using DeXin.EPG.Tools;

namespace DeXin.EPG.DAL
{
	public class BATInfoDAL : IBATInfoDAL
	{
		private SendNoticesDAL noticeDAL = new SendNoticesDAL();

		public IList<BATInfo> GetByNetworkIdentity(long NetworkIdentity)
		{
			IList<BATInfo> list = new List<BATInfo>();
			string cmdText = "SELECT * FROM [bat_info] WHERE nit_table_id=@nit_id";
			SqlDataReader sqlDataReader = null;
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@nit_id", NetworkIdentity, SqlDbType.BigInt) };
			try
			{
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
				while (sqlDataReader.Read())
				{
					long @int = Tool.GetInt64(sqlDataReader["id"]);
					string text = sqlDataReader["bat_name"].ToString();
					BATInfo bATInfo = new BATInfo(@int, text);
					bATInfo.NitTableId = Tool.GetInt64(sqlDataReader["nit_table_id"]);
					bATInfo.BATId = Tool.GetInt32(sqlDataReader["bat_id"]);
					bATInfo.BATName = text;
					bATInfo.BATVersionNumber = Tool.GetInt32(sqlDataReader["bat_version_number"]);
					BATInfo bATInfo2 = bATInfo;
					bATInfo2.Node = bATInfo2;
					bATInfo2.ReadNIT += bat_ReadNIT;
					list.Add(bATInfo2);
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

		public BATInfo GetByIdentity(long Identity)
		{
			string cmdText = "SELECT * FROM [bat_info] WHERE id = @id";
			SqlDataReader sqlDataReader = null;
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@id", Identity, SqlDbType.BigInt) };
			try
			{
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
				if (sqlDataReader.Read())
				{
					long @int = Tool.GetInt64(sqlDataReader["id"]);
					string tableName = sqlDataReader["bat_name"].ToString();
					BATInfo bATInfo = new BATInfo(@int, tableName);
					bATInfo.NitTableId = Tool.GetInt64(sqlDataReader["nit_table_id"]);
					bATInfo.BATId = Tool.GetInt32(sqlDataReader["bat_id"]);
					bATInfo.BATVersionNumber = Tool.GetInt32(sqlDataReader["bat_version_number"]);
					BATInfo bATInfo2 = bATInfo;
					bATInfo2.Node = bATInfo2;
					bATInfo2.ReadNIT += bat_ReadNIT;
					return bATInfo2;
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

		public BATInfo GetByBATId(long NetworkIdentity, int BATId)
		{
			string cmdText = "SELECT * FROM [bat_info] WHERE nit_table_id = @nit_table_id AND bat_id = @bat_id";
			SqlDataReader sqlDataReader = null;
			SqlParameter[] commandParameters = new SqlParameter[2]
			{
				SqlHelper.AddParameter("@nit_table_id", NetworkIdentity, SqlDbType.BigInt),
				SqlHelper.AddParameter("@bat_id", BATId, SqlDbType.BigInt)
			};
			try
			{
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
				if (sqlDataReader.Read())
				{
					long @int = Tool.GetInt64(sqlDataReader["id"]);
					string text = sqlDataReader["bat_name"].ToString();
					BATInfo bATInfo = new BATInfo(@int, text);
					bATInfo.NitTableId = Tool.GetInt64(sqlDataReader["nit_table_id"]);
					bATInfo.BATId = Tool.GetInt32(sqlDataReader["bat_id"]);
					bATInfo.BATName = text;
					bATInfo.BATVersionNumber = Tool.GetInt32(sqlDataReader["bat_version_number"]);
					BATInfo bATInfo2 = bATInfo;
					bATInfo2.Node = bATInfo2;
					bATInfo2.ReadNIT += bat_ReadNIT;
					return bATInfo2;
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

		public bool CanSave(BATInfo BAT)
		{
			string cmdText = "select count(1) from bat_info where id <> @id and nit_table_id = @nit_table_id and bat_id = @bat_id ";
			SqlParameter[] commandParameters = new SqlParameter[3]
			{
				SqlHelper.AddParameter("@id", BAT.Identity),
				SqlHelper.AddParameter("@nit_table_id", BAT.NitTableId),
				SqlHelper.AddParameter("@bat_id", BAT.BATId)
			};
			object obj = SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
			if ((int)obj > 0)
			{
				return false;
			}
			return true;
		}

		public void Save(BATToProgram BATToPrgList, bool updateServiceList = true)
		{
			try
			{
				BATToProgramDAL bATToProgramDAL = new BATToProgramDAL();
				DescriptorService descriptorService = new DescriptorService();
				long num = 0L;
				BATInfo bAT = BATToPrgList.BAT;
				if (!CanSave(bAT))
				{
					throw new CustomException(ResourceManagement.GetLocationString("BATIDExists"), MessageType.SHOW);
				}
				if (bAT.Identity != 0)
				{
					string cmdText = "UPDATE [bat_info] SET [nit_table_id] = @nit_table_id ,[bat_id] = @bat_id ,[bat_name] = @bat_name ,[bat_version_number] = @bat_version_number WHERE id = @id ";
					SqlParameter[] commandParameters = new SqlParameter[5]
					{
						SqlHelper.AddParameter("@nit_table_id", bAT.NitTableId),
						SqlHelper.AddParameter("@bat_id", bAT.BATId),
						SqlHelper.AddParameter("@bat_name", bAT.BATName),
						SqlHelper.AddParameter("@bat_version_number", bAT.BATVersionNumber),
						SqlHelper.AddParameter("@id", bAT.Identity)
					};
					SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
					num = bAT.Identity;
				}
				else
				{
					string cmdText = "INSERT INTO [bat_info] ([nit_table_id] ,[bat_id] ,[bat_name] ,[bat_version_number]) VALUES (@nit_table_id ,@bat_id ,@bat_name ,@bat_version_number) SELECT @@IDENTITY ";
					SqlParameter[] commandParameters = new SqlParameter[4]
					{
						SqlHelper.AddParameter("@nit_table_id", bAT.NitTableId),
						SqlHelper.AddParameter("@bat_id", bAT.BATId),
						SqlHelper.AddParameter("@bat_name", bAT.BATName),
						SqlHelper.AddParameter("@bat_version_number", bAT.BATVersionNumber)
					};
					using (SqlDataReader sqlDataReader = SqlHelper.ExecuteReader(CommandType.Text, cmdText, commandParameters))
					{
						if (sqlDataReader.Read())
						{
							num = Tool.GetInt64(sqlDataReader[0]);
						}
					}
				}
				SendNotices sendNotices = new SendNotices();
				sendNotices.Location = "bat_info";
				sendNotices.Operation = ((bAT.Identity <= 0) ? 1 : 2);
				sendNotices.Code = 4;
				sendNotices.TableIdentity = num;
				SendNotices notice = sendNotices;
				noticeDAL.SaveWithTranscation(notice);
				StringNameDescriptor args = new StringNameDescriptor(num, EnumPossibleLoaction.BAT, bAT.BATName, EnumDescriptorTag.bouquet_name_descriptor);
				if (!descriptorService.AddDescriptor(args))
				{
					throw new CustomException(ResourceManagement.GetLocationString("SaveDescriptorError"), MessageType.SHOW);
				}
				if (updateServiceList)
				{
					bATToProgramDAL.DeleteByBATIdentityWtihTranscation(num);
					foreach (ProgramInfo program in BATToPrgList.ProgramList)
					{
						if (bATToProgramDAL.SaveWithTranscation(num, program.Identity) != 1)
						{
							throw new Exception(string.Format("Save bat_to_program Error:(batKey:{0};programKey:{1})", bAT.Identity, program.Identity));
						}
					}
					descriptorService.DeleteBAT_TSDescriptorWithTranscation(num, EnumDescriptorTag.service_list_descriptor);
					foreach (IGrouping<long, ProgramInfo> item in (from p in BATToPrgList.ProgramList
						group p by p.TsTableId).ToList())
					{
						DescriptorSigleBase descriptorSigleBase = new DescriptorSigleBase(EnumPossibleLoaction.BAT_TS, item.Key, num);
						descriptorSigleBase.DesTypeTag = 65;
						descriptorSigleBase.DataLength = item.Count() * 3;
						descriptorSigleBase.DescriptionData = new byte[descriptorSigleBase.DataLength];
						int startIndex = 0;
						foreach (ProgramInfo item2 in item)
						{
							Tool.ToByteArray((short)item2.ServiceId, descriptorSigleBase.DescriptionData, ref startIndex);
							Tool.ToByteArray((byte)item2.ServiceType, descriptorSigleBase.DescriptionData, ref startIndex);
						}
						string empty = string.Empty;
						descriptorService.AddDescriptor(descriptorSigleBase);
					}
				}
				SqlHelper.SaveChange();
			}
			catch (Exception ex)
			{
				SqlHelper.CancelChange();
				Exception ex2 = ((ex is CustomException) ? ex : new CustomException(ResourceManagement.GetLocationString("ApplicationProcessError"), ex, MessageType.SHOWANDLOG));
				throw ex2;
			}
		}

		public void Delete(IList<long> ListIdentity)
		{
			try
			{
				foreach (long item in ListIdentity)
				{
					SendNotices sendNotices = new SendNotices();
					sendNotices.Location = "bat_info";
					sendNotices.Operation = 3;
					sendNotices.Code = 4;
					sendNotices.TableIdentity = item;
					SendNotices notice = sendNotices;
					noticeDAL.SaveWithTranscation(notice);
					new DescriptorService().DeleteBATDescriptorByBATID(item);
					new BATToProgramDAL().DeleteByBATIdentityWtihTranscation(item);
					string cmdText = "DELETE FROM [bat_info] WHERE id = @id";
					SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@id", item) };
					SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
				}
				SqlHelper.SaveChange();
			}
			catch (Exception ex)
			{
				SqlHelper.CancelChange();
				throw new CustomException(ResourceManagement.GetLocationString("ApplicationProcessError"), ex, MessageType.SHOWANDLOG);
			}
		}

		public int DeleteByNITIdentityWithTranscation(long NITIdentity)
		{
			int num = 0;
			num = new BATToProgramDAL().DeleteByNITIdentityWithTranscation(NITIdentity);
			if (num < 0)
			{
				throw new Exception(num.ToString());
			}
			string cmdText = "DELETE FROM bat_info FROM bat_info b, nit_info n WHERE n.id = @id AND n.id = b.nit_table_id";
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@id", NITIdentity) };
			return SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
		}

		public IList<BATInfo> GetOutputBAT()
		{
			IList<BATInfo> list = new List<BATInfo>();
			string cmdText = "SELECT *  FROM bat_info b WHERE EXISTS (\tSELECT 1 FROM PAT pat, program_info p, ts_info t \tWHERE pat.program_table_id = p.id \tAND p.ts_table_id = t.id \tAND t.nit_table_id = b.nit_table_id )";
			using (SqlDataReader sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, (SqlParameter[])null))
			{
				while (sqlDataReader.Read())
				{
					long @int = Tool.GetInt64(sqlDataReader["id"]);
					string text = sqlDataReader["bat_name"].ToString();
					BATInfo bATInfo = new BATInfo(@int, text);
					bATInfo.NitTableId = Tool.GetInt64(sqlDataReader["nit_table_id"]);
					bATInfo.BATId = Tool.GetInt32(sqlDataReader["bat_id"]);
					bATInfo.BATName = text;
					bATInfo.BATVersionNumber = Tool.GetInt32(sqlDataReader["bat_version_number"]);
					BATInfo bATInfo2 = bATInfo;
					bATInfo2.Node = bATInfo2;
					bATInfo2.ReadNIT += bat_ReadNIT;
					list.Add(bATInfo2);
				}
			}
			return list;
		}

		private void bat_ReadNIT(object sender)
		{
			BATInfo bATInfo = sender as BATInfo;
			bATInfo.NIT = new NITInfoDAL().GetNITInfoByNetworkIdentity(bATInfo.NitTableId);
		}
	}
}
