using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Threading;

namespace EPGServiceBLL.SendOut.NetSocket
{
	public class NamedPipeServerManage
	{
		private static NamedPipeServerManage _Instance;

		private NamedPipeServerStream pipeServer;

		public static NamedPipeServerManage Instance
		{
			get
			{
				if (_Instance == null)
				{
					_Instance = new NamedPipeServerManage();
				}
				return _Instance;
			}
		}

		public void StartNamedPipeServer()
		{
			pipeServer = new NamedPipeServerStream("PSISIService", PipeDirection.InOut, 4, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
			ThreadPool.QueueUserWorkItem(delegate
			{
				if (!pipeServer.IsConnected)
				{
					pipeServer.BeginWaitForConnection(AcceptNamedPipeConnect, pipeServer);
				}
			});
		}

		public void AcceptNamedPipeConnect(IAsyncResult result)
		{
			NamedPipeServerStream namedPipeServerStream = (NamedPipeServerStream)result.AsyncState;
			namedPipeServerStream.EndWaitForConnection(result);
			while (namedPipeServerStream.CanRead)
			{
				if (!pipeServer.IsConnected)
				{
					StartNamedPipeServer();
					break;
				}
				StreamReader streamReader = new StreamReader(namedPipeServerStream);
				string text = streamReader.ReadLine();
				if (string.IsNullOrEmpty(text))
				{
					Thread.Sleep(10);
				}
				else if (text.Equals("GSpeed"))
				{
					StreamWriter streamWriter = new StreamWriter(namedPipeServerStream);
					streamWriter.WriteLine(TSOutputManager.CurrentSpeedTable.Sum((KeyValuePair<IPEndPoint, decimal> x) => x.Value).ToString("N4", CultureInfo.CurrentCulture));
					streamWriter.Flush();
				}
			}
		}
	}
}
