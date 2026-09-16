using System;

namespace Com.Cn.Tool.Model
{
	[Serializable]
	public class BaseKey : TBaseKey<string, string>
	{
		private string _key;

		private string _value = string.Empty;

		private bool _status = true;

		private string _PID;

		protected new BaseKey node;

		private int _level;

		public string BKKey
		{
			get
			{
				return _key;
			}
			set
			{
				_key = value;
			}
		}

		public string BKValue
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
			}
		}

		public bool BKStatus
		{
			get
			{
				return _status;
			}
			set
			{
				_status = value;
			}
		}

		public string PID
		{
			get
			{
				return _PID;
			}
			set
			{
				_PID = value;
			}
		}

		public new virtual BaseKey Node
		{
			get
			{
				return node;
			}
			set
			{
				node = value;
			}
		}

		public int level
		{
			get
			{
				return _level;
			}
			set
			{
				_level = value;
			}
		}

		public BaseKey()
		{
		}

		public BaseKey(string key, string value, bool status)
			: base(key, value)
		{
			BKKey = key;
			BKValue = value;
			BKStatus = status;
			Node = new BaseKey();
			Node.BKKey = key;
			Node.BKValue = value;
			Node.BKStatus = status;
		}

		public BaseKey(string key, string value, string pid, bool status)
			: base(key, value)
		{
			BKKey = key;
			BKValue = value;
			BKStatus = status;
			PID = pid;
			Node = new BaseKey();
			Node.BKKey = key;
			Node.BKValue = value;
			Node.BKStatus = status;
			Node.PID = pid;
		}
	}
}
