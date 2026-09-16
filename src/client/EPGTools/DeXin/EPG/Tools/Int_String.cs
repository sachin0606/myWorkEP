namespace DeXin.EPG.Tools
{
	public class Int_String
	{
		private int _Length = 1;

		private string _Value = string.Empty;

		public int Lengrh
		{
			get
			{
				return _Length;
			}
		}

		public string Value
		{
			get
			{
				return _Value;
			}
			set
			{
				_Value = ((value == null) ? "" : value);
			}
		}

		public Int_String(int len, string val)
		{
			_Length = len;
			Value = val;
		}

		public string ShowValue(int length)
		{
			return Value.Insert(4, ".");
		}
	}
}
