using System.Collections.Generic;
using System.Linq;
using DeXin.EPG.Tools;

namespace DeXin.EPG.Model
{
	public class ExtendedEventDescriptorBase : DescriptorBase
	{
		private long _EitTableID = 0L;

		private string _LanguageCode = string.Empty;

		private string _Remark = string.Empty;

		private List<KeyValuePair<string, string>> _Items = new List<KeyValuePair<string, string>>();

		private List<ExtendedEventDescriptor> _Extendeds = new List<ExtendedEventDescriptor>();

		public long EitTableID
		{
			get
			{
				return _EitTableID;
			}
			set
			{
				_EitTableID = value;
			}
		}

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

		public List<KeyValuePair<string, string>> Items
		{
			get
			{
				return _Items;
			}
			set
			{
				_Items = value;
			}
		}

		public List<ExtendedEventDescriptor> Extendeds
		{
			get
			{
				return _Extendeds;
			}
			set
			{
				_Extendeds = value;
			}
		}

		public ExtendedEventDescriptorBase(long _eittableId, string _languageCode)
		{
			base.DesTypeID = 0L;
			base.Location = EnumPossibleLoaction.EIT;
			base.TableID = _eittableId;
			EitTableID = _eittableId;
			LanguageCode = _languageCode;
		}

		public static List<KeyValuePair<string, string>> FormDB(List<DescriptorSigleBase> descriptors)
		{
			List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
			List<ExtendedEventDescriptor> list = (from item in descriptors
				select new ExtendedEventDescriptor(item) into p
				orderby p.Number
				select p).ToList();
			list.ForEach(delegate(ExtendedEventDescriptor item)
			{
				result.AddRange(item.kvs);
			});
			return result;
		}

		private int GetIndex(int maxbuffsize, int startindex, string str)
		{
			if (str.Length == 0)
			{
				return startindex;
			}
			if (startindex == str.Length)
			{
				return startindex;
			}
			int length = str.Length;
			string args = str.Substring(startindex, length - startindex);
			int num = CommTool.DescriptorStringToBytes(args).Length;
			if (num <= maxbuffsize)
			{
				return str.Length;
			}
			return GetIndex(maxbuffsize, startindex, startindex, str.Length, str);
		}

		private int GetIndex(int maxBuffSize, int calcindex, int startindex, int endindex, string str)
		{
			int num = (endindex + startindex) / 2;
			string args = str.Substring(calcindex, num - calcindex);
			int num2 = CommTool.DescriptorStringToBytes(args).Length;
			if (num2 > maxBuffSize)
			{
				args = str.Substring(calcindex, num - calcindex - 1);
				num2 = CommTool.DescriptorStringToBytes(args).Length;
				if (num2 > maxBuffSize)
				{
					return GetIndex(maxBuffSize, calcindex, startindex, num, str);
				}
				num--;
			}
			else if (num2 < maxBuffSize)
			{
				args = str.Substring(calcindex, num - calcindex + 1);
				num2 = CommTool.DescriptorStringToBytes(args).Length;
				if (num2 < maxBuffSize)
				{
					return GetIndex(maxBuffSize, calcindex, num, endindex, str);
				}
				if (num2 == maxBuffSize)
				{
					num++;
				}
			}
			return num;
		}

		public void Setkvs(List<KeyValuePair<string, string>> kvs)
		{
			Items.Clear();
			Items.AddRange(kvs);
		}

		public void SetRemark(string remark)
		{
			Extendeds.Clear();
			Remark = remark;
			int num = 247;
			List<byte> list = new List<byte>();
			List<KeyValuePair<string, string>> list2 = new List<KeyValuePair<string, string>>();
			foreach (KeyValuePair<string, string> item in Items)
			{
				if (Extendeds.Count >= 15)
				{
					list2.Clear();
					list.Clear();
					break;
				}
				byte[] array = item.KVtoBuff();
				if (list.Count + array.Length > num)
				{
					Extendeds.Add(new ExtendedEventDescriptor(EitTableID, LanguageCode, Extendeds.Count + 1, list2, string.Empty));
					list.Clear();
					list2.Clear();
				}
				else
				{
					list2.Add(item);
				}
			}
			int num2 = 0;
			int num3 = 0;
			string empty = string.Empty;
			while (num3 < Remark.Length && Extendeds.Count < 15)
			{
				num2 = num3;
				num3 = GetIndex(num - list.Count, num2, Remark);
				empty = Remark.Substring(num2, num3 - num2);
				Extendeds.Add(new ExtendedEventDescriptor(EitTableID, LanguageCode, Extendeds.Count + 1, list2, empty));
				list.Clear();
				list2.Clear();
				empty = string.Empty;
			}
			if (list2.Count > 0 && Extendeds.Count < 15)
			{
				Extendeds.Add(new ExtendedEventDescriptor(EitTableID, LanguageCode, Extendeds.Count + 1, list2, empty));
			}
			foreach (ExtendedEventDescriptor extended in Extendeds)
			{
				extended.LasterNumber = Extendeds.Count;
			}
		}
	}
}
