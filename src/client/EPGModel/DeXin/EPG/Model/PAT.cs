using System;

namespace DeXin.EPG.Model
{
	public class PAT
	{
		private long _Id;

		private long _ProgramTableId;

		private DateTime _CreateTime = DateTime.Now;

		private string _UserId = string.Empty;

		private string _Version;

		public long Id
		{
			get
			{
				return _Id;
			}
			set
			{
				_Id = value;
			}
		}

		public long ProgramTableId
		{
			get
			{
				return _ProgramTableId;
			}
			set
			{
				_ProgramTableId = value;
			}
		}

		public DateTime CreateTime
		{
			get
			{
				return _CreateTime;
			}
			set
			{
				_CreateTime = value;
			}
		}

		public string UserId
		{
			get
			{
				return _UserId;
			}
			set
			{
				_UserId = value;
			}
		}

		public string Version
		{
			get
			{
				return _Version;
			}
			set
			{
				_Version = value;
			}
		}
	}
}
