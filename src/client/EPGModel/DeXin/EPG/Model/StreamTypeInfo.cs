using DeXin.EPG.Tools;

namespace DeXin.EPG.Model
{
	public class StreamTypeInfo : BaseTable
	{
		private int _StreamType = 0;

		private string _StreamName = string.Empty;

		public int StreamType
		{
			get
			{
				return _StreamType;
			}
			set
			{
				_StreamType = value;
			}
		}

		public string StreamName
		{
			get
			{
				return _StreamName;
			}
			set
			{
				_StreamName = value;
			}
		}

		public StreamTypeInfo()
		{
			_TableType = EnumTableType.PMT;
		}

		public StreamTypeInfo(long identity, string tableName)
			: base(identity, tableName)
		{
		}
	}
}
