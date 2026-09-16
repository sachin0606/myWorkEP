using System;
using DeXin.EPG.Tools;

namespace DeXin.EPG.Model
{
	[Serializable]
	public class StringNameDescriptor : DescriptorSigleBase
	{
		public EnumDescriptorTag EnumDesTag;

		private string _StringName = string.Empty;

		public string StringName
		{
			get
			{
				return _StringName;
			}
			set
			{
				_StringName = value;
			}
		}

		public StringNameDescriptor(long tableId, EnumPossibleLoaction location, string name, EnumDescriptorTag tag)
			: base(location, tableId)
		{
			StringName = name;
			EnumDesTag = tag;
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
				StringName = CommTool.DescriptorBytesToString(base.DescriptionData);
			}
			base.InitBuff(des);
		}

		public override byte[] ToBytes()
		{
			byte[] array = CommTool.DescriptorStringToBytes(StringName);
			base.DataLength = array.Length;
			base.DescriptionData = array;
			return base.ToBytes();
		}
	}
}
