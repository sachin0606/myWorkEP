using System;

namespace DeXin.EPG.Model
{
	[Serializable]
	public class SendNotices
	{
		private long _id;

		private int _code;

		private string _location;

		private long _table_identity;

		private int _operation;

		private string _user_id;

		private DateTime _edit_date;

		private bool _state;

		public long Id
		{
			get
			{
				return _id;
			}
			set
			{
				_id = value;
			}
		}

		public int Code
		{
			get
			{
				return _code;
			}
			set
			{
				_code = value;
			}
		}

		public string Location
		{
			get
			{
				return _location;
			}
			set
			{
				_location = value;
			}
		}

		public long TableIdentity
		{
			get
			{
				return _table_identity;
			}
			set
			{
				_table_identity = value;
			}
		}

		public int Operation
		{
			get
			{
				return _operation;
			}
			set
			{
				_operation = value;
			}
		}

		public string UserId
		{
			get
			{
				return _user_id;
			}
			set
			{
				_user_id = value;
			}
		}

		public DateTime EditDate
		{
			get
			{
				return _edit_date;
			}
			set
			{
				_edit_date = value;
			}
		}

		public bool State
		{
			get
			{
				return _state;
			}
			set
			{
				_state = value;
			}
		}
	}
}
