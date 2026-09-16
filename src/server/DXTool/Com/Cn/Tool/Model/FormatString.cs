namespace Com.Cn.Tool.Model
{
	public class FormatString
	{
		private static FormatString _ins;

		public readonly string D = "D";

		public readonly string d = "d";

		public readonly string F = "F";

		public readonly string G = "G";

		public readonly string DataShort = "yyyy-MM-dd";

		public readonly string DateLong = "yyyy-MM-dd HH:mm:ss";

		public readonly string FFF = "yyyy-MM-dd HH:mm:ss.fff";

		public readonly string Number0 = "N0";

		public readonly string Number1 = "N1";

		public readonly string Number2 = "N2";

		public string Number3 = "N3";

		public readonly string Number4 = "N4";

		public readonly string Number5 = "N5";

		public readonly string Number6 = "N6";

		public readonly string X = "X";

		public readonly string X2 = "X2";

		public readonly string X4 = "X4";

		public readonly string X8 = "X8";

		public readonly string X16 = "X16";

		public readonly string Money = "c";

		public static FormatString Ins
		{
			get
			{
				if (_ins == null)
				{
					_ins = new FormatString();
				}
				return _ins;
			}
		}

		protected FormatString()
		{
		}
	}
}
