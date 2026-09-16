using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.IDAL;
using DeXin.EPG.Model;
using DeXin.EPG.Resource;
using DeXin.EPG.Tools;

namespace DeXin.EPG.DAL
{
	public class SendNoticesDAL : ISendNoticesDAL
	{
		public IList<SendNotices> GetAll()
		{
			IList<SendNotices> list = new List<SendNotices>();
			string cmdText = "SELECT * FROM send_notices WHERE state = 1 ORDER BY code ";
			using (SqlDataReader sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText))
			{
				while (sqlDataReader.Read())
				{
					SendNotices sendNotices = new SendNotices();
					sendNotices.Id = Tool.GetInt64(sqlDataReader["id"]);
					sendNotices.Location = sqlDataReader["location"].ToString();
					sendNotices.TableIdentity = Tool.GetInt64(sqlDataReader["table_identity"]);
					sendNotices.Code = Tool.GetInt32(sqlDataReader["code"]);
					sendNotices.Operation = Tool.GetInt32(sqlDataReader["operation"]);
					sendNotices.UserId = sqlDataReader["user_id"].ToString();
					sendNotices.EditDate = Tool.GetDateTime(sqlDataReader["edit_date"]);
					sendNotices.State = Tool.GetBoolean(sqlDataReader["state"]);
					SendNotices item = sendNotices;
					list.Add(item);
				}
			}
			return list;
		}

		private SendNotices GetByLocationAndTableidentityWithTranscation(string Location, long TableIdentity)
		{
			string cmdText = "SELECT * FROM send_notices WHERE location = @location AND table_identity = @table_identity ";
			SqlParameter[] commandParameters = new SqlParameter[2]
			{
				SqlHelper.AddParameter("@location", Location),
				SqlHelper.AddParameter("@table_identity", TableIdentity)
			};
			using (SqlDataReader sqlDataReader = SqlHelper.ExecuteReader(CommandType.Text, cmdText, commandParameters))
			{
				if (sqlDataReader.Read())
				{
					SendNotices sendNotices = new SendNotices();
					sendNotices.Id = Tool.GetInt64(sqlDataReader["id"]);
					sendNotices.Location = sqlDataReader["location"].ToString();
					sendNotices.TableIdentity = Tool.GetInt64(sqlDataReader["table_identity"]);
					sendNotices.Code = Tool.GetInt32(sqlDataReader["code"]);
					sendNotices.Operation = Tool.GetInt32(sqlDataReader["operation"]);
					sendNotices.UserId = sqlDataReader["user_id"].ToString();
					sendNotices.EditDate = Tool.GetDateTime(sqlDataReader["edit_date"]);
					sendNotices.State = Tool.GetBoolean(sqlDataReader["state"]);
					return sendNotices;
				}
			}
			return null;
		}

		private SendNotices GetByLocationAndTableidentityWithTranscation(SqlTransaction Transcation, string Location, long TableIdentity)
		{
			string cmdText = "SELECT * FROM send_notices WHERE location = @location AND table_identity = @table_identity ";
			SqlParameter[] commandParameters = new SqlParameter[2]
			{
				SqlHelper.AddParameter("@location", Location),
				SqlHelper.AddParameter("@table_identity", TableIdentity)
			};
			using (SqlDataReader sqlDataReader = SqlHelper.ExecuteReader(Transcation, CommandType.Text, cmdText, commandParameters))
			{
				if (sqlDataReader.Read())
				{
					SendNotices sendNotices = new SendNotices();
					sendNotices.Id = Tool.GetInt64(sqlDataReader["id"]);
					sendNotices.Location = sqlDataReader["location"].ToString();
					sendNotices.TableIdentity = Tool.GetInt64(sqlDataReader["table_identity"]);
					sendNotices.Code = Tool.GetInt32(sqlDataReader["code"]);
					sendNotices.Operation = Tool.GetInt32(sqlDataReader["operation"]);
					sendNotices.UserId = sqlDataReader["user_id"].ToString();
					sendNotices.EditDate = Tool.GetDateTime(sqlDataReader["edit_date"]);
					sendNotices.State = Tool.GetBoolean(sqlDataReader["state"]);
					return sendNotices;
				}
			}
			return null;
		}

