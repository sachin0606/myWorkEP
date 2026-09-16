using System;
using System.Data;
using System.Data.Common;

namespace Util.Common.DBConnectionHelpers
{
	public class Connection : IDisposable
	{
		private int _CurrentThreadID;

		private bool _IsDisposed;

		private DbConnection _connection;

		internal DbConnection connection
		{
			get
			{
				return _connection;
			}
			private set
			{
				_connection = value;
			}
		}

		internal bool IsDisposed
		{
			get
			{
				return _IsDisposed;
			}
			set
			{
				_IsDisposed = value;
			}
		}

		internal int CurrentThreadID
		{
			get
			{
				return _CurrentThreadID;
			}
			set
			{
				_CurrentThreadID = value;
			}
		}

		internal Connection(int currentThreadID, DbConnection connection)
		{
			this.connection = connection;
			CurrentThreadID = 0;
			IsDisposed = false;
		}

		public void Open()
		{
			if (connection.State != ConnectionState.Open)
			{
				connection.Open();
			}
		}

		public void Close()
		{
			try
			{
				if (connection.State != ConnectionState.Closed)
				{
					connection.Close();
				}
			}
			catch
			{
				Dispose();
			}
			finally
			{
				CurrentThreadID = 0;
			}
		}

		public void Dispose()
		{
			if (IsDisposed)
			{
				return;
			}
			IsDisposed = true;
			if (connection != null)
			{
				try
				{
					connection.Dispose();
				}
				catch
				{
				}
			}
		}
	}
}
