using System;
using System.Net;
using System.Net.Sockets;
using Util.Common.ExtensionHelper;

namespace Util.Common.Nets
{
	public class UdpServerDemo : NetBase, IDisposable
	{
		public int ReceiveTimeOut = 10000;

		public int SendTimeOut = 10000;

		private UdpClient _Listener;

		private IPEndPoint _ReomteEndPoint;

		public IPEndPoint ReomteEndPoint
		{
			get
			{
				return _ReomteEndPoint;
			}
			set
			{
				_ReomteEndPoint = value;
			}
		}

		public UdpClient Listener
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

		public bool IsRunning { get; set; }

		public bool IsDisposed { get; set; }

		public event Action<UdpServerDemo, byte[]> DoConnectedClient;

		public UdpServerDemo(string _local, int _localport, string _server, int _port)
			: base(_local, _localport, _server, _port)
		{
		}

		public UdpServerDemo(string _server, int _port)
			: base(_server, _port)
		{
		}

		protected override void Init()
		{
			base.Init();
		}

		public void BeginStart(int backlog)
		{
			if (!IsRunning)
			{
				IsRunning = true;
				IsDisposed = false;
				ReomteEndPoint = new IPEndPoint(base.server.ToIPAdress(), base.port);
				Listener = new UdpClient();
				if (base.server.IsMulticaseAddress())
				{
					Listener.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
					Listener.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
					Listener.Client.Bind(new IPEndPoint(base.local, base.port));
					IPAddress multicastAddr = base.server.ToIPAdress();
					Listener.JoinMulticastGroup(multicastAddr);
				}
				else if (base.server.GetAddress() > IPAddressHelper.MAX_MULTICASE_ADDRESS)
				{
					Listener.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
					Listener.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
					Listener.Client.Bind(new IPEndPoint(IPAddress.Any, base.port));
				}
				Listener.Client.ReceiveTimeout = ReceiveTimeOut;
				Listener.Client.SendTimeout = SendTimeOut;
				Listener.BeginReceive(AsyncReceive, this);
			}
		}

		protected virtual void AsyncReceive(IAsyncResult ar)
		{
			try
			{
				UdpServerDemo udpServerDemo = (UdpServerDemo)ar.AsyncState;
				if (udpServerDemo.IsRunning)
				{
					udpServerDemo.Listener.BeginReceive(udpServerDemo.AsyncReceive, udpServerDemo);
				}
				IPEndPoint remoteEP = udpServerDemo.ReomteEndPoint;
				byte[] arg = udpServerDemo.Listener.EndReceive(ar, ref remoteEP);
				if (udpServerDemo.DoConnectedClient != null)
				{
					udpServerDemo.DoConnectedClient(udpServerDemo, arg);
				}
			}
			catch (Exception)
			{
			}
		}

		public void Start(int backlog)
		{
			if (IsRunning)
			{
				return;
			}
			IsRunning = true;
			IsDisposed = false;
			ReomteEndPoint = new IPEndPoint(IPAddress.Any, base.port);
			if (base.server.IsMulticaseAddress())
			{
				Listener = new UdpClient();
				Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
				socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
				socket.Bind(new IPEndPoint(IPAddress.Any, base.port));
				Listener.Client = socket;
				IPAddress iPAddress = base.server.ToIPAdress();
				ReomteEndPoint = new IPEndPoint(iPAddress, base.port);
				Listener.JoinMulticastGroup(iPAddress);
			}
			else
			{
				Listener = new UdpClient(base.port);
			}
			IPEndPoint remoteEP = ReomteEndPoint;
			do
			{
				try
				{
					byte[] arg = Listener.Receive(ref remoteEP);
					if (this.DoConnectedClient != null)
					{
						this.DoConnectedClient(this, arg);
					}
				}
				catch (Exception ex)
				{
					IsRunning = false;
					if (!(ex is ObjectDisposedException) && !(ex is SocketException))
					{
						throw ex;
					}
				}
			}
			while (IsRunning);
		}

		public void Dispose()
		{
			if (IsDisposed)
			{
				return;
			}
			IsDisposed = true;
			IsRunning = false;
			if (Listener == null)
			{
				return;
			}
			try
			{
				Listener.Close();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}
