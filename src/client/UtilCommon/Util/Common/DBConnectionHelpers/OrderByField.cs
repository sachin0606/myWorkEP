namespace Util.Common.DBConnectionHelpers
{
	public class OrderByField
	{
		private string _FieldName;

		private bool _IsAsc = true;

		public string FieldName
		{
			get
			{
				return _FieldName;
			}
			set
			{
				_FieldName = value;
			}
		}

		public bool IsAsc
		{
			get
			{
				return _IsAsc;
			}
			set
			{
				_IsAsc = value;
			}
		}

		public OrderByField(string fieldName, bool isAsc = true)
		{
			FieldName = fieldName;
			IsAsc = isAsc;
		}

		public override string ToString()
		{
			return string.Format("{0} {1}", FieldName, IsAsc);
		}

		public string ToAsc()
		{
			return string.Format("{0} {1}", FieldName, IsAsc ? "ASC" : "DESC");
		}

		public string ToDesc()
		{
			return string.Format("{0} {1}", FieldName, IsAsc ? "DESC" : "ASC");
		}
	}
}
