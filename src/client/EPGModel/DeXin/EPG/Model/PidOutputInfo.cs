namespace DeXin.EPG.Model
{
	public class PidOutputInfo
	{
		private long _Id = 0L;

		private long _TsTableId = 0L;

		private TsInfo _TS;

		private int _Pat = 0;

		private int _Cat = 0;

		private int _SdtOrBat = 0;

		private int _Nit = 0;

		private int _TotOrTdt = 0;

		private int _Eit = 0;

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

		public int Pat
		{
			get
			{
				return _Pat;
			}
			set
			{
				_Pat = value;
			}
		}

		public int Cat
		{
			get
			{
				return _Cat;
			}
			set
			{
				_Cat = value;
			}
		}

		public int SdtOrBat
		{
			get
			{
				return _SdtOrBat;
			}
			set
			{
				_SdtOrBat = value;
			}
		}

		public int Nit
		{
			get
			{
				return _Nit;
			}
			set
			{
				_Nit = value;
			}
		}

		public int TotOrTdt
		{
			get
			{
				return _TotOrTdt;
			}
			set
			{
				_TotOrTdt = value;
			}
		}

		public int Eit
		{
			get
			{
				return _Eit;
			}
			set
			{
				_Eit = value;
			}
		}

		public event GetExtendPropertyHandeller ReadTS;
	}
}
