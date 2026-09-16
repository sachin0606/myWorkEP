using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.IDAL;
using DeXin.EPG.Model;
using DeXin.EPG.Tools;

namespace DeXin.EPG.DAL
{
	public class DescriptorTypeService : IDescriptorType
	{
		public IList<DescriptorType> GetDescriptorTypesByLoaction(string location)
		{
			IList<DescriptorType> list = new List<DescriptorType>();
			SqlDataReader sqlDataReader = null;
			try
			{
				string cmdText = "SELECT [id] ,[des_name] ,[des_tag] ,[des_location] ,[des_type],[visible] FROM [dbo].[descriptor_type] WHERE des_location = @des_location";
				SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@des_location", location) };
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
				while (sqlDataReader.Read())
				{
					list.Add(ReaderToEntity(sqlDataReader));
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

		public DescriptorType GetDescriptorTypeByID(long args)
		{
			DescriptorType result = null;
			SqlDataReader sqlDataReader = null;
			string cmdText = "SELECT [id] ,[des_name] ,[des_tag] ,[des_location] ,[des_type],[visible] FROM [dbo].[descriptor_type] WHERE id = @id";
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@id", args, SqlDbType.BigInt) };
			try
			{
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
				if (sqlDataReader.Read())
				{
					result = ReaderToEntity(sqlDataReader);
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
			return result;
		}

		public DescriptorType GetDescriptorTypeByLocationAndTag(EnumPossibleLoaction loaction, EnumDescriptorTag tag)
		{
			DescriptorType result = null;
			SqlDataReader sqlDataReader = null;
			string cmdText = "SELECT [id] ,[des_name] ,[des_tag] ,[des_location] ,[des_type],[visible] FROM [dbo].[descriptor_type] WHERE des_location = @des_location AND des_tag = @des_tag";
			SqlParameter[] commandParameters = new SqlParameter[2]
			{
				SqlHelper.AddParameter("@des_location", loaction.ToString(), SqlDbType.NVarChar),
				SqlHelper.AddParameter("@des_tag", (int)tag, SqlDbType.Int)
			};
			try
			{
				sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
				if (sqlDataReader.Read())
				{
					result = ReaderToEntity(sqlDataReader);
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
			return result;
		}

		public bool AddDescriptorType(DescriptorType args)
		{
			bool flag = false;
			string cmdText = "IF((SELECT COUNT(*) FROM descriptor_type WHERE des_location=@des_location AND des_tag = @des_tag) = 0)\r\n                            BEGIN\r\n                            INSERT descriptor_type\r\n                                    ( des_name ,\r\n                                      des_tag ,\r\n                                      des_location ,\r\n                                      des_type\r\n                                    )\r\n                            VALUES  ( @des_name ,\r\n                                      @des_tag , \r\n                                      @des_location , \r\n                                      @des_type  \r\n                                    )\r\n                            END";
			SqlParameter[] commandParameters = new SqlParameter[4]
			{
				SqlHelper.AddParameter("@des_name", args.DesName, SqlDbType.NVarChar, 50),
				SqlHelper.AddParameter("@des_tag", args.DesTag, SqlDbType.Int),
				SqlHelper.AddParameter("@des_location", args.DesLocation, SqlDbType.NVarChar, 50),
				SqlHelper.AddParameter("@des_type", args.DesType, SqlDbType.Int)
			};
			return SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters) > 0;
		}

		public bool ModifyDescriptorType(DescriptorType args)
		{
			bool flag = false;
			string cmdText = "UPDATE  descriptor_type \r\n                                    SET\t\tdes_name = @des_name,\r\n                                            des_tag = @des_tag,\r\n                                            des_location = @des_location,\r\n                                            des_type = @des_type\r\n                                    WHERE id = @id";
			SqlParameter[] commandParameters = new SqlParameter[5]
			{
				SqlHelper.AddParameter("@des_name", args.DesName, SqlDbType.NVarChar, 50),
				SqlHelper.AddParameter("@des_tag", args.DesTag, SqlDbType.Int),
				SqlHelper.AddParameter("@des_location", args.DesLocation, SqlDbType.NVarChar, 50),
				SqlHelper.AddParameter("@des_type", args.DesType, SqlDbType.Int),
				SqlHelper.AddParameter("@id", args.Id, SqlDbType.BigInt)
			};
			return SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters) > 0;
		}

		public bool DeleteDescriptorType(params long[] args)
		{
			bool flag = false;
			StringBuilder stringBuilder = new StringBuilder();
			foreach (long num in args)
			{
				stringBuilder.Append(",");
				stringBuilder.Append(num.ToString());
			}
			string cmdText = "DELETE FROM  descriptor_type WHERE id in (" + stringBuilder.ToString().Substring(1) + ")";
			return SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, cmdText, (SqlParameter[])null) > 0;
		}

		private DescriptorType ReaderToEntity(IDataReader reader)
		{
			long @int = Tool.GetInt64(reader["id"]);
			string tyName = Tool.GetString(reader["des_name"]);
			bool boolean = Tool.GetBoolean(reader["visible"]);
			DescriptorType descriptorType = new DescriptorType(@int, tyName, boolean);
			descriptorType.DesTag = Tool.GetInt32(reader["des_tag"]);
			descriptorType.DesLocation = Tool.GetString(reader["des_location"]);
			descriptorType.DesType = Tool.GetInt32(reader["des_type"]);
			return descriptorType;
		}
	}
}
