using System;
using System.Text;
using Com.Cn.Tool;
using DeXin.EPG.Tools;

namespace DeXin.EPG.Model
{
	[Serializable]
	public class ShortEventDescriptor : DescriptorSigleBase
	{
		public EnumDescriptorTag EnumDesTag = EnumDescriptorTag.short_envent_descriptor;

		private string _LanguageCode = string.Empty;

		private string _EventName = string.Empty;

		private string _Remark = string.Empty;

		public string LanguageCode
		{
			get
			{
				return _LanguageCode;
			}
			set
			{
				_LanguageCode = value;
			}
		}

		public string EventName
		{
			get
			{
				return _EventName;
			}
			set
			{
				_EventName = value;
			}
		}

		public string Remark
		{
			get
			{
				return _Remark;
			}
			set
			{
				_Remark = value;
			}
		}

		public ShortEventDescriptor(long _tableId, EnumPossibleLoaction _location, string languageCode, string eventName, string remark)
			: base(_location, _tableId)
		{
			LanguageCode = languageCode;
			EventName = eventName;
			Remark = remark;
			base.DesTypeTag = (int)EnumDesTag;
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
				int num = 0;
				byte b = 0;
				byte[] bytes = Tool.ResolveByte(base.DescriptionData, 0, 3);
				num += 3;
				LanguageCode = Encoding.ASCII.GetString(bytes);
				b = base.DescriptionData[num];
				num++;
				bytes = Tool.ResolveByte(base.DescriptionData, num, b);
				EventName = CommTool.DescriptorBytesToString(bytes);
				num += b;
				b = base.DescriptionData[num];
				num++;
				bytes = Tool.ResolveByte(base.DescriptionData, num, b);
				Remark = CommTool.DescriptorBytesToString(bytes);
			}
			base.InitBuff(des);
		}

		public override byte[] ToBytes()
		{
			int startIndex = 0;
			int num = 0;
			byte[] array = CommTool.DescriptorStringToBytes(EventName);
			byte[] array2 = CommTool.DescriptorStringToBytes(Remark);
			num = Remark.Length;
			if (array.Length + array2.Length + 5 > 255)
			{
				array2 = GetBytes(250 - array.Length, Remark, ref num);
			}
			base.DataLength = array.Length + array2.Length + 5;
			base.DescriptionData = new byte[base.DataLength];
			Tool.ToByteArray(Encoding.ASCII.GetBytes(LanguageCode), base.DescriptionData, ref startIndex);
			Tool.ToByteArray((byte)array.Length, base.DescriptionData, ref startIndex);
			Tool.ToByteArray(array, base.DescriptionData, ref startIndex);
			Tool.ToByteArray((byte)array2.Length, base.DescriptionData, ref startIndex);
			Tool.ToByteArray(array2, base.DescriptionData, ref startIndex);
			return base.ToBytes();
		}

		private byte[] GetBytes(int maxLength, string resource, ref int index)
		{
			index--;
			byte[] array = new byte[0];
			string args = resource.Substring(0, index);
			byte[] array2 = CommTool.DescriptorStringToBytes(args);
			if (array2.Length > maxLength)
			{
				return GetBytes(maxLength, resource, ref index);
			}
			return array2;
		}
	}
}
