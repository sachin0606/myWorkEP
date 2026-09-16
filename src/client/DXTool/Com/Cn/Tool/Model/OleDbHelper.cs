using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;

namespace Com.Cn.Tool.Model
{
	public static class OleDbHelper
	{
		private const string ExcelDefaultVersion = "8.0";

		private const string ConnectionStringTemplate = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source ={0};Extended Properties=Excel {1}";

		public static string connectionString = "";

		private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

		public static string ConnectionString(string filePath)
		{
			return string.Format("Provider = Microsoft.Jet.OLEDB.4.0; Data Source ={0};Extended Properties=Excel {1}", filePath, "8.0");
		}

		private static void PrepareCommand(OleDbCommand cmd, OleDbConnection conn, OleDbTransaction trans, CommandType cmdType, string cmdText, OleDbParameter[] cmdParms)
		{
			if (conn.State != ConnectionState.Open)
			{
				conn.Open();
			}
			cmd.Connection = conn;
			cmd.CommandText = cmdText;
			if (trans != null)
			{
				cmd.Transaction = trans;
			}
			cmd.CommandType = cmdType;
			if (cmdParms != null)
			{
				foreach (OleDbParameter value in cmdParms)
				{
					cmd.Parameters.Add(value);
				}
			}
		}

		public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params OleDbParameter[] commandParameters)
		{
			OleDbCommand oleDbCommand = new OleDbCommand();
			using (OleDbConnection conn = new OleDbConnection(connectionString))
			{
				PrepareCommand(oleDbCommand, conn, null, cmdType, cmdText, commandParameters);
				int result = oleDbCommand.ExecuteNonQuery();
				oleDbCommand.Parameters.Clear();
				return result;
			}
		}

		public static int ExecuteNonQueryOutPut(string connectionString, CommandType cmdType, string cmdText, string outputKey, params OleDbParameter[] commandParameters)
		{
			OleDbCommand oleDbCommand = new OleDbCommand();
			using (OleDbConnection conn = new OleDbConnection(connectionString))
			{
				PrepareCommand(oleDbCommand, conn, null, cmdType, cmdText, commandParameters);
				int result = oleDbCommand.ExecuteNonQuery();
				parmCache[outputKey] = commandParameters;
				oleDbCommand.Parameters.Clear();
				return result;
			}
		}

		public static int ExecuteNonQuery(OleDbConnection connection, CommandType cmdType, string cmdText, params OleDbParameter[] commandParameters)
		{
			OleDbCommand oleDbCommand = new OleDbCommand();
			PrepareCommand(oleDbCommand, connection, null, cmdType, cmdText, commandParameters);
			int result = oleDbCommand.ExecuteNonQuery();
			oleDbCommand.Parameters.Clear();
			return result;
		}

		public static int ExecuteNonQuery(OleDbTransaction trans, CommandType cmdType, string cmdText, params OleDbParameter[] commandParameters)
		{
			OleDbCommand oleDbCommand = new OleDbCommand();
			PrepareCommand(oleDbCommand, trans.Connection, trans, cmdType, cmdText, commandParameters);
			int result = oleDbCommand.ExecuteNonQuery();
			oleDbCommand.Parameters.Clear();
			return result;
		}

