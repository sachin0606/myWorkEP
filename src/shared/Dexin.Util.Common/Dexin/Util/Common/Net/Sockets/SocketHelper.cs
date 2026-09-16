using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace Dexin.Util.Common.Net.Sockets
{
	public static class SocketHelper
	{
		private static int _AvailableTimespan = 50;

		private static int _AvailableRetray = 3;

		public static int AvailableTimespan
		{
			get
			{
				return _AvailableTimespan;
			}
			set
			{
				_AvailableTimespan = value;
			}
		}

		public static int AvailableRetray
		{
			get
			{
				return _AvailableRetray;
			}
			set
			{
				_AvailableRetray = value;
			}
		}

		public static byte[] Receive(this Socket socket, int receivebuffsize, SocketFlags socketflag = SocketFlags.None)
		{
			List<byte> list = new List<byte>();
			int num = 0;
			do
			{
				byte[] array = new byte[receivebuffsize];
				int num2 = socket.Receive(array, socketflag);
				if (num2 > 0)
				{
					if (num2 != array.Length)
					{
						array = array.GetBytes(num2);
					}
					list.AddRange(array);
					continue;
				}
				while (true)
				{
					bool flag = true;
					if (socket.Available == 0)
					{
						if (num > AvailableRetray)
						{
							break;
						}
						Thread.Sleep(AvailableTimespan);
						num++;
						continue;
					}
					num = 0;
					break;
				}
			}
			while (socket.Available > 0);
			return list.ToArray();
		}
	}
}
