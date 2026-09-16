using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.IDAL;
using DeXin.EPG.Model;
using DeXin.EPG.Resource;
using DeXin.EPG.Resource.ResourcesSpecialFiles;
using DeXin.EPG.Tools;
using Dexin.Util.Common.Text;

namespace DeXin.EPG.DAL
{
	public class EITInfoDAL : IEITInfoDAL
	{
		private string CountryCode;

		private SendNoticesDAL noticeDAL = new SendNoticesDAL();

		private EitInfo EITSqlDataReader(IDataReader sdr, bool toLocalTime)
		{
			long @int = Tool.GetInt64(sdr["id"]);
			string text = sdr["event_name"].ToString();
			EitInfo eitInfo = new EitInfo(@int, text);
			eitInfo.EventId = Tool.GetInt32(sdr["event_id"]);
			eitInfo.EventName = text;
			eitInfo.EventRemark = sdr["event_remark"].ToString();
			eitInfo.EventLongRemark = sdr["event_long_remark"].ToString();
			eitInfo.ProgramTableId = Tool.GetInt64(sdr["program_table_id"]);
			eitInfo.StartTime = Tool.GetDateTime(sdr["start_time"]);
			eitInfo.EndTime = Tool.GetDateTime(sdr["end_time"]);
			eitInfo.EitRateValue = getRate(sdr["end_time"]);
			EitInfo eitInfo2 = eitInfo;
			eitInfo2.Node = eitInfo2;
			if (toLocalTime)
			{
				eitInfo2.StartTime = eitInfo2.StartTime.ToLocalTime();
				eitInfo2.EndTime = eitInfo2.EndTime.ToLocalTime();
			}
			eitInfo2.ReadProgram += eit_ReadProgram;
			eitInfo2.ReadEitRateSetting += eit_ReadEITRateSetting;
			return eitInfo2;
		}

		private EitInfo EITDataRowReader(DataRow iRow, bool toLocalTime)
		{
			long @int = Tool.GetInt64(iRow["id"]);
			string text = iRow["event_name"].ToString();
			EitInfo eitInfo = new EitInfo(@int, text);
			eitInfo.EventId = Tool.GetInt32(iRow["event_id"]);
			eitInfo.EventName = text;
			eitInfo.EventRemark = iRow["event_remark"].ToString();
			eitInfo.EventLongRemark = iRow["event_long_remark"].ToString();
			eitInfo.ProgramTableId = Tool.GetInt64(iRow["program_table_id"]);
			eitInfo.StartTime = Tool.GetDateTime(iRow["start_time"]);
			eitInfo.EndTime = Tool.GetDateTime(iRow["end_time"]);
			eitInfo.EitRateValue = getRate(iRow["event_rate"]);
			EitInfo eitInfo2 = eitInfo;
			eitInfo2.Node = eitInfo2;
			if (toLocalTime)
			{
				eitInfo2.StartTime = eitInfo2.StartTime.ToLocalTime();
				eitInfo2.EndTime = eitInfo2.EndTime.ToLocalTime();
			}
			eitInfo2.ReadProgram += eit_ReadProgram;
			eitInfo2.ReadEitRateSetting += eit_ReadEITRateSetting;
			return eitInfo2;
		}

		private int? getRate(object arg)
		{
			if (arg == null)
			{
				return null;
			}
			return Tool.GetInt32(arg);
		}

		public EitInfo GetbyID(long Identity)
		{
			EitInfo result = null;
			string cmdText = "select * from eit_info where id = @id ";
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@id", Identity) };
			try
			{
				using (IDataReader dataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters))
				{
					if (dataReader.Read())
					{
						result = EITSqlDataReader(dataReader, true);
					}
				}
			}
			catch (Exception ex)
			{
				throw new CustomException(ResourceManagement.GetLocationString("ReadDataError"), ex, MessageType.SHOWANDLOG);
			}
			return result;
		}

