using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TsSenders
{
	public class FileSenderBase : SenderBase
	{
		public const int MaxBufferSize = 8192;

		protected List<byte[]> Packages = new List<byte[]>();

		private byte[] Buffers = new byte[0];

		public decimal Rate { get; set; }

		protected int min_send_package_count { get; set; }

		public int calc_min_send_package_count { get; set; }

		protected int Buffer_Index { get; set; }

		protected decimal bytes_bysecond { get; private set; }

		protected decimal package_total_count_bysecond { get; private set; }

		protected decimal bytes_bymillisecond { get; private set; }

		protected decimal package_total_count_bymillisecond { get; private set; }

		public int BufferSize { get; set; }

		public void SetRate(decimal rate)
		{
			if (rate < 0m)
			{
				throw new Exception(string.Format("rate {0} error!", rate));
			}
			Rate = rate;
			bytes_bysecond = rate * 1024m * 1024m;
			package_total_count_bysecond = bytes_bysecond / 188m;
			if (package_total_count_bysecond < 1m)
			{
				package_total_count_bysecond = 1m;
			}
			bytes_bymillisecond = bytes_bysecond / 1000m;
			package_total_count_bymillisecond = bytes_bymillisecond / 188m;
			if (package_total_count_bymillisecond < 1m)
			{
				package_total_count_bymillisecond = 1m;
			}
			min_send_package_count = (int)(package_total_count_bymillisecond % 100m * 2m);
			if (min_send_package_count < 1)
			{
				min_send_package_count = 1;
			}
			base.total_package_count_bymillisecond = bytes_bymillisecond / 188m;
		}

		protected void LoadBuffers(Stream stream)
		{
			Buffer_Index = 0;
			BufferSize = 8192;
			if (Buffers.Length == 0)
			{
				Buffers = new byte[BufferSize];
			}
			Array.Clear(Buffers, 0, Buffers.Length);
			if (stream.Length - stream.Position < 8192)
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
			Array.Copy(array, Buffers, array.Length);
		}

		protected void DoStream(Stream stream, SendHandle sendaction, CheckPackageHandle checkpackage)
		{
			int num = 0;
			Buffer_Index = 0;
			base.StartTime = DateTime.Now;
			base.send_total_package_count = 0m;
			LoadBuffers(stream);
			while (base.RunningState == RunningStates.Run && Buffer_Index < BufferSize)
			{
				while (Packages.Count < min_send_package_count)
				{
					if (Buffer_Index + 188 + 1 > BufferSize)
					{
						stream.Position -= BufferSize - Buffer_Index;
						LoadBuffers(stream);
						if (BufferSize == 0)
						{
							base.RunningState = RunningStates.Stop;
							break;
						}
					}
					if (Buffers[Buffer_Index] != 71)
					{
						Buffer_Index++;
						continue;
					}
					if (BufferSize > 188 && Buffers[Buffer_Index + 188] != 71)
					{
						Buffer_Index++;
						continue;
					}
					byte[] array = new byte[188];
					Array.Copy(Buffers, Buffer_Index, array, 0, 188);
					Packages.Add(array);
					Buffer_Index += 188;
				}
				sendaction(Packages);
				base.send_total_package_count += (decimal)Packages.Count;
				Packages.Clear();
				num = checkpackage(null);
				if (num > 0)
				{
					min_send_package_count = calc_min_send_package_count;
					Console.WriteLine("================{0:0}================", num);
					Thread.Sleep(num);
				}
				else
				{
					min_send_package_count++;
				}
			}
		}

		protected void LoadToBuffers(Stream stream)
		{
			stream.Position -= BufferSize - Buffer_Index;
			Buffer_Index = 0;
			BufferSize = 8192;
			if (Buffers.Length == 0)
			{
				Buffers = new byte[BufferSize];
			}
			Array.Clear(Buffers, 0, Buffers.Length);
			if (stream.Length - stream.Position < BufferSize)
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
			Array.Copy(array, Buffers, array.Length);
		}

		protected void DoStream(Stream stream, SendHandle sendaction)
		{
			int num = 0;
			Buffer_Index = 0;
			base.StartTime = DateTime.Now;
			base.send_total_package_count = 0m;
			LoadBuffers(stream);
			while (base.RunningState == RunningStates.Run && Buffer_Index < BufferSize)
			{
				while (Packages.Count < min_send_package_count)
				{
					if (Buffer_Index + 188 + 1 > BufferSize)
					{
						stream.Position -= BufferSize - Buffer_Index;
						LoadBuffers(stream);
						if (BufferSize == 0)
						{
							base.RunningState = RunningStates.Stop;
							break;
						}
					}
					if (Buffers[Buffer_Index] != 71)
					{
						Buffer_Index++;
						continue;
					}
					if (BufferSize > 188 && Buffers[Buffer_Index + 188] != 71)
					{
						Buffer_Index++;
						continue;
					}
					byte[] array = new byte[188];
					Array.Copy(Buffers, Buffer_Index, array, 0, 188);
					if (Rate <= 0m)
					{
						int num2 = 0;
						num2 = CheckSendRate(array);
						if (num2 > num)
						{
							num = num2;
						}
					}
					Packages.Add(array);
					Buffer_Index += 188;
				}
				sendaction(Packages);
				base.send_total_package_count += (decimal)Packages.Count;
				Packages.Clear();
				if (Rate > 0m)
				{
					num = CheckThreadSleepTimeSpanWithRate();
					if (num > 0)
					{
						min_send_package_count = calc_min_send_package_count;
						Console.WriteLine("================{0:0}***********Rate:{1:0}================", num, Rate);
						Thread.Sleep(num);
					}
					else
					{
						min_send_package_count++;
					}
				}
				else if (num > 0)
				{
					Console.WriteLine("================{0:0}================", num);
					Thread.Sleep(num);
				}
			}
		}

		private int CheckSendRate(byte[] temp)
		{
			return 0;
		}

		public virtual void Send(string filename, UdpClient client, IPEndPoint ipendpoint)
		{
			try
			{
				using (FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read))
				{
					do
					{
						fileStream.Position = 0L;
						if (base.RunningState == RunningStates.Stop)
						{
							base.RunningState = RunningStates.Run;
						}
						DoStream(fileStream, delegate(List<byte[]> packages)
						{
							SenderHelper.Send(client, ipendpoint, packages.ToArray());
						});
					}
					while (base.RunningState == RunningStates.Stop && base.Loop);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			finally
			{
				Dispose();
			}
		}

		public int CheckThreadSleepTimeSpanWithRate()
		{
			decimal num = base.send_total_milliseconds * package_total_count_bymillisecond;
			if (num <= 0m)
			{
				return 0;
			}
			decimal num2 = base.send_total_package_count - num;
			Console.WriteLine("{0:0}/{1:0}--------相差(ms):{2:0}--------count:{3}------rate:{4:0.0000}K/s", base.send_total_package_count, num, num2, min_send_package_count, base.send_total_package_count * 188m * 1000m / (base.send_total_milliseconds * 1024m));
			return (int)num2;
		}
	}
}
