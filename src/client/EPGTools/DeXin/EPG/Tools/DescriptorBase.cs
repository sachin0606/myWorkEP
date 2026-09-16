namespace DeXin.EPG.Tools
{
	public class DescriptorBase
	{
		private EnumPossibleLoaction _Location = EnumPossibleLoaction.NIT;

		private long _TableID = 0L;

		private long _DesTypeID = 0L;

		public EnumPossibleLoaction Location
		{
			get
			{
				return _Location;
			}
			set
			{
				_Location = value;
			}
		}

		public long TableID
		{
			get
			{
				return _TableID;
			}
			set
			{
				_TableID = value;
			}
		}

		public long DesTypeID
		{
			get
			{
				return _DesTypeID;
			}
			set
			{
				_DesTypeID = value;
			}
		}
	}
}
