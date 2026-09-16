using System;
using System.Text;
using Com.Cn.Tool;
using DeXin.EPG.Tools;

namespace DeXin.EPG.Model
{
	public class LocalTimeOffSetEntity
	{
		private string _CountryCode = string.Empty;

		private int _CountryRegionID = 0;

		private byte reserved = 1;

		private int _LocalTimeOffsetPolarity = 0;

		private string _LocalTimeOffset = "";

		private DateTime _TimeOfChange = DateTime.Now;

		private string _NextTimeOffset = "";

		public string CountryCode
		{
			get
			{
				return _CountryCode;
			}
			set
			{
				_CountryCode = value;
			}
		}

		public int CountryRegionID
		{
			get
			{
				return _CountryRegionID;
			}
			set
			{
				_CountryRegionID = value;
			}
		}

		public int LocalTimeOffsetPolarity
		{
			get
			{
				return _LocalTimeOffsetPolarity;
			}
			set
			{
				_LocalTimeOffsetPolarity = value;
			}
		}

		public string LocalTimeOffset
		{
			get
			{
				return _LocalTimeOffset;
			}
			set
			{
				_LocalTimeOffset = value.PadLeft(4, '0');
			}
		}

		public DateTime TimeOfChange
		{
			get
			{
				return _TimeOfChange;
			}
			set
			{
				_TimeOfChange = value;
			}
		}

		public string NextTimeOffset
		{
			get
			{
				return _NextTimeOffset;
			}
			set
			{
				_NextTimeOffset = value.PadLeft(4, '0');
			}
		}

		public static LocalTimeOffSetEntity InitData(byte[] data)
		{
			LocalTimeOffSetEntity localTimeOffSetEntity = new LocalTimeOffSetEntity();
			localTimeOffSetEntity.CountryCode = Encoding.ASCII.GetString(Tool.ResolveByte(data, 0, 3));
			localTimeOffSetEntity.CountryRegionID = (data[3] >> 2) & 0x3F;
			localTimeOffSetEntity.LocalTimeOffsetPolarity = data[3] & 1;
			localTimeOffSetEntity.LocalTimeOffset = Tool.BytesToString(Tool.ResolveByte(data, 4, 2));
			localTimeOffSetEntity.TimeOfChange = CommTool.MJDBuffToTime(Tool.ResolveByte(data, 6, 5));
			localTimeOffSetEntity.NextTimeOffset = Tool.BytesToString(Tool.ResolveByte(data, 11, 2));
			return localTimeOffSetEntity;
		}

		public byte[] ToByte()
		{
			byte[] array = new byte[13];
			int num = 0;
			Array.Copy(Encoding.ASCII.GetBytes(CountryCode), 0, array, 0, 3);
			num += 3;
			array[num++] = (byte)(((CountryRegionID & 0x3F) << 2) | ((reserved & 1) << 1) | (LocalTimeOffsetPolarity & 1));
			Array.Copy(Tool.StringToBytes(LocalTimeOffset), 0, array, num, 2);
			num += 2;
			Array.Copy(CommTool.TimeToMJDBuf(TimeOfChange), 0, array, num, 5);
			num += 5;
			Array.Copy(Tool.StringToBytes(NextTimeOffset), 0, array, num, 2);
			num += 2;
			return array;
		}
	}
}
