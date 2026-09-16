using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Dexin.Util.Common.IO;

namespace Dexin.Util.Common.Net
{
	public static class HttpHelper
	{
		private class Values
		{
			public ValueType type = ValueType.Normal;

			public string name;

			public string file_name;

			public byte[] datas;
		}

		private enum ValueType
		{
			Normal,
			File
		}

		public static Encoding DEFAULTENCODE = Encoding.UTF8;

		public static HttpResponseData HttpRequest(string url, NameValueCollection data, int getResponseBufferSize = 1024)
		{
			return HttpRequest(url, data, DEFAULTENCODE, getResponseBufferSize);
		}

		public static HttpResponseData HttpRequest(string url, NameValueCollection data, Encoding encoding, int getResponseBufferSize = 1024)
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
			httpWebRequest.Method = "POST";
			httpWebRequest.ContentType = "application/x-www-form-urlencoded";
			using (Stream stream = httpWebRequest.GetRequestStream())
			{
				if (data != null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (string key in data.Keys)
					{
						stringBuilder.Append(string.Format("{0}={1}&", key, data[key]));
					}
					byte[] bytes = encoding.GetBytes(stringBuilder.ToString());
					stream.Write(bytes, 0, bytes.Length);
				}
			}
			HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			Encoding encoding2 = encoding;
			if (!string.IsNullOrEmpty(httpWebResponse.ContentEncoding))
			{
				encoding2 = Encoding.GetEncoding(httpWebResponse.ContentEncoding);
			}
			return new HttpResponseData(encoding2, httpWebResponse.GetResponseStream().ReadAllBytes(getResponseBufferSize));
		}

		public static HttpResponseData HttpRequest(string url, NameValueCollection data, NameValueCollection files, int getResponseBufferSize = 1024)
		{
			return HttpRequest(url, data, files, DEFAULTENCODE, getResponseBufferSize);
		}

		public static HttpResponseData HttpRequest(string url, NameValueCollection data, NameValueCollection files, Encoding encoding, int getResponseBufferSize = 1024)
		{
			string text = "---------------------------" + DateTime.Now.Ticks.ToString("x");
			byte[] bytes = Encoding.ASCII.GetBytes("\r\n--" + text + "\r\n");
			byte[] bytes2 = Encoding.ASCII.GetBytes("\r\n--" + text + "--\r\n");
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
			httpWebRequest.ContentType = "multipart/form-data; boundary=" + text;
			httpWebRequest.Method = "POST";
			httpWebRequest.KeepAlive = true;
			httpWebRequest.Credentials = CredentialCache.DefaultCredentials;
			using (Stream stream = httpWebRequest.GetRequestStream())
			{
				string format = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
				if (data != null)
				{
					foreach (string key in data.Keys)
					{
						stream.Write(bytes, 0, bytes.Length);
						string s = string.Format(format, key, data[key]);
						byte[] bytes3 = encoding.GetBytes(s);
						stream.Write(bytes3, 0, bytes3.Length);
					}
				}
				if (files != null)
				{
					string format2 = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n";
					byte[] array = new byte[4096];
					int num = 0;
					foreach (string key2 in files.Keys)
					{
						stream.Write(bytes, 0, bytes.Length);
						string s2 = string.Format(format2, "file", Path.GetFileName(files[key2]));
						byte[] bytes4 = encoding.GetBytes(s2);
						stream.Write(bytes4, 0, bytes4.Length);
						using (FileStream fileStream = new FileStream(files[key2], FileMode.Open, FileAccess.Read))
						{
							while ((num = fileStream.Read(array, 0, array.Length)) != 0)
							{
								stream.Write(array, 0, num);
							}
						}
					}
				}
				stream.Write(bytes2, 0, bytes2.Length);
			}
			HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			Encoding encoding2 = encoding;
			if (!string.IsNullOrEmpty(httpWebResponse.ContentEncoding))
			{
				encoding2 = Encoding.GetEncoding(httpWebResponse.ContentEncoding);
			}
			return new HttpResponseData(encoding2, httpWebResponse.GetResponseStream().ReadAllBytes(getResponseBufferSize));
		}

