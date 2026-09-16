using System;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.Tools;

namespace DeXin.EPG.Model
{
	[Serializable]
	public class ServiceDescriptor : DescriptorSigleBase
	{
		public EnumDescriptorTag EnumDesTag;

		private byte _ServiceType = 0;

		private string _ServiceName = string.Empty;

		private string _ServiceProviderName = string.Empty;

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

		public string ServiceName
		{
			get
			{
				return _ServiceName;
			}
			set
			{
				_ServiceName = value;
			}
		}

		public string ServiceProviderName
		{
			get
			{
				return _ServiceProviderName;
			}
			set
			{
				_ServiceProviderName = value;
			}
		}

		public ServiceDescriptor(long tableId, EnumPossibleLoaction location, byte serviceType, string serviceName, string providerName, EnumDescriptorTag tag)
			: base(location, tableId)
		{
			ServiceName = serviceName;
			ServiceProviderName = providerName;
			EnumDesTag = tag;
			ServiceType = serviceType;
			base.DesTypeTag = (int)tag;
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
			if (base.DescriptionData != null)
			{
				byte serviceType = base.DescriptionData[0];
				ServiceType = serviceType;
				serviceType = base.DescriptionData[1];
				int num = Convert.ToInt32(serviceType);
				ServiceProviderName = CommTool.DescriptorBytesToString(Tool.ResolveByte(base.DescriptionData, 2, num));
				serviceType = base.DescriptionData[num + 2];
				int length = Convert.ToInt32(serviceType);
				ServiceName = CommTool.DescriptorBytesToString(Tool.ResolveByte(base.DescriptionData, num + 3, length));
			}
			base.InitBuff(des);
		}

		public override byte[] ToBytes()
		{
			int startIndex = 0;
			byte[] array = CommTool.DescriptorStringToBytes(ServiceProviderName);
			byte[] array2 = CommTool.DescriptorStringToBytes(ServiceName);
			base.DataLength = 3 + array2.Length + array.Length;
			if (base.DataLength > 255)
			{
				throw new CustomException("ServiceName and ServiceProviderName overlong", MessageType.SHOW);
			}
			base.DescriptionData = new byte[base.DataLength];
			Tool.ToByteArray(ServiceType, base.DescriptionData, ref startIndex);
			Tool.ToByteArray(Convert.ToByte(array.Length), base.DescriptionData, ref startIndex);
			Tool.ToByteArray(array, base.DescriptionData, ref startIndex);
			Tool.ToByteArray(Convert.ToByte(array2.Length), base.DescriptionData, ref startIndex);
			Tool.ToByteArray(array2, base.DescriptionData, ref startIndex);
			return base.ToBytes();
		}
	}
}
