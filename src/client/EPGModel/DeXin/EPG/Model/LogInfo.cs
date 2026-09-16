using System;
using Com.Cn.Tool;
using DeXin.EPG.Resource;
using DeXin.EPG.Tools;

namespace DeXin.EPG.Model
{
	public class LogInfo
	{
		private string _LogID;

		private string _UserID;

		private string _UserName;

		private string _FunctionID;

		private string _OSName;

		private string _Description;

		private DateTime? _OperateDateTime;

		private string _IPAddress;

		private SysFunction _SysFunction;

		private string _LocationFunctionName;

		public string LogID
		{
			get
			{
				return _LogID;
			}
			set
			{
				_LogID = value;
			}
		}

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

		public string OSName
		{
			get
			{
				return _OSName;
			}
			set
			{
				_OSName = value;
			}
		}

		public string Description
		{
			get
			{
				return _Description;
			}
			set
			{
				_Description = value;
			}
		}

		public DateTime? OperateDateTime
		{
			get
			{
				return _OperateDateTime;
			}
			set
			{
				_OperateDateTime = value;
			}
		}

		public string IPAddress
		{
			get
			{
				return _IPAddress;
			}
			set
			{
				_IPAddress = value;
			}
		}

		public SysFunction SysFunction
		{
			get
			{
				return _SysFunction;
			}
			set
			{
				_SysFunction = value;
			}
		}

		public string LocationFunctionName
		{
			get
			{
				_LocationFunctionName = ResourceManagement.GetLocationString(Tool.GetEnum(typeof(EnumFunctionName), FunctionID).ToString());
				return _LocationFunctionName;
			}
		}
	}
}
