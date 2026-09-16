using System;

namespace TsSenders
{
	public class PalyerSender : SenderBase
	{
		private double _Rate = 150.0;

		public double Rate
		{
			get
			{
				return _Rate;
			}
			private set
			{
				_Rate = value;
			}
		}

		public void SetRate(double rate)
		{
			if (rate < 0.0)
			{
				throw new Exception(string.Format("rate {0} error!", rate));
			}
			Rate = rate;
		}

		public void Check()
		{
			if (!TsPlayer.IsOpen)
			{
				TsPlayer.OpenDevice(true);
				TsPlayer.SetRate(Rate);
			}
		}
	}
}
