using System.Text;

namespace Dexin.Util.Common.Net
{
	public class HttpResponseData
	{
		public Encoding ContentEncoding { get; set; }

		public byte[] ResponseData { get; set; }

		public HttpResponseData(Encoding encoding, byte[] data)
		{
			ContentEncoding = encoding;
			ResponseData = data;
		}

		public string GetEncodingString()
		{
			return ContentEncoding.GetString(ResponseData);
		}
	}
}
