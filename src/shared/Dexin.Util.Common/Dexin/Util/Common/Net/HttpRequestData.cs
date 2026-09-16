using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;

namespace Dexin.Util.Common.Net
{
	public class HttpRequestData
	{
		public NameValueCollection Params { get; private set; }

		public IList<HttpFile> Files { get; private set; }

		public IPAddress RemoteAddress { get; set; }

		public HttpRequestData()
		{
			Params = new NameValueCollection();
			Files = new List<HttpFile>();
		}
	}
}
