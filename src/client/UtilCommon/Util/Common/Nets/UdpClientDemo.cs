using System;
using System.Net;
using System.Net.Sockets;
using Util.Common.ExtensionHelper;

namespace Util.Common.Nets
{
	public class UdpClientDemo : NetBase, IDisposable
	{
		private IPEndPoint ReomteEndPoint;

		private UdpClient client;

		public UdpClientDemo(string _local, int _localport, string _server, int _port)
			: base(_local, _localport, _server, _port)
		{
		}

		public UdpClientDemo(string _server, int _port)
			: base(_server, _port)
		{
		}

		protected override void Init()
		{
			client = new UdpClient(0);
			if (base.server.IsIPAddress())
			{
				if (base.server.IsMulticaseAddress())
				{
					IPAddress address = base.server.ToIPAdress();
					ReomteEndPoint = new IPEndPoint(address, base.port);
				}
				else
				{
					IPAddress address2 = base.local;
					ReomteEndPoint = new IPEndPoint(address2, base.port);
				}
			}
		}

		public int Send(byte[] data)
		{
			if (base.server.IsIPAddress())
			{
				return client.Send(data, data.Length, ReomteEndPoint);
			}
			return client.Send(data, data.Length, base.server, base.port);
		}

		public void Dispose()
		{
		}
	}
}
