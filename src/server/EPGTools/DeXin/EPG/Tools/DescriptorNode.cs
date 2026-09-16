using System;

namespace DeXin.EPG.Tools
{
	[Serializable]
	public class DescriptorNode
	{
		private long _ID = 0L;

		private EnumPossibleLoaction _DesLocation = EnumPossibleLoaction.NIT;

		private BaseTable _network = null;

		private BaseTable _ts = null;

		private BaseTable _pmt = null;

		private object _media = null;

		private BaseTable _bat = null;

		private BaseTable _eit = null;

		public long ID
		{
			get
			{
				return _ID;
			}
			set
			{
				_ID = value;
			}
		}

		public EnumPossibleLoaction DesLocation
		{
			get
			{
				return _DesLocation;
			}
			set
			{
				_DesLocation = value;
			}
		}

		public BaseTable network
		{
			get
			{
				return _network;
			}
			set
			{
				_network = value;
			}
		}

		public BaseTable ts
		{
			get
			{
				return _ts;
			}
			set
			{
				_ts = value;
			}
		}

		public BaseTable pmt
		{
			get
			{
				return _pmt;
			}
			set
			{
				_pmt = value;
			}
		}

		public object media
		{
			get
			{
				return _media;
			}
			set
			{
				_media = value;
			}
		}

		public BaseTable bat
		{
			get
			{
				return _bat;
			}
			set
			{
				_bat = value;
			}
		}

		public BaseTable eit
		{
			get
			{
				return _eit;
			}
			set
			{
				_eit = value;
			}
		}
	}
}
