using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using DeXin.EPG.Tools;

namespace EPGServiceBLL.SendOut.NetSocket
{
	public class UdpClientManager : IDisposable
	{
		private static object lockObject = new object();

		private static UdpClientManager _Instance;

		private List<UdpClient> _UdpInstanceList;

		private Dictionary<IPEndPoint, List<byte>> data = new Dictionary<IPEndPoint, List<byte>>();

		public static UdpClientManager Instance
		{
			get
			{
				lock (lockObject)
				{
					if (_Instance == null)
					{
						_Instance = new UdpClientManager();
					}
					return _Instance;
				}
			}
			private set
			{
				_Instance = value;
			}
		}

		public UdpClientManager()
		{
			List<IPEndPoint> list = (from ipString in LocalAdapterSettingHelper.GetLocalAdapterSettings()
				select new IPEndPoint(IPAddress.Parse(ipString), 0)).ToList();
			_UdpInstanceList = new List<UdpClient>();
			if (list.Count == 0)
			{
				_UdpInstanceList.Add(new UdpClient());
				return;
			}
			list.ForEach(delegate(IPEndPoint point)
			{
				_UdpInstanceList.Add(new UdpClient(point));
			});
		}

		public void Send(IList<byte[]> packageBuffer, IPEndPoint endPoind)
		{
			List<byte> tem = new List<byte>();
			lock (lockObject)
			{
				if (!data.ContainsKey(endPoind))
				{
					data[endPoind] = new List<byte>();
				}
				tem = data[endPoind];
			}
			lock (tem)
			{
				List<byte[]> list = packageBuffer.ToList();
				while (list.Count > 0)
				{
					tem.AddRange(list[0]);
					list.RemoveAt(0);
					if (tem.Count >= EPGSettings.Instatce.Package_Count * 188)
					{
						_UdpInstanceList.ForEach(delegate(UdpClient udpClient)
						{
							udpClient.Send(tem.ToArray(), tem.Count, endPoind);
						});
						tem.Clear();
					}
				}
			}
		}

		public static void Send(byte[] dataBuffer, IPEndPoint endEP)
		{
			using (UdpClient udpClient = new UdpClient())
			{
				udpClient.Send(dataBuffer, dataBuffer.Length, endEP);
			}
		}

		public static byte[] Recieve(IPAddress locIPAddress, int remotePort, int timeout = 5000)
		{
			using (UdpClient udpClient = new UdpClient())
			{
				while (true)
				{
					bool flag = true;
					try
					{
						udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, remotePort));
					}
					catch
					{
						Thread.Sleep(10);
						continue;
					}
					break;
				}
				udpClient.Client.ReceiveTimeout = timeout;
				IPEndPoint remoteEP = new IPEndPoint(locIPAddress, 0);
				return udpClient.Receive(ref remoteEP);
			}
		}

		public static byte[] RecieveMulticast(IPAddress localIPAddress, IPEndPoint remoteEP, int timeout = 5000)
		{
			using (UdpClient udpClient = new UdpClient())
			{
				while (true)
				{
					bool flag = true;
					try
					{
						udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, remoteEP.Port));
					}
					catch
					{
						Thread.Sleep(10);
						continue;
					}
					break;
				}
				udpClient.Client.ReceiveTimeout = timeout;
				udpClient.JoinMulticastGroup(remoteEP.Address, localIPAddress);
				return udpClient.Receive(ref remoteEP);
			}
		}

		public void Dispose()
		{
			lock (lockObject)
			{
				_UdpInstanceList.ForEach(delegate(UdpClient p)
				{
					p.Close();
				});
				_UdpInstanceList.Clear();
				Instance = null;
			}
		}
	}
}
