using System;
using System.Data;
using System.Data.OleDb;

namespace Util.Common.DBConnectionHelpers
{
	public class OleDbConnectionPool : ConnectionPool<OleDbConnection>
	{
		public static void SetOleDbConnectionString(string path, bool? hasHeader)
		{
			string empty = string.Empty;
			if (!hasHeader.HasValue)
			{
				ConnectionPool<OleDbConnection>.ConnectionString = string.Format("Provider = Microsoft.Jet.OLEDB.4.0; Data Source ={0};Extended Properties=Excel 8.0", path);
				return;
			}
			if (path.ToLower().EndsWith(".xls"))
			{
				ConnectionPool<OleDbConnection>.ConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1};IMEX=1'", path, hasHeader.Value ? "YES" : "NO");
				return;
			}
			if (path.ToLower().EndsWith(".xlsx"))
			{
				ConnectionPool<OleDbConnection>.ConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0 Xml;HDR={1};IMEX=1'", path, hasHeader.Value ? "YES" : "NO");
				return;
			}
			throw new NullReferenceException("Excel path is not legal!");
		}

		public static OleDbDataReader ExecuteReader(CommandType cmdType, string cmdText, params OleDbParameter[] parameter)
		{
			OleDbCommand oleDbCommand = new OleDbCommand();
			ConnectionPool<OleDbConnection>.PrepareCommand(oleDbCommand, cmdType, cmdText, parameter);
			OleDbDataReader result = ((ConnectionPool<OleDbConnection>.CurrentTransaction == null) ? oleDbCommand.ExecuteReader(CommandBehavior.CloseConnection) : oleDbCommand.ExecuteReader());
			oleDbCommand.Parameters.Clear();
			return result;
		}

		public static int ExecuteNonQuery(CommandType cmdType, string cmdText, params OleDbParameter[] parameter)
		{
			OleDbCommand oleDbCommand = new OleDbCommand();
			ConnectionPool<OleDbConnection>.PrepareCommand(oleDbCommand, cmdType, cmdText, parameter);
			int result = oleDbCommand.ExecuteNonQuery();
			oleDbCommand.Parameters.Clear();
			return result;
		}

		public static object ExecuteScalar(CommandType cmdType, string cmdText, params OleDbParameter[] parameter)
		{
			OleDbCommand oleDbCommand = new OleDbCommand();
			ConnectionPool<OleDbConnection>.PrepareCommand(oleDbCommand, cmdType, cmdText, parameter);
			object result = oleDbCommand.ExecuteScalar();
			oleDbCommand.Parameters.Clear();
			return result;
		}

		public static OleDbDataReader ExecuteReader(PageQuery<OleDbParameter> query)
		{
			object obj = ExecuteScalar(CommandType.Text, string.Format("SELECT COUNT(1) FROM {0} WHERE 1 = 1 {1}", query.TableName, query.Condiction), query.CmdParameters.ToArray());
			query.TotalCount = ((obj != null) ? Convert.ToInt32(obj) : 0);
			return ExecuteReader(CommandType.Text, query.GetSql(), query.GetParameters().ToArray());
		}

		public static DataSet ExecuteDataTable(CommandType cmdType, string cmdText, params OleDbParameter[] parameter)
		{
			OleDbCommand oleDbCommand = new OleDbCommand();
			ConnectionPool<OleDbConnection>.PrepareCommand(oleDbCommand, cmdType, cmdText, parameter);
			OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand);
			DataSet dataSet = new DataSet();
			oleDbDataAdapter.Fill(dataSet);
			return dataSet;
		}
	}
}
