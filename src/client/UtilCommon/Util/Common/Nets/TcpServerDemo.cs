using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Util.Common.ExtensionHelper;

namespace Util.Common.Nets
{
	public class TcpServerDemo : IDisposable
	{
		private bool _IsDisposed;

		private TcpListener _Listener;

		private List<TcpClientDemo> _ClientPool = new List<TcpClientDemo>();

		public bool IsRunning { get; private set; }

		public List<TcpClientDemo> ClientPool
		{
			get
			{
				return _ClientPool;
			}
			set
			{
				_ClientPool = value;
			}
		}

		public TcpListener Listener
		{
			get
			{
				return _Listener;
			}
			set
			{
				_Listener = value;
			}
		}

		public bool IsDisposed
		{
			get
			{
				return _IsDisposed;
			}
			private set
			{
				_IsDisposed = value;
			}
		}

		public event Action<TcpServerDemo, TcpClientDemo> DoConnectedClient;

		public TcpServerDemo(string host, int port)
		{
			Listener = new TcpListener(host.IsIPAddress() ? host.ToIPAdress() : IPAddress.Any, port);
		}

		public void Start(int backlog)
		{
			if (!IsRunning)
			{
				IsRunning = true;
				IsDisposed = false;
				Listener.Start(backlog);
				Listener.BeginAcceptTcpClient(AcceptTcpClient, this);
			}
		}

		private void AcceptTcpClient(IAsyncResult ar)
		{
			if (!IsRunning)
			{
				return;
			}
			TcpServerDemo tcpServerDemo = (TcpServerDemo)ar.AsyncState;
			if (tcpServerDemo.IsRunning)
			{
				tcpServerDemo.Listener.BeginAcceptTcpClient(tcpServerDemo.AcceptTcpClient, tcpServerDemo);
			}
			TcpClient client = tcpServerDemo.Listener.EndAcceptTcpClient(ar);
			TcpClientDemo tcpClientDemo = new TcpClientDemo(client);
			if (tcpServerDemo.DoConnectedClient != null)
			{
				tcpServerDemo.Add(tcpClientDemo);
				try
				{
					tcpServerDemo.DoConnectedClient(tcpServerDemo, tcpClientDemo);
				}
				catch (Exception)
				{
				}
			}
		}

		private void Add(TcpClientDemo client)
		{
			lock (ClientPool)
			{
				TcpClientDemo tcpClientDemo = ClientPool.FirstOrDefault((TcpClientDemo p) => p.Key.Equals(client.Key));
				if (tcpClientDemo == null)
				{
					ClientPool.Add(client);
					return;
				}
				if (!tcpClientDemo.TcpClient.Connected)
				{
					Remove(tcpClientDemo);
				}
				ClientPool.Add(client);
			}
		}

		public void Remove(TcpClientDemo client)
		{
			lock (ClientPool)
			{
				TcpClientDemo tcpClientDemo = ClientPool.FirstOrDefault((TcpClientDemo p) => p.Key.Equals(client.Key));
				if (tcpClientDemo != null)
				{
					tcpClientDemo.Dispose();
					ClientPool.Remove(tcpClientDemo);
				}
			}
		}

		private void DisposeClientPool()
		{
			lock (ClientPool)
			{
				while (ClientPool.Count > 0)
				{
					try
					{
						ClientPool[0].Dispose();
					}
					catch (Exception)
					{
					}
					ClientPool.RemoveAt(0);
				}
			}
		}

		public void Dispose()
		{
			if (IsDisposed)
			{
				return;
			}
			IsRunning = false;
			IsDisposed = true;
			DisposeClientPool();
			if (Listener == null)
			{
				return;
			}
			try
			{
				Listener.Stop();
			}
			catch (Exception)
			{
			}
		}
	}
}