		private bool NeedSendOut(SendNotices notice, SqlTransaction Transcation)
		{
			EnumSendNoticesCodes code = (EnumSendNoticesCodes)notice.Code;
			string text = string.Empty;
			switch (code)
			{
			case EnumSendNoticesCodes.DES_BAT_TS:
				text = "SELECT  COUNT(1) FROM    bat_ts_des_values x ,         bat_info b ,         ts_info t ,         program_info p ,         PAT pat WHERE   x.id = @xKey         AND x.bat_table_id = b.id         AND b.nit_table_id = t.nit_table_id         AND t.id = p.ts_table_id         AND p.id = pat.program_table_id";
				break;
			case EnumSendNoticesCodes.DES_BAT:
				text = "SELECT  COUNT(1) FROM    bat_des_values x ,         bat_info b ,         ts_info t ,         program_info p ,         PAT pat WHERE   x.id = @xKey         AND x.bat_table_id = b.id         AND b.nit_table_id = t.nit_table_id         AND t.id = p.ts_table_id         AND p.id = pat.program_table_id";
				break;
			case EnumSendNoticesCodes.BAT:
				text = "SELECT  COUNT(1) FROM    bat_info b ,         ts_info t ,         program_info p ,         PAT pat WHERE   b.id = @xKey         AND b.nit_table_id = t.nit_table_id         AND t.id = p.ts_table_id         AND p.id = pat.program_table_id";
				break;
			case EnumSendNoticesCodes.DES_CAT:
				text = "SELECT  COUNT(1) FROM    cat_des_values x         INNER JOIN program_info p ON p.ts_table_id = x.ts_table_id         INNER JOIN PAT pat ON p.id = pat.program_table_id WHERE   x.id = @xKey ";
				break;
			case EnumSendNoticesCodes.TS:
				text = "SELECT  COUNT(1) FROM    ts_info t         INNER JOIN program_info p ON t.id = p.ts_table_id         INNER JOIN PAT pat ON p.id = pat.program_table_id WHERE   t.id = @xKey ";
				break;
			case EnumSendNoticesCodes.DES_EIT:
				text = "SELECT  COUNT(1) FROM    eit_des_values x         INNER JOIN eit_info e ON x.eit_table_id = e.id         INNER JOIN PAT pat ON e.program_table_id = pat.program_table_id WHERE   x.id = @xKey ";
				break;
			case EnumSendNoticesCodes.EIT:
				text = "SELECT  COUNT(1) FROM    eit_info e         INNER JOIN PAT pat ON e.program_table_id = pat.program_table_id WHERE   e.id = @xKey ";
				break;
			case EnumSendNoticesCodes.DES_ES:
				text = "SELECT  COUNT(1) FROM    multimedia_des_values x ,         multimedia m ,         PAT pat WHERE   x.id = @xKey         AND x.multimedia_table_id = m.id         AND m.program_table_id = pat.program_table_id";
				break;
			case EnumSendNoticesCodes.multimedia:
				text = "SELECT  COUNT(1) FROM    multimedia m ,         PAT pat WHERE   m.id = @xKey         AND m.program_table_id = pat.program_table_id";
				break;
			case EnumSendNoticesCodes.DES_NIT:
				text = "SELECT  COUNT(1) FROM    nit_des_values x ,         ts_info t ,         program_info p ,         PAT pat WHERE   x.id = @xKey         AND x.nit_table_id = t.nit_table_id         AND t.id = p.ts_table_id         AND p.id = pat.program_table_id";
				break;
			case EnumSendNoticesCodes.DES_NIT_TS:
				text = "SELECT  COUNT(1) FROM    ts_des_values x ,         program_info p ,         PAT pat WHERE   x.id = @xKey         AND x.ts_table_id = p.ts_table_id         AND p.id = pat.program_table_id";
				break;
			case EnumSendNoticesCodes.NIT:
				text = "SELECT  COUNT(1) FROM    nit_info n ,         ts_info t ,         program_info p ,         PAT pat WHERE   n.id = @xKey         AND n.id = t.nit_table_id         AND t.id = p.ts_table_id         AND p.id = pat.program_table_id";
				break;
			case EnumSendNoticesCodes.DES_SDT:
				text = "SELECT  COUNT(1) FROM    sdt_des_value x ,         PAT pat WHERE   x.id = @xKey         AND x.program_table_id = pat.program_table_id";
				break;
			case EnumSendNoticesCodes.DES_PMT:
				text = "SELECT  COUNT(1) FROM    pmt_des_values x ,         PAT pat WHERE   x.id = @xKey         AND x.program_table_id = pat.program_table_id";
				break;
			case EnumSendNoticesCodes.PROGRAM:
				text = "SELECT  COUNT(1) FROM    program_info x ,         PAT pat WHERE   x.id = @xKey         AND x.id = pat.program_table_id";
				break;
			case EnumSendNoticesCodes.PMTPIDRemapping:
				text = "SELECT  COUNT(1) FROM    pmt_pid_output_info x ,         PAT pat WHERE   x.id = @xKey         AND x.program_table_id = pat.program_table_id";
				break;
			case EnumSendNoticesCodes.ReMapping:
				text = "SELECT  COUNT(1) FROM    pid_output_info x ,         program_info p ,         PAT pat WHERE   x.id = @xKey         AND x.ts_table_id = p.ts_table_id         AND p.id = pat.program_table_id";
				break;
			case EnumSendNoticesCodes.SendOrNot:
				text = "SELECT  COUNT(1) FROM    send_or_not_info x ,         program_info p ,         PAT pat WHERE   x.id = @xKey         AND x.ts_table_id = p.ts_table_id         AND p.id = pat.program_table_id";
				break;
			case EnumSendNoticesCodes.SendSpeedInfo:
				text = "SELECT  COUNT(1) FROM    send_speed_info x ,         program_info p ,         PAT pat WHERE   x.id = @xKey         AND x.ts_table_id = p.ts_table_id         AND p.id = pat.program_table_id";
				break;
			}
			if (string.IsNullOrEmpty(text))
			{
				return true;
			}
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@xKey", notice.TableIdentity) };
			int num = 0;
			if (((Transcation != null) ? ((int)SqlHelper.ExecuteScalar(Transcation, CommandType.Text, text, commandParameters)) : ((int)SqlHelper.ExecuteScalar(CommandType.Text, text, commandParameters))) == 0)
			{
				return false;
			}
			return true;
		}

