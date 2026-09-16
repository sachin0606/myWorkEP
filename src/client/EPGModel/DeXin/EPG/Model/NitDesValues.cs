using DeXin.EPG.Tools;

namespace DeXin.EPG.Model
{
	public class NitDesValues : DescriptorSigleBase
	{
		private long _Id = 0L;

		private long _NitTableId = 0L;

		private long _DesTypeTableId = 0L;

		private byte[] _DesValue = new byte[255];

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

		public long DesTypeTableId
		{
			get
			{
				return _DesTypeTableId;
			}
			set
			{
				_DesTypeTableId = value;
			}
		}

		public byte[] DesValue
		{
			get
			{
				return _DesValue;
			}
			set
			{
				_DesValue = value;
			}
		}

		public NitDesValues(EnumPossibleLoaction plocation, long ptableId, long pdesTypeID, int plength, byte[] pdata)
			: base(plocation, ptableId, pdesTypeID, plength, pdata)
		{
			NitTableId = ptableId;
			DesTypeTableId = pdesTypeID;
			DesValue = pdata;
		}
	}
}
