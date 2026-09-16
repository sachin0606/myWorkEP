using System;

namespace DeXin.EPG.Model
{
	public class PermitGroupManagement
	{
		private int _GroupID = 0;

		private string _FunctionID = string.Empty;

		private bool _CanAdd = false;

		private bool _CanDelete = false;

		private bool _CanEdit = false;

		private bool _CanQuery = false;

		private bool _CanPrint = false;

		private string _Creator = string.Empty;

		private DateTime _CreateTime = DateTime.Now;

		private string _LastEditor = string.Empty;

		private DateTime _LastEditTime = DateTime.Now;

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

		public string FunctionID
		{
			get
			{
				return _FunctionID;
			}
			set
			{
				_FunctionID = value;
			}
		}

		public bool CanAdd
		{
			get
			{
				return _CanAdd;
			}
			set
			{
				_CanAdd = value;
			}
		}

		public bool CanDelete
		{
			get
			{
				return _CanDelete;
			}
			set
			{
				_CanDelete = value;
			}
		}

		public bool CanEdit
		{
			get
			{
				return _CanEdit;
			}
			set
			{
				_CanEdit = value;
			}
		}

		public bool CanQuery
		{
			get
			{
				return _CanQuery;
			}
			set
			{
				_CanQuery = value;
			}
		}

		public bool CanPrint
		{
			get
			{
				return _CanPrint;
			}
			set
			{
				_CanPrint = value;
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

		public DateTime LastEditTime
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
