using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Util.Common
{
	public class SendPlug : IDisposable
	{
		private static SendPlug _Ins;

		private IPEndPoint _EndPoint = new IPEndPoint(IPAddress.Parse("224.2.2.2"), 5001);

		private List<MyMessage> Messages = new List<MyMessage>();

		private bool _IsDisposed;

		public static SendPlug Ins
		{
			get
			{
				if (_Ins == null)
				{
					_Ins = new SendPlug();
				}
				return _Ins;
			}
		}

		public IPEndPoint EndPoint
		{
			get
			{
				return _EndPoint;
			}
			set
			{
				_EndPoint = value;
			}
		}

		public bool IsDisposed
		{
			get
			{
				return _IsDisposed;
			}
			private set
			{
				_IsDisposed = value;
			}
		}

		private SendPlug()
		{
		}

		public void Start()
		{
			IsDisposed = false;
			new Thread(Send).Start();
			new Thread(ClearMessage).Start();
		}

		public void Stop()
		{
			Dispose();
		}

		private void ClearMessage()
		{
			while (!IsDisposed)
			{
				lock (Messages)
				{
					Messages = Messages.Where((MyMessage p) => p.State).ToList();
				}
				Thread.Sleep(5000);
			}
		}

		private MyMessage Get()
		{
			lock (Messages)
			{
				return Messages.OrderByDescending((MyMessage p) => p.Priority).FirstOrDefault((MyMessage p) => p.State);
			}
		}

		private void Send()
		{
			using (UdpClient udpClient = new UdpClient())
			{
				udpClient.Client.SendTimeout = 20000;
				while (!IsDisposed)
				{
					MyMessage myMessage = Get();
					if (myMessage == null)
					{
						Thread.Sleep(1000);
						continue;
					}
					byte[] array = myMessage.ToBuff();
					udpClient.Send(array, array.Length, EndPoint);
					myMessage.State = false;
					Thread.Sleep(100);
				}
			}
		}

		public void AddMessage(byte[] buff, int priority)
		{
			lock (Messages)
			{
				Messages.Add(new MyMessage(buff, priority));
			}
		}

		public void Dispose()
		{
			IsDisposed = true;
			lock (Messages)
			{
				Messages.Clear();
			}
		}
	}
}
