using System;
using System.Data;
using System.Data.SqlClient;
using Util.Common.ExtensionHelper;

namespace Util.Common.DBConnectionHelpers
{
	public class SqlConnectionPool : ConnectionPool<SqlConnection>
	{
		public static string getKillOtherConnectionString(string dbname)
		{
			string text = dbname;
			if (dbname.ToUpper().StartsWith("DBO."))
			{
				text = dbname.Substring(4, dbname.Length - 4);
			}
			return "\r\n            DECLARE @sql NVARCHAR(max)\r\n            DECLARE @SPID NVARCHAR(max)\r\n            DECLARE Mycursor CURSOR\r\n            FOR SELECT spid FROM master..sysprocesses  WHERE dbid=DB_ID('{0}') \r\n            OPEN Mycursor \r\n            FETCH NEXT FROM Mycursor INTO @SPID\r\n            WHILE @@FETCH_STATUS = 0 \r\n                BEGIN\r\n                    SET @sql = ' kill ' + @SPID\r\n                    EXEC(@sql)\r\n                    FETCH NEXT FROM Mycursor INTO @SPID\r\n                END\r\n            CLOSE Mycursor\r\n            DEALLOCATE Mycursor\r\n            ".Fmt(text);
		}

		public static bool CheckDBIsExist(string dbname)
		{
			string cmdText = "SELECT DB_ID('{0}') AS ID".Fmt(dbname);
			object obj = ExecuteScalar(CommandType.Text, cmdText, (SqlParameter[])null);
			long num = obj.ToInt64(0L);
			return num > 0;
		}

		public static void CreateDataBase(string dbname)
		{
			string cmdText = getKillOtherConnectionString(dbname) + " \r\n            \r\n            IF ((SELECT DB_ID('{0}') AS ID)>0) \r\n            BEGIN\r\n                DROP DATABASE {0}\r\n            END\r\n            CREATE DATABASE {0};\r\n                                                ".Fmt(dbname);
			ExecuteNonQuery(CommandType.Text, cmdText, (SqlParameter[])null);
		}

		public static void Restore(string dbname, string fullpath)
		{
			string cmdText = getKillOtherConnectionString(dbname) + " \r\nRESTORE DATABASE {0} FROM DISK = '{1}' WITH REPLACE ".Fmt(dbname, fullpath);
			ExecuteNonQuery(CommandType.Text, cmdText, (SqlParameter[])null);
		}

		public static void Backup(string dbname, string fullpath)
		{
			string cmdText = " BACKUP DATABASE {0} TO DISK = '{1}' ".Fmt(dbname, fullpath);
			ExecuteNonQuery(CommandType.Text, cmdText, (SqlParameter[])null);
		}

		public static SqlDataReader ExecuteReader(CommandType cmdType, string cmdText, params SqlParameter[] parameter)
		{
			SqlCommand sqlCommand = new SqlCommand();
			ConnectionPool<SqlConnection>.PrepareCommand(sqlCommand, cmdType, cmdText, parameter);
			SqlDataReader result = ((ConnectionPool<SqlConnection>.CurrentTransaction == null) ? sqlCommand.ExecuteReader(CommandBehavior.CloseConnection) : sqlCommand.ExecuteReader());
			sqlCommand.Parameters.Clear();
			return result;
		}

		public static int ExecuteNonQuery(CommandType cmdType, string cmdText, params SqlParameter[] parameter)
		{
			SqlCommand sqlCommand = new SqlCommand();
			ConnectionPool<SqlConnection>.PrepareCommand(sqlCommand, cmdType, cmdText, parameter);
			int result = sqlCommand.ExecuteNonQuery();
			sqlCommand.Parameters.Clear();
			return result;
		}

		public static object ExecuteScalar(CommandType cmdType, string cmdText, params SqlParameter[] parameter)
		{
			SqlCommand sqlCommand = new SqlCommand();
			ConnectionPool<SqlConnection>.PrepareCommand(sqlCommand, cmdType, cmdText, parameter);
			object result = sqlCommand.ExecuteScalar();
			sqlCommand.Parameters.Clear();
			return result;
		}

		public static SqlDataReader ExecuteReader(PageQuery<SqlParameter> query)
		{
			object obj = ExecuteScalar(CommandType.Text, string.Format("SELECT COUNT(1) FROM {0} WHERE 1 = 1 {1}", query.TableName, query.Condiction), query.CmdParameters.ToArray());
			query.TotalCount = ((obj != null) ? Convert.ToInt32(obj) : 0);
			return ExecuteReader(CommandType.Text, query.GetSql(), query.GetParameters().ToArray());
		}
	}
}
