using System.Collections.Generic;
using Com.Cn.Tool;
using DeXin.EPG.Tools;

namespace DeXin.EPG.Model
{
	public class ParentalRatingBase : DescriptorSigleBase
	{
		private List<ParentalRating> _Items = new List<ParentalRating>();

		public List<ParentalRating> Items
		{
			get
			{
				return _Items;
			}
			private set
			{
				_Items = value;
			}
		}

		public ParentalRatingBase(long eittableid)
			: base(EnumPossibleLoaction.EIT, eittableid)
		{
			base.DesTypeTag = 85;
			base.TableID = eittableid;
		}

		public ParentalRatingBase(long eittableid, string countroycode, string code, byte level)
			: base(EnumPossibleLoaction.EIT, eittableid)
		{
			base.DesTypeTag = 85;
			base.TableID = eittableid;
			Items.Add(new ParentalRating(countroycode, code, level));
		}

		public void Add(string countroycode, string code, byte level)
		{
			Items.Add(new ParentalRating(countroycode, code, level));
		}

		public static List<ParentalRating> DataConvert(byte[] data)
		{
			List<ParentalRating> list = new List<ParentalRating>();
			for (int i = 0; i < data.Length; i += 4)
			{
				byte[] data2 = Tool.ResolveByte(data, i, 4);
				ParentalRating item = new ParentalRating(data2);
				list.Add(item);
			}
			return list;
		}

		public override byte[] ToBytes()
		{
			int num = Items.Count * 4;
			byte[] array = new byte[num];
			base.DataLength = num;
			int startIndex = 0;
			foreach (ParentalRating item in Items)
			{
				Tool.ToByteArray(item.ToByte(), array, ref startIndex);
			}
			return array;
		}
	}
}
