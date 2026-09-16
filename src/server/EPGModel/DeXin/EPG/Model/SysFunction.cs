using System;
using DeXin.EPG.Resource;
using DeXin.EPG.Tools;

namespace DeXin.EPG.Model
{
	[Serializable]
	public class SysFunction
	{
		private EnumFunctionName _FunctionKeyEnum = EnumFunctionName.SysFuncNETWORK;

		private string _LocationFunctionName;

		private string _AssemblyPath;

		private string _AssemblyName;

		private string _ClassName;

		private string _Param1;

		private string _Param2;

		private string _Param3;

		private string _Param4;

		private string _Creator;

		private DateTime? _CreateTime;

		private string _LastEditor;

		private DateTime? _LastEditTime;

		private bool _Visible = false;

		private bool _CanSetPermit = false;

		public int FunctionID
		{
			get
			{
				return (int)FunctionKeyEnum;
			}
		}

		public EnumFunctionName FunctionKeyEnum
		{
			get
			{
				return _FunctionKeyEnum;
			}
			set
			{
				_FunctionKeyEnum = value;
			}
		}

		public string FunctionKey
		{
			get
			{
				return FunctionKeyEnum.ToString();
			}
		}

		public string LocationFunctionName
		{
			get
			{
				_LocationFunctionName = ResourceManagement.GetLocationString(FunctionKey);
				return _LocationFunctionName;
			}
		}

		public string AssemblyPath
		{
			get
			{
				return _AssemblyPath;
			}
			set
			{
				_AssemblyPath = value;
			}
		}

		public string AssemblyName
		{
			get
			{
				return _AssemblyName;
			}
			set
			{
				_AssemblyName = value;
			}
		}

		public string ClassName
		{
			get
			{
				return _ClassName;
			}
			set
			{
				_ClassName = value;
			}
		}

		public string Param1
		{
			get
			{
				return _Param1;
			}
			set
			{
				_Param1 = value;
			}
		}

		public string Param2
		{
			get
			{
				return _Param2;
			}
			set
			{
				_Param2 = value;
			}
		}

		public string Param3
		{
			get
			{
				return _Param3;
			}
			set
			{
				_Param3 = value;
			}
		}

		public string Param4
		{
			get
			{
				return _Param4;
			}
			set
			{
				_Param4 = value;
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

		public bool Visible
		{
			get
			{
				return _Visible;
			}
			set
			{
				_Visible = value;
			}
		}

		public bool CanSetPermit
		{
			get
			{
				return _CanSetPermit;
			}
			set
			{
				_CanSetPermit = value;
			}
		}

		public SysFunction()
		{
		}

		public SysFunction(EnumFunctionName enumFuncName)
		{
			FunctionKeyEnum = enumFuncName;
		}
	}
}
