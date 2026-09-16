using System;

namespace TsSenders
{
	public abstract class SenderBase : IDisposable
	{
		protected const byte syns_byte = 71;

		protected const int package_size = 188;

		private bool _Loop;

		private SenderModels _SenderModel = SenderModels.IP;

		private RunningStates _RunningState;

		protected decimal send_total_package_count { get; set; }

		protected decimal total_package_count_bymillisecond { get; set; }

		public DateTime StartTime { get; protected set; }

		protected decimal send_total_milliseconds
		{
			get
			{
				return (decimal)(DateTime.Now - StartTime).TotalMilliseconds;
			}
		}

		public SenderModels SenderModel
		{
			get
			{
				return _SenderModel;
			}
			set
			{
				_SenderModel = value;
			}
		}

		public bool Loop
		{
			get
			{
				return _Loop;
			}
			set
			{
				_Loop = value;
			}
		}

		public RunningStates RunningState
		{
			get
			{
				return _RunningState;
			}
			set
			{
				_RunningState = value;
			}
		}

		public void MyDispose()
		{
		}

		public void Dispose()
		{
			RunningState = RunningStates.Stop;
			MyDispose();
		}
	}
}
