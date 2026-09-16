using System;

namespace Dexin.Util.Common.Net
{
	public abstract class SendBase
	{
		private const int PACKAGE_SIZE = 188;

		private int send_packages_size = 7;

		private int _MaxBufferSize = 7520;

		private byte[] Buffers = new byte[0];

		public bool Loop { get; set; }

		public decimal Rate { get; set; }

		public int Send_packages_size
		{
			get
			{
				return send_packages_size;
			}
			set
			{
				send_packages_size = value;
			}
		}

		public int MaxBufferSize
		{
			get
			{
				return _MaxBufferSize;
			}
			set
			{
				_MaxBufferSize = value;
			}
		}

		protected decimal send_packages_count { get; private set; }

		protected decimal packages_count_bysecond
		{
			get
			{
				return Rate * 1024m * 1024m / 188m;
			}
		}

		protected DateTime start_time { get; private set; }

		protected decimal send_total_seconds
		{
			get
			{
				return (decimal)(DateTime.Now - start_time).TotalSeconds;
			}
		}

		private int BufferSize { get; set; }

		private int Buffer_Position { get; set; }
	}
}
