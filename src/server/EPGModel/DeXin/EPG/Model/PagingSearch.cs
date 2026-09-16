namespace DeXin.EPG.Model
{
	public class PagingSearch
	{
		private string _PKName;

		private string _ColumsNames;

		private string _TableName;

		private string _Condition;

		private string _OrderBy;

		private int _PageIndex;

		private int _PageSize;

		private int _TotalCount;

		public string PKName
		{
			get
			{
				return _PKName;
			}
			set
			{
				_PKName = value;
			}
		}

		public string ColumsNames
		{
			get
			{
				return _ColumsNames;
			}
			set
			{
				_ColumsNames = value;
			}
		}

		public string TableName
		{
			get
			{
				return _TableName;
			}
			set
			{
				_TableName = value;
			}
		}

		public string Condition
		{
			get
			{
				return _Condition;
			}
			set
			{
				_Condition = value;
			}
		}

		public string OrderBy
		{
			get
			{
				return _OrderBy;
			}
			set
			{
				_OrderBy = value;
			}
		}

		public int PageIndex
		{
			get
			{
				return _PageIndex;
			}
			set
			{
				_PageIndex = value;
			}
		}

		public int PageSize
		{
			get
			{
				return _PageSize;
			}
			set
			{
				_PageSize = value;
			}
		}

		public int TotalCount
		{
			get
			{
				return _TotalCount;
			}
			set
			{
				_TotalCount = value;
			}
		}
	}
}
