using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TsSenders
{
	public class SenderHelper : IDisposable
	{
		private static SenderHelper _Ins;

		public byte syns_byte = 71;

		public int package_size = 188;

		private decimal max_send_packages_count = 1000m;

		public int MaxBufferSize = 7520;

		private byte[] Buffers = new byte[0];

		private int pid;

		private DateTime first_os_time;

		private DateTime last_os_time;

		private long pcr;

		private long first_pcr;

		private long last_pcr;

		private long max_pcr_wobble = 1000L;

		private Thread thread;

		public static SenderHelper Ins
		{
			get
			{
				if (_Ins == null)
				{
					_Ins = new SenderHelper();
				}
				return _Ins;
			}
		}

		public decimal send_packages_count { get; private set; }

		public decimal packages_count_bysecond { get; private set; }

		private decimal default_send_packages_count { get; set; }

		private decimal current_send_packages_count { get; set; }

		public DateTime StartTime { get; private set; }

		protected decimal send_total_seconds
		{
			get
			{
				return (decimal)(DateTime.Now - StartTime).TotalSeconds;
			}
		}

		public SenderModels SenderModel { get; set; }

		public bool loop { get; set; }

		public decimal Rate { get; set; }

		public int BufferSize { get; set; }

		private int Buffer_Position { get; set; }

		public RunningStates RunningState { get; private set; }

		private long pcr_wobble
		{
			get
			{
				long num = pcr - first_pcr;
				return num * 1000 / 27000000;
			}
		}

		private long pcr_last_wobble
		{
			get
			{
				long num = pcr - last_pcr;
				return num * 1000 / 27000000;
			}
		}

		public event Action SenderStpoedHandller;

		private SenderHelper()
		{
		}

		public void Init(bool _loop, SenderModels _sendermodel)
		{
			Reset();
			loop = _loop;
			SenderModel = _sendermodel;
		}

		private void SetRate(decimal _rate)
		{
			if (_rate < 0m)
			{
				throw new Exception(string.Format("rate {0} error!", _rate));
			}
			Rate = _rate;
			decimal num = Rate * 1024m * 1024m;
			packages_count_bysecond = num / (decimal)package_size;
			decimal num2 = num / 1000m;
			default_send_packages_count = Rate * 50m;
			if (default_send_packages_count < 1m)
			{
				default_send_packages_count = 1m;
			}
			current_send_packages_count = default_send_packages_count;
			if (SenderModel == SenderModels.Plyer_Box)
			{
				CheckPlayer(Rate);
			}
		}

		protected void LoadToBuffers(Stream stream)
		{
			Buffer_Position = 0;
			BufferSize = MaxBufferSize;
			if (Buffers.Length < MaxBufferSize)
			{
				Buffers = new byte[MaxBufferSize];
			}
			else
			{
				Array.Clear(Buffers, 0, Buffers.Length);
			}
			if (stream.Length - stream.Position < MaxBufferSize)
			{
				BufferSize = (int)(stream.Length - stream.Position);
				if (BufferSize <= 0)
				{
					BufferSize = 0;
					return;
				}
			}
			byte[] array = new byte[BufferSize];
			if (stream.Read(array, 0, BufferSize) == 0)
			{
				BufferSize = 0;
			}
			Array.Copy(array, 0, Buffers, 0, BufferSize);
		}

		protected void DoStreamWithRate(Stream stream, SendHandle sendaction)
		{
			int num = 0;
			BufferSize = MaxBufferSize;
			Buffer_Position = 0;
			send_packages_count = 0m;
			RunningState = RunningStates.Run;
			StartTime = DateTime.Now;
			List<byte[]> list = new List<byte[]>();
			LoadToBuffers(stream);
			while (RunningState == RunningStates.Run && Buffer_Position < BufferSize)
			{
				while ((decimal)list.Count < current_send_packages_count)
				{
					if (Buffer_Position + package_size + 1 > BufferSize)
					{
						stream.Position -= BufferSize - Buffer_Position;
						LoadToBuffers(stream);
						if (BufferSize == 0)
						{
							RunningState = RunningStates.Stop;
							break;
						}
					}
					if (Buffers[Buffer_Position] != syns_byte)
					{
						Buffer_Position++;
						continue;
					}
					if (Buffer_Position + package_size > BufferSize && Buffers[Buffer_Position + package_size] != syns_byte)
					{
						Buffer_Position++;
						continue;
					}
					byte[] array = new byte[package_size];
					Array.Copy(Buffers, Buffer_Position, array, 0, package_size);
					list.Add(array);
					Buffer_Position += package_size;
				}
				sendaction(list);
				send_packages_count += (decimal)list.Count;
				list.Clear();
				num = (CheckThreadSleepTimeSpanWithRate() + 1) / 2;
				if (num > 0)
				{
					if (current_send_packages_count > default_send_packages_count)
					{
						current_send_packages_count = (default_send_packages_count + current_send_packages_count) / 2m;
					}
					Thread.Sleep(num);
				}
				else
				{
					current_send_packages_count = ((current_send_packages_count > max_send_packages_count) ? max_send_packages_count : (current_send_packages_count + 1m));
				}
			}
			RunningState = RunningStates.Stop;
		}

		protected void DoStreamWithPCR(Stream stream, SendHandle sendaction)
		{
			int num = 0;
			BufferSize = 0;
			Buffer_Position = 0;
			send_packages_count = 0m;
			RunningState = RunningStates.Run;
			StartTime = DateTime.Now;
			List<byte[]> list = new List<byte[]>();
			LoadToBuffers(stream);
			while (RunningState == RunningStates.Run && Buffer_Position < BufferSize)
			{
				while ((decimal)list.Count < current_send_packages_count)
				{
					if (Buffer_Position + package_size + 1 > BufferSize)
					{
						stream.Position -= BufferSize - Buffer_Position;
						LoadToBuffers(stream);
						if (BufferSize == 0)
						{
							RunningState = RunningStates.Stop;
							break;
						}
					}
					if (Buffers[Buffer_Position] != syns_byte)
					{
						Buffer_Position++;
						continue;
					}
					if (Buffer_Position + package_size > BufferSize && Buffers[Buffer_Position + package_size] != syns_byte)
					{
						Buffer_Position++;
						continue;
					}
					byte[] array = new byte[package_size];
					Array.Copy(Buffers, Buffer_Position, array, 0, package_size);
					int num2 = CheckPackage(array);
					if (num2 > num)
					{
						num = num2;
					}
					list.Add(array);
					Buffer_Position += package_size;
				}
				sendaction(list);
				send_packages_count += (decimal)list.Count;
				list.Clear();
				if (num > 0)
				{
					Thread.Sleep(num);
				}
			}
		}

		public int CheckThreadSleepTimeSpanWithRate()
		{
			decimal num = send_total_seconds * packages_count_bysecond;
			if (num <= 0m)
			{
				return 0;
			}
			decimal num2 = send_packages_count - num;
			decimal num3 = num2 / packages_count_bysecond * 1000m;
			Console.WriteLine("===========sleep(ms):{0:0}   send count:{1:0}  rate: {2:0.0000}===========", num3, current_send_packages_count, send_packages_count * (decimal)package_size / 1024m / 1024m / send_total_seconds);
			return (int)num3;
		}

		private int CheckPackage(byte[] package)
		{
			int num = 0;
			if ((package[3] & 0x20) == 0)
			{
				return num;
			}
			if ((package[5] & 0x10) == 0)
			{
				return num;
			}
			if (package[4] < 7)
			{
				return num;
			}
			long num2 = 0L;
			long num3 = 0L;
			int num4 = ((package[1] & 0x1F) << 8) | package[2];
			if (num4 == 8191)
			{
				return num;
			}
			if (pid == 0)
			{
				first_os_time = DateTime.Now;
				pid = num4;
				num2 = ((package[6] << 24) | (package[7] << 16) | (package[8] << 8) | package[9]) & 0xFFFFFFFFu;
				num2 <<= 1;
				num2 |= (byte)(package[10] >> 7);
				num3 = ((package[10] & 1) << 8) | package[11];
				pcr = num2 * 300 + num3;
				first_pcr = pcr;
			}
			else if (pid == num4)
			{
				num2 = ((package[6] << 24) | (package[7] << 16) | (package[8] << 8) | package[9]) & 0xFFFFFFFFu;
				num2 <<= 1;
				num2 |= (byte)(package[10] >> 7);
				num3 = ((package[10] & 1) << 8) | package[11];
				pcr = num2 * 300 + num3;
				DateTime now = DateTime.Now;
				long num5 = (long)(now - first_os_time).TotalMilliseconds;
				num = (int)(pcr_wobble - num5);
				long num6 = (long)(now - last_os_time).TotalMilliseconds;
				Console.WriteLine("fPID:[{0}]\tpcr interval:[{1}]\tos interval:[{2}]\t[{3}]", pid, pcr_last_wobble, num6, num);
				if (pcr_last_wobble < max_pcr_wobble * -1)
				{
					Console.WriteLine("reset wobble little: {0}", num);
					ReSet(now, 0, pcr);
				}
				if (num < 0)
				{
					num = 0;
				}
				last_pcr = pcr;
				last_os_time = now;
			}
			return num;
		}

		private void ReSet(DateTime _dtnow, int _pid, long _pcr)
		{
			first_os_time = _dtnow;
			first_pcr = _pcr;
			pid = _pid;
		}

		internal static void CheckPlayer(decimal rate)
		{
			if (!TsPlayer.IsOpen)
			{
				TsPlayer.OpenDevice(true);
			}
			TsPlayer.SetRate((double)rate);
		}

		internal static void Send(decimal Rate, params byte[][] packages)
		{
			if (packages != null && packages.Length != 0)
			{
				CheckPlayer(Rate);
				packages.ToList().ForEach(delegate(byte[] package)
				{
					TsPlayer.WriteData(package);
				});
			}
		}

		internal static void Send(UdpClient client, IPEndPoint ipendpoint, params byte[][] packages)
		{
			if (packages != null && packages.Length != 0)
			{
				packages.ToList().ForEach(delegate(byte[] package)
				{
					client.Send(package, package.Length, ipendpoint);
				});
			}
		}

		public void Send(string filepath, decimal rate)
		{
			Reset();
			thread = new Thread((ThreadStart)delegate
			{
				using (FileStream fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read))
				{
					do
					{
						if (RunningState == RunningStates.Stop)
						{
							RunningState = RunningStates.Run;
						}
						SetRate(rate);
						fileStream.Position = 0L;
						DoStreamWithRate(fileStream, delegate(List<byte[]> packages)
						{
							Send(rate, packages.ToArray());
						});
					}
					while (RunningState == RunningStates.Stop && loop);
					if (this.SenderStpoedHandller != null)
					{
						this.SenderStpoedHandller();
					}
				}
			});
			thread.Start();
		}

		public void Send(string filepath, UdpClient client, IPEndPoint ipendpoint)
		{
			Reset();
			thread = new Thread((ThreadStart)delegate
			{
				using (FileStream stream = new FileStream(filepath, FileMode.Open, FileAccess.Read))
				{
					do
					{
						if (RunningState == RunningStates.Stop)
						{
							RunningState = RunningStates.Run;
						}
						DoStreamWithRate(stream, delegate(List<byte[]> packages)
						{
							Send(client, ipendpoint, packages.ToArray());
						});
					}
					while (RunningState == RunningStates.Stop && loop);
				}
			});
			thread.Start();
		}

		public void Send(string filepath, decimal rate, UdpClient client, IPEndPoint ipendpoint)
		{
			Reset();
			thread = new Thread((ThreadStart)delegate
			{
				using (FileStream fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read))
				{
					do
					{
						if (RunningState == RunningStates.Stop)
						{
							RunningState = RunningStates.Run;
						}
						SetRate(rate);
						fileStream.Position = 0L;
						DoStreamWithRate(fileStream, delegate(List<byte[]> packages)
						{
							Send(client, ipendpoint, packages.ToArray());
						});
					}
					while (RunningState == RunningStates.Stop && loop);
					if (this.SenderStpoedHandller != null)
					{
						this.SenderStpoedHandller();
					}
				}
			})
			{
				Priority = ThreadPriority.Highest
			};
			thread.Start();
		}

		private void Reset()
		{
			BufferSize = 0;
			Buffer_Position = 0;
			send_packages_count = 0m;
		}

		public void Dispose()
		{
			RunningState = RunningStates.Stop;
			Reset();
			try
			{
				thread.Abort();
			}
			catch
			{
			}
			if (this.SenderStpoedHandller != null)
			{
				this.SenderStpoedHandller();
			}
		}
	}
}
