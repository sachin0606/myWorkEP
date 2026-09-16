using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.IDAL;
using DeXin.EPG.Model;
using DeXin.EPG.Tools;

namespace DeXin.EPG.DAL
{
	public class DescriptorService : IDescriptor
	{
		private SendNoticesDAL sendNoticeService = new SendNoticesDAL();

		public IList<DescriptorSigleBase> GetDescriptors(EnumPossibleLoaction location, long tableIdentity)
		{
			IList<DescriptorSigleBase> list = new List<DescriptorSigleBase>();
			string cmdText = QueryString(location, DescriptorCondtitionEnum.ALL);
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@tableId", tableIdentity, SqlDbType.BigInt) };
			using (SqlDataReader sqlDataReader = SqlHelper.ExecuteReader(CommandType.Text, cmdText, commandParameters))
			{
				while (sqlDataReader.Read())
				{
					list.Add(ReaderToEnitty(sqlDataReader, location));
				}
			}
			return list;
		}

		public IList<DescriptorSigleBase> GetDescriptors(long batId, long tsId)
		{
			IList<DescriptorSigleBase> list = new List<DescriptorSigleBase>();
			SqlDataReader sqlDataReader = null;
			SqlParameter[] array = null;
			try
			{
				string cmdText = QueryString(EnumPossibleLoaction.BAT_TS, DescriptorCondtitionEnum.ALL);
				array = new SqlParameter[2]
				{
					SqlHelper.AddParameter("@bat_table_id", batId, SqlDbType.BigInt),
					SqlHelper.AddParameter("@ts_table_id", tsId, SqlDbType.BigInt)
				};
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, array);
				while (sqlDataReader.Read())
				{
					list.Add(ReaderToEnitty(sqlDataReader, EnumPossibleLoaction.BAT_TS));
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

		public DescriptorSigleBase GetDescriptorByID(EnumPossibleLoaction location, long id)
		{
			DescriptorSigleBase descriptorSigleBase = new DescriptorSigleBase();
			SqlDataReader sqlDataReader = null;
			try
			{
				descriptorSigleBase.Location = location;
				string cmdText = QueryString(location, DescriptorCondtitionEnum.ONE);
				SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@ID", id, SqlDbType.BigInt) };
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
				if (sqlDataReader.Read())
				{
					descriptorSigleBase = ReaderToEnitty(sqlDataReader, location);
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
			return descriptorSigleBase;
		}

		public IList<DescriptorSigleBase> GetBAT_TS_DescriptorsByBATIDEntity(long batId)
		{
			IList<DescriptorSigleBase> list = new List<DescriptorSigleBase>();
			SqlDataReader sqlDataReader = null;
			SqlParameter[] array = null;
			try
			{
				string cmdText = "SELECT val.id AS ID, bat_table_id AS batTableID, ts_table_id AS tsTableID ,typ.des_location AS location,typ.id AS typeId,typ.des_tag AS tag,typ.des_name, des_length, des_value FROM bat_ts_des_values AS val INNER JOIN descriptor_type AS typ ON typ.id = val.des_type_table_id  WHERE typ.des_location = @location AND bat_table_id = @bat_table_id ORDER BY val.id ";
				array = new SqlParameter[2]
				{
					SqlHelper.AddParameter("@location", EnumPossibleLoaction.BAT_TS, SqlDbType.NVarChar),
					SqlHelper.AddParameter("@bat_table_id", batId, SqlDbType.BigInt)
				};
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, array);
				while (sqlDataReader.Read())
				{
					list.Add(ReaderToEnitty(sqlDataReader, EnumPossibleLoaction.BAT_TS));
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

		public IList<DescriptorSigleBase> GetNIT_TS_DescriptorsByNetWorkIDEntity(long netWorkIdentity)
		{
			IList<DescriptorSigleBase> list = new List<DescriptorSigleBase>();
			SqlDataReader sqlDataReader = null;
			SqlParameter[] array = null;
			try
			{
				string cmdText = "SELECT val.id AS ID, ts_table_id AS tableID,typ.des_location AS location,typ.id AS typeId,typ.des_tag AS tag,typ.des_name, des_length, des_value FROM ts_des_values AS val INNER JOIN descriptor_type AS typ ON typ.id = val.des_type_table_id  INNER JOIN ts_info AS ts ON ts.id = val.ts_table_id WHERE typ.des_location = @location AND ts.nit_table_id = @netWorkID ORDER BY val.id";
				array = new SqlParameter[2]
				{
					SqlHelper.AddParameter("@location", EnumPossibleLoaction.NIT_TS, SqlDbType.NVarChar),
					SqlHelper.AddParameter("@netWorkID", netWorkIdentity, SqlDbType.BigInt)
				};
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, array);
				while (sqlDataReader.Read())
				{
					list.Add(ReaderToEnitty(sqlDataReader, EnumPossibleLoaction.NIT_TS));
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

		public bool AddDescriptor(EnumPossibleLoaction location, DescriptorSigleBase args)
		{
			bool result = false;
			SqlConnection sqlConnection = null;
			SqlTransaction sqlTransaction = null;
			try
			{
				if (args.DescriptionData.Length > 255)
				{
					byte[] array = new byte[255];
					Array.Copy(args.DescriptionData, array, 255);
					args.DescriptionData = new byte[255];
					Array.Copy(array, args.DescriptionData, 255);
				}
				SqlParameter[] array2 = null;
				string text = QueryString(location, DescriptorCondtitionEnum.ADD);
				text += " SELECT @@IDENTITY AS IDKEY";
				array2 = ((location != EnumPossibleLoaction.BAT_TS) ? new SqlParameter[4]
				{
					SqlHelper.AddParameter("@tableId", args.TableID, SqlDbType.BigInt),
					SqlHelper.AddParameter("@desTypeId", args.DesTypeID, SqlDbType.BigInt),
					SqlHelper.AddParameter("@desLength", args.DataLength, SqlDbType.Int),
					SqlHelper.AddParameter("@desValue", args.DescriptionData, SqlDbType.Binary)
				} : new SqlParameter[5]
				{
					SqlHelper.AddParameter("@bat_table_id", args.BATTableID, SqlDbType.BigInt),
					SqlHelper.AddParameter("@ts_table_id", args.TableID, SqlDbType.BigInt),
					SqlHelper.AddParameter("@desTypeId", args.DesTypeID, SqlDbType.BigInt),
					SqlHelper.AddParameter("@desLength", args.DataLength, SqlDbType.Int),
					SqlHelper.AddParameter("@desValue", args.DescriptionData, SqlDbType.Binary)
				});
				using (sqlConnection = new SqlConnection(SqlHelper.ConnectionString))
				{
					sqlConnection.Open();
					sqlTransaction = sqlConnection.BeginTransaction();
					args.Identity = Tool.GetInt64(SqlHelper.ExecuteScalar(sqlTransaction, CommandType.Text, text, array2));
					if (args.Identity > 0)
					{
						result = true;
						ManagementSendNotices(sqlTransaction, args.SendLocation, args.Identity, 1);
						sqlTransaction.Commit();
					}
					else
					{
						sqlTransaction.Rollback();
					}
				}
			}
			catch (Exception ex)
			{
				if (sqlTransaction != null)
				{
					sqlTransaction.Rollback();
				}
				SqlHelper.CancelChange();
				throw ex;
			}
			finally
			{
				SqlHelper.CloseAll(sqlConnection, null);
			}
			return result;
		}

		internal bool AddDescriptor(EnumPossibleLoaction location, DescriptorSigleBase args, SqlTransaction tran)
		{
			bool result = false;
			try
			{
				if (args.DescriptionData.Length > 255)
				{
					byte[] array = new byte[255];
					Array.Copy(args.DescriptionData, array, 255);
					args.DescriptionData = new byte[255];
					Array.Copy(array, args.DescriptionData, 255);
				}
				SqlParameter[] array2 = null;
				string text = QueryString(location, DescriptorCondtitionEnum.ADD);
				text += " SELECT @@IDENTITY AS IDKEY";
				array2 = ((location != EnumPossibleLoaction.BAT_TS) ? new SqlParameter[4]
				{
					SqlHelper.AddParameter("@tableId", args.TableID, SqlDbType.BigInt),
					SqlHelper.AddParameter("@desTypeId", args.DesTypeID, SqlDbType.BigInt),
					SqlHelper.AddParameter("@desLength", args.DataLength, SqlDbType.Int),
					SqlHelper.AddParameter("@desValue", args.DescriptionData, SqlDbType.Binary)
				} : new SqlParameter[5]
				{
					SqlHelper.AddParameter("@bat_table_id", args.BATTableID, SqlDbType.BigInt),
					SqlHelper.AddParameter("@ts_table_id", args.TableID, SqlDbType.BigInt),
					SqlHelper.AddParameter("@desTypeId", args.DesTypeID, SqlDbType.BigInt),
					SqlHelper.AddParameter("@desLength", args.DataLength, SqlDbType.Int),
					SqlHelper.AddParameter("@desValue", args.DescriptionData, SqlDbType.Binary)
				});
				args.Identity = Tool.GetInt64(SqlHelper.ExecuteScalar(tran, CommandType.Text, text, array2));
				if (args.Identity > 0)
				{
					ManagementSendNotices(tran, args.SendLocation, args.Identity, 1);
					result = true;
				}
			}
			catch (Exception ex)
			{
				result = false;
				Tool.WriteTXTLog(ex.Message + ex.StackTrace);
			}
			return result;
		}

		public bool AddDescriptor(DescriptorSigleBase args, bool isDeleteOldDescriptor = true)
		{
			bool result = false;
			SqlParameter[] array = null;
			string text = "";
			EnumDescriptorTag enumDescriptorTag = EnumDescriptorTag.network_name_name_descriptor;
			try
			{
				enumDescriptorTag = (EnumDescriptorTag)args.DesTypeTag;
			}
			catch (Exception ex)
			{
				throw new CustomException(ex.Message, MessageType.SHOW);
			}
			DescriptorTypeService descriptorTypeService = new DescriptorTypeService();
			DescriptorType descriptorTypeByLocationAndTag = descriptorTypeService.GetDescriptorTypeByLocationAndTag(args.Location, enumDescriptorTag);
			if (descriptorTypeByLocationAndTag == null)
			{
				return result;
			}
			switch (enumDescriptorTag)
			{
			case EnumDescriptorTag.network_name_name_descriptor:
			case EnumDescriptorTag.bouquet_name_descriptor:
			case EnumDescriptorTag.mutilingual_service_name_descriptor:
			{
				StringNameDescriptor stringNameDescriptor = args as StringNameDescriptor;
				if (stringNameDescriptor == null)
				{
					return result;
				}
				args.DescriptionData = stringNameDescriptor.ToBytes();
				break;
			}
			case EnumDescriptorTag.service_descriptor:
			{
				ServiceDescriptor serviceDescriptor = args as ServiceDescriptor;
				if (serviceDescriptor == null)
				{
					return result;
				}
				args.DescriptionData = serviceDescriptor.ToBytes();
				break;
			}
			case EnumDescriptorTag.short_envent_descriptor:
			{
				ShortEventDescriptor shortEventDescriptor = args as ShortEventDescriptor;
				if (shortEventDescriptor == null)
				{
					return result;
				}
				args.DescriptionData = shortEventDescriptor.ToBytes();
				break;
			}
			case EnumDescriptorTag.extended_event_descriptor:
			{
				ExtendedEventDescriptor extendedEventDescriptor = args as ExtendedEventDescriptor;
				if (extendedEventDescriptor == null)
				{
					return result;
				}
				args.DescriptionData = extendedEventDescriptor.ToBytes();
				break;
			}
			case EnumDescriptorTag.parental_rating_descriptor:
			{
				ParentalRatingBase parentalRatingBase = args as ParentalRatingBase;
				if (parentalRatingBase == null)
				{
					return result;
				}
				args.DescriptionData = parentalRatingBase.ToBytes();
				break;
			}
			default:
				return result;
			case EnumDescriptorTag.service_list_descriptor:
				break;
			}
			if (isDeleteOldDescriptor)
			{
				DeleteDescriptor(args.Location, enumDescriptorTag, args, descriptorTypeByLocationAndTag.Id);
			}
			text = QueryString(args.Location, DescriptorCondtitionEnum.ADD);
			text += " SELECT @@IDENTITY AS IDKEY";
			if (args.DescriptionData.Length > 255)
			{
				byte[] array2 = new byte[255];
				Array.Copy(args.DescriptionData, array2, 255);
				args.DescriptionData = new byte[255];
				Array.Copy(array2, args.DescriptionData, 255);
			}
			EnumPossibleLoaction location = args.Location;
			array = ((location != EnumPossibleLoaction.BAT_TS) ? new SqlParameter[4]
			{
				SqlHelper.AddParameter("@tableId", args.TableID, SqlDbType.BigInt),
				SqlHelper.AddParameter("@desTypeId", descriptorTypeByLocationAndTag.Id, SqlDbType.BigInt),
				SqlHelper.AddParameter("@desLength", args.DataLength, SqlDbType.Int),
				SqlHelper.AddParameter("@desValue", args.DescriptionData, SqlDbType.Binary)
			} : new SqlParameter[5]
			{
				SqlHelper.AddParameter("@bat_table_id", args.BATTableID, SqlDbType.BigInt),
				SqlHelper.AddParameter("@ts_table_id", args.TableID, SqlDbType.BigInt),
				SqlHelper.AddParameter("@desTypeId", descriptorTypeByLocationAndTag.Id, SqlDbType.BigInt),
				SqlHelper.AddParameter("@desLength", args.DataLength, SqlDbType.Int),
				SqlHelper.AddParameter("@desValue", args.DescriptionData, SqlDbType.Binary)
			});
			args.Identity = Tool.GetInt64(SqlHelper.ExecuteScalar(CommandType.Text, text, array));
			if (args.Identity > 0)
			{
				ManagementSendNotices(args.SendLocation, args.Identity, 1);
				result = true;
			}
			return result;
		}

		public bool ModifyDescriptor(EnumPossibleLoaction location, DescriptorSigleBase args)
		{
			bool result = false;
			SqlConnection sqlConnection = null;
			SqlTransaction sqlTransaction = null;
			try
			{
				if (args.DescriptionData.Length > 255)
				{
					byte[] array = new byte[255];
					Array.Copy(args.DescriptionData, array, 255);
					args.DescriptionData = new byte[255];
					Array.Copy(array, args.DescriptionData, 255);
				}
				string cmdText = QueryString(location, DescriptorCondtitionEnum.EDIT);
				SqlParameter[] commandParameters = new SqlParameter[3]
				{
					SqlHelper.AddParameter("@id", args.Identity, SqlDbType.BigInt),
					SqlHelper.AddParameter("@desLength", args.DataLength, SqlDbType.Int),
					SqlHelper.AddParameter("@desValue", args.DescriptionData, SqlDbType.Binary)
				};
				using (sqlConnection = new SqlConnection(SqlHelper.ConnectionString))
				{
					sqlConnection.Open();
					sqlTransaction = sqlConnection.BeginTransaction();
					result = SqlHelper.ExecuteNonQuery(sqlTransaction, CommandType.Text, cmdText, commandParameters) > 0;
					ManagementSendNotices(sqlTransaction, args.SendLocation, args.Identity, 2);
					sqlTransaction.Commit();
				}
			}
			catch (Exception ex)
			{
				if (sqlTransaction != null)
				{
					sqlTransaction.Rollback();
				}
				throw ex;
			}
			finally
			{
				SqlHelper.CloseAll(sqlConnection, null);
			}
			return result;
		}

		public bool DeleteDescriptorByIDEntity(EnumPossibleLoaction location, params long[] args)
		{
			bool result = true;
			SqlParameter[] array = null;
			SqlTransaction sqlTransaction = null;
			SqlConnection sqlConnection = null;
			try
			{
				using (sqlConnection = new SqlConnection(SqlHelper.ConnectionString))
				{
					sqlConnection.Open();
					sqlTransaction = sqlConnection.BeginTransaction();
					string cmdText = QueryString(location, DescriptorCondtitionEnum.DELETEBYID);
					foreach (long num in args)
					{
						ManagementSendNotices(sqlTransaction, TypeManagent<EnumSendNoticesCodes>.GetEnum("DES_" + location), num, 3);
						array = new SqlParameter[1] { SqlHelper.AddParameter("@id", num) };
						SqlHelper.ExecuteNonQuery(sqlTransaction, CommandType.Text, cmdText, array);
					}
					sqlTransaction.Commit();
					result = true;
				}
			}
			catch (Exception ex)
			{
				if (sqlTransaction != null)
				{
					sqlTransaction.Rollback();
				}
				throw ex;
			}
			finally
			{
				if (sqlConnection != null)
				{
					sqlConnection.Close();
				}
			}
			return result;
		}

		private void DeleteDescriptor(EnumPossibleLoaction location, EnumDescriptorTag tag, DescriptorSigleBase args, long typeId)
		{
			SqlParameter[] array = null;
			string text = QueryString(location, DescriptorCondtitionEnum.DELETEBYTABLEIDANDTYPEID);
			if (location == EnumPossibleLoaction.BAT_TS)
			{
				text += "  AND bat_table_id = @bat_table_id";
				array = new SqlParameter[3]
				{
					SqlHelper.AddParameter("@bat_table_id ", args.BATTableID),
					SqlHelper.AddParameter("@tableID ", args.TableID),
					SqlHelper.AddParameter("@desTypeId", typeId)
				};
			}
			else
			{
				array = new SqlParameter[2]
				{
					SqlHelper.AddParameter("@tableID ", args.TableID),
					SqlHelper.AddParameter("@desTypeId", typeId)
				};
			}
			SqlHelper.ExecuteNonQuery(CommandType.Text, text, array);
		}

		internal void DeleteDescriptorByNetWorkID(long netWorkID)
		{
			StringBuilder stringBuilder = new StringBuilder();
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@nitid", netWorkID) };
			stringBuilder.Append(" DELETE FROM eit_des_values \r\n                                    FROM eit_des_values AS des,eit_info AS eit,program_info AS pro,ts_info AS ts\r\n                                    WHERE des.eit_table_id = eit.id AND eit.program_table_id = pro.id AND pro.ts_table_id = ts.id\r\n                                    AND ts.nit_table_id = @nitid");
			stringBuilder.Append(" DELETE FROM bat_ts_des_values \r\n                                    FROM bat_ts_des_values AS des,bat_info AS bat\r\n                                    WHERE des.bat_table_id = bat.id\r\n                                    AND bat.nit_table_id = @nitid");
			stringBuilder.Append(" DELETE FROM bat_des_values \r\n                                    FROM bat_des_values AS des,bat_info AS bat\r\n                                    WHERE des.bat_table_id = bat.id\r\n                                    AND bat.nit_table_id = @nitid");
			stringBuilder.Append(" DELETE FROM cat_des_values\r\n                                    FROM cat_des_values AS des, ts_info AS ts\r\n                                    WHERE des.ts_table_id = ts.id\r\n                                    AND ts.nit_table_id = @nitid");
			stringBuilder.Append(" DELETE FROM sdt_des_value\r\n                                    FROM sdt_des_value AS des, program_info AS pro,ts_info AS ts\r\n                                    WHERE des.program_table_id = pro.id AND pro.ts_table_id = ts.id\r\n                                    AND ts.nit_table_id = @nitid");
			stringBuilder.Append(" DELETE FROM multimedia_des_values \r\n                                    FROM multimedia_des_values AS des,multimedia AS media,program_info AS pro,ts_info AS ts\r\n                                    WHERE des.multimedia_table_id = media.id AND media.program_table_id = pro.id AND pro.ts_table_id = ts.id\r\n                                    AND ts.nit_table_id = @nitid");
			stringBuilder.Append(" DELETE FROM pmt_des_values\r\n                                    FROM pmt_des_values AS des, program_info AS pro,ts_info AS ts\r\n                                    WHERE des.program_table_id = pro.id AND pro.ts_table_id = ts.id\r\n                                    AND ts.nit_table_id = @nitid");
			stringBuilder.Append(" DELETE FROM ts_des_values\r\n                                    FROM ts_des_values AS des,ts_info AS ts \r\n                                    WHERE  des.ts_table_id = ts.id\r\n                                    AND ts.nit_table_id = @nitid");
			stringBuilder.Append(" DELETE FROM nit_des_values WHERE nit_table_id = @nitid");
			SqlHelper.ExecuteNonQuery(CommandType.Text, stringBuilder.ToString(), commandParameters);
		}

		internal void DeleteDescriptorByTSID(long tsID)
		{
			StringBuilder stringBuilder = new StringBuilder();
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@tsid", tsID) };
			stringBuilder.Append(" DELETE FROM eit_des_values \r\n                                    FROM eit_des_values AS des,eit_info AS eit,program_info AS pro \r\n                                    WHERE des.eit_table_id = eit.id AND eit.program_table_id = pro.id \r\n                                    AND pro.ts_table_id = @tsid");
			stringBuilder.Append(" DELETE FROM bat_ts_des_values \r\n                                    WHERE ts_table_id = @tsid");
			stringBuilder.Append(" DELETE FROM cat_des_values\r\n                                    FROM cat_des_values AS des\r\n                                    WHERE des.ts_table_id = @tsid");
			stringBuilder.Append(" DELETE FROM sdt_des_value\r\n                                    FROM sdt_des_value AS des, program_info AS pro\r\n                                    WHERE des.program_table_id = pro.id \r\n                                    AND pro.ts_table_id = @tsid");
			stringBuilder.Append(" DELETE FROM multimedia_des_values \r\n                                    FROM multimedia_des_values AS des,multimedia AS media,program_info AS pro\r\n                                    WHERE des.multimedia_table_id = media.id \r\n                                    AND media.program_table_id = pro.id \r\n                                    AND pro.ts_table_id = @tsid");
			stringBuilder.Append(" DELETE FROM pmt_des_values\r\n                                    FROM pmt_des_values AS des, program_info AS pro\r\n                                    WHERE des.program_table_id = pro.id \r\n                                    AND pro.ts_table_id = @tsid");
			stringBuilder.Append(" DELETE FROM ts_des_values\r\n                                    WHERE  ts_table_id = @tsid");
			SqlHelper.ExecuteNonQuery(CommandType.Text, stringBuilder.ToString(), commandParameters);
		}

		internal void DeleteDescriptorByProgramID(long programID)
		{
			StringBuilder stringBuilder = new StringBuilder();
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@programid", programID) };
			stringBuilder.Append(" DELETE FROM eit_des_values \r\n                                    FROM eit_des_values AS des,eit_info AS eit\r\n                                    WHERE des.eit_table_id = eit.id \r\n                                    AND eit.program_table_id = @programid");
			stringBuilder.Append(" DELETE FROM sdt_des_value\r\n                                    FROM sdt_des_value AS des, program_info AS pro,ts_info AS ts\r\n                                    WHERE des.program_table_id = @programid");
			stringBuilder.Append(" DELETE FROM multimedia_des_values \r\n                                    FROM multimedia_des_values AS des,multimedia AS media,program_info AS pro,ts_info AS ts\r\n                                    WHERE des.multimedia_table_id = media.id \r\n                                    AND media.program_table_id =  @programid");
			stringBuilder.Append(" DELETE FROM pmt_des_values\r\n                                    FROM pmt_des_values AS des\r\n                                    WHERE des.program_table_id = @programid");
			SqlHelper.ExecuteNonQuery(CommandType.Text, stringBuilder.ToString(), commandParameters);
		}

		internal void DeleteBATDescriptorByBATID(long batID)
		{
			StringBuilder stringBuilder = new StringBuilder();
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@batid", batID) };
			DeleteBAT_TSDescriptorWithTranscation(batID);
			string cmdText = "  SELECT TOP 1 des.id FROM bat_des_values AS des\r\n                                                WHERE des.bat_table_id = @batid";
			object obj = SqlHelper.ExecuteScalar(CommandType.Text, cmdText, SqlHelper.AddParameter("@batid", batID));
			ManagementSendNotices(EnumSendNoticesCodes.DES_BAT, Tool.GetInt64(obj), 3);
			stringBuilder.Append(" DELETE FROM bat_des_values \r\n                                    FROM bat_des_values AS des\r\n                                    WHERE des.bat_table_id = @batid");
			SqlHelper.ExecuteNonQuery(CommandType.Text, stringBuilder.ToString(), commandParameters);
		}

		private void DeleteBAT_TSDescriptorWithTranscation(long batID)
		{
			StringBuilder stringBuilder = new StringBuilder();
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@bat_id", batID) };
			string cmdText = "  SELECT TOP 1 dv.id  FROM bat_ts_des_values dv , descriptor_type dt\r\n                                    WHERE   dv.bat_table_id = @bat_id\r\n                                    AND dt.des_location = 'BAT_TS'\r\n                                    AND dv.des_type_table_id = dt.id";
			object obj = SqlHelper.ExecuteScalar(CommandType.Text, cmdText, SqlHelper.AddParameter("@bat_id", batID));
			ManagementSendNotices(EnumSendNoticesCodes.DES_BAT_TS, Tool.GetInt64(obj), 3);
			stringBuilder.Append("DELETE  FROM bat_ts_des_values\r\n                                    FROM    bat_ts_des_values dv ,\r\n                                    descriptor_type dt\r\n                                    WHERE   dv.bat_table_id = @bat_id\r\n                                    AND dt.des_location = 'BAT_TS'\r\n                                    AND dv.des_type_table_id = dt.id");
			SqlHelper.ExecuteNonQuery(CommandType.Text, stringBuilder.ToString(), commandParameters);
		}

		internal void DeleteBAT_TSDescriptorWithTranscation(long batID, EnumDescriptorTag desTag)
		{
			StringBuilder stringBuilder = new StringBuilder();
			SqlParameter[] commandParameters = new SqlParameter[2]
			{
				SqlHelper.AddParameter("@tag", (int)desTag),
				SqlHelper.AddParameter("@bat_id", batID)
			};
			stringBuilder.Append("DELETE  FROM bat_ts_des_values\r\n                            FROM    bat_ts_des_values dv ,\r\n                                    descriptor_type dt\r\n                            WHERE   dt.des_tag = @tag\r\n                                    AND dv.bat_table_id = @bat_id\r\n                                    AND dt.des_location = 'BAT_TS'\r\n                                    AND dv.des_type_table_id = dt.id");
			string cmdText = "SELECT TOP 1 dv.id  FROM bat_ts_des_values dv , descriptor_type dt\r\n                                            WHERE   dt.des_tag = @tag\r\n                                            AND dv.bat_table_id = @bat_id\r\n                                            AND dt.des_location = 'BAT_TS'\r\n                                            AND dv.des_type_table_id = dt.id";
			object obj = SqlHelper.ExecuteScalar(CommandType.Text, cmdText, SqlHelper.AddParameter("@tag", (int)desTag), SqlHelper.AddParameter("@bat_id", batID));
			ManagementSendNotices(EnumSendNoticesCodes.DES_BAT_TS, Tool.GetInt64(obj), 3);
			SqlHelper.ExecuteNonQuery(CommandType.Text, stringBuilder.ToString(), commandParameters);
		}

		internal void DeleteEITDescriptorByEventID(long eventID)
		{
			StringBuilder stringBuilder = new StringBuilder();
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@eventid", eventID) };
			string cmdText = " SELECT TOP 1 des.id FROM eit_des_values AS des\r\n                                    WHERE des.eit_table_id = @eventid";
			object obj = SqlHelper.ExecuteScalar(CommandType.Text, cmdText, SqlHelper.AddParameter("@eventid", eventID));
			ManagementSendNotices(EnumSendNoticesCodes.DES_EIT, Tool.GetInt64(obj), 3);
			stringBuilder.Append(" DELETE FROM eit_des_values \r\n                                    FROM eit_des_values AS des\r\n                                    WHERE des.eit_table_id = @eventid");
			SqlHelper.ExecuteNonQuery(CommandType.Text, stringBuilder.ToString(), commandParameters);
		}

		internal void DeleteDescriptorByESID(long itemKey)
		{
			StringBuilder stringBuilder = new StringBuilder();
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@tableid", itemKey) };
			string cmdText = " SELECT TOP 1 des.id FROM multimedia_des_values AS des\r\n                                    WHERE des.multimedia_table_id = @tableid";
			object obj = SqlHelper.ExecuteScalar(CommandType.Text, cmdText, SqlHelper.AddParameter("@tableid", itemKey));
			ManagementSendNotices(EnumSendNoticesCodes.DES_ES, Tool.GetInt64(obj), 3);
			stringBuilder.Append(" DELETE FROM multimedia_des_values \r\n                                    FROM multimedia_des_values AS des\r\n                                    WHERE des.multimedia_table_id = @tableid");
			SqlHelper.ExecuteNonQuery(CommandType.Text, stringBuilder.ToString(), commandParameters);
		}

		public IList<BaseKey> GetCountryCode()
		{
			IList<BaseKey> list = new List<BaseKey>();
			BaseKey baseKey = null;
			SqlDataReader sqlDataReader = null;
			try
			{
				string cmdText = "SELECT [CountryCode]\r\n                                      ,[CountryName]\r\n                                  FROM CountryTable";
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, (SqlParameter[])null);
				while (sqlDataReader.Read())
				{
					baseKey = new BaseKey();
					baseKey.BKKey = Tool.GetString(sqlDataReader["CountryCode"]);
					baseKey.BKValue = Tool.GetString(sqlDataReader["CountryName"]);
					baseKey.BKStatus = true;
					list.Add(baseKey);
				}
			}
			catch (Exception ex)
			{
				SqlHelper.CloseAll(null, sqlDataReader);
				throw ex;
			}
			return list;
		}

		public IList<BaseKey> GetLanguageCode()
		{
			IList<BaseKey> list = new List<BaseKey>();
			BaseKey baseKey = null;
			SqlDataReader sqlDataReader = null;
			try
			{
				string cmdText = "SELECT [LanguageCode]\r\n                                      ,[LanguageName]\r\n                                  FROM LanguageTable";
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, (SqlParameter[])null);
				while (sqlDataReader.Read())
				{
					baseKey = new BaseKey();
					baseKey.BKKey = Tool.GetString(sqlDataReader["LanguageCode"]);
					baseKey.BKValue = Tool.GetString(sqlDataReader["LanguageName"]);
					baseKey.BKStatus = true;
					list.Add(baseKey);
				}
			}
			catch (Exception ex)
			{
				SqlHelper.CloseAll(null, sqlDataReader);
				throw ex;
			}
			return list;
		}

		public IList<DescriptorSigleBase> GetEITDescriptorsForSend()
		{
			IList<DescriptorSigleBase> list = new List<DescriptorSigleBase>();
			SqlDataReader sqlDataReader = null;
			StringBuilder stringBuilder = new StringBuilder();
			EnumPossibleLoaction enumPossibleLoaction = EnumPossibleLoaction.EIT;
			stringBuilder.Append(" SELECT val.id AS ID, eit_table_id AS tableID,typ.des_location AS location,typ.id AS typeId,typ.des_tag AS tag,typ.des_name, des_length, des_value ");
			stringBuilder.Append(" FROM eit_des_values AS val ");
			stringBuilder.Append(" INNER JOIN descriptor_type AS typ ON typ.id = val.des_type_table_id AND typ.des_location = @location ");
			stringBuilder.Append(" INNER JOIN eit_info AS eit ON eit.id = val.eit_table_id ");
			stringBuilder.Append(" INNER JOIN PAT ON PAT.program_table_id = eit.program_table_id ");
			stringBuilder.Append(" ORDER BY val.id");
			try
			{
				SqlParameter sqlParameter = new SqlParameter("@location", enumPossibleLoaction);
				sqlParameter.SqlDbType = SqlDbType.NVarChar;
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, stringBuilder.ToString(), sqlParameter);
				while (sqlDataReader.Read())
				{
					list.Add(ReaderToEnitty(sqlDataReader, enumPossibleLoaction));
				}
			}
			catch (Exception)
			{
			}
			return list;
		}

		public IList<BaseKey> GetContentNibbleLevelByParentID(int pid)
		{
			IList<BaseKey> list = new List<BaseKey>();
			SqlDataReader sqlDataReader = null;
			BaseKey baseKey = null;
			try
			{
				string cmdText = "SELECT * FROM content_nibble_level WHERE parent_id = @pid";
				SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@pid", pid) };
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
				while (sqlDataReader.Read())
				{
					baseKey = new BaseKey();
					baseKey.BKKey = Convert.ToByte(sqlDataReader["level_id"]).ToString(FormatString.Ins.X2);
					baseKey.BKValue = Tool.GetString(sqlDataReader["level_name"]);
					baseKey.BKStatus = Tool.GetBoolean(sqlDataReader["visible"]);
					baseKey.PID = Convert.ToInt32(sqlDataReader["parent_id"]).ToString();
					baseKey.Node = baseKey;
					list.Add(baseKey);
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

		private string QueryString(EnumPossibleLoaction location, DescriptorCondtitionEnum condition)
		{
			StringBuilder stringBuilder = new StringBuilder();
			switch (location)
			{
			case EnumPossibleLoaction.NIT:
				stringBuilder.Append("SELECT val.id AS ID, nit_table_id AS tableID,typ.des_location AS location,typ.id AS typeId,typ.des_tag AS tag,typ.des_name, des_length, des_value FROM nit_des_values AS val INNER JOIN descriptor_type AS typ ON typ.id = val.des_type_table_id WHERE 1 = 1");
				stringBuilder.Append(" AND typ.des_location = '" + location.ToString() + "'");
				switch (condition)
				{
				case DescriptorCondtitionEnum.ALL:
					stringBuilder.Append("  AND nit_table_id = @tableId ORDER BY val.id");
					break;
				case DescriptorCondtitionEnum.ONE:
					stringBuilder.Append("  AND val.id = @ID ORDER BY val.id");
					break;
				case DescriptorCondtitionEnum.ADD:
					stringBuilder = new StringBuilder("INSERT INTO nit_des_values( nit_table_id ,des_type_table_id ,des_length ,des_value)VALUES( @tableId,@desTypeId,@desLength,@desValue)");
					break;
				case DescriptorCondtitionEnum.EDIT:
					stringBuilder = new StringBuilder("UPDATE nit_des_values SET des_length =  @desLength ,des_value = @desValue WHERE id = @id");
					break;
				case DescriptorCondtitionEnum.DELETEBYID:
					stringBuilder = new StringBuilder("DELETE FROM nit_des_values WHERE  id = @id");
					break;
				case DescriptorCondtitionEnum.DELETEBYTABLEIDANDTYPEID:
					stringBuilder = new StringBuilder("DELETE FROM nit_des_values WHERE  nit_table_id = @tableId AND des_type_table_id = @desTypeId");
					break;
				case DescriptorCondtitionEnum.DELETEBYLOCATIONANDTABLEID:
					stringBuilder = new StringBuilder("DELETE FROM nit_des_values WHERE  nit_table_id = @tableId");
					break;
				}
				break;
			case EnumPossibleLoaction.NIT_TS:
				stringBuilder.Append("SELECT val.id AS ID, ts_table_id AS tableID,typ.des_location AS location,typ.id AS typeId,typ.des_tag AS tag,typ.des_name, des_length, des_value FROM ts_des_values AS val INNER JOIN descriptor_type AS typ ON typ.id = val.des_type_table_id  WHERE 1 = 1");
				stringBuilder.Append(" AND typ.des_location = '" + location.ToString() + "'");
				switch (condition)
				{
				case DescriptorCondtitionEnum.ALL:
					stringBuilder.Append("  AND ts_table_id = @tableId ORDER BY val.id");
					break;
				case DescriptorCondtitionEnum.ONE:
					stringBuilder.Append("  AND val.id = @ID");
					break;
				case DescriptorCondtitionEnum.ADD:
					stringBuilder = new StringBuilder("INSERT INTO ts_des_values( ts_table_id ,des_type_table_id ,des_length ,des_value)VALUES( @tableId,@desTypeId,@desLength,@desValue)");
					break;
				case DescriptorCondtitionEnum.EDIT:
					stringBuilder = new StringBuilder("UPDATE ts_des_values SET des_length =  @desLength ,des_value = @desValue WHERE id = @id");
					break;
				case DescriptorCondtitionEnum.DELETEBYID:
					stringBuilder = new StringBuilder("DELETE FROM ts_des_values WHERE  id = @id");
					break;
				case DescriptorCondtitionEnum.DELETEBYTABLEIDANDTYPEID:
					stringBuilder = new StringBuilder("DELETE FROM ts_des_values WHERE  ts_table_id = @tableId AND des_type_table_id = @desTypeId");
					break;
				case DescriptorCondtitionEnum.DELETEBYLOCATIONANDTABLEID:
					stringBuilder = new StringBuilder("DELETE FROM ts_des_values WHERE  ts_table_id = @tableId");
					break;
				}
				break;
			case EnumPossibleLoaction.PMT:
				stringBuilder.Append("SELECT val.id AS ID, program_table_id AS tableID,typ.des_location AS location,typ.id AS typeId,typ.des_tag AS tag,typ.des_name, des_length, des_value FROM pmt_des_values AS val INNER JOIN descriptor_type AS typ ON typ.id = val.des_type_table_id  WHERE 1 = 1");
				stringBuilder.Append(" AND typ.des_location = '" + location.ToString() + "'");
				switch (condition)
				{
				case DescriptorCondtitionEnum.ALL:
					stringBuilder.Append("  AND program_table_id = @tableId ORDER BY val.id");
					break;
				case DescriptorCondtitionEnum.ONE:
					stringBuilder.Append("  AND val.id = @ID ORDER BY val.id");
					break;
				case DescriptorCondtitionEnum.ADD:
					stringBuilder = new StringBuilder("INSERT INTO pmt_des_values( program_table_id ,des_type_table_id ,des_length ,des_value)VALUES( @tableId,@desTypeId,@desLength,@desValue)");
					break;
				case DescriptorCondtitionEnum.EDIT:
					stringBuilder = new StringBuilder("UPDATE pmt_des_values SET des_length =  @desLength ,des_value = @desValue WHERE id = @id");
					break;
				case DescriptorCondtitionEnum.DELETEBYID:
					stringBuilder = new StringBuilder("DELETE FROM pmt_des_values WHERE  id = @id");
					break;
				case DescriptorCondtitionEnum.DELETEBYTABLEIDANDTYPEID:
					stringBuilder = new StringBuilder("DELETE FROM pmt_des_values WHERE  program_table_id = @tableId AND des_type_table_id = @desTypeId");
					break;
				case DescriptorCondtitionEnum.DELETEBYLOCATIONANDTABLEID:
					stringBuilder = new StringBuilder("DELETE FROM pmt_des_values WHERE  program_table_id = @tableId");
					break;
				}
				break;
			case EnumPossibleLoaction.SDT:
				stringBuilder.Append("SELECT val.id AS ID, program_table_id AS tableID,typ.des_location AS location,typ.id AS typeId,typ.des_tag AS tag,typ.des_name, des_length, des_value FROM sdt_des_value AS val INNER JOIN descriptor_type AS typ ON typ.id = val.des_type_table_id  WHERE 1 = 1");
				stringBuilder.Append(" AND typ.des_location = '" + location.ToString() + "'");
				switch (condition)
				{
				case DescriptorCondtitionEnum.ALL:
					stringBuilder.Append("  AND program_table_id = @tableId ORDER BY val.id");
					break;
				case DescriptorCondtitionEnum.ONE:
					stringBuilder.Append("  AND val.id = @ID ORDER BY val.id");
					break;
				case DescriptorCondtitionEnum.ADD:
					stringBuilder = new StringBuilder("INSERT INTO sdt_des_value( program_table_id ,des_type_table_id ,des_length ,des_value)VALUES( @tableId,@desTypeId,@desLength,@desValue)");
					break;
				case DescriptorCondtitionEnum.EDIT:
					stringBuilder = new StringBuilder("UPDATE sdt_des_value SET des_length =  @desLength ,des_value = @desValue WHERE id = @id");
					break;
				case DescriptorCondtitionEnum.DELETEBYID:
					stringBuilder = new StringBuilder("DELETE FROM sdt_des_value WHERE  id = @id");
					break;
				case DescriptorCondtitionEnum.DELETEBYTABLEIDANDTYPEID:
					stringBuilder = new StringBuilder("DELETE FROM sdt_des_value WHERE  program_table_id = @tableId AND des_type_table_id = @desTypeId");
					break;
				case DescriptorCondtitionEnum.DELETEBYLOCATIONANDTABLEID:
					stringBuilder = new StringBuilder("DELETE FROM sdt_des_value WHERE  program_table_id = @tableId");
					break;
				}
				break;
			case EnumPossibleLoaction.BAT:
				stringBuilder.Append("SELECT val.id AS ID, bat_table_id AS tableID,typ.des_location AS location,typ.id AS typeId,typ.des_tag AS tag,typ.des_name, des_length, des_value FROM bat_des_values AS val INNER JOIN descriptor_type AS typ ON typ.id = val.des_type_table_id  WHERE 1 = 1");
				stringBuilder.Append(" AND typ.des_location = '" + location.ToString() + "'");
				switch (condition)
				{
				case DescriptorCondtitionEnum.ALL:
					stringBuilder.Append("  AND bat_table_id = @tableId ORDER BY val.id");
					break;
				case DescriptorCondtitionEnum.ONE:
					stringBuilder.Append("  AND val.id = @ID ORDER BY val.id");
					break;
				case DescriptorCondtitionEnum.ADD:
					stringBuilder = new StringBuilder("INSERT INTO bat_des_values( bat_table_id ,des_type_table_id ,des_length ,des_value)VALUES( @tableId,@desTypeId,@desLength,@desValue)");
					break;
				case DescriptorCondtitionEnum.EDIT:
					stringBuilder = new StringBuilder("UPDATE bat_des_values SET des_length =  @desLength ,des_value = @desValue WHERE id = @id");
					break;
				case DescriptorCondtitionEnum.DELETEBYID:
					stringBuilder = new StringBuilder("DELETE FROM bat_des_values WHERE  id = @id");
					break;
				case DescriptorCondtitionEnum.DELETEBYTABLEIDANDTYPEID:
					stringBuilder = new StringBuilder("DELETE FROM bat_des_values WHERE  bat_table_id = @tableId AND des_type_table_id = @desTypeId");
					break;
				case DescriptorCondtitionEnum.DELETEBYLOCATIONANDTABLEID:
					stringBuilder = new StringBuilder("DELETE FROM bat_des_values WHERE  bat_table_id = @tableId ");
					break;
				}
				break;
			case EnumPossibleLoaction.BAT_TS:
				stringBuilder.Append("SELECT val.id AS ID, bat_table_id AS batTableID, ts_table_id AS tsTableID ,typ.des_location AS location,typ.id AS typeId,typ.des_tag AS tag,typ.des_name, des_length, des_value FROM bat_ts_des_values AS val INNER JOIN descriptor_type AS typ ON typ.id = val.des_type_table_id  WHERE 1 = 1");
				stringBuilder.Append(" AND typ.des_location = '" + location.ToString() + "'");
				switch (condition)
				{
				case DescriptorCondtitionEnum.ALL:
					stringBuilder.Append("  AND bat_table_id = @bat_table_id AND ts_table_id = @ts_table_id ORDER BY val.id");
					break;
				case DescriptorCondtitionEnum.ONE:
					stringBuilder.Append("  AND val.id = @ID ORDER BY val.id");
					break;
				case DescriptorCondtitionEnum.ADD:
					stringBuilder = new StringBuilder("INSERT INTO bat_ts_des_values( bat_table_id , ts_table_id ,des_type_table_id ,des_length ,des_value)VALUES( @bat_table_id , @ts_table_id ,@desTypeId,@desLength,@desValue)");
					break;
				case DescriptorCondtitionEnum.EDIT:
					stringBuilder = new StringBuilder("UPDATE bat_ts_des_values SET des_length =  @desLength ,des_value = @desValue WHERE id = @id");
					break;
				case DescriptorCondtitionEnum.DELETEBYID:
					stringBuilder = new StringBuilder("DELETE FROM bat_ts_des_values WHERE  id = @id");
					break;
				case DescriptorCondtitionEnum.DELETEBYTABLEIDANDTYPEID:
					stringBuilder = new StringBuilder("DELETE FROM bat_ts_des_values WHERE  bat_table_id = @bat_table_id AND ts_table_id = @tableId AND des_type_table_id = @desTypeId");
					break;
				case DescriptorCondtitionEnum.DELETEBYLOCATIONANDTABLEID:
					stringBuilder = new StringBuilder("DELETE FROM bat_ts_des_values WHERE  ts_table_id = @tableId");
					break;
				}
				break;
			case EnumPossibleLoaction.ES:
				stringBuilder.Append("SELECT val.id AS ID, multimedia_table_id AS tableID,typ.des_location AS location,typ.id AS typeId,typ.des_tag AS tag,typ.des_name, des_length, des_value FROM multimedia_des_values AS val INNER JOIN descriptor_type AS typ ON typ.id = val.des_type_table_id  WHERE 1 = 1");
				stringBuilder.Append(" AND typ.des_location = '" + location.ToString() + "'");
				switch (condition)
				{
				case DescriptorCondtitionEnum.ALL:
					stringBuilder.Append("  AND multimedia_table_id = @tableId ORDER BY val.id");
					break;
				case DescriptorCondtitionEnum.ONE:
					stringBuilder.Append("  AND val.id = @ID ORDER BY val.id");
					break;
				case DescriptorCondtitionEnum.ADD:
					stringBuilder = new StringBuilder("INSERT INTO multimedia_des_values( multimedia_table_id ,des_type_table_id ,des_length ,des_value)VALUES( @tableId,@desTypeId,@desLength,@desValue)");
					break;
				case DescriptorCondtitionEnum.EDIT:
					stringBuilder = new StringBuilder("UPDATE multimedia_des_values SET des_length =  @desLength ,des_value = @desValue WHERE id = @id");
					break;
				case DescriptorCondtitionEnum.DELETEBYID:
					stringBuilder = new StringBuilder("DELETE FROM multimedia_des_values WHERE  id = @id");
					break;
				case DescriptorCondtitionEnum.DELETEBYTABLEIDANDTYPEID:
					stringBuilder = new StringBuilder("DELETE FROM multimedia_des_values WHERE  multimedia_table_id = @tableId AND des_type_table_id = @desTypeId");
					break;
				case DescriptorCondtitionEnum.DELETEBYLOCATIONANDTABLEID:
					stringBuilder = new StringBuilder("DELETE FROM multimedia_des_values WHERE  multimedia_table_id = @tableId");
					break;
				}
				break;
			case EnumPossibleLoaction.CAT:
				stringBuilder.Append("SELECT val.id AS ID, ts_table_id AS tableID,typ.des_location AS location,typ.id AS typeId,typ.des_tag AS tag,typ.des_name, des_length, des_value FROM cat_des_values AS val INNER JOIN descriptor_type AS typ ON typ.id = val.des_type_table_id WHERE 1 = 1");
				stringBuilder.Append(" AND typ.des_location = '" + location.ToString() + "'");
				switch (condition)
				{
				case DescriptorCondtitionEnum.ALL:
					stringBuilder.Append("  AND ts_table_id = @tableId ORDER BY val.id");
					break;
				case DescriptorCondtitionEnum.ONE:
					stringBuilder.Append("  AND val.id = @ID ORDER BY val.id");
					break;
				case DescriptorCondtitionEnum.ADD:
					stringBuilder = new StringBuilder("INSERT INTO cat_des_values( ts_table_id ,des_type_table_id ,des_length ,des_value)VALUES( @tableId,@desTypeId,@desLength,@desValue)");
					break;
				case DescriptorCondtitionEnum.EDIT:
					stringBuilder = new StringBuilder("UPDATE cat_des_values SET des_length =  @desLength ,des_value = @desValue WHERE id = @id");
					break;
				case DescriptorCondtitionEnum.DELETEBYID:
					stringBuilder = new StringBuilder("DELETE FROM cat_des_values WHERE  id = @id");
					break;
				case DescriptorCondtitionEnum.DELETEBYTABLEIDANDTYPEID:
					stringBuilder = new StringBuilder("DELETE FROM cat_des_values WHERE  ts_table_id = @tableId AND des_type_table_id = @desTypeId");
					break;
				case DescriptorCondtitionEnum.DELETEBYLOCATIONANDTABLEID:
					stringBuilder = new StringBuilder("DELETE FROM cat_des_values WHERE  ts_table_id = @tableId");
					break;
				}
				break;
			case EnumPossibleLoaction.EIT:
				stringBuilder.Append("SELECT val.id AS ID, eit_table_id AS tableID,typ.des_location AS location,typ.id AS typeId,typ.des_tag AS tag,typ.des_name, des_length, des_value FROM eit_des_values AS val INNER JOIN descriptor_type AS typ ON typ.id = val.des_type_table_id WHERE 1 = 1");
				stringBuilder.Append(" AND typ.des_location = '" + location.ToString() + "'");
				switch (condition)
				{
				case DescriptorCondtitionEnum.ALL:
					stringBuilder.Append("  AND eit_table_id = @tableId ORDER BY val.id");
					break;
				case DescriptorCondtitionEnum.ONE:
					stringBuilder.Append("  AND val.id = @ID ORDER BY val.id");
					break;
				case DescriptorCondtitionEnum.ADD:
					stringBuilder = new StringBuilder("INSERT INTO eit_des_values( eit_table_id ,des_type_table_id ,des_length ,des_value)VALUES( @tableId,@desTypeId,@desLength,@desValue)");
					break;
				case DescriptorCondtitionEnum.EDIT:
					stringBuilder = new StringBuilder("UPDATE eit_des_values SET des_length =  @desLength ,des_value = @desValue WHERE id = @id");
					break;
				case DescriptorCondtitionEnum.DELETEBYID:
					stringBuilder = new StringBuilder("DELETE FROM eit_des_values WHERE  id = @id");
					break;
				case DescriptorCondtitionEnum.DELETEBYTABLEIDANDTYPEID:
					stringBuilder = new StringBuilder("DELETE FROM eit_des_values WHERE  eit_table_id = @tableId AND des_type_table_id = @desTypeId");
					break;
				case DescriptorCondtitionEnum.DELETEBYLOCATIONANDTABLEID:
					stringBuilder = new StringBuilder("DELETE FROM eit_des_values WHERE  eit_table_id = @tableId");
					break;
				}
				break;
			case EnumPossibleLoaction.TOT:
				stringBuilder.Append("SELECT val.id AS ID, tot_table_id AS tableID,typ.des_location AS location,typ.id AS typeId,typ.des_tag AS tag,typ.des_name, des_length, des_value FROM tot_des_values AS val INNER JOIN descriptor_type AS typ ON typ.id = val.des_type_table_id WHERE 1 = 1");
				stringBuilder.Append(" AND typ.des_location = '" + location.ToString() + "'");
				switch (condition)
				{
				case DescriptorCondtitionEnum.ALL:
					stringBuilder.Append("  AND (1=1  OR  eit_table_id = @tableId) ORDER BY val.id");
					break;
				case DescriptorCondtitionEnum.ADD:
					stringBuilder = new StringBuilder("DELETE FROM tot_des_values; \r\n                                                                            INSERT INTO tot_des_values( tot_table_id ,des_type_table_id ,des_length ,des_value)VALUES( @tableId,@desTypeId,@desLength,@desValue)");
					break;
				}
				break;
			}
			return stringBuilder.ToString();
		}

		private DescriptorSigleBase ReaderToEnitty(SqlDataReader reader, EnumPossibleLoaction location)
		{
			DescriptorSigleBase descriptorSigleBase = new DescriptorSigleBase();
			descriptorSigleBase.Identity = Tool.GetInt64(reader["ID"]);
			switch (location)
			{
			case EnumPossibleLoaction.BAT_TS:
			{
				descriptorSigleBase.BATTableID = Tool.GetInt64(reader["batTableID"]);
				long tSTableID = (descriptorSigleBase.TableID = Tool.GetInt64(reader["tsTableID"]));
				descriptorSigleBase.TSTableID = tSTableID;
				break;
			}
			case EnumPossibleLoaction.NIT_TS:
			{
				long tSTableID = (descriptorSigleBase.TableID = Tool.GetInt64(reader["tableID"]));
				descriptorSigleBase.TSTableID = tSTableID;
				break;
			}
			default:
				descriptorSigleBase.TableID = Tool.GetInt64(reader["tableID"]);
				break;
			}
			descriptorSigleBase.DesTypeID = Tool.GetInt64(reader["typeId"]);
			descriptorSigleBase.Location = CommTool.GetLocatingEnum(reader["location"]);
			descriptorSigleBase.DesTypeTag = Tool.GetInt32(reader["tag"]);
			descriptorSigleBase.DesTypeName = Tool.GetString(reader["des_name"]);
			descriptorSigleBase.DataLength = Tool.GetInt32(reader["des_length"]);
			if (descriptorSigleBase.DataLength > 0)
			{
				descriptorSigleBase.DescriptionData = Tool.GetBytes(reader["des_value"]);
			}
			return descriptorSigleBase;
		}

		private void ManagementSendNotices(SqlTransaction tran, EnumSendNoticesCodes location, long identity, int oper)
		{
			if (identity > 0)
			{
				SendNotices sendNotices = new SendNotices();
				sendNotices.Location = location.ToString();
				sendNotices.Code = (int)location;
				sendNotices.Operation = oper;
				sendNotices.TableIdentity = identity;
				sendNotices.UserId = "";
				sendNotices.State = false;
				sendNoticeService.SaveWithTranscation(tran, sendNotices);
			}
		}

		private void ManagementSendNotices(EnumSendNoticesCodes location, long identity, int oper)
		{
			if (identity > 0)
			{
				SendNotices sendNotices = new SendNotices();
				sendNotices.Location = location.ToString();
				sendNotices.Code = (int)location;
				sendNotices.Operation = oper;
				sendNotices.TableIdentity = identity;
				sendNotices.UserId = "";
				sendNotices.State = false;
				sendNoticeService.SaveWithTranscation(sendNotices);
			}
		}

		internal bool AddEitExtendDescriptor(long eittableid, string languagecode, string eventname, string shortremrk, string longremark)
		{
			ShortEventDescriptor args = new ShortEventDescriptor(eittableid, EnumPossibleLoaction.EIT, languagecode, eventname, shortremrk);
			AddDescriptor(args);
			DeleteExtendedEventDescriptor(eittableid);
			List<KeyValuePair<string, string>> extendedEventkvs = GetExtendedEventkvs(eittableid);
			if (extendedEventkvs.Count == 0 && string.IsNullOrEmpty(longremark))
			{
				return true;
			}
			ExtendedEventDescriptorBase extendedEventDescriptorBase = new ExtendedEventDescriptorBase(eittableid, languagecode);
			extendedEventDescriptorBase.Setkvs(extendedEventkvs);
			extendedEventDescriptorBase.SetRemark(longremark);
			extendedEventDescriptorBase.Extendeds.OrderBy((ExtendedEventDescriptor p) => p.Number).ToList().ForEach(delegate(ExtendedEventDescriptor descriptor)
			{
				AddDescriptor(descriptor, false);
			});
			return true;
		}

		internal List<KeyValuePair<string, string>> GetExtendedEventkvs(long eittableid)
		{
			List<DescriptorSigleBase> descriptors = (from p in GetDescriptors(EnumPossibleLoaction.EIT, eittableid)
				where p.DesTypeTag == 78
				select p).ToList();
			return ExtendedEventDescriptorBase.FormDB(descriptors);
		}

		private bool MoifyShortDescriptor(ShortEventDescriptor item)
		{
			return true;
		}

		public bool AddDescriptor(ExtendedEventDescriptorBase item)
		{
			try
			{
				DeleteExtendedEventDescriptor(item.EitTableID);
				EITInfoDAL eITInfoDAL = new EITInfoDAL();
				EitInfo eitInfo = eITInfoDAL.GetbyID(item.TableID);
				if (eitInfo != null)
				{
					eitInfo.EventLongRemark = item.Remark;
					eITInfoDAL.SaveWithTranscation(eitInfo, false, false);
					item.Extendeds.OrderBy((ExtendedEventDescriptor p) => p.Number).ToList().ForEach(delegate(ExtendedEventDescriptor descriptor)
					{
						AddDescriptor(descriptor, false);
					});
				}
				SqlHelper.SaveChange();
				return true;
			}
			catch (Exception)
			{
				SqlHelper.CancelChange();
			}
			return false;
		}

		internal bool AddEitParentalRatingDescriptor(long eittableid, string countrycode, byte rating)
		{
			DeleteParentalRateingDescriptor(eittableid);
			if (!string.IsNullOrEmpty(countrycode))
			{
				ParentalRatingBase args = new ParentalRatingBase(eittableid, countrycode, "", rating);
				AddDescriptor(args, false);
			}
			return true;
		}

		public bool AddDescriptor(ParentalRatingBase item)
		{
			try
			{
				DeleteParentalRateingDescriptor(item.TableID);
				if (item.Items.Count > 0)
				{
					EITInfoDAL eITInfoDAL = new EITInfoDAL();
					EitInfo eitInfo = eITInfoDAL.GetbyID(item.TableID);
					if (eitInfo != null)
					{
						ParentalRating parentalRating = item.Items.FirstOrDefault();
						eitInfo.EITRateSetting = new EitRateSetting
						{
							RateCode = parentalRating.rate_code,
							RateValue = parentalRating.Rating
						};
						eITInfoDAL.SaveWithTranscation(eitInfo, false, false);
						AddDescriptor(item, false);
					}
				}
				SqlHelper.SaveChange();
				return true;
			}
			catch (Exception)
			{
				SqlHelper.CancelChange();
			}
			return false;
		}

		private void DeleteExtendedEventDescriptor(long eittableid)
		{
			string cmdText = "\r\n                                    DELETE FROM dbo.eit_des_values\r\n                                    FROM dbo.descriptor_type AS t\r\n                                    WHERE \r\n                                    t.id = eit_des_values.des_type_table_id\r\n                                    AND t.des_tag = @tag AND t.des_location = @location\r\n                                    AND eit_des_values.eit_table_id = @eitid\r\n                                        ";
			SqlParameter[] commandParameters = new SqlParameter[3]
			{
				SqlHelper.AddParameter("@tag", 78),
				SqlHelper.AddParameter("@location", EnumPossibleLoaction.EIT.ToString()),
				SqlHelper.AddParameter("@eitid", eittableid)
			};
			SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
		}

		private void DeleteParentalRateingDescriptor(long eittableid)
		{
			string cmdText = "\r\n                                    DELETE FROM dbo.eit_des_values\r\n                                    FROM dbo.descriptor_type AS t\r\n                                    WHERE \r\n                                    t.id = eit_des_values.des_type_table_id\r\n                                    AND t.des_tag = @tag AND t.des_location = @location\r\n                                    AND eit_des_values.eit_table_id = @eitid\r\n                                        ";
			SqlParameter[] commandParameters = new SqlParameter[3]
			{
				SqlHelper.AddParameter("@tag", 85),
				SqlHelper.AddParameter("@location", EnumPossibleLoaction.EIT.ToString()),
				SqlHelper.AddParameter("@eitid", eittableid)
			};
			SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
		}

		internal void UpdateLCNAndServiceList(long tstableid, int serviceid)
		{
			IList<DescriptorSigleBase> descriptors = GetDescriptors(EnumPossibleLoaction.NIT_TS, tstableid);
			IEnumerable<DescriptorSigleBase> enumerable = descriptors.Where((DescriptorSigleBase p) => p.DesTypeTag == 131);
			foreach (DescriptorSigleBase item in enumerable)
			{
				byte[] array = UpdateLcnDescriptor(item, serviceid).ToArray();
				item.DataLength = array.Length;
				item.DescriptionData = array;
				ModifyDescriptor(item.Location, item);
			}
			IEnumerable<DescriptorSigleBase> enumerable2 = descriptors.Where((DescriptorSigleBase p) => p.DesTypeTag == 65);
			foreach (DescriptorSigleBase item2 in enumerable2)
			{
				byte[] array = UpdateServiceListDescriptor(item2, serviceid).ToArray();
				item2.DataLength = array.Length;
				item2.DescriptionData = array;
				ModifyDescriptor(item2.Location, item2);
			}
			descriptors = GetBAT_TSDescriptorsByTsTableId(tstableid);
			enumerable = descriptors.Where((DescriptorSigleBase p) => p.DesTypeTag == 131);
			foreach (DescriptorSigleBase item3 in enumerable)
			{
				byte[] array = UpdateLcnDescriptor(item3, serviceid).ToArray();
				item3.DataLength = array.Length;
				item3.DescriptionData = array;
				ModifyDescriptor(item3.Location, item3);
			}
			enumerable2 = descriptors.Where((DescriptorSigleBase p) => p.DesTypeTag == 65);
			foreach (DescriptorSigleBase item4 in enumerable2)
			{
				byte[] array = UpdateServiceListDescriptor(item4, serviceid).ToArray();
				item4.DataLength = array.Length;
				item4.DescriptionData = array;
				ModifyDescriptor(item4.Location, item4);
			}
		}

		private List<byte> UpdateLcnDescriptor(DescriptorSigleBase ET, int serviceid)
		{
			List<byte> list = new List<byte>();
			int num = 0;
			while (num < ET.DataLength)
			{
				byte[] collection = Tool.ResolveByte(ET.DescriptionData, num, 4);
				short num2 = Tool.ResolveInt16(Tool.ResolveByte(ET.DescriptionData, num, 2), NetWorkEnum.NetWorkToHost);
				num += 4;
				if (num2 != serviceid)
				{
					list.AddRange(collection);
				}
			}
			return list;
		}

		private List<byte> UpdateServiceListDescriptor(DescriptorSigleBase ET, int serviceid)
		{
			List<byte> list = new List<byte>();
			int num = 0;
			while (num < ET.DataLength)
			{
				byte[] collection = Tool.ResolveByte(ET.DescriptionData, num, 3);
				short num2 = Tool.ResolveInt16(Tool.ResolveByte(ET.DescriptionData, num, 2), NetWorkEnum.NetWorkToHost);
				num += 3;
				if (num2 != serviceid)
				{
					list.AddRange(collection);
				}
			}
			return list;
		}

		private List<DescriptorSigleBase> GetBAT_TSDescriptorsByTsTableId(long tstableid)
		{
			List<DescriptorSigleBase> list = new List<DescriptorSigleBase>();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT val.id AS ID, bat_table_id AS batTableID, ts_table_id AS tsTableID ,typ.des_location AS location,typ.id AS typeId,typ.des_tag AS tag,typ.des_name, des_length, des_value FROM bat_ts_des_values AS val INNER JOIN descriptor_type AS typ ON typ.id = val.des_type_table_id  WHERE 1 = 1");
			stringBuilder.Append(" AND typ.des_location = '" + EnumPossibleLoaction.BAT_TS.ToString() + "'");
			stringBuilder.Append("  AND ts_table_id = @ts_table_id ORDER BY val.id");
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@ts_table_id", tstableid, SqlDbType.BigInt) };
			using (SqlDataReader sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, stringBuilder.ToString(), commandParameters))
			{
				while (sqlDataReader.Read())
				{
					list.Add(ReaderToEnitty(sqlDataReader, EnumPossibleLoaction.BAT_TS));
				}
			}
			return list;
		}

		internal void ClearAllEitDescriptors()
		{
			string cmdText = "DELETE FROM eit_des_values";
			SqlParameter[] commandParameters = new SqlParameter[0];
			SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
		}
	}
}