		public int SaveWithTranscation(SendNotices Notice)
		{
			if (!NeedSendOut(Notice, null))
			{
				return 0;
			}
			SendNotices byLocationAndTableidentityWithTranscation = GetByLocationAndTableidentityWithTranscation(Notice.Location, Notice.TableIdentity);
			if (byLocationAndTableidentityWithTranscation != null && Notice.Operation != 1)
			{
				if (Notice.Operation == 2)
				{
					return 0;
				}
				if (Notice.Operation == 3)
				{
					switch (byLocationAndTableidentityWithTranscation.Operation)
					{
					case 1:
					case 2:
						DeleteByIdentityWithTranscation(byLocationAndTableidentityWithTranscation.Id);
						if (!byLocationAndTableidentityWithTranscation.State)
						{
							return 1;
						}
						break;
					case 3:
						return 0;
					}
				}
			}
			Notice.UserId = ((Notice.UserId == null) ? "" : Notice.UserId);
			string cmdText = "INSERT INTO [send_notices] ([location] ,[table_identity] ,[code] ,[operation] ,[user_id] ,[edit_date] ,[state]) VALUES (@location ,@table_identity ,@code ,@operation ,@user_id ,GETDATE() ,0 ) ";
			SqlParameter[] commandParameters = new SqlParameter[5]
			{
				SqlHelper.AddParameter("@location", Notice.Location),
				SqlHelper.AddParameter("@table_identity", Notice.TableIdentity),
				SqlHelper.AddParameter("@code", Notice.Code),
				SqlHelper.AddParameter("@operation", Notice.Operation),
				SqlHelper.AddParameter("@user_id", Notice.UserId)
			};
			return SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
		}

		public int SaveWithTranscation(SqlTransaction Transcation, SendNotices Notice)
		{
			if (!NeedSendOut(Notice, Transcation))
			{
				return 0;
			}
			SendNotices byLocationAndTableidentityWithTranscation = GetByLocationAndTableidentityWithTranscation(Transcation, Notice.Location, Notice.TableIdentity);
			if (byLocationAndTableidentityWithTranscation != null && Notice.Operation != 1)
			{
				if (Notice.Operation == 2)
				{
					return 0;
				}
				if (Notice.Operation == 3)
				{
					switch (byLocationAndTableidentityWithTranscation.Operation)
					{
					case 1:
					case 2:
						DeleteByIdentityWithTranscation(Transcation, byLocationAndTableidentityWithTranscation.Id);
						if (!byLocationAndTableidentityWithTranscation.State)
						{
							return 1;
						}
						break;
					case 3:
						return 0;
					}
				}
			}
			Notice.UserId = ((Notice.UserId == null) ? "" : Notice.UserId);
			string cmdText = "INSERT INTO [send_notices] ([location] ,[table_identity] ,[code] ,[operation] ,[user_id] ,[edit_date] ,[state]) VALUES (@location ,@table_identity ,@code ,@operation ,@user_id ,GETDATE() ,0 ) ";
			SqlParameter[] commandParameters = new SqlParameter[5]
			{
				SqlHelper.AddParameter("@location", Notice.Location),
				SqlHelper.AddParameter("@table_identity", Notice.TableIdentity),
				SqlHelper.AddParameter("@code", Notice.Code),
				SqlHelper.AddParameter("@operation", Notice.Operation),
				SqlHelper.AddParameter("@user_id", Notice.UserId)
			};
			return SqlHelper.ExecuteNonQuery(Transcation, CommandType.Text, cmdText, commandParameters);
		}

