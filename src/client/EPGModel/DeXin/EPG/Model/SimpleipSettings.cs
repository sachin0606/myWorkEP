namespace DeXin.EPG.Model
{
	public class SimpleipSettings
	{
		private string _Ip = string.Empty;

		private int _Port = 0;

		public string Ip
		{
			get
			{
				return _Ip;
			}
			set
			{
				_Ip = value;
			}
		}

		public int Port
		{
			get
			{
				return _Port;
			}
			set
			{
				_Port = value;
			}
		}
	}
}