		public static OleDbDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params OleDbParameter[] commandParameters)
		{
			OleDbCommand oleDbCommand = new OleDbCommand();
			OleDbConnection oleDbConnection = new OleDbConnection(connectionString);
			try
			{
				PrepareCommand(oleDbCommand, oleDbConnection, null, cmdType, cmdText, commandParameters);
				OleDbDataReader result = oleDbCommand.ExecuteReader(CommandBehavior.CloseConnection);
				oleDbCommand.Parameters.Clear();
				return result;
			}
			catch
			{
				oleDbConnection.Close();
				throw;
			}
		}

		public static OleDbDataReader ExecuteReader(OleDbTransaction sqltrans, CommandType cmdType, string cmdText, params OleDbParameter[] commandParameters)
		{
			OleDbCommand oleDbCommand = new OleDbCommand();
			try
			{
				PrepareCommand(oleDbCommand, sqltrans.Connection, sqltrans, cmdType, cmdText, commandParameters);
				OleDbDataReader result = oleDbCommand.ExecuteReader();
				oleDbCommand.Parameters.Clear();
				return result;
			}
			catch
			{
				throw;
			}
		}

		public static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params OleDbParameter[] commandParameters)
		{
			OleDbCommand oleDbCommand = new OleDbCommand();
			using (OleDbConnection conn = new OleDbConnection(connectionString))
			{
				PrepareCommand(oleDbCommand, conn, null, cmdType, cmdText, commandParameters);
				object result = oleDbCommand.ExecuteScalar();
				oleDbCommand.Parameters.Clear();
				return result;
			}
		}

		public static object ExecuteScalar(OleDbConnection connection, CommandType cmdType, string cmdText, params OleDbParameter[] commandParameters)
		{
			OleDbCommand oleDbCommand = new OleDbCommand();
			PrepareCommand(oleDbCommand, connection, null, cmdType, cmdText, commandParameters);
			object result = oleDbCommand.ExecuteScalar();
			oleDbCommand.Parameters.Clear();
			return result;
		}

		public static object ExecuteScalar(OleDbTransaction trans, CommandType cmdType, string cmdText, params OleDbParameter[] commandParameters)
		{
			OleDbCommand oleDbCommand = new OleDbCommand();
			PrepareCommand(oleDbCommand, trans.Connection, trans, cmdType, cmdText, commandParameters);
			object result = oleDbCommand.ExecuteScalar();
			oleDbCommand.Parameters.Clear();
			return result;
		}

		public static void CacheParameters(string cacheKey, params OleDbParameter[] commandParameters)
		{
			parmCache[cacheKey] = commandParameters;
		}

		public static OleDbParameter[] GetCachedParameters(string cacheKey)
		{
			OleDbParameter[] array = (OleDbParameter[])parmCache[cacheKey];
			if (array == null)
			{
				return null;
			}
			OleDbParameter[] array2 = new OleDbParameter[array.Length];
			int i = 0;
			for (int num = array.Length; i < num; i++)
			{
				array2[i] = (OleDbParameter)((ICloneable)array[i]).Clone();
			}
			return array2;
		}

		public static void CloseAll(OleDbConnection conn, OleDbDataReader reader)
		{
			if (reader != null)
			{
				reader.Close();
			}
			if (conn != null && conn.State != ConnectionState.Closed)
			{
				conn.Close();
			}
		}

		public static OleDbParameter AddParameter(string paraName, object value)
		{
			if (value == null)
			{
				value = DBNull.Value;
			}
			return new OleDbParameter(paraName, value);
		}

		public static OleDbParameter AddParameter(string paraName, object value, ParameterDirection direction, OleDbType type)
		{
			if (value == null)
			{
				value = DBNull.Value;
			}
			OleDbParameter oleDbParameter = new OleDbParameter(paraName, value);
			oleDbParameter.Direction = direction;
			oleDbParameter.OleDbType = type;
			return oleDbParameter;
		}

		public static OleDbParameter AddParameter(string paraName, object value, OleDbType type)
		{
			if (value == null)
			{
				value = DBNull.Value;
			}
			OleDbParameter oleDbParameter = new OleDbParameter(paraName, value);
			oleDbParameter.OleDbType = type;
			return oleDbParameter;
		}

		public static OleDbParameter AddParameter(string paraName, object value, OleDbType type, int size)
		{
			if (value == null)
			{
				value = DBNull.Value;
			}
			OleDbParameter oleDbParameter = new OleDbParameter(paraName, value);
			oleDbParameter.OleDbType = type;
			oleDbParameter.Size = size;
			return oleDbParameter;
		}

		public static DataTable ExecuteDataTable(OleDbTransaction trans, CommandType cmdType, string cmdText, params OleDbParameter[] commandParameters)
		{
			OleDbCommand oleDbCommand = new OleDbCommand();
			PrepareCommand(oleDbCommand, trans.Connection, trans, cmdType, cmdText, commandParameters);
			OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand);
			DataTable dataTable = new DataTable();
			oleDbDataAdapter.Fill(dataTable);
			return dataTable;
		}

		public static DataTable ExecuteDataTable(string connectionString, CommandType cmdType, string cmdText, params OleDbParameter[] commandParameters)
		{
			OleDbCommand oleDbCommand = new OleDbCommand();
			using (OleDbConnection conn = new OleDbConnection(connectionString))
			{
				PrepareCommand(oleDbCommand, conn, null, cmdType, cmdText, commandParameters);
				OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand);
				DataTable dataTable = new DataTable();
				oleDbDataAdapter.Fill(dataTable);
				return dataTable;
			}
		}

		public static int ExecuteDataTable(OleDbTransaction trans, string cmdText, DataTable dt)
		{
			OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter();
			oleDbDataAdapter.InsertCommand = new OleDbCommand(cmdText, trans.Connection);
			oleDbDataAdapter.InsertCommand.Transaction = trans;
			foreach (DataColumn column in dt.Columns)
			{
				oleDbDataAdapter.InsertCommand.Parameters.Add("@" + column.Caption, OleDbType.Char, 0, column.Caption);
			}
			return oleDbDataAdapter.Update(dt);
		}

		public static DataSet ExecuteDataset(string connectionString, string commandText)
		{
			DataSet dataSet = new DataSet();
			DataTable dataTable = new DataTable();
			using (OleDbConnection selectConnection = new OleDbConnection(connectionString))
			{
				OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(commandText, selectConnection);
				oleDbDataAdapter.Fill(dataTable);
			}
			dataSet.Tables.Add(dataTable);
			return dataSet;
		}
	}
}
