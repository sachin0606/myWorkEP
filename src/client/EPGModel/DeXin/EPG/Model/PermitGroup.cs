using System;
using Com.Cn.Tool;
using DeXin.EPG.Resource;
using DeXin.EPG.Tools;

namespace DeXin.EPG.Model
{
	public class PermitGroup
	{
		private int _GroupID;

		private string _FunctionID;

		private bool _CanAdd;

		private bool _CanDelete;

		private bool _CanEdit;

		private bool _CanQuery;

		private bool _CanPrint;

		private string _Creator;

		private DateTime? _CreateTime;

		private string _LastEditor;

		private DateTime? _LastEditTime;

		private string _GroupName;

		private string _FunctionName;

		private bool _GroupStatus;

		private string _LocationFunctionName;

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

		public string FunctionName
		{
			get
			{
				_FunctionName = Tool.GetEnum(typeof(EnumFunctionName), _FunctionID).ToString();
				return _FunctionName;
			}
		}

		public bool GroupStatus
		{
			get
			{
				return _GroupStatus;
			}
			set
			{
				_GroupStatus = value;
			}
		}

		public string LocationFunctionName
		{
			get
			{
				_LocationFunctionName = ResourceManagement.GetLocationString(FunctionName);
				return _LocationFunctionName;
			}
		}
	}
}
