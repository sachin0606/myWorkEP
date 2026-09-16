using System;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.Tools;

namespace DeXin.EPG.Model
{
	[Serializable]
	public class ServiceListDescriptor : DescriptorSigleBase
	{
		public readonly EnumDescriptorTag EnumDesTag = EnumDescriptorTag.service_list_descriptor;

		private short _ServiceId = 0;

		private byte _ServiceType = 0;

		public short ServiceId
		{
			get
			{
				return _ServiceId;
			}
			set
			{
				_ServiceId = value;
			}
		}

		public byte ServiceType
		{
			get
			{
				return _ServiceType;
			}
			set
			{
				_ServiceType = value;
			}
		}

		public ServiceListDescriptor(long tableId, EnumPossibleLoaction location, short serviceId, byte serviceType)
			: base(location, tableId)
		{
			ServiceId = serviceId;
			ServiceType = serviceType;
		}

		public override void InitBuff(DescriptorSigleBase des)
		{
			base.Identity = des.Identity;
			base.BATTableID = des.BATTableID;
			base.DescriptionData = base.DescriptionData;
			base.DataLength = des.DataLength;
			base.DesTypeID = des.DesTypeID;
			base.DesTypeName = des.DesTypeName;
			base.DesTypeTag = des.DesTypeTag;
			base.TableID = des.TableID;
			if (base.DescriptionData != null && base.DescriptionData.Length >= 3)
			{
				ServiceId = Tool.ResolveInt16(base.DescriptionData, NetWorkEnum.Normal);
				ServiceType = Tool.ResolveByte(Tool.ResolveByte(base.DescriptionData, 2, 1));
			}
			base.InitBuff(des);
		}

		public override byte[] ToBytes()
		{
			int startIndex = 0;
			base.DataLength = 3;
			base.DescriptionData = new byte[3];
			Tool.ToByteArray(ServiceId, base.DescriptionData, ref startIndex);
			Tool.ToByteArray(ServiceType, base.DescriptionData, ref startIndex);
			return base.ToBytes();
		}
	}
}
