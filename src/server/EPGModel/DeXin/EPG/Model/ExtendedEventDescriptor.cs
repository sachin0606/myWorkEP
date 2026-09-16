using System;
using System.Collections.Generic;
using System.Text;
using Com.Cn.Tool;
using DeXin.EPG.Tools;

namespace DeXin.EPG.Model
{
	[Serializable]
	public class ExtendedEventDescriptor : DescriptorSigleBase
	{
		private int _Number = 0;

		private int _LasterNumber = 0;

		private string _LanguageCode = string.Empty;

		private List<KeyValuePair<string, string>> _kvs = new List<KeyValuePair<string, string>>();

		private string _Remark = string.Empty;

		public int Number
		{
			get
			{
				return _Number;
			}
			private set
			{
				_Number = value;
			}
		}

		public string LanguageCode
		{
			get
			{
				return _LanguageCode;
			}
			private set
			{
				_LanguageCode = value;
			}
		}

		public List<KeyValuePair<string, string>> kvs
		{
			get
			{
				return _kvs;
			}
			set
			{
				_kvs = value;
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

		public int LasterNumber
		{
			get
			{
				return _LasterNumber;
			}
			set
			{
				_LasterNumber = value;
			}
		}

		public ExtendedEventDescriptor(long tableId, string languageCode, int number, List<KeyValuePair<string, string>> kvitems, string remark)
			: base(EnumPossibleLoaction.EIT, tableId)
		{
			base.DesTypeTag = 78;
			Remark = remark;
			Number = number;
			LanguageCode = languageCode;
			Action<KeyValuePair<string, string>> action = delegate(KeyValuePair<string, string> item)
			{
				kvs.Add(new KeyValuePair<string, string>(item.Key, item.Value));
			};
			kvitems.ForEach(action);
		}

		public ExtendedEventDescriptor(DescriptorSigleBase item)
			: base(EnumPossibleLoaction.EIT, item.TableID)
		{
			base.DesTypeTag = 78;
			int start = 0;
			byte[] array = Tool.ResolveByte(item.DescriptionData, 1, ref start);
			Number = (array[0] >> 4) & 0xF;
			LasterNumber = array[0] & 0xF;
			array = Tool.ResolveByte(item.DescriptionData, 3, ref start);
			LanguageCode = Encoding.ASCII.GetString(array);
			byte b = Tool.ResolveByte(item.DescriptionData, 1, ref start)[0];
			while (start < b + 5)
			{
				array = Tool.ResolveByte(item.DescriptionData, 1, ref start);
				byte length = array[0];
				byte[] args = Tool.ResolveByte(item.DescriptionData, length, ref start);
				array = Tool.ResolveByte(item.DescriptionData, 1, ref start);
				byte length2 = array[0];
				byte[] args2 = Tool.ResolveByte(item.DescriptionData, length2, ref start);
				kvs.Add(new KeyValuePair<string, string>(CommTool.DescriptorBytesToString(args), CommTool.DescriptorBytesToString(args2)));
			}
			int length3 = Tool.ResolveByte(item.DescriptionData, 1, ref start)[0];
			array = Tool.ResolveByte(item.DescriptionData, length3, ref start);
			Remark = CommTool.DescriptorBytesToString(array);
		}

		public override byte[] ToBytes()
		{
			int startIndex = 0;
			List<byte> list = new List<byte>();
			byte[] array = CommTool.DescriptorStringToBytes(Remark);
			foreach (KeyValuePair<string, string> kv in kvs)
			{
				list.AddRange(kv.KVtoBuff());
			}
			base.DataLength = 6 + array.Length + list.Count;
			base.DescriptionData = new byte[base.DataLength];
			byte number = (byte)(((Number & 0xF) << 4) | (LasterNumber & 0xF));
			Tool.ToByteArray(number, base.DescriptionData, ref startIndex);
			Tool.ToByteArray(Encoding.ASCII.GetBytes(LanguageCode), base.DescriptionData, ref startIndex);
			Tool.ToByteArray((byte)list.Count, base.DescriptionData, ref startIndex);
			Tool.ToByteArray(list.ToArray(), base.DescriptionData, ref startIndex);
			Tool.ToByteArray((byte)array.Length, base.DescriptionData, ref startIndex);
			Tool.ToByteArray(array, base.DescriptionData, ref startIndex);
			return base.ToBytes();
		}
	}
}