		public IList<EitInfo> GetByProgramIdentity(long ProgramIdentity)
		{
			IList<EitInfo> list = new List<EitInfo>();
			string cmdText = "select * from eit_info where program_table_id = @program_table_id ORDER BY start_time ";
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@program_table_id", ProgramIdentity) };
			SqlDataReader sqlDataReader = null;
			try
			{
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
				while (sqlDataReader.Read())
				{
					list.Add(EITSqlDataReader(sqlDataReader, true));
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

		private void eit_ReadProgram(object sender)
		{
			EitInfo eitInfo = sender as EitInfo;
			eitInfo.Program = new ProgramInfoDAL().GetByIdentity(eitInfo.ProgramTableId);
		}

		private void eit_ReadEITRateSetting(object sender)
		{
			EitInfo eitInfo = sender as EitInfo;
			if (eitInfo.EitRateValue.HasValue)
			{
				eitInfo.EITRateSetting = new SystemSettingDAL().GetEitRateSettingByValue(eitInfo.EitRateValue.Value);
			}
		}

		public IList<EitInfo> GetByProgramIdentity(long programIdentity, int pageIndex, int pageSize, out int totalSize)
		{
			IList<EitInfo> list = new List<EitInfo>();
			totalSize = 0;
			SqlParameter[] array = new SqlParameter[8]
			{
				SqlHelper.AddParameter("@PKName", "id"),
				SqlHelper.AddParameter("@ClolumnNames", "*"),
				SqlHelper.AddParameter("@tblName", "eit_info"),
				SqlHelper.AddParameter("@Condiction", string.Format(" and program_table_id = {0} ", programIdentity)),
				SqlHelper.AddParameter("@OrderBy", "start_time"),
				SqlHelper.AddParameter("@PageIndex", pageIndex),
				SqlHelper.AddParameter("@PageSize", pageSize),
				SqlHelper.AddParameter("@TotalCount", totalSize, ParameterDirection.Output, SqlDbType.Int)
			};
			DataSet dataSet = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.StoredProcedure, "sp_PagingSearch", array);
			totalSize = (int)array[7].Value;
			foreach (DataRow row in dataSet.Tables[0].Rows)
			{
				list.Add(EITDataRowReader(row, true));
			}
			return list;
		}

		public EitInfo GetByEITId(long ProgramIdentity, int EITId)
		{
			string cmdText = "select * from eit_info where program_table_id = @program_table_id and event_id = @event_id ";
			SqlParameter[] commandParameters = new SqlParameter[2]
			{
				SqlHelper.AddParameter("@program_table_id", ProgramIdentity),
				SqlHelper.AddParameter("@event_id", EITId)
			};
			SqlDataReader sqlDataReader = null;
			try
			{
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
				if (sqlDataReader.Read())
				{
					return EITSqlDataReader(sqlDataReader, true);
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

		public int GetEITIdFromEITInfo(ProgramInfo Program)
		{
			int num = 0;
			ushort num2 = (ushort)(Program.PrgManualeventid + 1);
			if (GetByEITId(Program.Identity, num2) == null)
			{
				return num2;
			}
			while (num < 2)
			{
				num2++;
				if (num2 == ushort.MaxValue)
				{
					num++;
				}
				if (GetByEITId(Program.Identity, num2) == null)
				{
					return num2;
				}
			}
			throw new CustomException(ResourceManagement.GetLocationString("NoAvailableEventIDInTheProgram"), MessageType.SHOW);
		}

		private int CanSaveWithTranscation(EitInfo EIT)
		{
			string cmdText = "select count(1) from eit_info where program_table_id = @program_table_id and event_id = @event_id and id <> @id ";
			SqlParameter[] commandParameters = new SqlParameter[3]
			{
				SqlHelper.AddParameter("@program_table_id", EIT.ProgramTableId),
				SqlHelper.AddParameter("@event_id", EIT.EventId),
				SqlHelper.AddParameter("@id", EIT.Identity)
			};
			string cmdText2 = "SELECT COUNT(1) FROM eit_info a WHERE a.program_table_id = @program_table_id AND a.id <> @id AND  ( (a.start_time <= @start_time and a.end_time > @start_time) OR (a.start_time < @end_time and a.end_time >= @end_time) OR (@start_time <= a.start_time and @end_time > a.start_time) OR (@start_time < a.end_time and @end_time >= a.end_time) )";
			SqlParameter[] commandParameters2 = new SqlParameter[4]
			{
				SqlHelper.AddParameter("@program_table_id", EIT.ProgramTableId),
				SqlHelper.AddParameter("@id", EIT.Identity),
				SqlHelper.AddParameter("@start_time", EIT.StartTime.ToUniversalTime()),
				SqlHelper.AddParameter("@end_time", EIT.EndTime.ToUniversalTime())
			};
			int num = (int)SqlHelper.ExecuteScalar(CommandType.Text, cmdText, commandParameters);
			if (num > 0)
			{
				return 1;
			}
			int num2 = (int)SqlHelper.ExecuteScalar(CommandType.Text, cmdText2, commandParameters2);
			if (num2 > 0)
			{
				return 2;
			}
			return 0;
		}

		internal int SaveWithTranscation(EitInfo EIT, bool updateDescriptor, bool sendNotice)
		{
			while (true)
			{
				bool flag = true;
				switch (CanSaveWithTranscation(EIT))
				{
				case 1:
					break;
				default:
				{
					DateTime now = DateTime.Now;
					string logFileName = CommTool.GetLogFileName(EIT.Program.EITFileName, false);
					DirectoryInfo directoryInfo = new DirectoryInfo(CommTool.GetFaildedReportFolder());
					if (!directoryInfo.Exists)
					{
						directoryInfo.Create();
					}
					string msg = string.Format("#Error: {3} --  EIT Name:[{0}] Start Time [{1}] End Time [{2}]", EIT.EventName, EIT.StartTime, EIT.EndTime, ResourceManagement.GetLocationString("TimeDurationOverlaps"));
					TextHelper.WriteTXTLog(msg);
					TextHelper.WriteTXTLog(msg, directoryInfo.FullName, logFileName);
					throw new CustomException(string.Format("{0}:{1}", ResourceManagement.GetLocationString("TimeDurationOverlaps"), EIT.ToString()), MessageType.SHOWANDLOG);
				}
				case 0:
				{
					long num = 0L;
					if (EIT.Identity != 0)
					{
						string cmdText = "UPDATE [eit_info] SET [event_id] = @event_id ,[event_name] = @event_name ,[event_remark] = @event_remark ,[event_long_remark] = @event_long_remark ,[program_table_id] = @program_table_id ,[start_time] = @start_time ,[end_time] = @end_time ,[event_rate] = @event_rate WHERE id = @id ";
						SqlParameter[] commandParameters = new SqlParameter[9]
						{
							SqlHelper.AddParameter("@event_id", EIT.EventId),
							SqlHelper.AddParameter("@event_name", EIT.EventName),
							SqlHelper.AddParameter("@event_remark", EIT.EventRemark),
							SqlHelper.AddParameter("@event_long_remark", EIT.EventLongRemark),
							SqlHelper.AddParameter("@program_table_id", EIT.ProgramTableId),
							SqlHelper.AddParameter("@start_time", EIT.StartTime.ToUniversalTime()),
							SqlHelper.AddParameter("@end_time", EIT.EndTime.ToUniversalTime()),
							SqlHelper.AddParameter("@event_rate", EIT.EitRateValue),
							SqlHelper.AddParameter("@id", EIT.Identity)
						};
						SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
						num = EIT.Identity;
					}
					else
					{
						string cmdText = "INSERT INTO [eit_info] ([event_id] ,[event_name] ,[event_remark] ,[event_long_remark] ,[program_table_id] ,[start_time] ,[end_time] ,[event_rate]) VALUES (@event_id ,@event_name ,@event_remark ,@event_long_remark ,@program_table_id ,@start_time ,@end_time ,@event_rate) SELECT @@IDENTITY ";
						SqlParameter[] commandParameters = new SqlParameter[8]
						{
							SqlHelper.AddParameter("@event_id", EIT.EventId),
							SqlHelper.AddParameter("@event_name", EIT.EventName),
							SqlHelper.AddParameter("@event_remark", EIT.EventRemark),
							SqlHelper.AddParameter("@event_long_remark", EIT.EventLongRemark),
							SqlHelper.AddParameter("@program_table_id", EIT.ProgramTableId),
							SqlHelper.AddParameter("@start_time", EIT.StartTime.ToUniversalTime()),
							SqlHelper.AddParameter("@end_time", EIT.EndTime.ToUniversalTime()),
							SqlHelper.AddParameter("@event_rate", EIT.EitRateValue)
						};
						using (SqlDataReader sqlDataReader = SqlHelper.ExecuteReader(CommandType.Text, cmdText, commandParameters))
						{
							if (sqlDataReader.Read())
							{
								num = Tool.GetInt64(sqlDataReader[0]);
							}
						}
					}
					if (sendNotice)
					{
						SendNotices sendNotices = new SendNotices();
						sendNotices.Code = 10;
						sendNotices.Location = "eit_info";
						sendNotices.Operation = ((EIT.Identity <= 0) ? 1 : 2);
						sendNotices.TableIdentity = num;
						SendNotices notice = sendNotices;
						noticeDAL.SaveWithTranscation(notice);
					}
					if (updateDescriptor)
					{
						DescriptorService descriptorService = new DescriptorService();
						try
						{
							if (!descriptorService.AddEitExtendDescriptor(num, EIT.Program.TS.NIT.LanguageCode, EIT.EventName, EIT.EventRemark, EIT.EventLongRemark))
							{
								throw new Exception();
							}
							if (EIT.EitRateValue.HasValue)
							{
								if (string.IsNullOrEmpty(CountryCode))
								{
									SystemSetting systemSetting = new SystemSettingDAL().GetSystemSetting(SystemSettingCode.CountryCode);
									if (systemSetting == null || string.IsNullOrEmpty(systemSetting.Value))
									{
										throw new Exception(ResourceSpecial.PlsCfgCountryCodeFirst);
									}
									CountryCode = systemSetting.Value;
								}
								if (!descriptorService.AddEitParentalRatingDescriptor(num, CountryCode, (byte)EIT.EitRateValue.Value))
								{
									throw new Exception();
								}
							}
						}
						catch (Exception innerException)
						{
							throw new Exception(ResourceManagement.GetLocationString("SaveDescriptorError"), innerException);
						}
					}
					return EIT.EventId;
				}
				}
				EIT.EventId += 1;
			}
		}

		public int Save(IList<EitInfo> EITList, bool AllowIgnore)
		{
			try
			{
				bool flag = false;
				int num = 0;
				int num2 = 0;
				int prgManualeventid = EITList.First().Program.PrgManualeventid;
				int num3 = prgManualeventid;
				foreach (EitInfo EIT in EITList)
				{
					if (EIT.EventId < 0)
					{
						EIT.EventId = (ushort)(num3 + 1);
					}
					try
					{
						num3 = SaveWithTranscation(EIT, true, true);
						flag = true;
						num2++;
					}
					catch (CustomException ex)
					{
						if (!flag && AllowIgnore)
						{
							num++;
							continue;
						}
						throw ex;
					}
				}
				if (num > 0)
				{
					Tool.WriteTXTLog("#" + string.Format(ResourceManagement.GetLocationString("ImportEITIgnore"), EITList.First().Program.EITFileName, num));
				}
				if (prgManualeventid != num3)
				{
					ProgramInfo program = EITList.Last().Program;
					program.PrgManualeventid = num3;
					program.EITFileName = program.EITFileName.Split('\\').Last();
					int num4 = new ProgramInfoDAL().SaveWithTranscation(program);
				}
				SqlHelper.SaveChange();
				return EITList.Count - num;
			}
			catch (Exception ex2)
			{
				SqlHelper.CancelChange();
				Exception ex3 = ((ex2 is CustomException) ? ex2 : new CustomException(ResourceManagement.GetLocationString("ApplicationProcessError"), ex2, MessageType.SHOWANDLOG));
				throw ex3;
			}
		}

		public int DeleteByNITWithTranscation(long NITIdentity)
		{
			string cmdText = "DELETE FROM eit_info FROM eit_info e, program_info p, ts_info t, nit_info n WHERE n.id = @id AND n.id = t.nit_table_id AND t.id = p.ts_table_id AND p.id = e.program_table_id";
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@id", NITIdentity) };
			return SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
		}

		public int DeleteByTSWithTranscation(long TSIdentity)
		{
			string cmdText = "DELETE FROM eit_info FROM eit_info e, program_info p, ts_info t WHERE t.id = @id AND t.id = p.ts_table_id AND p.id = e.program_table_id";
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@id", TSIdentity) };
			return SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
		}

		public int DeleteByPrgWithTranscation(long PrgIdentity)
		{
			string cmdText = "DELETE FROM eit_info FROM eit_info e, program_info p WHERE p.id = @id AND p.id = e.program_table_id";
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@id", PrgIdentity) };
			return SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
		}

		public void Delete(IList<long> ListIdentity)
		{
			try
			{
				foreach (long item in ListIdentity)
				{
					SendNotices sendNotices = new SendNotices();
					sendNotices.Code = 10;
					sendNotices.Location = "eit_info";
					sendNotices.Operation = 3;
					sendNotices.TableIdentity = item;
					SendNotices notice = sendNotices;
					noticeDAL.SaveWithTranscation(notice);
					new DescriptorService().DeleteEITDescriptorByEventID(item);
					string cmdText = "DELETE FROM [eit_info] WHERE id = @id";
					SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@id", item) };
					SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
				}
				SqlHelper.SaveChange();
			}
			catch (Exception ex)
			{
				SqlHelper.CancelChange();
				throw new CustomException(ResourceManagement.GetLocationString("DeleteDataError"), ex, MessageType.SHOWANDLOG);
			}
		}

		public void DeleteOutofdate(DateTime utcNowTime)
		{
			try
			{
				string cmdText = "DELETE FROM eit_des_values WHERE eit_table_id IN (SELECT ID FROM eit_info WHERE end_time < @end_time)";
				string cmdText2 = "DELETE FROM eit_info WHERE end_time < @end_time";
				SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@end_time", utcNowTime) };
				SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
				SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText2, commandParameters);
				SqlHelper.SaveChange();
			}
			catch (Exception ex)
			{
				SqlHelper.CancelChange();
				throw new CustomException(ResourceManagement.GetLocationString("DeleteDataError"), ex, MessageType.SHOWANDLOG);
			}
		}

		public void DeleteByProgramTableId(long prgtableid, DateTime utcNowTime)
		{
			try
			{
				List<SqlParameter> list = new List<SqlParameter>();
				list.Add(SqlHelper.AddParameter("@end_time", utcNowTime));
				StringBuilder stringBuilder = new StringBuilder("DELETE FROM eit_des_values WHERE eit_table_id IN (SELECT ID FROM eit_info WHERE 1 = 1 ");
				StringBuilder stringBuilder2 = new StringBuilder("DELETE FROM eit_info WHERE end_time >= @end_time ");
				if (prgtableid > 0)
				{
					stringBuilder.Append(" AND program_table_id = @program_table_id ");
					stringBuilder2.Append(" AND program_table_id = @program_table_id ");
					list.Add(SqlHelper.AddParameter("@program_table_id", prgtableid));
				}
				stringBuilder.Append(" AND end_time >= @end_time) ");
				SqlHelper.ExecuteNonQuery(CommandType.Text, stringBuilder.ToString(), list.ToArray());
				SqlHelper.ExecuteNonQuery(CommandType.Text, stringBuilder2.ToString(), list.ToArray());
				SqlHelper.SaveChange();
			}
			catch (Exception ex)
			{
				SqlHelper.CancelChange();
				throw new CustomException(ResourceManagement.GetLocationString("DeleteDataError"), ex, MessageType.SHOWANDLOG);
			}
		}

		public void DeleteAll()
		{
			try
			{
				SendNotices sendNotices = new SendNotices();
				sendNotices.Code = 10;
				sendNotices.Location = "eit_info";
				sendNotices.Operation = 3;
				sendNotices.TableIdentity = 0L;
				SendNotices notice = sendNotices;
				noticeDAL.SaveWithTranscation(notice);
				string cmdText = "DELETE FROM eit_info";
				SqlParameter[] commandParameters = new SqlParameter[0];
				SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
				new DescriptorService().ClearAllEitDescriptors();
				SqlHelper.SaveChange();
			}
			catch (Exception ex)
			{
				SqlHelper.CancelChange();
				throw new CustomException(ResourceManagement.GetLocationString("DeleteDataError"), ex, MessageType.SHOWANDLOG);
			}
		}

		public IList<EitInfo> GetOutputEIT(DateTime utcNowTime, int days)
		{
			IList<EitInfo> list = new List<EitInfo>();
			string cmdText = "SELECT * FROM eit_info e WHERE e.start_time < DATEADD(DAY, @days, @uctNow) AND EXISTS  ( \tSELECT 1 FROM program_info p, PAT pat \tWHERE pat.program_table_id = p.id \tAND p.id = e.program_table_id ) ORDER BY e.start_time ";
			SqlParameter[] commandParameters = new SqlParameter[2]
			{
				SqlHelper.AddParameter("@days", days),
				SqlHelper.AddParameter("@uctNow", utcNowTime.Date)
			};
			using (SqlDataReader sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters))
			{
				while (sqlDataReader.Read())
				{
					list.Add(EITSqlDataReader(sqlDataReader, false));
				}
			}
			return list;
		}
	}
}
