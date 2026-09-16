namespace DeXin.EPG.Model
{
	public class SystemSetting
	{
		private SystemSettingCode _Key = SystemSettingCode.DBVERSION;

		private string _Value = string.Empty;

		private string _Comment = string.Empty;

		public SystemSettingCode Key
		{
			get
			{
				return _Key;
			}
			private set
			{
				_Key = value;
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
				_Value = value;
			}
		}

		public string Comment
		{
			get
			{
				return _Comment;
			}
			set
			{
				_Comment = value;
			}
		}

		public SystemSetting(SystemSettingCode _code)
		{
			Key = _code;
		}
	}
}
