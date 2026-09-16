using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Dexin.Util.Common.Net.Sockets
{
	public static class UdpClientHelper
	{
		public static byte[] Receive(this UdpClient client, IPEndPoint ipendpoint)
		{
			List<byte> list = new List<byte>();
			do
			{
				byte[] collection = client.Receive(ref ipendpoint);
				list.AddRange(collection);
			}
			while (client.Available > 0);
			return list.ToArray();
		}
	}
}
