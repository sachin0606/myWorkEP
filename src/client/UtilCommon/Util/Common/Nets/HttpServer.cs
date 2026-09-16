using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Util.Common.ExtensionHelper;

namespace Util.Common.Nets
{
	public class HttpServer
	{
		private static HttpServer _Ins;

		public static HttpServer Ins
		{
			get
			{
				if (_Ins == null)
				{
					_Ins = new HttpServer();
				}
				return _Ins;
			}
		}

		public HttpListener Listener { get; protected set; }

		public bool IsRuning { get; protected set; }

		public event Action<HttpListenerContext> DoHttpListenerContext;

		private void SimpleListenerExample(params string[] prefixes)
		{
			if (!HttpListener.IsSupported)
			{
				Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
				return;
			}
			if (prefixes == null || prefixes.Length == 0)
			{
				throw new ArgumentException("prefixes");
			}
			HttpListener httpListener = new HttpListener();
			foreach (string uriPrefix in prefixes)
			{
				httpListener.Prefixes.Add(uriPrefix);
			}
			httpListener.Start();
			Console.WriteLine("Listening...");
			HttpListenerContext context = httpListener.GetContext();
			HttpListenerRequest request = context.Request;
			Console.WriteLine(request);
			HttpListenerResponse response = context.Response;
			string s = "<HTML><BODY> Hello world!</BODY></HTML>";
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			response.ContentLength64 = bytes.Length;
			Stream outputStream = response.OutputStream;
			outputStream.Write(bytes, 0, bytes.Length);
			outputStream.Close();
			httpListener.Stop();
		}

		public void Start(int port, params string[] prefixes)
		{
			if (!HttpListener.IsSupported)
			{
				Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
			}
			else
			{
				if (IsRuning)
				{
					return;
				}
				IsRuning = true;
				Listener = new HttpListener();
				Listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
				string formatString = "http://{0}:{1}/";
				bool flag = false;
				bool flag2 = false;
				List<string> list = new List<string>();
				if (prefixes != null)
				{
					foreach (string text in prefixes)
					{
						if (text.ToLower().Equals("localhost"))
						{
							flag = true;
						}
						else if (text.ToLower().Equals("127.0.0.1"))
						{
							flag2 = true;
						}
						list.Add(text);
					}
				}
				if (!flag)
				{
					list.Add("localhost");
				}
				if (!flag2)
				{
					list.Add("127.0.0.1");
				}
				foreach (string item in list)
				{
					Listener.Prefixes.Add(formatString.Fmt(item, port));
				}
				Listener.Start();
				Listener.BeginGetContext(AcceptHttpClient, this);
			}
		}

		private void AcceptHttpClient(IAsyncResult ar)
		{
			HttpServer httpServer = (HttpServer)ar.AsyncState;
			if (httpServer.IsRuning)
			{
				httpServer.Listener.BeginGetContext(httpServer.AcceptHttpClient, ar.AsyncState);
			}
			HttpListenerContext httpListenerContext = httpServer.Listener.EndGetContext(ar);
			httpListenerContext.Response.StatusCode = 200;
			if (httpServer.DoHttpListenerContext != null)
			{
				httpServer.DoHttpListenerContext(httpListenerContext);
			}
		}

		private void TaskProc(object o)
		{
		}

		public void Stop()
		{
			if (IsRuning)
			{
				IsRuning = false;
				try
				{
					Listener.Stop();
					Listener.Close();
				}
				catch (Exception)
				{
				}
			}
		}
	}
}
