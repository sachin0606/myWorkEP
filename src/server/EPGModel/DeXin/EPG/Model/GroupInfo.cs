using System;

namespace DeXin.EPG.Model
{
	public class GroupInfo
	{
		private int _GroupID;

		private string _GroupName;

		private bool _Status;

		private string _Creator;

		private DateTime? _CreateTime;

		private string _LastEditor;

		private DateTime? _LastEditTime;

		public int GroupID
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

		public string GroupName
		{
			get
			{
				return _GroupName;
			}
			set
			{
				_GroupName = value;
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
	}
}
