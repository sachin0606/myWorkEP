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
	public class MultimediaDAL : IMultimediaDAL
	{
		private SendNoticesDAL noticeDAL = new SendNoticesDAL();

		public IList<Multimedia> GetByProgramIdentity(long ProgramIdentity)
		{
			IList<Multimedia> list = new List<Multimedia>();
			string cmdText = "select * from multimedia where program_table_id = @program_table_id ";
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@program_table_id", ProgramIdentity) };
			SqlDataReader sqlDataReader = null;
			try
			{
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
				while (sqlDataReader.Read())
				{
					Multimedia multimedia = new Multimedia();
					multimedia.Id = Tool.GetInt64(sqlDataReader["id"]);
					multimedia.ProgramTableId = ProgramIdentity;
					multimedia.StreamTypeTableId = Tool.GetInt64(sqlDataReader["stream_type_table_id"]);
					multimedia.PesPid = Tool.GetInt32(sqlDataReader["pes_pid"]);
					Multimedia multimedia2 = multimedia;
					multimedia2.ReadProgram += media_ReadProgram;
					multimedia2.ReadPESType += media_ReadPESType;
					list.Add(multimedia2);
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

		private void media_ReadPESType(object sender)
		{
			Multimedia multimedia = sender as Multimedia;
			multimedia.PESType = new StreamTypeInfoDAL().GetByIdentity(multimedia.StreamTypeTableId);
		}

		private void media_ReadProgram(object sender)
		{
			Multimedia multimedia = sender as Multimedia;
			multimedia.Program = new ProgramInfoDAL().GetByIdentity(multimedia.ProgramTableId);
		}

		public bool CanSave(Multimedia Media)
		{
			string cmdText = "select count(1) from multimedia where program_table_id = @program_table_id and pes_pid = @pes_pid and id <> @id ";
			SqlParameter[] commandParameters = new SqlParameter[3]
			{
				SqlHelper.AddParameter("@program_table_id", Media.ProgramTableId),
				SqlHelper.AddParameter("@pes_pid", Media.PesPid),
				SqlHelper.AddParameter("@id", Media.Id)
			};
			int num = (int)SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
			if (num > 0)
			{
				return false;
			}
			return true;
		}

		public int Save(Multimedia Media)
		{
			if (!CanSave(Media))
			{
				throw new CustomException(ResourceManagement.GetLocationString("ElementaryPIDExists"), MessageType.SHOW);
			}
			int num = 0;
			long tableIdentity = 0L;
			try
			{
				if (Media.Id != 0)
				{
					string cmdText = "UPDATE [multimedia] SET [stream_type_table_id] = @stream_type_table_id ,[program_table_id] = @program_table_id ,[pes_pid] = @pes_pid WHERE id = @id ";
					SqlParameter[] commandParameters = new SqlParameter[4]
					{
						SqlHelper.AddParameter("stream_type_table_id", Media.StreamTypeTableId),
						SqlHelper.AddParameter("program_table_id", Media.ProgramTableId),
						SqlHelper.AddParameter("pes_pid", Media.PesPid),
						SqlHelper.AddParameter("@id", Media.Id)
					};
					num = SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
					tableIdentity = Media.Id;
				}
				else
				{
					string cmdText = "INSERT INTO [multimedia] ([stream_type_table_id] ,[program_table_id] ,[pes_pid]) VALUES (@stream_type_table_id ,@program_table_id ,@pes_pid) SELECT @@IDENTITY ";
					SqlParameter[] commandParameters = new SqlParameter[3]
					{
						SqlHelper.AddParameter("stream_type_table_id", Media.StreamTypeTableId),
						SqlHelper.AddParameter("program_table_id", Media.ProgramTableId),
						SqlHelper.AddParameter("@pes_pid", Media.PesPid)
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
				sendNotices.Code = 12;
				sendNotices.Location = "multimedia";
				sendNotices.Operation = ((Media.Id <= 0) ? 1 : 2);
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
					sendNotices.Code = 12;
					sendNotices.Location = "multimedia";
					sendNotices.Operation = 3;
					sendNotices.TableIdentity = item;
					SendNotices notice = sendNotices;
					noticeDAL.SaveWithTranscation(notice);
					new DescriptorService().DeleteDescriptorByESID(item);
					string cmdText = "DELETE FROM [multimedia] WHERE id = @id";
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

		public int DeleteByIdentityWithTranscation(long Identity)
		{
			string cmdText = "DELETE FROM [multimedia] WHERE id = @id";
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@id", Identity) };
			return SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
		}

		public int DeleteByPrgIdentityWithTransacation(long PrgIdentity)
		{
			int num = 0;
			string cmdText = "DELETE FROM multimedia FROM multimedia m, program_info p WHERE p.id = @id AND p.id = m.program_table_id";
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@id", PrgIdentity) };
			num = SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
			if (num < 0)
			{
				throw new Exception(num.ToString());
			}
			return num;
		}

		public int DeleteByTSIdentityWithTranscation(long TSIdentity)
		{
			int num = 0;
			string cmdText = "DELETE FROM multimedia FROM multimedia m, program_info p, ts_info t WHERE t.id = @id AND t.id = p.ts_table_id AND p.id = m.program_table_id";
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@id", TSIdentity) };
			num = SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
			if (num < 0)
			{
				throw new Exception(num.ToString());
			}
			return num;
		}

		public int DeltetByNITIdentityWithTranscation(long NITIdentity)
		{
			int num = 0;
			string cmdText = "DELETE FROM multimedia FROM multimedia m, program_info p, ts_info t, nit_info n WHERE n.id = @id AND n.id = t.nit_table_id AND t.id = p.ts_table_id AND p.id = m.program_table_id";
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@id", NITIdentity) };
			num = SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
			if (num < 0)
			{
				throw new Exception(num.ToString());
			}
			return num;
		}

		public IList<Multimedia> GetOutput()
		{
			IList<Multimedia> list = new List<Multimedia>();
			string cmdText = "SELECT * FROM multimedia m WHERE EXISTS ( \tSELECT 1 FROM program_info p, PAT pat \tWHERE m.program_table_id = p.id \tAND p.id=pat.program_table_id )";
			SqlDataReader sqlDataReader = null;
			try
			{
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, (SqlParameter[])null);
				while (sqlDataReader.Read())
				{
					Multimedia multimedia = new Multimedia();
					multimedia.Id = Tool.GetInt64(sqlDataReader["id"]);
					multimedia.ProgramTableId = Tool.GetInt64(sqlDataReader["program_table_id"]);
					multimedia.StreamTypeTableId = Tool.GetInt64(sqlDataReader["stream_type_table_id"]);
					multimedia.PesPid = Tool.GetInt32(sqlDataReader["pes_pid"]);
					Multimedia multimedia2 = multimedia;
					multimedia2.ReadProgram += media_ReadProgram;
					multimedia2.ReadPESType += media_ReadPESType;
					list.Add(multimedia2);
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
	}
}
