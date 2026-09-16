using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace Com.Cn.Tool.Model
{
	public class SqlHelper
	{
		private static Dictionary<int, SqlConnection> ConnectionPools = new Dictionary<int, SqlConnection>();

		private static Dictionary<int, SqlTransaction> TranscationPools = new Dictionary<int, SqlTransaction>();

		public static string ConnectionString = "server=192.168.0.254;database=epg2;uid=sa;pwd=123";

		private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

		public static void SetConnectionString(string connectionString)
		{
			ConnectionString = connectionString;
		}

		private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
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
				foreach (SqlParameter value in cmdParms)
				{
					cmd.Parameters.Add(value);
				}
			}
		}

		public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
		{
			SqlCommand sqlCommand = new SqlCommand();
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				PrepareCommand(sqlCommand, conn, null, cmdType, cmdText, commandParameters);
				int result = sqlCommand.ExecuteNonQuery();
				sqlCommand.Parameters.Clear();
				return result;
			}
		}

		public static int ExecuteNonQueryOutPut(string connectionString, CommandType cmdType, string cmdText, string outputKey, params SqlParameter[] commandParameters)
		{
			SqlCommand sqlCommand = new SqlCommand();
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				PrepareCommand(sqlCommand, conn, null, cmdType, cmdText, commandParameters);
				int result = sqlCommand.ExecuteNonQuery();
				parmCache[outputKey] = commandParameters;
				sqlCommand.Parameters.Clear();
				return result;
			}
		}

		public static int ExecuteNonQuery(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
		{
			SqlCommand sqlCommand = new SqlCommand();
			PrepareCommand(sqlCommand, connection, null, cmdType, cmdText, commandParameters);
			int result = sqlCommand.ExecuteNonQuery();
			sqlCommand.Parameters.Clear();
			return result;
		}

		public static int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
		{
			SqlCommand sqlCommand = new SqlCommand();
			PrepareCommand(sqlCommand, trans.Connection, trans, cmdType, cmdText, commandParameters);
			int result = sqlCommand.ExecuteNonQuery();
			sqlCommand.Parameters.Clear();
			return result;
		}

		public static int ExecuteNonQueryWithAsync(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
		{
			SqlCommand sqlCommand = new SqlCommand();
			int num = 0;
			PrepareCommand(sqlCommand, trans.Connection, trans, cmdType, cmdText, commandParameters);
			IAsyncResult asyncResult = sqlCommand.BeginExecuteNonQuery();
			while (asyncResult.IsCompleted)
			{
				num++;
			}
			sqlCommand.EndExecuteNonQuery(asyncResult);
			sqlCommand.Parameters.Clear();
			return num;
		}

		public static SqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
		{
			SqlCommand sqlCommand = new SqlCommand();
			SqlConnection sqlConnection = new SqlConnection(connectionString);
			try
			{
				PrepareCommand(sqlCommand, sqlConnection, null, cmdType, cmdText, commandParameters);
				SqlDataReader result = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
				sqlCommand.Parameters.Clear();
				return result;
			}
			catch
			{
				sqlConnection.Close();
				throw;
			}
		}

		public static SqlDataReader ExecuteReader(SqlTransaction sqltrans, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
		{
			SqlCommand sqlCommand = new SqlCommand();
			try
			{
				PrepareCommand(sqlCommand, sqltrans.Connection, sqltrans, cmdType, cmdText, commandParameters);
				SqlDataReader result = sqlCommand.ExecuteReader();
				sqlCommand.Parameters.Clear();
				return result;
			}
			catch
			{
				throw;
			}
		}

		public static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
		{
			SqlCommand sqlCommand = new SqlCommand();
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				PrepareCommand(sqlCommand, conn, null, cmdType, cmdText, commandParameters);
				object result = sqlCommand.ExecuteScalar();
				sqlCommand.Parameters.Clear();
				return result;
			}
		}

		public static object ExecuteScalar(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
		{
			SqlCommand sqlCommand = new SqlCommand();
			PrepareCommand(sqlCommand, connection, null, cmdType, cmdText, commandParameters);
			object result = sqlCommand.ExecuteScalar();
			sqlCommand.Parameters.Clear();
			return result;
		}

		public static object ExecuteScalar(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
		{
			SqlCommand sqlCommand = new SqlCommand();
			PrepareCommand(sqlCommand, trans.Connection, trans, cmdType, cmdText, commandParameters);
			object result = sqlCommand.ExecuteScalar();
			sqlCommand.Parameters.Clear();
			return result;
		}

		public static void CacheParameters(string cacheKey, params SqlParameter[] commandParameters)
		{
			parmCache[cacheKey] = commandParameters;
		}

		public static SqlParameter[] GetCachedParameters(string cacheKey)
		{
			SqlParameter[] array = (SqlParameter[])parmCache[cacheKey];
			if (array == null)
			{
				return null;
			}
			SqlParameter[] array2 = new SqlParameter[array.Length];
			int i = 0;
			for (int num = array.Length; i < num; i++)
			{
				array2[i] = (SqlParameter)((ICloneable)array[i]).Clone();
			}
			return array2;
		}

		public static void CloseAll(SqlConnection conn, SqlDataReader reader)
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

		public static SqlParameter AddParameter(string paraName, object value)
		{
			if (value == null)
			{
				value = DBNull.Value;
			}
			return new SqlParameter(paraName, value);
		}

		public static SqlParameter AddParameter(string paraName, object value, ParameterDirection direction, SqlDbType type)
		{
			if (value == null)
			{
				value = DBNull.Value;
			}
			SqlParameter sqlParameter = new SqlParameter(paraName, value);
			sqlParameter.Direction = direction;
			sqlParameter.SqlDbType = type;
			return sqlParameter;
		}

		public static SqlParameter AddParameter(string paraName, object value, SqlDbType type)
		{
			if (value == null)
			{
				value = DBNull.Value;
			}
			SqlParameter sqlParameter = new SqlParameter(paraName, value);
			sqlParameter.SqlDbType = type;
			return sqlParameter;
		}

		public static SqlParameter AddParameter(string paraName, byte[] value)
		{
			if (value == null)
			{
				value = new byte[1024];
			}
			SqlParameter sqlParameter = new SqlParameter(paraName, value);
			sqlParameter.SqlDbType = SqlDbType.VarBinary;
			sqlParameter.Size = value.Length;
			return sqlParameter;
		}

		public static SqlParameter AddParameter(string paraName, object value, SqlDbType type, int size)
		{
			if (value == null)
			{
				value = DBNull.Value;
			}
			SqlParameter sqlParameter = new SqlParameter(paraName, value);
			sqlParameter.SqlDbType = type;
			sqlParameter.Size = size;
			return sqlParameter;
		}

		public static SqlDataReader ExecuteReader(string PKName, string ClolumnNames, string tblName, string Condiction, string OrderBy, int PageIndex, int PageSize)
		{
			SqlDataReader sqlDataReader = null;
			try
			{
				string cmdText = "sp_SerachByCondiction";
				SqlParameter[] commandParameters = new SqlParameter[7]
				{
					AddParameter("@PKName", PKName, SqlDbType.NVarChar),
					AddParameter("@ClolumnNames", ClolumnNames, SqlDbType.NVarChar),
					AddParameter("@tblName", tblName, SqlDbType.NVarChar),
					AddParameter("@Condiction", Condiction, SqlDbType.NVarChar),
					AddParameter("@OrderBy", OrderBy, SqlDbType.NVarChar),
					AddParameter("@PageIndex", PageIndex, SqlDbType.Int),
					AddParameter("@PageSize", PageSize, SqlDbType.Int)
				};
				return ExecuteReader(ConnectionString, CommandType.StoredProcedure, cmdText, commandParameters);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static DataSet PageDataSet(string PKName, string ClolumnNames, string tblName, string Condiction, string OrderBy, int PageIndex, int PageSize, out int TotalCount)
		{
			SqlParameter[] array = new SqlParameter[8]
			{
				new SqlParameter("@PKName", SqlDbType.NVarChar),
				new SqlParameter("@ClolumnNames", SqlDbType.NVarChar),
				new SqlParameter("@tblName", SqlDbType.NVarChar),
				new SqlParameter("@Condiction", SqlDbType.NVarChar),
				new SqlParameter("@OrderBy", SqlDbType.NVarChar),
				new SqlParameter("@PageIndex", SqlDbType.Int),
				new SqlParameter("@PageSize", SqlDbType.Int),
				new SqlParameter("@TotalCount", SqlDbType.Int)
			};
			array[0].Value = PKName;
			array[1].Value = ClolumnNames;
			array[2].Value = tblName;
			array[3].Value = Condiction;
			array[4].Value = OrderBy;
			array[5].Value = PageIndex;
			array[6].Value = PageSize;
			array[7].Direction = ParameterDirection.Output;
			DataSet result = ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "sp_PagingSearch", array);
			TotalCount = Convert.ToInt32(array[7].Value);
			return result;
		}

		public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText)
		{
			return ExecuteDataset(connectionString, commandType, commandText, (SqlParameter[])null);
		}

		public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
		{
			if (connectionString == null || connectionString.Length == 0)
			{
				throw new ArgumentNullException("connectionString");
			}
			using (SqlConnection sqlConnection = new SqlConnection(connectionString))
			{
				sqlConnection.Open();
				return ExecuteDataset(sqlConnection, commandType, commandText, commandParameters);
			}
		}

		public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
		{
			if (connection == null)
			{
				throw new ArgumentNullException("connection");
			}
			SqlCommand sqlCommand = new SqlCommand();
			bool mustCloseConnection = false;
			PrepareCommand(sqlCommand, connection, null, commandType, commandText, commandParameters, out mustCloseConnection);
			using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
			{
				DataSet dataSet = new DataSet();
				sqlDataAdapter.Fill(dataSet);
				sqlCommand.Parameters.Clear();
				if (mustCloseConnection)
				{
					connection.Close();
				}
				return dataSet;
			}
		}

		private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, out bool mustCloseConnection)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}
			if (commandText == null || commandText.Length == 0)
			{
				throw new ArgumentNullException("commandText");
			}
			if (connection.State != ConnectionState.Open)
			{
				mustCloseConnection = true;
				connection.Open();
			}
			else
			{
				mustCloseConnection = false;
			}
			command.Connection = connection;
			command.CommandText = commandText;
			if (transaction != null)
			{
				if (transaction.Connection == null)
				{
					throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
				}
				command.Transaction = transaction;
			}
			command.CommandType = commandType;
			if (commandParameters != null)
			{
				AttachParameters(command, commandParameters);
			}
		}

		private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}
			if (commandParameters == null)
			{
				return;
			}
			foreach (SqlParameter sqlParameter in commandParameters)
			{
				if (sqlParameter != null)
				{
					if ((sqlParameter.Direction == ParameterDirection.InputOutput || sqlParameter.Direction == ParameterDirection.Input) && sqlParameter.Value == null)
					{
						sqlParameter.Value = DBNull.Value;
					}
					command.Parameters.Add(sqlParameter);
				}
			}
		}

		public static int ExecuteNonQuery(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
		{
			int managedThreadId = Thread.CurrentThread.ManagedThreadId;
			SqlTransaction value;
			TranscationPools.TryGetValue(managedThreadId, out value);
			SqlConnection value2;
			ConnectionPools.TryGetValue(managedThreadId, out value2);
			if (value == null)
			{
				value2 = ((value2 == null) ? new SqlConnection(ConnectionString) : value2);
				if (value2.State != ConnectionState.Open)
				{
					value2.Open();
				}
				value = value2.BeginTransaction();
				TranscationPools.Add(managedThreadId, value);
				ConnectionPools.Add(managedThreadId, value2);
			}
			return ExecuteNonQuery(value, cmdType, cmdText, commandParameters);
		}

		public static SqlDataReader ExecuteReader(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
		{
			int managedThreadId = Thread.CurrentThread.ManagedThreadId;
			SqlTransaction value;
			TranscationPools.TryGetValue(managedThreadId, out value);
			SqlConnection value2;
			ConnectionPools.TryGetValue(managedThreadId, out value2);
			if (value == null)
			{
				value2 = ((value2 == null) ? new SqlConnection(ConnectionString) : value2);
				if (value2.State != ConnectionState.Open)
				{
					value2.Open();
				}
				value = value2.BeginTransaction();
				TranscationPools.Add(managedThreadId, value);
				ConnectionPools.Add(managedThreadId, value2);
			}
			return ExecuteReader(value, cmdType, cmdText, commandParameters);
		}

		public static object ExecuteScalar(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
		{
			int managedThreadId = Thread.CurrentThread.ManagedThreadId;
			SqlTransaction value;
			TranscationPools.TryGetValue(managedThreadId, out value);
			SqlConnection value2;
			ConnectionPools.TryGetValue(managedThreadId, out value2);
			if (value == null)
			{
				value2 = ((value2 == null) ? new SqlConnection(ConnectionString) : value2);
				if (value2.State != ConnectionState.Open)
				{
					value2.Open();
				}
				value = value2.BeginTransaction();
				TranscationPools.Add(managedThreadId, value);
				ConnectionPools.Add(managedThreadId, value2);
			}
			return ExecuteScalar(value, cmdType, cmdText, commandParameters);
		}

		public static void SaveChange()
		{
			int managedThreadId = Thread.CurrentThread.ManagedThreadId;
			SqlTransaction value;
			TranscationPools.TryGetValue(managedThreadId, out value);
			SqlConnection value2;
			ConnectionPools.TryGetValue(managedThreadId, out value2);
			if (value != null)
			{
				value.Commit();
			}
			if (value2 != null && value2.State != ConnectionState.Closed)
			{
				value2.Close();
				value2.Dispose();
				value2 = null;
			}
			if (value != null)
			{
				value.Dispose();
				value = null;
			}
			TranscationPools.Remove(managedThreadId);
			ConnectionPools.Remove(managedThreadId);
		}

		public static void CancelChange()
		{
			int managedThreadId = Thread.CurrentThread.ManagedThreadId;
			SqlTransaction value;
			TranscationPools.TryGetValue(managedThreadId, out value);
			SqlConnection value2;
			ConnectionPools.TryGetValue(managedThreadId, out value2);
			if (value != null)
			{
				value.Rollback();
			}
			if (value2 != null && value2.State != ConnectionState.Closed)
			{
				value2.Close();
				value2.Dispose();
				value2 = null;
			}
			if (value != null)
			{
				value.Dispose();
				value = null;
			}
			TranscationPools.Remove(managedThreadId);
			ConnectionPools.Remove(managedThreadId);
		}

		public static SqlDataReader ExecuteReaderOutput(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
		{
			SqlCommand sqlCommand = new SqlCommand();
			SqlConnection sqlConnection = new SqlConnection(connectionString);
			try
			{
				PrepareCommand(sqlCommand, sqlConnection, null, cmdType, cmdText, commandParameters);
				return sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
			}
			catch
			{
				sqlConnection.Close();
				throw;
			}
		}
	}
}
