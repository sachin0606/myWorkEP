using DeXin.EPG.Tools;

namespace DeXin.EPG.Views.TableDescriptor
{
	public class NodeItem
	{
		private long _TableID = 0L;

		private DescriptorLevel _DesLevel = DescriptorLevel.TABLE;

		private EnumTableType _TableType = EnumTableType.NIT;

		private EnumPossibleLoaction _Location = EnumPossibleLoaction.NIT;

		private long _NetWorkId = 0L;

		private long _TSId = 0L;

		private long _ProgramId = 0L;

		private long _MediaId = 0L;

		private long _BATId = 0L;

		private long _BAT_TS_Id = 0L;

		private long _EventId = 0L;

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

		public DescriptorLevel DesLevel
		{
			get
			{
				return _DesLevel;
			}
			set
			{
				_DesLevel = value;
			}
		}

		public EnumTableType TableType
		{
			get
			{
				return _TableType;
			}
			set
			{
				_TableType = value;
			}
		}

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

		public long NetWorkId
		{
			get
			{
				return _NetWorkId;
			}
			set
			{
				_NetWorkId = value;
			}
		}

		public long TSId
		{
			get
			{
				return _TSId;
			}
			set
			{
				_TSId = value;
			}
		}

		public long ProgramId
		{
			get
			{
				return _ProgramId;
			}
			set
			{
				_ProgramId = value;
			}
		}

		public long MediaId
		{
			get
			{
				return _MediaId;
			}
			set
			{
				_MediaId = value;
			}
		}

		public long BATId
		{
			get
			{
				return _BATId;
			}
			set
			{
				_BATId = value;
			}
		}

		public long BAT_TS_Id
		{
			get
			{
				return _BAT_TS_Id;
			}
			set
			{
				_BAT_TS_Id = value;
			}
		}

		public long EventId
		{
			get
			{
				return _EventId;
			}
			set
			{
				_EventId = value;
			}
		}

		public static NodeItem CloneNode(NodeItem item)
		{
			NodeItem nodeItem = new NodeItem();
			nodeItem.TableType = item.TableType;
			nodeItem.DesLevel = item.DesLevel;
			nodeItem.Location = item.Location;
			nodeItem.NetWorkId = item.NetWorkId;
			nodeItem.TSId = item.TSId;
			nodeItem.ProgramId = item.ProgramId;
			nodeItem.BATId = item.BATId;
			return nodeItem;
		}
	}
}
