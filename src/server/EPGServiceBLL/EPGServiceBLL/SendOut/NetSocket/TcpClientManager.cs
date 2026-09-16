using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;

namespace EPGServiceBLL.SendOut.NetSocket
{
	public class TcpClientManager
	{
		public class TcpContact
		{
			private static object locObject = new object();

			private Socket Client;

			public TcpContact(IPEndPoint point)
			{
				Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				Client.Connect(point);
			}

			public string SendSocket(string ServerIP, int ServerPort, string Message)
			{
				using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
				{
					try
					{
						socket.Connect(IPAddress.Parse(ServerIP), ServerPort);
						byte[] bytes = Encoding.UTF8.GetBytes(Message);
						int num = 8;
						int num2;
						for (int i = 0; i < bytes.Length; i += num2)
						{
							num2 = ((i + num > bytes.Length) ? (bytes.Length - i) : num);
							byte[] array = new byte[num2];
							Array.Copy(bytes, i, array, 0, num2);
							socket.Send(array);
							Thread.Sleep(5);
						}
						Console.WriteLine("Send complete, total size : " + bytes.Length);
						byte[] array2 = new byte[256];
						StringBuilder stringBuilder = new StringBuilder();
						do
						{
							int count = socket.Receive(array2);
							stringBuilder.Append(Encoding.UTF8.GetString(array2, 0, count));
						}
						while (socket.Available > 0);
						return stringBuilder.ToString();
					}
					catch (Exception ex)
					{
						return ex.Message;
					}
				}
			}

			public byte[] SendSocket(string ServerIP, int ServerPort, byte[] SendBuffer)
			{
				using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
				{
					try
					{
						socket.Connect(IPAddress.Parse(ServerIP), ServerPort);
						int num = 256;
						int num2;
						for (int i = 0; i < SendBuffer.Length; i += num2)
						{
							num2 = ((i + num > SendBuffer.Length) ? (SendBuffer.Length - i) : num);
							byte[] array = new byte[num2];
							Array.Copy(SendBuffer, i, array, 0, num2);
							socket.Send(array);
							Thread.Sleep(5);
						}
						List<byte[]> list = new List<byte[]>();
						int num3 = 0;
						byte[] array2 = new byte[256];
						do
						{
							int num4 = socket.Receive(array2);
							byte[] array3 = new byte[num4];
							Array.Copy(array2, array3, num4);
							list.Add(array3);
							num3 += num4;
						}
						while (socket.Available > 0);
						byte[] array4 = new byte[num3];
						int destinationIndex = 0;
						foreach (byte[] item in list)
						{
							Array.Copy(item, 0, array4, destinationIndex, item.Length);
						}
						return array4;
					}
					catch (Exception ex)
					{
						if (ex is SocketException)
						{
							throw ex;
						}
						return new byte[2] { 0, 255 };
					}
				}
			}

			public void SendSocketOnly(byte[] SendBuffer)
			{
				lock (locObject)
				{
					try
					{
						int sendBufferSize = Client.SendBufferSize;
						int num;
						for (int i = 0; i < SendBuffer.Length; i += num)
						{
							num = ((i + sendBufferSize > SendBuffer.Length) ? (SendBuffer.Length - i) : sendBufferSize);
							byte[] destinationArray = new byte[num];
							Array.Copy(SendBuffer, i, destinationArray, 0, num);
							Client.Send(SendBuffer);
						}
					}
					catch (Exception ex)
					{
						Tool.ShowMessage(new CustomException("", ex, MessageType.LOG));
						IPEndPoint remoteEP = Client.RemoteEndPoint as IPEndPoint;
						Client.Connect(remoteEP);
						SendSocketOnly(SendBuffer);
					}
				}
			}
		}

		private static object locObject = new object();

		private static TcpClientManager _Instance;

		private Dictionary<IPEndPoint, TcpContact> udpTable = new Dictionary<IPEndPoint, TcpContact>();

		public static TcpClientManager Instance
		{
			get
			{
				lock (locObject)
				{
					if (_Instance == null)
					{
						_Instance = new TcpClientManager();
					}
					return _Instance;
				}
			}
		}

		public TcpContact this[IPEndPoint point]
		{
			get
			{
				lock (locObject)
				{
					if (!udpTable.Keys.Contains(point))
					{
						udpTable.Add(point, new TcpContact(point));
					}
					return udpTable[point];
				}
			}
		}

		private TcpClientManager()
		{
		}
	}
}
