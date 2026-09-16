using System.Collections.Generic;
using DeXin.EPG.Tools;

namespace DeXin.EPG.Model
{
	public class TsInfo : BaseTable
	{
		private long _NitTableId = 0L;

		private int _TsId = 0;

		private string _TsName = string.Empty;

		private int _OriginalNetworkId;

		private int _PatVersionNumber;

		private int _CatVersionNumber;

		private int _SdtVersionNumber;

		private NitInfo _NIT;

		private IList<DescriptorSigleBase> _Descriptors = new List<DescriptorSigleBase>();

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

		public int TsId
		{
			get
			{
				return _TsId;
			}
			set
			{
				_TsId = value;
			}
		}

		public string TsName
		{
			get
			{
				return _TsName;
			}
			set
			{
				_TsName = value;
			}
		}

		public int OriginalNetworkId
		{
			get
			{
				return _OriginalNetworkId;
			}
			set
			{
				_OriginalNetworkId = value;
			}
		}

		public int PatVersionNumber
		{
			get
			{
				return _PatVersionNumber;
			}
			set
			{
				_PatVersionNumber = value;
			}
		}

		public int CatVersionNumber
		{
			get
			{
				return _CatVersionNumber;
			}
			set
			{
				_CatVersionNumber = value;
			}
		}

		public int SdtVersionNumber
		{
			get
			{
				return _SdtVersionNumber;
			}
			set
			{
				_SdtVersionNumber = value;
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

		public IList<DescriptorSigleBase> Descriptors
		{
			get
			{
				return _Descriptors;
			}
			set
			{
				_Descriptors = value;
			}
		}

		public event GetExtendPropertyHandeller ReadNIT;

		public TsInfo()
		{
			_TableType = EnumTableType.TS;
		}

		public TsInfo(long identity, string tableName)
			: base(identity, tableName)
		{
			_TableType = EnumTableType.TS;
		}

		public override string ToString()
		{
			return string.Format("{1}({0})", TsId.ToString("X2"), TsName);
		}
	}
}
