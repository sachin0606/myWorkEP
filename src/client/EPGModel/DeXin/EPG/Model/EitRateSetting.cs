namespace DeXin.EPG.Model
{
	public class EitRateSetting
	{
		private string _RateCode;

		private string _RateName;

		private int _RateValue;

		public string RateCode
		{
			get
			{
				return _RateCode;
			}
			set
			{
				_RateCode = value;
			}
		}

		public string RateName
		{
			get
			{
				return _RateName;
			}
			set
			{
				_RateName = value;
			}
		}

		public int RateValue
		{
			get
			{
				return _RateValue;
			}
			set
			{
				_RateValue = value;
			}
		}
	}
}
