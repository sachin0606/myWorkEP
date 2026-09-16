using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.IDAL;
using DeXin.EPG.Model;
using DeXin.EPG.Resource;

namespace DeXin.EPG.DAL
{
	public class SendSpeedInfoDAL : ISendSpeedInfoDAL
	{
		private SendNoticesDAL noticeDAL = new SendNoticesDAL();

		public SendSpeedInfo GetByTSIdentity(long TSIdentity)
		{
			string cmdText = "SELECT * FROM [send_speed_info] WHERE ts_table_id = @ts_table_id";
			SqlDataReader sqlDataReader = null;
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@ts_table_id", TSIdentity) };
			try
			{
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
				if (sqlDataReader.Read())
				{
					SendSpeedInfo sendSpeedInfo = new SendSpeedInfo();
					sendSpeedInfo.Id = Tool.GetInt64(sqlDataReader["id"]);
					sendSpeedInfo.TsTableId = TSIdentity;
					sendSpeedInfo.Pat = Tool.GetInt32(sqlDataReader["pat"]);
					sendSpeedInfo.Cat = Tool.GetInt32(sqlDataReader["cat"]);
					sendSpeedInfo.Pmt = Tool.GetInt32(sqlDataReader["pmt"]);
					sendSpeedInfo.SdtAct = Tool.GetInt32(sqlDataReader["sdt_act"]);
					sendSpeedInfo.SdtOth = Tool.GetInt32(sqlDataReader["sdt_oth"]);
					sendSpeedInfo.NitAct = Tool.GetInt32(sqlDataReader["nit_act"]);
					sendSpeedInfo.NitOth = Tool.GetInt32(sqlDataReader["nit_oth"]);
					sendSpeedInfo.Tdt = Tool.GetInt32(sqlDataReader["tdt"]);
					sendSpeedInfo.Tot = Tool.GetInt32(sqlDataReader["tot"]);
					sendSpeedInfo.EitActPf = Tool.GetInt32(sqlDataReader["eit_act_pf"]);
					sendSpeedInfo.EitActSc = Tool.GetInt32(sqlDataReader["eit_act_sc"]);
					sendSpeedInfo.EitOthPf = Tool.GetInt32(sqlDataReader["eit_oth_pf"]);
					sendSpeedInfo.EitOthSc = Tool.GetInt32(sqlDataReader["eit_oth_sc"]);
					sendSpeedInfo.Bat = Tool.GetInt32(sqlDataReader["bat"]);
					SendSpeedInfo sendSpeedInfo2 = sendSpeedInfo;
					sendSpeedInfo2.ReadTS += speed_ReadTS;
					return sendSpeedInfo2;
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

		private void speed_ReadTS(object sender)
		{
			SendSpeedInfo sendSpeedInfo = sender as SendSpeedInfo;
			sendSpeedInfo.TS = new TSInfoDAL().GetTSInfoByTSIdentity(sendSpeedInfo.TsTableId);
		}

		public IList<SendSpeedInfo> GetByNITIdentity(long NITIdentity)
		{
			IList<SendSpeedInfo> list = new List<SendSpeedInfo>();
			string cmdText = "SELECT s.* FROM send_speed_info s, ts_info t, nit_info n WHERE n.id=@id AND t.nit_table_id=n.id AND s.ts_table_id=t.id";
			SqlDataReader sqlDataReader = null;
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@id", NITIdentity) };
			try
			{
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
				while (sqlDataReader.Read())
				{
					SendSpeedInfo sendSpeedInfo = new SendSpeedInfo();
					sendSpeedInfo.Id = Tool.GetInt64(sqlDataReader["id"]);
					sendSpeedInfo.TsTableId = Tool.GetInt64(sqlDataReader["ts_table_id"]);
					sendSpeedInfo.Pat = Tool.GetInt32(sqlDataReader["pat"]);
					sendSpeedInfo.Cat = Tool.GetInt32(sqlDataReader["cat"]);
					sendSpeedInfo.Pmt = Tool.GetInt32(sqlDataReader["pmt"]);
					sendSpeedInfo.SdtAct = Tool.GetInt32(sqlDataReader["sdt_act"]);
					sendSpeedInfo.SdtOth = Tool.GetInt32(sqlDataReader["sdt_oth"]);
					sendSpeedInfo.NitAct = Tool.GetInt32(sqlDataReader["nit_act"]);
					sendSpeedInfo.NitOth = Tool.GetInt32(sqlDataReader["nit_oth"]);
					sendSpeedInfo.Tdt = Tool.GetInt32(sqlDataReader["tdt"]);
					sendSpeedInfo.Tot = Tool.GetInt32(sqlDataReader["tot"]);
					sendSpeedInfo.EitActPf = Tool.GetInt32(sqlDataReader["eit_act_pf"]);
					sendSpeedInfo.EitActSc = Tool.GetInt32(sqlDataReader["eit_act_sc"]);
					sendSpeedInfo.EitOthPf = Tool.GetInt32(sqlDataReader["eit_oth_pf"]);
					sendSpeedInfo.EitOthSc = Tool.GetInt32(sqlDataReader["eit_oth_sc"]);
					sendSpeedInfo.Bat = Tool.GetInt32(sqlDataReader["bat"]);
					SendSpeedInfo sendSpeedInfo2 = sendSpeedInfo;
					sendSpeedInfo2.ReadTS += speed_ReadTS;
					list.Add(sendSpeedInfo2);
				}
				return list;
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

		public IList<SendSpeedInfo> GetOutPutSpeedSetting()
		{
			IList<SendSpeedInfo> list = new List<SendSpeedInfo>();
			string cmdText = "SELECT * FROM send_speed_info s WHERE EXISTS ( \tSELECT 1 FROM program_info p, PAT pat \tWHERE pat.program_table_id = p.id \tAND p.ts_table_id = s.ts_table_id )";
			using (SqlDataReader sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, (SqlParameter[])null))
			{
				while (sqlDataReader.Read())
				{
					SendSpeedInfo sendSpeedInfo = new SendSpeedInfo();
					sendSpeedInfo.Id = Tool.GetInt64(sqlDataReader["id"]);
					sendSpeedInfo.TsTableId = Tool.GetInt64(sqlDataReader["ts_table_id"]);
					sendSpeedInfo.Pat = Tool.GetInt32(sqlDataReader["pat"]);
					sendSpeedInfo.Cat = Tool.GetInt32(sqlDataReader["cat"]);
					sendSpeedInfo.Pmt = Tool.GetInt32(sqlDataReader["pmt"]);
					sendSpeedInfo.SdtAct = Tool.GetInt32(sqlDataReader["sdt_act"]);
					sendSpeedInfo.SdtOth = Tool.GetInt32(sqlDataReader["sdt_oth"]);
					sendSpeedInfo.NitAct = Tool.GetInt32(sqlDataReader["nit_act"]);
					sendSpeedInfo.NitOth = Tool.GetInt32(sqlDataReader["nit_oth"]);
					sendSpeedInfo.Tdt = Tool.GetInt32(sqlDataReader["tdt"]);
					sendSpeedInfo.Tot = Tool.GetInt32(sqlDataReader["tot"]);
					sendSpeedInfo.EitActPf = Tool.GetInt32(sqlDataReader["eit_act_pf"]);
					sendSpeedInfo.EitActSc = Tool.GetInt32(sqlDataReader["eit_act_sc"]);
					sendSpeedInfo.EitOthPf = Tool.GetInt32(sqlDataReader["eit_oth_pf"]);
					sendSpeedInfo.EitOthSc = Tool.GetInt32(sqlDataReader["eit_oth_sc"]);
					sendSpeedInfo.Bat = Tool.GetInt32(sqlDataReader["bat"]);
					SendSpeedInfo sendSpeedInfo2 = sendSpeedInfo;
					sendSpeedInfo2.ReadTS += speed_ReadTS;
					list.Add(sendSpeedInfo2);
				}
			}
			return list;
		}

		public SendSpeedInfo GetByIdentity(long Identity)
		{
			string cmdText = "SELECT * FROM [send_speed_info] WHERE id = @id";
			SqlDataReader sqlDataReader = null;
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@id", Identity) };
			try
			{
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
				if (sqlDataReader.Read())
				{
					SendSpeedInfo sendSpeedInfo = new SendSpeedInfo();
					sendSpeedInfo.Id = Identity;
					sendSpeedInfo.TsTableId = Tool.GetInt32(sqlDataReader["ts_table_id"]);
					sendSpeedInfo.Pat = Tool.GetInt32(sqlDataReader["pat"]);
					sendSpeedInfo.Cat = Tool.GetInt32(sqlDataReader["cat"]);
					sendSpeedInfo.Pmt = Tool.GetInt32(sqlDataReader["pmt"]);
					sendSpeedInfo.SdtAct = Tool.GetInt32(sqlDataReader["sdt_act"]);
					sendSpeedInfo.SdtOth = Tool.GetInt32(sqlDataReader["sdt_oth"]);
					sendSpeedInfo.NitAct = Tool.GetInt32(sqlDataReader["nit_act"]);
					sendSpeedInfo.NitOth = Tool.GetInt32(sqlDataReader["nit_oth"]);
					sendSpeedInfo.Tdt = Tool.GetInt32(sqlDataReader["tdt"]);
					sendSpeedInfo.Tot = Tool.GetInt32(sqlDataReader["tot"]);
					sendSpeedInfo.EitActPf = Tool.GetInt32(sqlDataReader["eit_act_pf"]);
					sendSpeedInfo.EitActSc = Tool.GetInt32(sqlDataReader["eit_act_sc"]);
					sendSpeedInfo.EitOthPf = Tool.GetInt32(sqlDataReader["eit_oth_pf"]);
					sendSpeedInfo.EitOthSc = Tool.GetInt32(sqlDataReader["eit_oth_sc"]);
					sendSpeedInfo.Bat = Tool.GetInt32(sqlDataReader["bat"]);
					SendSpeedInfo sendSpeedInfo2 = sendSpeedInfo;
					sendSpeedInfo2.ReadTS += speed_ReadTS;
					return sendSpeedInfo2;
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

		public bool CanSave(SendSpeedInfo controller)
		{
			string cmdText = "select count(1) from send_speed_info where id <> @id and ts_table_id = @ts_id";
			SqlParameter[] commandParameters = new SqlParameter[2]
			{
				SqlHelper.AddParameter("@id", controller.Id),
				SqlHelper.AddParameter("@ts_id", controller.TsTableId)
			};
			object obj = SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
			if ((int)obj > 0)
			{
				return false;
			}
			return true;
		}

		public int Save(SendSpeedInfo controller)
		{
			if (!CanSave(controller))
			{
				throw new CustomException(ResourceManagement.GetLocationString("TSIDExists"), MessageType.SHOW);
			}
			int num = 0;
			long tableIdentity = 0L;
			if (controller.Id != 0)
			{
				string cmdText = "UPDATE [send_speed_info] SET [ts_table_id] = @ts_table_id ,[pat] = @pat ,[cat] = @cat ,[pmt] = @pmt ,[sdt_act] = @sdt_act ,[sdt_oth] = @sdt_oth ,[nit_act] = @nit_act ,[nit_oth] = @nit_oth ,[tdt] = @tdt ,[tot] = @tot ,[eit_act_pf] = @eit_act_pf ,[eit_act_sc] = @eit_act_sc ,[eit_oth_pf] = @eit_oth_pf ,[eit_oth_sc] = @eit_oth_sc ,[bat] = @bat WHERE id = @id ";
				SqlParameter[] commandParameters = new SqlParameter[16]
				{
					SqlHelper.AddParameter("@ts_table_id", controller.TsTableId),
					SqlHelper.AddParameter("@pat", controller.Pat),
					SqlHelper.AddParameter("@cat", controller.Cat),
					SqlHelper.AddParameter("@pmt", controller.Pmt),
					SqlHelper.AddParameter("@sdt_act", controller.SdtAct),
					SqlHelper.AddParameter("@sdt_oth", controller.SdtOth),
					SqlHelper.AddParameter("@nit_act", controller.NitAct),
					SqlHelper.AddParameter("@nit_oth", controller.NitOth),
					SqlHelper.AddParameter("@tdt", controller.Tdt),
					SqlHelper.AddParameter("@tot", controller.Tot),
					SqlHelper.AddParameter("@eit_act_pf", controller.EitActPf),
					SqlHelper.AddParameter("@eit_act_sc", controller.EitActSc),
					SqlHelper.AddParameter("@eit_oth_pf", controller.EitOthPf),
					SqlHelper.AddParameter("@eit_oth_sc", controller.EitOthSc),
					SqlHelper.AddParameter("@bat", controller.Bat),
					SqlHelper.AddParameter("@id", controller.Id)
				};
				num = SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
				tableIdentity = controller.Id;
			}
			else
			{
				string cmdText = "INSERT INTO [send_speed_info] ([ts_table_id] ,[pat] ,[cat] ,[pmt] ,[sdt_act] ,[sdt_oth] ,[nit_act] ,[nit_oth] ,[tdt] ,[tot] ,[eit_act_pf] ,[eit_act_sc] ,[eit_oth_pf] ,[eit_oth_sc] ,[bat]) VALUES (@ts_table_id ,@pat ,@cat ,@pmt ,@sdt_act ,@sdt_oth ,@nit_act ,@nit_oth ,@tdt ,@tot ,@eit_act_pf ,@eit_act_sc ,@eit_oth_pf ,@eit_oth_sc ,@bat) SELECT @@IDENTITY ";
				SqlParameter[] commandParameters = new SqlParameter[15]
				{
					SqlHelper.AddParameter("@ts_table_id", controller.TsTableId),
					SqlHelper.AddParameter("@pat", controller.Pat),
					SqlHelper.AddParameter("@cat", controller.Cat),
					SqlHelper.AddParameter("@pmt", controller.Pmt),
					SqlHelper.AddParameter("@sdt_act", controller.SdtAct),
					SqlHelper.AddParameter("@sdt_oth", controller.SdtOth),
					SqlHelper.AddParameter("@nit_act", controller.NitAct),
					SqlHelper.AddParameter("@nit_oth", controller.NitOth),
					SqlHelper.AddParameter("@tdt", controller.Tdt),
					SqlHelper.AddParameter("@tot", controller.Tot),
					SqlHelper.AddParameter("@eit_act_pf", controller.EitActPf),
					SqlHelper.AddParameter("@eit_act_sc", controller.EitActSc),
					SqlHelper.AddParameter("@eit_oth_pf", controller.EitOthPf),
					SqlHelper.AddParameter("@eit_oth_sc", controller.EitOthSc),
					SqlHelper.AddParameter("@bat", controller.Bat)
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
			noticeDAL.SaveWithTranscation(new SendNotices
			{
				Code = 23,
				Location = "send_speed_info",
				Operation = ((controller.Id <= 0) ? 1 : 2),
				TableIdentity = tableIdentity
			});
			if (num == 1)
			{
				SqlHelper.SaveChange();
				return num;
			}
			SqlHelper.CancelChange();
			return -1;
		}

		public int Delete(long Identity)
		{
			string cmdText = "DELETE FROM [send_speed_info] WHERE id = @id";
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@id", Identity) };
			return SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
		}

		public int Delete(IList<long> ListIdentity)
		{
			try
			{
				foreach (long item in ListIdentity)
				{
					noticeDAL.SaveWithTranscation(new SendNotices
					{
						Code = 23,
						Location = "send_speed_info",
						Operation = 3,
						TableIdentity = item
					});
					DeleteWithTranscation(item);
				}
				SqlHelper.SaveChange();
				return ListIdentity.Count;
			}
			catch (Exception ex)
			{
				SqlHelper.CancelChange();
				throw new CustomException(ResourceManagement.GetLocationString("DeleteDataError"), ex, MessageType.SHOWANDLOG);
			}
		}

		public int DeleteWithTranscation(long Identity)
		{
			string cmdText = "DELETE FROM [send_speed_info] WHERE id = @id";
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@id", Identity) };
			return SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
		}

		public int DeleteByTSIdentityWithTranscation(long TSIdentity)
		{
			string cmdText = "DELETE FROM [send_speed_info] WHERE ts_table_id = @ts_table_id";
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@ts_table_id", TSIdentity) };
			return SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
		}

		public int DeleteByNITIdentityWithTranscation(long NITIdentity)
		{
			string cmdText = "DELETE FROM [send_speed_info] FROM ts_info a, send_speed_info b WHERE a.nit_table_id = @nit_table_id and a.id = b.ts_table_id";
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@nit_table_id", NITIdentity) };
			return SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
		}
	}
}