		public static HttpRequestData ReceiveHttpRequest(HttpListenerRequest request, int receivebuffsize)
		{
			HttpRequestData httpRequestData = new HttpRequestData();
			httpRequestData.RemoteAddress = request.RemoteEndPoint.Address;
			if (string.Compare("GET", request.HttpMethod, true) == 0)
			{
				string[] allKeys = request.QueryString.AllKeys;
				foreach (string name in allKeys)
				{
					httpRequestData.Params.Add(name, request.QueryString[name]);
				}
			}
			else if (string.Compare("POST", request.HttpMethod, true) == 0)
			{
				if (request.ContentType.Length > 20 && string.Compare(request.ContentType.Substring(0, 20), "multipart/form-data;", true) == 0)
				{
					Encoding dEFAULTENCODE = DEFAULTENCODE;
					string[] value = request.ContentType.Split(';').Skip(1).ToArray();
					string text = string.Join(";", value).Replace("boundary=", "").Trim();
					byte[] bytes = dEFAULTENCODE.GetBytes("--" + text + "\r\n");
					byte[] bytes2 = dEFAULTENCODE.GetBytes("--" + text + "--\r\n");
					byte[] buffer = request.InputStream.ReadAllBytes(receivebuffsize);
					Stream sourceStream = new MemoryStream(buffer);
					MemoryStream memoryStream = new MemoryStream();
					bool flag = true;
					Values values = null;
					int num = 0;
					while (flag)
					{
						byte[] array = sourceStream.ReadLineAsBytes();
						if (array.Length == 0)
						{
							break;
						}
						if (dEFAULTENCODE.GetString(array).Equals("\r\n"))
						{
							num++;
						}
						if (!dEFAULTENCODE.GetString(array).Equals("\r\n") || num >= 2)
						{
							memoryStream.Write(array, 0, array.Length);
						}
						if (ArrayHelper.CompareBytes(bytes, array))
						{
							num = 0;
							byte[] array2 = new byte[memoryStream.Length - bytes.Length];
							memoryStream.Position = 0L;
							memoryStream.Read(array2, 0, array2.Length);
							flag = true;
							if (array2.Length > 0)
							{
								values.datas = array2;
								ParseValue(httpRequestData, dEFAULTENCODE, values);
							}
							values = new Values();
							memoryStream.Dispose();
							memoryStream = new MemoryStream();
						}
						else if (dEFAULTENCODE.GetString(array).Contains("Content-Disposition"))
						{
							byte[] array2 = new byte[memoryStream.Length - 2];
							memoryStream.Position = 0L;
							memoryStream.Read(array2, 0, array2.Length);
							flag = true;
							string text2 = dEFAULTENCODE.GetString(array2);
							values.name = text2.Substring(text2.IndexOf("name")).Split('=')[1].Replace("\"", "");
							values.file_name = text2.Substring(text2.IndexOf("filename")).Split('=')[1].Replace("\"", "");
							memoryStream.Dispose();
							memoryStream = new MemoryStream();
						}
						else if (dEFAULTENCODE.GetString(array).Contains("Content-Type"))
						{
							num = 0;
							flag = true;
							values.type = ValueType.File;
							memoryStream.Dispose();
							memoryStream = new MemoryStream();
						}
						else if (ArrayHelper.CompareBytes(bytes2, array))
						{
							byte[] array2 = new byte[memoryStream.Length - bytes2.Length - 2];
							memoryStream.Position = 0L;
							memoryStream.Read(array2, 0, array2.Length);
							values.datas = array2;
							ParseValue(httpRequestData, request.ContentEncoding, values);
							memoryStream.Dispose();
							flag = false;
						}
					}
				}
				else
				{
					string text3 = "";
					using (StreamReader streamReader = new StreamReader(request.InputStream, request.ContentEncoding))
					{
						text3 = streamReader.ReadToEnd();
					}
					string[] allKeys = text3.Split('&');
					foreach (string name in allKeys)
					{
						string[] array3 = name.Split('=');
						httpRequestData.Params.Add(array3[0], array3[1]);
					}
				}
			}
			return httpRequestData;
		}

		private static void ParseValue(HttpRequestData postData, Encoding Encoding, Values data)
		{
			if (data != null)
			{
				if (data.type == ValueType.Normal)
				{
					postData.Params.Add(data.name, Encoding.GetString(data.datas));
				}
				else if (data.type == ValueType.File)
				{
					postData.Files.Add(new HttpFile(data.name, data.file_name, data.datas));
				}
			}
		}

