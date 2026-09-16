using System.Collections.Generic;
using DeXin.EPG.Tools;

namespace DeXin.EPG.Model
{
	public class NitInfo : BaseTable
	{
		private int _NetworkId = 0;

		private string _NetworkName = string.Empty;

		private int _VersionNumber = 0;

		private string _LanguageCode;

		private IList<TsInfo> _SendTS = new List<TsInfo>();

		private IList<BATInfo> _SendBAT = new List<BATInfo>();

		public int NetworkId
		{
			get
			{
				return _NetworkId;
			}
			set
			{
				_NetworkId = value;
			}
		}

		public string NetworkName
		{
			get
			{
				return _NetworkName;
			}
			set
			{
				_NetworkName = value;
			}
		}

		public int VersionNumber
		{
			get
			{
				return _VersionNumber;
			}
			set
			{
				_VersionNumber = value;
			}
		}

		public string LanguageCode
		{
			get
			{
				return _LanguageCode;
			}
			set
			{
				_LanguageCode = value;
			}
		}

		public IList<TsInfo> SendTS
		{
			get
			{
				return _SendTS;
			}
			set
			{
				_SendTS = value;
			}
		}

		public IList<BATInfo> SendBAT
		{
			get
			{
				return _SendBAT;
			}
			set
			{
				_SendBAT = value;
			}
		}

		public NitInfo()
		{
			_TableType = EnumTableType.NIT;
		}

		public NitInfo(long identity, string tableName)
			: base(identity, tableName)
		{
			_TableType = EnumTableType.NIT;
		}

		public override string ToString()
		{
			return string.Format("{1}({0})", NetworkId.ToString("X2"), NetworkName);
		}
	}
}
