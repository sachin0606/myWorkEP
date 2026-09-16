using Com.Cn.Tool.Model;
using DeXin.EPG.Tools;

namespace DeXin.EPG.Model
{
	public class PosibleLocation : BaseKey
	{
		private EnumPossibleLoaction _LocationKey = EnumPossibleLoaction.NIT;

		public EnumPossibleLoaction LocationKey
		{
			get
			{
				return _LocationKey;
			}
			set
			{
				_LocationKey = value;
			}
		}

		public string LocatingName
		{
			get
			{
				return LocationKey.ToString();
			}
		}

		public PosibleLocation(EnumPossibleLoaction pkey)
		{
			LocationKey = pkey;
			base.BKKey = pkey.ToString();
			base.BKValue = LocatingName;
			base.BKStatus = true;
			Node = this;
		}
	}
}
