using System;
using System.Text;
using Com.Cn.Tool;

namespace DeXin.EPG.Model
{
	public class ParentalRating
	{
		private string _rate_code;

		private string _countroy_code;

		private byte _rating = 0;

		public string rate_code
		{
			get
			{
				return _rate_code;
			}
			set
			{
				_rate_code = value;
			}
		}

		public string countroy_code
		{
			get
			{
				return _countroy_code;
			}
			set
			{
				_countroy_code = value;
			}
		}

		public byte Rating
		{
			get
			{
				return _rating;
			}
			set
			{
				_rating = value;
			}
		}

		public ParentalRating(string countroycode, string code, byte level)
		{
			rate_code = code;
			countroy_code = countroycode;
			Rating = level;
		}

		public ParentalRating(byte[] data)
		{
			countroy_code = Encoding.ASCII.GetString(Tool.ResolveByte(data, 0, 3));
			Rating = data[3];
		}

		public byte[] ToByte()
		{
			byte[] array = new byte[4];
			Array.Copy(Encoding.ASCII.GetBytes(countroy_code), 0, array, 0, 3);
			array[3] = Rating;
			return array;
		}
	}
}
