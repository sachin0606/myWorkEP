using System;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.Resource;
using DeXin.EPG.Tools;

namespace DeXin.EPG.Model
{
	public class DescriptorType : BaseKey
	{
		private long _Id = 0L;

		private string _DesName = string.Empty;

		private int _DesTag = 0;

		private string _DesLocation = string.Empty;

		private int _DesType = 0;

		public long Id
		{
			get
			{
				return _Id;
			}
			set
			{
				_Id = value;
			}
		}

		public string DesName
		{
			get
			{
				return _DesName;
			}
			set
			{
				_DesName = value;
			}
		}

		public int DesTag
		{
			get
			{
				return _DesTag;
			}
			set
			{
				_DesTag = value;
			}
		}

		public string DesLocation
		{
			get
			{
				return _DesLocation;
			}
			set
			{
				_DesLocation = value;
			}
		}

		public EnumPossibleLoaction DesLocationEnum
		{
			get
			{
				EnumPossibleLoaction enumPossibleLoaction = EnumPossibleLoaction.NIT;
				try
				{
					return (EnumPossibleLoaction)(object)Tool.GetEnum(typeof(EnumPossibleLoaction), DesLocation);
				}
				catch (Exception ex)
				{
					Tool.DoNoneException(ex);
					return EnumPossibleLoaction.NIT;
				}
			}
			set
			{
				DesLocation = value.ToString();
			}
		}

		public int DesType
		{
			get
			{
				return _DesType;
			}
			set
			{
				_DesType = value;
			}
		}

		public string DesTypeString
		{
			get
			{
				string locationString = ResourceManagement.GetLocationString("SystemDefined");
				int desType = DesType;
				if (desType == 1)
				{
					locationString = ResourceManagement.GetLocationString("UserDefined");
				}
				return locationString;
			}
		}

		public DescriptorType()
		{
		}

		public DescriptorType(long id, string tyName, bool visible)
			: base(id.ToString(), tyName, visible)
		{
			Id = id;
			DesName = tyName;
			Node = this;
		}
	}
}
