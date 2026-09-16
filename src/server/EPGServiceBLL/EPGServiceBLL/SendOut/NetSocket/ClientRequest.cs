using System.Net.Sockets;

namespace EPGServiceBLL.SendOut.NetSocket
{
	internal class ClientRequest
	{
		private Socket _Client;

		private SocketListener _Listener;

		public Socket Client
		{
			get
			{
				return _Client;
			}
			set
			{
				_Client = value;
			}
		}

		public SocketListener Listener
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
	}
}
