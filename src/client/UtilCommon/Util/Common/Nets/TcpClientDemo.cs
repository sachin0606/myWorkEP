using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Util.Common.ExtensionHelper;

namespace Util.Common.Nets
{
	public class TcpClientDemo : IDisposable
	{
		private int default_package_size = 1024;

		private string _Key = "";

		private TcpClient _TcpClient;

		public bool IsDisposed { get; private set; }

		public TcpClient TcpClient
		{
			get
			{
				return _TcpClient;
			}
			set
			{
				_TcpClient = value;
			}
		}

		public string Key
		{
			get
			{
				return _Key;
			}
			private set
			{
				_Key = value;
			}
		}

		public TcpClientDemo(TcpClient client)
		{
			TcpClient = client;
			Init();
		}

		public TcpClientDemo(string server, int port)
		{
			TcpClient = new TcpClient();
			if (server.IsIPAddress())
			{
				IPEndPoint remoteEP = new IPEndPoint(server.ToIPAdress(), port);
				TcpClient.Connect(remoteEP);
			}
			Init();
		}

		public TcpClientDemo(string localip, int localport, string server, int port)
		{
			IPEndPoint localEP = new IPEndPoint(localip.ToIPAdress(), localport);
			TcpClient = new TcpClient(localEP);
			if (server.IsIPAddress())
			{
				IPEndPoint remoteEP = new IPEndPoint(server.ToIPAdress(), port);
				TcpClient.Connect(remoteEP);
			}
			Init();
		}

		private void Init()
		{
			IsDisposed = false;
			Key = ((TcpClient == null) ? "" : TcpClient.Client.RemoteEndPoint.ToStr());
		}

		public NetworkStream GetNetworkStream()
		{
			return TcpClient.GetStream();
		}

		public void SetTimeOut(int sendtimeout, int receivetimeout)
		{
			TcpClient.SendTimeout = sendtimeout;
			TcpClient.ReceiveTimeout = receivetimeout;
		}

		public void SetBufferSize(int sendbuffersize, int receivebuffersize)
		{
			TcpClient.SendBufferSize = sendbuffersize;
			TcpClient.ReceiveBufferSize = receivebuffersize;
		}

		public void Do(Action<NetworkStream> action)
		{
			NetworkStream networkStream = GetNetworkStream();
			action(networkStream);
		}

		public void Send(byte[] data)
		{
			Do(delegate(NetworkStream stream)
			{
				if (stream.CanWrite)
				{
					stream.Write(data, 0, data.Length);
					stream.Flush();
				}
			});
		}

		public List<byte[]> Receives()
		{
			List<byte[]> list = new List<byte[]>();
			NetworkStream networkStream = GetNetworkStream();
			if (networkStream.CanRead)
			{
				do
				{
					byte[] array = new byte[default_package_size];
					int num = networkStream.Read(array, 0, array.Length);
					if (num < 0)
					{
						break;
					}
					if (num < default_package_size)
					{
						byte[] bytes = array.GetBytes(num);
						list.Add(bytes);
					}
					else
					{
						list.Add(array);
					}
				}
				while (networkStream.DataAvailable);
			}
			return list;
		}

		public byte[] Receive()
		{
			List<byte> result = new List<byte>();
			Receives().ForEach(delegate(byte[] d)
			{
				result.AddRange(d);
			});
			return result.ToArray();
		}

		public void Receive(Func<byte[], bool> action)
		{
			byte[] arg;
			do
			{
				arg = Receive();
			}
			while (action(arg));
		}

		public void Dispose()
		{
			if (IsDisposed)
			{
				return;
			}
			IsDisposed = true;
			if (TcpClient == null)
			{
				return;
			}
			try
			{
				TcpClient.Close();
			}
			catch (Exception)
			{
			}
		}

		public override string ToString()
		{
			return Key;
		}
	}
}
