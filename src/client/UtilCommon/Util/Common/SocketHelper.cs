using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Util.Common.ExtensionHelper;

namespace Util.Common
{
	public static class SocketHelper
	{
		public static int Retry = 2;

		public static int timespan = 100;

		public static byte[] Receive(this Socket socket, int receivebuffsize, SocketFlags socketflag = SocketFlags.None)
		{
			int index = 0;
			long data_length;
			List<byte[]> list = socket.ReceiveLongData(receivebuffsize, socketflag, out data_length);
			byte[] array = new byte[data_length];
			foreach (byte[] item in list)
			{
				array.CopyBytes(item, ref index);
			}
			return array;
		}

		public static List<byte[]> ReceiveLongData(this Socket socket, int receivebuffsize, SocketFlags socketflag, out long data_length)
		{
			data_length = 0L;
			List<byte[]> list = new List<byte[]>();
			do
			{
				byte[] array = new byte[receivebuffsize];
				int num = socket.Receive(array, socketflag);
				if (num <= 0)
				{
					break;
				}
				if (num == receivebuffsize)
				{
					list.Add(array);
				}
				else
				{
					byte[] bytes = array.GetBytes(num);
					list.Add(bytes);
				}
				data_length += num;
			}
			while (socket.Available > 0);
			return list;
		}

		public static void DoDispose(this Socket socket)
		{
			try
			{
				using (socket)
				{
					socket.Close();
				}
			}
			catch
			{
			}
		}

		public static List<byte[]> GetResponseData(this HttpWebRequest request, int receivebuffsize, out long receive_data_length)
		{
			List<byte[]> list = new List<byte[]>();
			receive_data_length = 0L;
			using (WebResponse webResponse = request.GetResponse())
			{
				using (Stream stream = webResponse.GetResponseStream())
				{
					do
					{
						byte[] array = new byte[receivebuffsize];
						int num = stream.Read(array, 0, receivebuffsize);
						if (num > 0)
						{
							receive_data_length += num;
							if (num < receivebuffsize)
							{
								list.Add(array.GetBytes(num));
							}
							else
							{
								list.Add(array);
							}
							continue;
						}
						break;
					}
					while (stream.CanRead);
				}
			}
			return list;
		}

		public static byte[] GetResponseData(this HttpWebRequest request, int receivebuffsize = 1024)
		{
			int index = 0;
			long receive_data_length;
			List<byte[]> responseData = request.GetResponseData(receivebuffsize, out receive_data_length);
			byte[] array = new byte[receive_data_length];
			foreach (byte[] item in responseData)
			{
				array.CopyBytes(item, ref index);
			}
			return array;
		}

		public static void Do()
		{
			Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
			IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 5001);
			EndPoint remoteEP = iPEndPoint;
			socket.Bind(remoteEP);
			MulticastOption optionValue = new MulticastOption(IPAddress.Parse("224.2.2.2"));
			socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, optionValue);
			Console.WriteLine("开始接收多播组 234.5.6.7 上的数据...");
			byte[] array = new byte[1024];
			while (true)
			{
				int num = socket.ReceiveFrom(array, ref remoteEP);
				if (num > 0)
				{
					byte[] bytes = array.GetBytes(num);
					string value = bytes.BytesToString(" ");
					Console.WriteLine(value);
				}
			}
		}
	}
}
