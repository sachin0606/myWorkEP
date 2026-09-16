using System.Net;
using Util.Common.ExtensionHelper;

namespace Util.Common.Nets
{
	public class NetBase
	{
		protected IPAddress local { get; set; }

		protected int localport { get; set; }

		protected string server { get; set; }

		protected int port { get; set; }

		public NetBase()
		{
			Init();
		}

		public NetBase(string _local, int _localport, string _server, int _port)
		{
			local = _local.ToIPAdress();
			localport = _localport;
			server = _server;
			port = _port;
			Init();
		}

		public NetBase(string _server, int _port)
		{
			local = IPAddress.Any;
			localport = 0;
			server = _server;
			port = _port;
			Init();
		}

		protected virtual void Init()
		{
		}
	}
}
