using DeXin.EPG.Resource;

namespace DeXin.EPG.Model
{
	public class TreeKey
	{
		private TreeKey _node;

		private string _key;

		private string _value;

		private string _LocationValue;

		private string _pId;

		public TreeKey Node
		{
			get
			{
				return _node;
			}
			set
			{
				_node = value;
			}
		}

		public string Key
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

		public string Value
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

		public string LocationValue
		{
			get
			{
				_LocationValue = ResourceManagement.GetLocationString(_value);
				return _LocationValue;
			}
		}

		public string PId
		{
			get
			{
				return _pId;
			}
			set
			{
				_pId = value;
			}
		}

		public string para1 { get; set; }

		public string para2 { get; set; }

		public string para3 { get; set; }

		public string para4 { get; set; }

		public TreeKey()
		{
		}

		public TreeKey(string key, string value, string pid)
		{
			Key = key;
			Value = value;
			PId = pid;
		}

		public TreeKey(string key, TreeKey item)
		{
			Key = key;
			Node = item;
		}
	}
}
