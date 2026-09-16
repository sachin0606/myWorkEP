using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.IDAL;
using DeXin.EPG.Model;

namespace DeXin.EPG.DAL
{
	public class SystemSettingDAL : ISystemSetting
	{
		public SystemSetting GetSystemSetting(SystemSettingCode key)
		{
			string cmdText = "SELECT * FROM SystemSetting WHERE [key] = @key";
			SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@key", key.ToString()) };
			SystemSetting result = null;
			using (SqlDataReader sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters))
			{
				if (sqlDataReader.Read())
				{
					SystemSetting systemSetting = new SystemSetting(key);
					systemSetting.Value = Tool.GetString(sqlDataReader["value"]);
					systemSetting.Comment = Tool.GetString(sqlDataReader["comment"]);
					result = systemSetting;
				}
			}
			return result;
		}

		public void Save(SystemSetting item)
		{
			string cmdText = "if exists(select * from SystemSetting where [key] = @key)\r\n                                        UPDATE SystemSetting SET [value] = @value,comment = @comment WHERE [key] = @key\r\n                                    ELSE\r\n                                        INSERT INTO [SystemSetting] ([key] ,[value]  ,[comment]) VALUES (@key,@value ,@comment )\r\n                                            ";
			SqlParameter[] commandParameters = new SqlParameter[3]
			{
				SqlHelper.AddParameter("@key", item.Key.ToString()),
				SqlHelper.AddParameter("@value", item.Value.ToString()),
				SqlHelper.AddParameter("@comment", item.Comment.ToString())
			};
			SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
		}

		public List<EitRateSetting> GetEitRateSetting()
		{
			string cmdText = "SELECT * FROM eit_rate_setting";
			List<EitRateSetting> list = new List<EitRateSetting>();
			using (SqlDataReader sqlDataReader = SqlHelper.ExecuteReader(CommandType.Text, cmdText, (SqlParameter[])null))
			{
				while (sqlDataReader.Read())
				{
					list.Add(new EitRateSetting
					{
						RateCode = Tool.GetString(sqlDataReader["rate_code"]),
						RateValue = Tool.GetInt32(sqlDataReader["rate_value"]),
						RateName = Tool.GetString(sqlDataReader["rate_name"])
					});
				}
			}
			return list;
		}

		internal EitRateSetting GetEitRateSettingByValue(int ragevalue)
		{
			return GetEitRateSetting().FirstOrDefault((EitRateSetting p) => p.RateValue == ragevalue);
		}

		public void SaveOrUpdate(List<EitRateSetting> items)
		{
			try
			{
				string cmdText = "DELETE FROM [eit_rate_setting]";
				SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText, (SqlParameter[])null);
				string cmdText2 = " INSERT INTO dbo.eit_rate_setting ( rate_code, rate_name, rate_value )\r\n                                              VALUES  (@rate_code, @rate_name, @rate_value)";
				foreach (EitRateSetting item in items)
				{
					SqlHelper.ExecuteNonQuery(CommandType.Text, cmdText2, SqlHelper.AddParameter("@rate_code", item.RateCode), SqlHelper.AddParameter("@rate_name", item.RateName), SqlHelper.AddParameter("@rate_value", item.RateValue));
				}
				SqlHelper.SaveChange();
			}
			catch (Exception ex)
			{
				SqlHelper.CancelChange();
				throw ex;
			}
		}
	}
}
