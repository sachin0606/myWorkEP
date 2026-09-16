namespace DeXin.EPG.Model
{
	public class MultipleipSettings
	{
		private long _Id = 0L;

		private long _TsTableId = 0L;

		private string _Ip = string.Empty;

		private int _Port = 0;

		public long Id
		{
			get
			{
				return _Id;
			}
			set
			{
				_Id = value;
			}
		}

		public long TsTableId
		{
			get
			{
				return _TsTableId;
			}
			set
			{
				_TsTableId = value;
			}
		}

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
