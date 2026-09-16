namespace Com.Cn.Tool
{
	public class DeviceInfo
	{
		private OutPutFomat _OutputFormat;

		private decimal _PageWidth;

		private decimal _PageHeight;

		private decimal _MarginTop;

		private decimal _MarginLeft;

		private decimal _MarginRight;

		private decimal _MarginBottom;

		public OutPutFomat OutputFormat
		{
			get
			{
				return _OutputFormat;
			}
			set
			{
				_OutputFormat = value;
			}
		}

		public decimal PageWidth
		{
			get
			{
				return _PageWidth;
			}
			set
			{
				_PageWidth = value;
			}
		}

		public decimal PageHeight
		{
			get
			{
				return _PageHeight;
			}
			set
			{
				_PageHeight = value;
			}
		}

		public decimal MarginTop
		{
			get
			{
				return _MarginTop;
			}
			set
			{
				_MarginTop = value;
			}
		}

		public decimal MarginLeft
		{
			get
			{
				return _MarginLeft;
			}
			set
			{
				_MarginLeft = value;
			}
		}

		public decimal MarginRight
		{
			get
			{
				return _MarginRight;
			}
			set
			{
				_MarginRight = value;
			}
		}

		public decimal MarginBottom
		{
			get
			{
				return _MarginBottom;
			}
			set
			{
				_MarginBottom = value;
			}
		}

		public DeviceInfo()
		{
		}

		public DeviceInfo(OutPutFomat _OutputFormat, decimal _PageWidth, decimal _PageHeight, decimal _MarginTop, decimal _MarginLeft, decimal _MarginRight, decimal _MarginBottom)
		{
			this._OutputFormat = _OutputFormat;
			this._PageWidth = _PageWidth;
			this._PageHeight = _PageHeight;
			this._MarginTop = _MarginTop;
			this._MarginLeft = _MarginLeft;
			this._MarginRight = _MarginRight;
			this._MarginBottom = _MarginBottom;
		}
	}
}
