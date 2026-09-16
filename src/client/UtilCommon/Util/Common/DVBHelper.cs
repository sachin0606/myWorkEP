using System;
using Util.Common.ExtensionHelper;

namespace Util.Common
{
	public static class DVBHelper
	{
		public static string ToBCDString(this string str, int leftLength, int rightLength)
		{
			string text = "";
			string text2 = "";
			string text3 = "";
			if (str.IndexOf('.') < 0)
			{
				text2 = str.PadLeft(leftLength, '0');
				text = text2.Substring(text2.Length - leftLength, leftLength);
				return text.PadRight(leftLength + rightLength, '0');
			}
			text2 = str.Substring(0, str.IndexOf('.'));
			text2 = text2.PadLeft(leftLength, '0');
			text2 = text2.Substring(text2.Length - leftLength, leftLength);
			text3 = str.Substring(str.IndexOf('.') + 1);
			text3 = text3.PadRight(rightLength, '0');
			text3 = text3.Substring(0, rightLength);
			return text2 + text3;
		}

		public static byte[] ToMJDBuff(this DateTime datetime)
		{
			byte[] array = new byte[5];
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			int num8 = 0;
			num2 = datetime.Year - 1900;
			num3 = datetime.Month;
			num4 = datetime.Day;
			num5 = datetime.Hour;
			num6 = datetime.Minute;
			num7 = datetime.Second;
			num = ((num3 == 1 || num3 == 2) ? 1 : 0);
			num8 = 14956 + num4 + (int)((double)(num2 - num) * 365.25) + (int)((double)(num3 + 1 + num * 12) * 30.6001);
			array[0] = (byte)((num8 >> 8) & 0xFF);
			array[1] = (byte)(num8 & 0xFF);
			array[2] = (byte)(num5 / 10 * 16 + num5 % 10);
			array[3] = (byte)(num6 / 10 * 16 + num6 % 10);
			array[4] = (byte)(num7 / 10 * 16 + num7 % 10);
			return array;
		}

		public static DateTime MJDToDateTime(this byte[] buff)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			byte[] array = new byte[2]
			{
				buff[0],
				buff[1]
			};
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse(array);
			}
			ushort num8 = BitConverter.ToUInt16(array, 0);
			num2 = (ushort)(((double)(int)num8 - 15078.2) / 365.25);
			num3 = (int)(((double)(int)num8 - 14956.1 - (double)(int)((double)num2 * 365.25)) / 30.6001);
			num4 = num8 - 14956 - (int)((double)num2 * 365.25) - (int)((double)num3 * 30.6001);
			byte[] array2 = new byte[3]
			{
				buff[2],
				buff[3],
				buff[4]
			};
			string text = BitConverter.ToString(array2);
			string[] array3 = text.Split('-');
			int num9 = 0;
			string[] array4 = array3;
			foreach (string s in array4)
			{
				int num10 = int.Parse(s);
				array2[num9] = (byte)num10;
				num9++;
			}
			num5 = array2[0];
			num6 = array2[1];
			num7 = array2[2];
			num = ((num3 == 14 || num3 == 15) ? 1 : 0);
			num2 += 1900 + num;
			num3 = num3 - 1 - num * 12;
			return new DateTime(num2, num3, num4, num5, num6, num7);
		}

		public static void CopyMJDDateTime(this byte[] data, DateTime datetime, ref int index)
		{
			byte[] resource = datetime.ToMJDBuff();
			data.CopyBytes(resource, ref index);
		}

		public static DateTime GetMJDDateTime(this byte[] data, ref int index)
		{
			byte[] bytes = data.GetBytes(5, ref index);
			return bytes.MJDToDateTime();
		}
	}
}
