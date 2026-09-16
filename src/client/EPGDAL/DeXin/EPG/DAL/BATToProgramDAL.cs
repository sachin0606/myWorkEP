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
	public class BATToProgramDAL : IBATToProgramDAL
	{
		public BATToProgram GetByBAT(long BATIdtntity)
		{
			BATToProgram bATToProgram = new BATToProgram();
			bATToProgram.BAT = new BATInfoDAL().GetByIdentity(BATIdtntity);
			bATToProgram.ProgramList = new List<ProgramInfo>();
			string cmdText = "SELECT * FROM [bat_to_program] WHERE bat_table_id = @bat_table_id";
			SqlDataReader sqlDataReader = null;
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@bat_table_id", BATIdtntity) };
			try
			{
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
				while (sqlDataReader.Read())
				{
					long identity = Tool.GetInt32(sqlDataReader["prg_table_id"]);
					ProgramInfo byIdentity = new ProgramInfoDAL().GetByIdentity(identity);
					if (byIdentity != null)
					{
						bATToProgram.ProgramList.Add(byIdentity);
					}
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
			return bATToProgram;
		}

		public BATToProgram GetByProgram(long ProgramIdentity)
		{
			BATToProgram bATToProgram = new BATToProgram();
			bATToProgram.ProgramList = new List<ProgramInfo> { new ProgramInfoDAL().GetByIdentity(ProgramIdentity) };
			string cmdText = "SELECT * FROM [bat_to_program] WHERE prg_table_id = @prg_table_id";
			SqlDataReader sqlDataReader = null;
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@prg_table_id", ProgramIdentity) };
			try
			{
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
				while (sqlDataReader.Read())
				{
					long identity = Tool.GetInt32(sqlDataReader["bat_table_id"]);
					bATToProgram.BAT = new BATInfoDAL().GetByIdentity(identity);
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
			return bATToProgram;
		}

		public int SaveWithTranscation(long BATIdentity, long ProgramIdentity)
		{
			string cmdText = "INSERT INTO bat_to_program (bat_table_id, prg_table_id) VALUES(@bat_table_id, @prg_table_id)";
			SqlParameter[] commandParameters = new SqlParameter[2]
			{
				SqlHelper.AddParameter("@bat_table_id", BATIdentity),
				SqlHelper.AddParameter("@prg_table_id", ProgramIdentity)
			};
			return SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
		}

		public int DeleteByBATIdentityWtihTranscation(long BATIdentity)
		{
			string cmdText = "DELETE FROM bat_to_program WHERE bat_table_id = @bat_table_id";
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@bat_table_id", BATIdentity) };
			return SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
		}

		public int DeleteByPrgWtihTranscation(long PrgIdentity)
		{
			string cmdText = "DELETE FROM bat_to_program WHERE prg_table_id = @prg_table_id";
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@prg_table_id", PrgIdentity) };
			return SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
		}

		public int DeleteByTSIdentityWithTranscation(long TSIdentity)
		{
			string cmdText = "DELETE FROM bat_to_program FROM bat_to_program bp, program_info p, ts_info t WHERE t.id = @id AND t.id = p.ts_table_id AND p.id = bp.prg_table_id";
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@id", TSIdentity) };
			return SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
		}

		public int DeleteByNITIdentityWithTranscation(long NITIdentity)
		{
			int num = 0;
			string cmdText = "DELETE FROM bat_to_program FROM bat_to_program bp, bat_info b, nit_info n WHERE n.id = @id AND n.id = b.nit_table_id AND b.id = bp.bat_table_id";
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@id", NITIdentity) };
			int num2 = SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
			if (num2 < 0)
			{
				throw new Exception(num2.ToString());
			}
			cmdText = "DELETE FROM bat_to_program FROM bat_to_program bp, program_info p, ts_info t, nit_info n WHERE n.id = @id AND n.id = t.nit_table_id AND t.id = p.ts_table_id AND p.id = bp.prg_table_id";
			SqlParameter[] commandParameters2 = new SqlParameter[1] { SqlHelper.AddParameter("@id", NITIdentity) };
			int num3 = SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, commandParameters2);
			if (num2 < 0)
			{
				throw new Exception(num2.ToString());
			}
			return num2 + num3;
		}
	}
}