		public int SetAllNoticeOn()
		{
			string cmdText = "UPDATE send_notices SET state = 1 WHERE state = 0 ";
			return SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, cmdText, (SqlParameter[])null);
		}

		public int DeleteByIdentityWithTranscation(params long[] noticeIdentity)
		{
			int result = 0;
			try
			{
				if (noticeIdentity.Length == 0)
				{
					return result;
				}
				StringBuilder stringBuilder = new StringBuilder("DELETE FROM send_notices");
				SqlParameter[] array = new SqlParameter[noticeIdentity.Length];
				stringBuilder.Append(" WHERE 1 = 1 AND id IN ");
				for (int i = 0; i < noticeIdentity.Length; i++)
				{
					array[i] = SqlHelper.AddParameter("@id" + i, noticeIdentity[i], SqlDbType.BigInt);
					if (i == 0)
					{
						if (noticeIdentity.Length > 1)
						{
							stringBuilder.Append(" (@id" + i + ",");
						}
						else
						{
							stringBuilder.Append(" (@id" + i + ")");
						}
					}
					else if (i == noticeIdentity.Length - 1)
					{
						stringBuilder.Append(" @id" + i + ")");
					}
					else
					{
						stringBuilder.Append(" @id" + i + ",");
					}
				}
				result = SqlHelper.ExecuteNonQuery(CommandType.Text, stringBuilder.ToString(), array);
			}
			catch (Exception ex)
			{
				result = -1;
				throw ex;
			}
			return result;
		}

		public int DeleteByIdentityWithTranscation(SqlTransaction Transcation, params long[] noticeIdentity)
		{
			int result = 0;
			try
			{
				if (noticeIdentity.Length == 0)
				{
					return result;
				}
				StringBuilder stringBuilder = new StringBuilder("DELETE FROM send_notices");
				SqlParameter[] array = new SqlParameter[noticeIdentity.Length];
				stringBuilder.Append(" WHERE 1 = 1 AND id IN ");
				for (int i = 0; i < noticeIdentity.Length; i++)
				{
					array[i] = SqlHelper.AddParameter("@id" + i, noticeIdentity[i], SqlDbType.BigInt);
					if (i == 0)
					{
						if (noticeIdentity.Length > 1)
						{
							stringBuilder.Append(" (@id" + i + ",");
						}
						else
						{
							stringBuilder.Append(" (@id" + i + ")");
						}
					}
					else if (i == noticeIdentity.Length - 1)
					{
						stringBuilder.Append(" @id" + i + ")");
					}
					else
					{
						stringBuilder.Append(" @id" + i + ",");
					}
				}
				result = SqlHelper.ExecuteNonQuery(Transcation, CommandType.Text, stringBuilder.ToString(), array);
			}
			catch (Exception ex)
			{
				result = -1;
				throw ex;
			}
			return result;
		}

		public void Delete(long Identity)
		{
			try
			{
				DeleteByIdentityWithTranscation(Identity);
				SqlHelper.SaveChange();
			}
			catch (Exception ex)
			{
				SqlHelper.CancelChange();
				throw new CustomException(ResourceManagement.GetLocationString("DeleteDataError"), ex, MessageType.SHOWANDLOG);
			}
		}

		public void DeleteByCode(int locationCode)
		{
			try
			{
				string cmdText = "DELETE FROM send_notices WHERE state = 1 AND code = @code ";
				SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@code", locationCode) };
				SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
			}
			catch (Exception ex)
			{
				throw new CustomException(ResourceManagement.GetLocationString("DeleteDataError"), ex, MessageType.SHOWANDLOG);
			}
		}

		public void DeleteAll()
		{
			try
			{
				string cmdText = "DELETE FROM send_notices";
				SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, cmdText, (SqlParameter[])null);
			}
			catch (Exception ex)
			{
				throw new CustomException(ResourceManagement.GetLocationString("DeleteDataError"), ex, MessageType.SHOWANDLOG);
			}
		}
	}
}
