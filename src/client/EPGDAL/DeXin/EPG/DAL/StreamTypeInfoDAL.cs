using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.IDAL;
using DeXin.EPG.Model;

namespace DeXin.EPG.DAL
{
	public class StreamTypeInfoDAL : IStreamTypeInfoDAL
	{
		public StreamTypeInfo GetByIdentity(long Identity)
		{
			string cmdText = "SELECT * FROM [stream_type_info] WHERE id = @id";
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@id", Identity) };
			SqlDataReader sqlDataReader = null;
			try
			{
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
				if (sqlDataReader.Read())
				{
					string text = sqlDataReader["stream_name"].ToString();
					StreamTypeInfo streamTypeInfo = new StreamTypeInfo(Identity, text);
					streamTypeInfo.StreamType = Tool.GetInt32(sqlDataReader["stream_type"]);
					streamTypeInfo.StreamName = text;
					StreamTypeInfo streamTypeInfo2 = streamTypeInfo;
					streamTypeInfo2.Node = streamTypeInfo2;
					return streamTypeInfo2;
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
			return null;
		}

		public StreamTypeInfo GetByStreamTypeId(int StreamTypeId)
		{
			string cmdText = "SELECT * FROM [stream_type_info] WHERE stream_type = @stream_type";
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@stream_type", StreamTypeId) };
			SqlDataReader sqlDataReader = null;
			try
			{
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
				if (sqlDataReader.Read())
				{
					long @int = Tool.GetInt64(sqlDataReader["id"]);
					string text = sqlDataReader["stream_name"].ToString();
					StreamTypeInfo streamTypeInfo = new StreamTypeInfo(@int, text);
					streamTypeInfo.StreamType = StreamTypeId;
					streamTypeInfo.StreamName = text;
					StreamTypeInfo streamTypeInfo2 = streamTypeInfo;
					streamTypeInfo2.Node = streamTypeInfo2;
					return streamTypeInfo2;
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
			return null;
		}

		public IList<StreamTypeInfo> GetAll()
		{
			IList<StreamTypeInfo> list = new List<StreamTypeInfo>();
			string cmdText = "SELECT * FROM [stream_type_info]";
			SqlDataReader sqlDataReader = null;
			try
			{
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText);
				while (sqlDataReader.Read())
				{
					long @int = Tool.GetInt64(sqlDataReader["id"]);
					string text = sqlDataReader["stream_name"].ToString();
					StreamTypeInfo streamTypeInfo = new StreamTypeInfo(@int, text);
					streamTypeInfo.StreamType = Tool.GetInt32(sqlDataReader["stream_type"]);
					streamTypeInfo.StreamName = text;
					StreamTypeInfo streamTypeInfo2 = streamTypeInfo;
					streamTypeInfo2.Node = streamTypeInfo2;
					list.Add(streamTypeInfo2);
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
	}
}
