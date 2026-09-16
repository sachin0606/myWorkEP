using System;

namespace DeXin.EPG.Model
{
	[Serializable]
	public class UserInfo
	{
		private string _UserID;

		private string _UserName;

		private string _Password;

		private string _GroupID;

		private bool _Sex = true;

		private string _PhoneNumber;

		private string _Creator;

		private DateTime? _CreateTime;

		private string _LastEditor;

		private DateTime? _LastEditTime;

		private bool _Status = true;

		private LogInfo _log = new LogInfo();

		public string UserID
		{
			get
			{
				return _UserID;
			}
			set
			{
				_UserID = value;
			}
		}

		public string UserName
		{
			get
			{
				return _UserName;
			}
			set
			{
				_UserName = value;
			}
		}

		public string Password
		{
			get
			{
				return _Password;
			}
			set
			{
				_Password = value;
			}
		}

		public string GroupID
		{
			get
			{
				return _GroupID;
			}
			set
			{
				_GroupID = value;
			}
		}

		public bool Sex
		{
			get
			{
				return _Sex;
			}
			set
			{
				_Sex = value;
			}
		}

		public string PhoneNumber
		{
			get
			{
				return _PhoneNumber;
			}
			set
			{
				_PhoneNumber = value;
			}
		}

		public string Creator
		{
			get
			{
				return _Creator;
			}
			set
			{
				_Creator = value;
			}
		}

		public DateTime? CreateTime
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

		public string LastEditor
		{
			get
			{
				return _LastEditor;
			}
			set
			{
				_LastEditor = value;
			}
		}

		public DateTime? LastEditTime
		{
			get
			{
				return _LastEditTime;
			}
			set
			{
				_LastEditTime = value;
			}
		}

		public bool Status
		{
			get
			{
				return _Status;
			}
			set
			{
				_Status = value;
			}
		}

		public LogInfo log
		{
			get
			{
				return _log;
			}
			set
			{
				_log = value;
			}
		}
	}
}
