using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace EPGServiceBLL.SendOut.NetSocket
{
	public class TcpSocketServer
	{
		private static TcpSocketServer _Instance;

		private bool _ISRunning;

		private Socket Server;

		public static TcpSocketServer Instance
		{
			get
			{
				if (_Instance != null)
				{
					return _Instance;
				}
				return _Instance = new TcpSocketServer();
			}
			set
			{
				_Instance = value;
			}
		}

		public bool ISRunning
		{
			get
			{
				return _ISRunning;
			}
		}

		private TcpSocketServer()
		{
		}

		public void StartListener(string ServerIP, int ServerPort, SocketListener listener)
		{
			_ISRunning = true;
			using (Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
			{
				Server.Bind(new IPEndPoint(IPAddress.Parse(ServerIP), ServerPort));
				Server.Listen(5);
				while (_ISRunning)
				{
					Socket socket = Server.Accept();
					try
					{
						Thread thread = new Thread(HandelConnectionLong);
						thread.Name = "HandelConnectionLong";
						thread.Start(new ClientRequest
						{
							Client = socket,
							Listener = listener
						});
					}
					catch (Exception ex)
					{
						Console.WriteLine("--------------listener exception : " + ex.Message + "--------------------");
						socket.Send(Encoding.UTF8.GetBytes("-1|ERROR|"));
					}
				}
			}
		}

		private void HandelConnection(object args)
		{
			ClientRequest clientRequest = args as ClientRequest;
			using (Socket socket = clientRequest.Client)
			{
				IPEndPoint iPEndPoint = socket.RemoteEndPoint as IPEndPoint;
				byte[] array = new byte[256];
				List<byte[]> list = new List<byte[]>();
				string s = socket.RemoteEndPoint.ToString() + ":";
				list.Add(Encoding.UTF8.GetBytes(s));
				int num = 0;
				do
				{
					bool connected = socket.Connected;
					num = socket.Receive(array);
					if (num > 0)
					{
						byte[] array2 = new byte[num];
						Array.Copy(array, array2, num);
						list.Add(array2);
					}
				}
				while (socket.Available > 0);
				int num2 = 0;
				if (list.Count <= 0)
				{
					return;
				}
				foreach (byte[] item in list)
				{
					num2 += item.Length;
				}
				byte[] array3 = new byte[num2];
				int num3 = 0;
				foreach (byte[] item2 in list)
				{
					item2.CopyTo(array3, num3);
					num3 += item2.Length;
				}
				socket.Send(array3);
				if (clientRequest.Listener != null)
				{
					clientRequest.Listener(array3);
				}
			}
		}

		private void CheckLongConnectionTimeout(object args)
		{
			ClientRequest clientRequest = args as ClientRequest;
			int num = 0;
			while (num < 60 && _ISRunning && clientRequest.Client.Connected)
			{
				num++;
				Thread.Sleep(1000);
			}
			if (clientRequest.Client != null && clientRequest.Client.Connected)
			{
				clientRequest.Client.Close();
				Console.WriteLine("Active close client at time out : " + num);
			}
			else
			{
				Console.WriteLine("Client closed socket at : " + num);
			}
		}

		private void HandelConnectionLong(object args)
		{
			ClientRequest clientRequest = args as ClientRequest;
			Socket client = clientRequest.Client;
			try
			{
				byte[] array = new byte[256];
				List<byte[]> list = new List<byte[]>();
				string s = client.RemoteEndPoint.ToString() + ":";
				list.Add(Encoding.UTF8.GetBytes(s));
				int num = 0;
				while (_ISRunning)
				{
					if (client.Available == 0)
					{
						num++;
						Thread.Sleep(50);
						if (num > 20)
						{
							Console.WriteLine("break {0} at : {1}", client.RemoteEndPoint, num);
							break;
						}
						continue;
					}
					int num2 = client.Receive(array);
					if (num2 > 0)
					{
						byte[] array2 = new byte[num2];
						Array.Copy(array, array2, num2);
						list.Add(array2);
						num = 0;
					}
				}
				int num3 = 0;
				if (list.Count <= 0)
				{
					return;
				}
				foreach (byte[] item in list)
				{
					num3 += item.Length;
				}
				byte[] array3 = new byte[num3];
				int num4 = 0;
				foreach (byte[] item2 in list)
				{
					item2.CopyTo(array3, num4);
					num4 += item2.Length;
				}
				client.Send(array3);
				Console.WriteLine("send back to {0} complete", client.RemoteEndPoint);
				if (clientRequest.Listener != null)
				{
					clientRequest.Listener(array3);
					Console.WriteLine("complete handel listener for client : {0}", client.RemoteEndPoint);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("--------------listener exception : " + ex.Message + "--------------------");
				client.Send(Encoding.UTF8.GetBytes(ex.Message));
			}
			finally
			{
			}
		}

		public void StopListener()
		{
			_ISRunning = false;
			if (Server != null)
			{
				Server.Close();
				Server.Dispose();
			}
		}
	}
}
