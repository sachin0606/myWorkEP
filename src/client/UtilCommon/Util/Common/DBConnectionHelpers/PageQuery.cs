using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Util.Common.DBConnectionHelpers
{
	public class PageQuery<T> where T : DbParameter, new()
	{
		private int _PageIndex;

		private int _PageSize = 20;

		private int _TotalCount;

		private string _TableName = string.Empty;

		private string _FieldNames = "*";

		private string _Condiction = string.Empty;

		private List<OrderByField> _OrderBy = new List<OrderByField>();

		private List<T> _CmdParameters = new List<T>();

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

		public int TotalPage
		{
			get
			{
				return (TotalCount - 1) / PageSize + 1;
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

		public string FieldNames
		{
			get
			{
				return _FieldNames;
			}
			set
			{
				_FieldNames = value;
			}
		}

		public string Condiction
		{
			get
			{
				return _Condiction;
			}
			set
			{
				_Condiction = value;
			}
		}

		public List<OrderByField> OrderBy
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

		public List<T> CmdParameters
		{
			get
			{
				return _CmdParameters;
			}
			set
			{
				_CmdParameters = value;
			}
		}

		public PageQuery(string tableName, List<OrderByField> orderby, string fieldnames = "*")
		{
			TableName = tableName;
			FieldNames = fieldnames;
			OrderBy = orderby;
		}

		public string GetSql()
		{
			string format = "SELECT *\r\n            \t                    FROM\r\n            \t                    (\r\n            \t\t                    SELECT TOP (@TopSize) *\r\n            \t\t                    FROM\r\n            \t\t                    (\r\n            \t\t\t                    SELECT TOP (@SkipSize + @TopSize)  {1} \r\n            \t\t\t                    FROM {0}\r\n                                            WHERE 1 = 1 {2}\r\n            \t\t\t                    ORDER BY {3}\r\n            \t\t                    ) t1\r\n            \t\t                    ORDER BY {4} \r\n            \t                    ) t2\r\n            \t                    ORDER BY {3}\r\n                                    ";
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			for (int i = 0; i < OrderBy.Count; i++)
			{
				OrderByField orderByField = OrderBy[i];
				stringBuilder.Append(orderByField.ToAsc());
				stringBuilder2.Append(orderByField.ToDesc());
				if (i < OrderBy.Count - 1)
				{
					stringBuilder.Append(",");
					stringBuilder2.Append(",");
				}
			}
			return string.Format(format, TableName, FieldNames, Condiction, stringBuilder, stringBuilder2);
		}

		public List<T> GetParameters()
		{
			if (PageIndex < 0)
			{
				PageIndex = 0;
			}
			if (PageIndex > TotalPage - 1)
			{
				PageIndex = TotalPage - 1;
			}
			int num = PageSize;
			int num2 = PageSize * PageIndex;
			if (TotalCount < PageSize * (PageIndex + 1))
			{
				num = TotalCount - PageSize * PageIndex;
			}
			List<T> list = new List<T>();
			if (CmdParameters != null)
			{
				list.AddRange(CmdParameters);
			}
			list.Add(new T
			{
				ParameterName = "@TopSize",
				Value = num,
				DbType = DbType.Int32
			});
			list.Add(new T
			{
				ParameterName = "@SkipSize",
				Value = num2,
				DbType = DbType.Int32
			});
			return list;
		}
	}
}
