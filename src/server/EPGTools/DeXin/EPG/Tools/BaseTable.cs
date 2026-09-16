using System;
using System.Text;
using Com.Cn.Tool.Model;

namespace DeXin.EPG.Tools
{
	[Serializable]
	public class BaseTable : BaseKey
	{
		protected EnumTableType _TableType = EnumTableType.PMT;

		private long _Identity;

		private string _TableName = string.Empty;

		private BaseTable _ParentTable = null;

		private BaseTable _network = null;

		private BaseTable _ts = null;

		public EnumTableType TableType
		{
			get
			{
				return _TableType;
			}
		}

		public long Identity
		{
			get
			{
				return _Identity;
			}
			set
			{
				_Identity = value;
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

		public BaseTable ParentTable
		{
			get
			{
				return _ParentTable;
			}
			set
			{
				_ParentTable = value;
			}
		}

		public BaseTable network
		{
			get
			{
				return _network;
			}
			set
			{
				_network = value;
			}
		}

		public BaseTable ts
		{
			get
			{
				return _ts;
			}
			set
			{
				_ts = value;
			}
		}

		public BaseTable()
		{
		}

		public BaseTable(long identity, string tableName)
			: base(identity.ToString(), tableName, true)
		{
			Identity = identity;
			TableName = tableName;
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (ParentTable != null)
			{
				stringBuilder.Append(string.Format("[{0}][{1}][{2}] -- ", ParentTable.TableType, ParentTable.Identity, ParentTable.TableName));
			}
			stringBuilder.Append(string.Format("[{0}][{1}][{2}]", TableType, Identity, TableName));
			return stringBuilder.ToString();
		}
	}
}
