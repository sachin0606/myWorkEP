using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;

namespace Util.Common.DBConnectionHelpers
{
	public abstract class ConnectionPool<T> where T : DbConnection, new()
	{
		private static string _ConnectionString;

		private static List<Connection> Pool;

		private static Dictionary<int, DbTransaction> PoolTransactions;

		public static string ConnectionString
		{
			get
			{
				return _ConnectionString;
			}
			set
			{
				_ConnectionString = value;
			}
		}

		public static Connection CurrentConnection
		{
			get
			{
				bool lockTaken = false;
				List<Connection> pool = default(List<Connection>);
				try
				{
					Monitor.Enter(pool = Pool, ref lockTaken);
					int id = Thread.CurrentThread.ManagedThreadId;
					Connection connection = Pool.FirstOrDefault((Connection p) => p.CurrentThreadID == id);
					if (connection == null)
					{
						connection = Pool.FirstOrDefault((Connection p) => !p.IsDisposed && p.CurrentThreadID == 0);
						if (connection == null)
						{
							connection = BuildConnection(id);
							Pool.Add(connection);
						}
						connection.CurrentThreadID = id;
					}
					return connection;
				}
				finally
				{
					if (lockTaken)
					{
						Monitor.Exit(pool);
					}
				}
			}
		}

		public static DbTransaction CurrentTransaction
		{
			get
			{
				lock (PoolTransactions)
				{
					int managedThreadId = Thread.CurrentThread.ManagedThreadId;
					return PoolTransactions.Keys.Contains(managedThreadId) ? PoolTransactions[managedThreadId] : null;
				}
			}
		}

		static ConnectionPool()
		{
			_ConnectionString = "server=.;database=master;uid=sa;pwd=123;";
			PoolTransactions = new Dictionary<int, DbTransaction>();
			Pool = new List<Connection>();
		}

		public static void PrepareCommand(DbCommand cmd, CommandType cmdType, string cmdText, DbParameter[] cmdParms)
		{
			cmd.CommandText = cmdText;
			int managedThreadId = Thread.CurrentThread.ManagedThreadId;
			Connection currentConnection = CurrentConnection;
			DbTransaction currentTransaction = CurrentTransaction;
			currentConnection.Open();
			if (currentTransaction != null)
			{
				cmd.Transaction = currentTransaction;
			}
			cmd.Connection = currentConnection.connection;
			cmd.CommandType = cmdType;
			if (cmdParms != null)
			{
				foreach (DbParameter value in cmdParms)
				{
					cmd.Parameters.Add(value);
				}
			}
		}

		public static void OpenTransaction()
		{
			lock (PoolTransactions)
			{
				int managedThreadId = Thread.CurrentThread.ManagedThreadId;
				DbTransaction currentTransaction = CurrentTransaction;
				if (currentTransaction == null)
				{
					Connection currentConnection = CurrentConnection;
					currentConnection.Open();
					PoolTransactions[managedThreadId] = currentConnection.connection.BeginTransaction();
				}
			}
		}

		public static void DisposePool()
		{
			lock (PoolTransactions)
			{
				foreach (int key in PoolTransactions.Keys)
				{
					try
					{
						PoolTransactions[key].Rollback();
					}
					catch
					{
					}
				}
				PoolTransactions.Clear();
			}
			lock (Pool)
			{
				foreach (Connection item in Pool)
				{
					try
					{
						item.Dispose();
					}
					catch
					{
					}
				}
				Pool.Clear();
			}
		}

		public static void Completed(bool flag)
		{
			Connection currentConnection = CurrentConnection;
			lock (PoolTransactions)
			{
				DbTransaction currentTransaction = CurrentTransaction;
				if (currentTransaction != null)
				{
					if (flag)
					{
						currentTransaction.Commit();
					}
					else
					{
						currentTransaction.Rollback();
					}
					int managedThreadId = Thread.CurrentThread.ManagedThreadId;
					if (PoolTransactions.Keys.Contains(managedThreadId))
					{
						PoolTransactions.Remove(managedThreadId);
					}
				}
			}
			if (currentConnection != null)
			{
				currentConnection.Close();
			}
		}

		private static Connection BuildConnection(int threadid)
		{
			if (typeof(T) == typeof(SqlConnection))
			{
				return new Connection(threadid, new SqlConnection(ConnectionString));
			}
			if (typeof(T) == typeof(OleDbConnection))
			{
				return new Connection(threadid, new OleDbConnection(ConnectionString));
			}
			return null;
		}
	}
}
