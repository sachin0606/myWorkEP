using System;

namespace Util.Common.Nets
{
	public class TcpServerHelper
	{
		private static TcpServerDemo _Ins;

		public static void Start(string host, int port, int blockfog, Action<TcpClientDemo> doconnectedclient)
		{
			if (_Ins == null || _Ins.IsDisposed)
			{
				_Ins = new TcpServerDemo(host, port);
				_Ins.DoConnectedClient += delegate(TcpServerDemo server, TcpClientDemo client)
				{
					try
					{
						doconnectedclient(client);
					}
					catch (Exception ex)
					{
						if (ex != null)
						{
							Console.WriteLine(ex.StackTrace);
						}
					}
					server.Remove(client);
				};
			}
			if (!_Ins.IsRunning)
			{
				_Ins.Start(blockfog);
			}
		}

		public static void Stop()
		{
			if (_Ins != null)
			{
				_Ins.Dispose();
			}
		}
	}
}
