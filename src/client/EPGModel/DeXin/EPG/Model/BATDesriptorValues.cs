using DeXin.EPG.Tools;

namespace DeXin.EPG.Model
{
	public class BATDesriptorValues : DescriptorSigleBase
	{
		private long _Id = 0L;

		private long _BatTableId = 0L;

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

		public long BatTableId
		{
			get
			{
				return _BatTableId;
			}
			set
			{
				_BatTableId = value;
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
	}
}