		public static string HttpRequest(string url, string referUrl, IDictionary<string, string> parameters, IDictionary<string, string> headers, MethodType method)
		{
			if (method.ToString().ToLower() == "post")
			{
				HttpWebRequest httpWebRequest = null;
				HttpWebResponse httpWebResponse = null;
				Stream stream = null;
				try
				{
					httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
					httpWebRequest.Method = method.ToString();
					httpWebRequest.KeepAlive = false;
					httpWebRequest.ProtocolVersion = HttpVersion.Version10;
					httpWebRequest.Timeout = 5000;
					httpWebRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
					httpWebRequest.Referer = referUrl;
					if (headers != null)
					{
						foreach (KeyValuePair<string, string> header in headers)
						{
							httpWebRequest.Headers.Add(header.Key, header.Value);
						}
					}
					byte[] bytes = Encoding.UTF8.GetBytes(BuildQuery(parameters, "utf8"));
					stream = httpWebRequest.GetRequestStream();
					stream.Write(bytes, 0, bytes.Length);
					httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
					Encoding encoding = Encoding.GetEncoding(httpWebResponse.CharacterSet);
					return GetResponseAsString(httpWebResponse, encoding);
				}
				catch (Exception ex)
				{
					throw ex;
				}
				finally
				{
					if (stream != null)
					{
						stream.Close();
					}
					if (httpWebResponse != null)
					{
						httpWebResponse.Close();
					}
				}
			}
			HttpWebRequest httpWebRequest2 = (HttpWebRequest)WebRequest.Create(url + "?" + BuildQuery(parameters, "utf8"));
			httpWebRequest2.Method = "GET";
			httpWebRequest2.ReadWriteTimeout = 5000;
			httpWebRequest2.ContentType = "text/html;charset=UTF-8";
			if (headers != null)
			{
				foreach (KeyValuePair<string, string> header2 in headers)
				{
					httpWebRequest2.Headers.Add(header2.Key, header2.Value);
				}
			}
			HttpWebResponse httpWebResponse2 = (HttpWebResponse)httpWebRequest2.GetResponse();
			Stream responseStream = httpWebResponse2.GetResponseStream();
			StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
			return streamReader.ReadToEnd();
		}

		private static string BuildQuery(IDictionary<string, string> parameters, string encode)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			if (parameters == null)
			{
				return "";
			}
			IEnumerator<KeyValuePair<string, string>> enumerator = parameters.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string key = enumerator.Current.Key;
				string value = enumerator.Current.Value;
				if (!string.IsNullOrEmpty(key))
				{
					if (flag)
					{
						stringBuilder.Append("&");
					}
					stringBuilder.Append(key);
					stringBuilder.Append("=");
					if (encode == "gb2312")
					{
						stringBuilder.Append(HttpUtility.UrlEncode(value, Encoding.GetEncoding("gb2312")));
					}
					else if (encode == "utf8")
					{
						stringBuilder.Append(HttpUtility.UrlEncode(value, Encoding.UTF8));
					}
					else
					{
						stringBuilder.Append(value);
					}
					flag = true;
				}
			}
			return stringBuilder.ToString();
		}

		private static string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
		{
			Stream stream = null;
			StreamReader streamReader = null;
			try
			{
				stream = rsp.GetResponseStream();
				streamReader = new StreamReader(stream, encoding);
				return streamReader.ReadToEnd();
			}
			finally
			{
				if (streamReader != null)
				{
					streamReader.Close();
				}
				if (stream != null)
				{
					stream.Close();
				}
				if (rsp != null)
				{
					rsp.Close();
				}
			}
		}

		private static Image GetResponseAsImage(HttpWebResponse rsp)
		{
			Stream stream = null;
			StreamReader streamReader = null;
			try
			{
				stream = rsp.GetResponseStream();
				return Image.FromStream(stream);
			}
			finally
			{
				if (streamReader != null)
				{
					streamReader.Close();
				}
				if (stream != null)
				{
					stream.Close();
				}
				if (rsp != null)
				{
					rsp.Close();
				}
			}
		}
	}
}
