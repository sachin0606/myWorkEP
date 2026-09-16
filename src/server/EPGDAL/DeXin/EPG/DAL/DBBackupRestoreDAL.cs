using System;
using System.Data;
using System.Data.SqlClient;
using Com.Cn.Tool.Model;
using DeXin.EPG.IDAL;

namespace DeXin.EPG.DAL
{
	public class DBBackupRestoreDAL : IDBBackupRestoreDAL
	{
		public bool BackupDB(string path)
		{
			bool result = true;
			try
			{
				SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(SqlHelper.ConnectionString);
				SqlConnectionStringBuilder sqlConnectionStringBuilder2 = new SqlConnectionStringBuilder(SqlHelper.ConnectionString);
				sqlConnectionStringBuilder2.InitialCatalog = "master";
				string initialCatalog = sqlConnectionStringBuilder.InitialCatalog;
				string cmdText = " backup database " + initialCatalog + "  to disk=@path ";
				SqlParameter[] commandParameters = new SqlParameter[1] { SqlHelper.AddParameter("@path", path, SqlDbType.NVarChar, 128) };
				SqlHelper.ExecuteNonQuery(sqlConnectionStringBuilder2.ConnectionString, CommandType.Text, cmdText, commandParameters);
			}
			catch (Exception ex)
			{
				result = false;
				throw ex;
			}
			return result;
		}

		public bool RestoreDB(string path)
		{
			bool result = true;
			SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(SqlHelper.ConnectionString);
			SqlConnectionStringBuilder sqlConnectionStringBuilder2 = new SqlConnectionStringBuilder(SqlHelper.ConnectionString);
			sqlConnectionStringBuilder2.InitialCatalog = "master";
			string initialCatalog = sqlConnectionStringBuilder.InitialCatalog;
			using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStringBuilder2.ConnectionString))
			{
				sqlConnection.Open();
				try
				{
					SqlCommand sqlCommand = sqlConnection.CreateCommand();
					sqlCommand.CommandType = CommandType.Text;
					string commandText = "\r\n                            --循环kill数据库联接\r\n                            use master\r\n                            declare @sql  nvarchar(100)\r\n                            declare @SPID nvarchar(50)\r\n                            declare Mycursor cursor\r\n                            for select spid from master..sysprocesses  where dbid=db_id(@dbName) \r\n                            open Mycursor \r\n                            fetch NEXT from Mycursor into @SPID\r\n                            while @@FETCH_STATUS = 0\r\n                            begin\r\n\t                            --print @SPID\r\n\t                            set @sql = ' kill '+ @SPID\r\n\t                            exec(@sql)\r\n\t                            fetch NEXT from Mycursor into @SPID\r\n                            end\r\n                            close Mycursor\r\n                            deallocate Mycursor\r\n                            --还原数据库\r\n                            restore database @dbName   from disk=@path With REPLACE\r\n                        ";
					sqlCommand.CommandText = commandText;
					SqlParameter[] values = new SqlParameter[2]
					{
						SqlHelper.AddParameter("@dbName", initialCatalog),
						SqlHelper.AddParameter("@path", path)
					};
					sqlCommand.Parameters.AddRange(values);
					sqlCommand.ExecuteNonQuery();
				}
				catch (Exception ex)
				{
					result = false;
					throw ex;
				}
			}
			return result;
		}
	}
}
