using System;

namespace Com.Cn.Tool.Model
{
	[Serializable]
	public class TBaseKey<T1, T2>
	{
		private T1 _TKey;

		private T2 _TValue;

		private T1 _TPKey;

		private bool _TStatus = true;

		protected TBaseKey<T1, T2> node;

		private string _Text = string.Empty;

		public T1 TKey
		{
			get
			{
				return _TKey;
			}
			set
			{
				_TKey = value;
			}
		}

		public T2 TValue
		{
			get
			{
				return _TValue;
			}
			set
			{
				_TValue = value;
			}
		}

		public T1 TPKey
		{
			get
			{
				return _TPKey;
			}
			set
			{
				_TPKey = value;
			}
		}

		public bool TStatus
		{
			get
			{
				return _TStatus;
			}
			set
			{
				_TStatus = value;
			}
		}

		public virtual TBaseKey<T1, T2> Node
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

		public string Text
		{
			get
			{
				return _Text;
			}
			set
			{
				_Text = value;
			}
		}

		public TBaseKey()
		{
		}

		public TBaseKey(T1 tkey, T2 tvalue)
		{
			TKey = tkey;
			TValue = tvalue;
		}

		public TBaseKey(T1 tkey, T2 tvalue, bool tstatus)
		{
			TKey = tkey;
			TValue = tvalue;
			TStatus = tstatus;
		}

		public TBaseKey(T1 tkey, T2 tvalue, T1 tpkey)
		{
			TKey = tkey;
			TValue = tvalue;
			TPKey = tpkey;
		}

		public TBaseKey(T1 tkey, T2 tvalue, T1 tpkey, bool tstatus)
		{
			TKey = tkey;
			TValue = tvalue;
			TPKey = tpkey;
			TStatus = tstatus;
		}

		public TBaseKey(TBaseKey<T1, T2> item)
		{
			TKey = item.TKey;
			TValue = item.TValue;
			TPKey = item.TPKey;
			TStatus = item.TStatus;
			Node = item.Node;
		}

		public override string ToString()
		{
			if (TValue != null)
			{
				return TValue.ToString();
			}
			return "";
		}
	}
}
