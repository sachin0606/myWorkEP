using System;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;

namespace DeXin.EPG.Tools
{
	[Serializable]
	public class DescriptorSigleBase : DescriptorBase
	{
		private long _Identity = 0L;

		private long _BATTableID = 0L;

		private long _TSTableID = 0L;

		private int _DesTypeTag = -1;

		private string _DesTypeName = string.Empty;

		private int _DataLength = 0;

		private byte[] _DescriptionData = new byte[0];

		private DescriptorSigleBase _Node = null;

		public long Identity
		{
			get
			{
				return _Identity;
			}
			set
			{
				_Identity = value;
			}
		}

		public long BATTableID
		{
			get
			{
				return _BATTableID;
			}
			set
			{
				_BATTableID = value;
			}
		}

		public long TSTableID
		{
			get
			{
				return _TSTableID;
			}
			set
			{
				_TSTableID = value;
			}
		}

		public EnumSendNoticesCodes SendLocation
		{
			get
			{
				return TypeManagent<EnumSendNoticesCodes>.GetEnum("DES_" + base.Location);
			}
		}

		public int DesTypeTag
		{
			get
			{
				return _DesTypeTag;
			}
			set
			{
				_DesTypeTag = value;
			}
		}

		public string DesTypeName
		{
			get
			{
				return _DesTypeName;
			}
			set
			{
				_DesTypeName = value;
			}
		}

		public int DataLength
		{
			get
			{
				return _DataLength;
			}
			set
			{
				if (value > 255)
				{
					_DataLength = 255;
				}
				else
				{
					_DataLength = value;
				}
			}
		}

		public byte[] DescriptionData
		{
			get
			{
				return _DescriptionData;
			}
			set
			{
				_DescriptionData = value;
			}
		}

		private DescriptorSigleBase Node
		{
			get
			{
				return _Node;
			}
			set
			{
				_Node = value;
			}
		}

		public DescriptorSigleBase()
		{
		}

		public DescriptorSigleBase(EnumPossibleLoaction location, long tableId)
		{
			base.Location = location;
			base.TableID = tableId;
			Node = this;
		}

		public DescriptorSigleBase(EnumPossibleLoaction location, long tableId, long batId)
		{
			base.Location = location;
			base.TableID = tableId;
			BATTableID = batId;
			Node = this;
		}

		public DescriptorSigleBase(long id, EnumPossibleLoaction location, long tableId, long desTypeID, int tag, string name, byte[] data)
		{
			Identity = id;
			base.TableID = tableId;
			base.DesTypeID = desTypeID;
			base.Location = location;
			DesTypeTag = tag;
			DesTypeName = name;
			DescriptionData = data;
			DataLength = ((data != null) ? data.Length : 0);
			Node = this;
		}

		public DescriptorSigleBase(EnumPossibleLoaction location, long tableId, long desTypeID, int length, byte[] data)
		{
			base.TableID = tableId;
			base.DesTypeID = desTypeID;
			base.Location = location;
			DataLength = length;
			DescriptionData = data;
			Node = this;
		}

		public virtual void InitBuff(DescriptorSigleBase des)
		{
		}

		public virtual byte[] ToBytes()
		{
			return DescriptionData;
		}

		public virtual byte[] GetData()
		{
			byte[] array = new byte[DescriptionData.Length + 2];
			int startIndex = 0;
			Tool.ToByteArray((byte)DesTypeTag, array, ref startIndex);
			Tool.ToByteArray((byte)DescriptionData.Length, array, ref startIndex);
			Tool.ToByteArray(DescriptionData, array, ref startIndex);
			return array;
		}
	}
}
