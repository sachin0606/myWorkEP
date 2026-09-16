using DeXin.EPG.Tools;

namespace DeXin.EPG.Model
{
	public class ProgramInfo : BaseTable
	{
		private long _TsTableId = 0L;

		private int _ServiceId = 0;

		private string _ProgramName = string.Empty;

		private int _ServiceType;

		private string _ProviderName = string.Empty;

		private long _PmtId = 0L;

		private int _PCRPID = 0;

		private int _PmtVersionNumber = 0;

		private int _PrgSdtEitscflag = 0;

		private int _PrgSdtEitpfflag = 0;

		private int _PrgSdtRunningstate = 0;

		private int _PrgSdtFreecamod = 0;

		private int _PrgManualeventid = 0;

		private string _EITFileName;

		private TsInfo _TS;

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

		public int ServiceId
		{
			get
			{
				return _ServiceId;
			}
			set
			{
				_ServiceId = value;
			}
		}

		public string ProgramName
		{
			get
			{
				return _ProgramName;
			}
			set
			{
				_ProgramName = value;
			}
		}

		public int ServiceType
		{
			get
			{
				return _ServiceType;
			}
			set
			{
				_ServiceType = value;
			}
		}

		public string ProviderName
		{
			get
			{
				return _ProviderName;
			}
			set
			{
				_ProviderName = value;
			}
		}

		public long PmtId
		{
			get
			{
				return _PmtId;
			}
			set
			{
				_PmtId = value;
			}
		}

		public int PCRPID
		{
			get
			{
				return _PCRPID;
			}
			set
			{
				_PCRPID = value;
			}
		}

		public int PmtVersionNumber
		{
			get
			{
				return _PmtVersionNumber;
			}
			set
			{
				_PmtVersionNumber = value;
			}
		}

		public int PrgSdtEitscflag
		{
			get
			{
				return _PrgSdtEitscflag;
			}
			set
			{
				_PrgSdtEitscflag = value;
			}
		}

		public int PrgSdtEitpfflag
		{
			get
			{
				return _PrgSdtEitpfflag;
			}
			set
			{
				_PrgSdtEitpfflag = value;
			}
		}

		public int PrgSdtRunningstate
		{
			get
			{
				return _PrgSdtRunningstate;
			}
			set
			{
				_PrgSdtRunningstate = value;
			}
		}

		public int PrgSdtFreecamod
		{
			get
			{
				return _PrgSdtFreecamod;
			}
			set
			{
				_PrgSdtFreecamod = value;
			}
		}

		public int PrgManualeventid
		{
			get
			{
				return _PrgManualeventid;
			}
			set
			{
				_PrgManualeventid = value;
			}
		}

		public string EITFileName
		{
			get
			{
				return _EITFileName;
			}
			set
			{
				_EITFileName = value;
			}
		}

		public TsInfo TS
		{
			get
			{
				if (_TS == null && this.ReadTS != null)
				{
					this.ReadTS(this);
				}
				return _TS;
			}
			set
			{
				_TS = value;
			}
		}

		public event GetExtendPropertyHandeller ReadTS;

		public ProgramInfo()
		{
			_TableType = EnumTableType.PMT;
		}

		public ProgramInfo(long identity, string tableName)
			: base(identity, tableName)
		{
			_TableType = EnumTableType.PMT;
		}

		public override string ToString()
		{
			return string.Format("{0}[0x{1}]-{2}[0x{3}]-{4}[0x{5}]", TS.NIT.NetworkName, TS.NIT.NetworkId.ToString("X4"), TS.TsName, TS.TsId.ToString("X4"), ProgramName, ServiceId.ToString("X4"));
		}
	}
}
