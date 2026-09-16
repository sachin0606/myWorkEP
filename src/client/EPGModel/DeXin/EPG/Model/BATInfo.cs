using System.ComponentModel;
using DeXin.EPG.Tools;

namespace DeXin.EPG.Model
{
	[TypeDescriptionProvider(typeof(MyTypeDescriptionProvider))]
	public class BATInfo : BaseTable
	{
		private long _NitTableId = 0L;

		private int _BATId = 0;

		private string _BATName;

		private int _BATVersionNumber = 0;

		private NitInfo _NIT;

		public long NitTableId
		{
			get
			{
				return _NitTableId;
			}
			set
			{
				_NitTableId = value;
			}
		}

		public int BATId
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

		public string BATName
		{
			get
			{
				return _BATName;
			}
			set
			{
				_BATName = value;
			}
		}

		public int BATVersionNumber
		{
			get
			{
				return _BATVersionNumber;
			}
			set
			{
				_BATVersionNumber = value;
			}
		}

		public NitInfo NIT
		{
			get
			{
				if (_NIT == null && this.ReadNIT != null)
				{
					this.ReadNIT(this);
				}
				return _NIT;
			}
			set
			{
				_NIT = value;
			}
		}

		public event GetExtendPropertyHandeller ReadNIT;

		public BATInfo()
		{
			_TableType = EnumTableType.BAT;
		}

		public BATInfo(long identity, string tableName)
			: base(identity, tableName)
		{
		}
	}
}
