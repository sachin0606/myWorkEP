using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Util.Common.ExtensionHelper
{
	public static class NetWorkHelper
	{
		public static short ToNetWorkNumber(this short obj, NetWorkEnum netWork = NetWorkEnum.Normal)
		{
			switch (netWork)
			{
			case NetWorkEnum.HostToNetWork:
				return IPAddress.HostToNetworkOrder(obj);
			case NetWorkEnum.NetWorkToHost:
				return IPAddress.NetworkToHostOrder(obj);
			default:
				return obj;
			}
		}

		public static int ToNetWorkNumber(this int obj, NetWorkEnum netWork = NetWorkEnum.Normal)
		{
			switch (netWork)
			{
			case NetWorkEnum.HostToNetWork:
				return IPAddress.HostToNetworkOrder(obj);
			case NetWorkEnum.NetWorkToHost:
				return IPAddress.NetworkToHostOrder(obj);
			default:
				return obj;
			}
		}

		public static long ToNetWorkNumber(this long obj, NetWorkEnum netWork = NetWorkEnum.Normal)
		{
			switch (netWork)
			{
			case NetWorkEnum.HostToNetWork:
				return IPAddress.HostToNetworkOrder(obj);
			case NetWorkEnum.NetWorkToHost:
				return IPAddress.NetworkToHostOrder(obj);
			default:
				return obj;
			}
		}

		public static ushort ToNetWorkNumber(this ushort obj, NetWorkEnum netWork = NetWorkEnum.Normal)
		{
			switch (netWork)
			{
			case NetWorkEnum.HostToNetWork:
				return (ushort)IPAddress.HostToNetworkOrder((short)obj);
			case NetWorkEnum.NetWorkToHost:
				return (ushort)IPAddress.NetworkToHostOrder((short)obj);
			default:
				return obj;
			}
		}

		public static uint ToNetWorkNumber(this uint obj, NetWorkEnum netWork = NetWorkEnum.Normal)
		{
			switch (netWork)
			{
			case NetWorkEnum.HostToNetWork:
				return (uint)IPAddress.HostToNetworkOrder((int)obj);
			case NetWorkEnum.NetWorkToHost:
				return (uint)IPAddress.NetworkToHostOrder((int)obj);
			default:
				return obj;
			}
		}

		public static ulong ToNetWorkNumber(this ulong obj, NetWorkEnum netWork = NetWorkEnum.Normal)
		{
			switch (netWork)
			{
			case NetWorkEnum.HostToNetWork:
				return (ulong)IPAddress.HostToNetworkOrder((long)obj);
			case NetWorkEnum.NetWorkToHost:
				return (ulong)IPAddress.NetworkToHostOrder((long)obj);
			default:
				return obj;
			}
		}

		public static void Copy(this byte[] obj, byte resource, ref int index)
		{
			obj[index++] = resource;
		}

		public static void Copy(this byte[] obj, short resource, ref int index)
		{
			byte[] bytes = BitConverter.GetBytes(resource.ToNetWorkNumber(NetWorkEnum.HostToNetWork));
			obj.CopyBytes(bytes, ref index);
		}

		public static void Copy(this byte[] obj, ushort resource, ref int index)
		{
			byte[] bytes = BitConverter.GetBytes(resource.ToNetWorkNumber(NetWorkEnum.HostToNetWork));
			obj.CopyBytes(bytes, ref index);
		}

		public static void Copy(this byte[] obj, int resource, ref int index)
		{
			byte[] bytes = BitConverter.GetBytes(resource.ToNetWorkNumber(NetWorkEnum.HostToNetWork));
			obj.CopyBytes(bytes, ref index);
		}

		public static void Copy(this byte[] obj, uint resource, ref int index)
		{
			byte[] bytes = BitConverter.GetBytes(resource.ToNetWorkNumber(NetWorkEnum.HostToNetWork));
			obj.CopyBytes(bytes, ref index);
		}

		public static void Copy(this byte[] obj, long resource, ref int index)
		{
			byte[] bytes = BitConverter.GetBytes(resource.ToNetWorkNumber(NetWorkEnum.HostToNetWork));
			obj.CopyBytes(bytes, ref index);
		}

		public static void Copy(this byte[] obj, ulong resource, ref int index)
		{
			byte[] bytes = BitConverter.GetBytes(resource.ToNetWorkNumber(NetWorkEnum.HostToNetWork));
			obj.CopyBytes(bytes, ref index);
		}

		public static void CopyBytes(this byte[] obj, byte[] resource, ref int index)
		{
			Array.Copy(resource, 0, obj, index, resource.Length);
			index += resource.Length;
		}

		public static void CopyBytes(this byte[] buff, byte[] resource, int length, ref int index)
		{
			Array.Copy(resource, 0, buff, index, length);
			index += length;
		}

		public static void CopyBytes(this byte[] buff, byte[] resource, int sourceindex, int length, ref int index)
		{
			Array.Copy(resource, sourceindex, buff, index, length);
			index += length;
		}

		public static void CopyDateTime(this byte[] obj, DateTime resource, ref int index)
		{
			obj.CopyBytes(resource.DateTimeToBytes(), ref index);
		}

		public static byte[] DateTimeToBytes(this DateTime dt)
		{
			int index = 0;
			byte[] array = new byte[7];
			array.Copy((ushort)dt.Year, ref index);
			array.Copy((byte)dt.Month, ref index);
			array.Copy((byte)dt.Day, ref index);
			array.Copy((byte)dt.Hour, ref index);
			array.Copy((byte)dt.Minute, ref index);
			array.Copy((byte)dt.Second, ref index);
			return array;
		}

		public static void CopyString(this byte[] obj, string resource, int length, Encoding encoding, ref int index)
		{
			string text = resource.ToStr();
			byte[] array = new byte[length];
			byte[] bytes;
			while (true)
			{
				bytes = encoding.GetBytes(text);
				if (bytes.Length <= length - 1)
				{
					break;
				}
				text = text.Substring(0, text.Length - 1);
			}
			array.CopyBytes(bytes, 0, bytes.Length);
			obj.CopyBytes(array, ref index);
		}

		public static void CopyString2(this byte[] obj, string resource, Encoding encoding, ref int index)
		{
			string s = resource.ToStr();
			byte[] bytes = encoding.GetBytes(s);
			obj.CopyBytes(bytes, ref index);
		}

		public static void Copy(this byte[] obj, byte resource, int index = 0)
		{
			obj.Copy(resource, ref index);
		}

		public static void Copy(this byte[] obj, short resource, int index = 0)
		{
			obj.Copy(resource, ref index);
		}

		public static void Copy(this byte[] obj, ushort resource, int index = 0)
		{
			obj.Copy(resource, ref index);
		}

		public static void Copy(this byte[] obj, int resource, int index = 0)
		{
			obj.Copy(resource, ref index);
		}

		public static void Copy(this byte[] obj, uint resource, int index = 0)
		{
			obj.Copy(resource, ref index);
		}

		public static void Copy(this byte[] obj, long resource, int index = 0)
		{
			obj.Copy(resource, ref index);
		}

		public static void Copy(this byte[] obj, ulong resource, int index = 0)
		{
			obj.Copy(resource, ref index);
		}

		public static void CopyBytes(this byte[] obj, byte[] resource, int index = 0, int length = -1)
		{
			length = ((length < 0) ? resource.Length : length);
			Array.Copy(resource, 0, obj, index, length);
		}

		public static void CopyDateTime(this byte[] obj, DateTime resource, int index = 0)
		{
			obj.CopyDateTime(resource, ref index);
		}

		public static void CopyString(this byte[] obj, string resource, int length, Encoding encoding, int index)
		{
			obj.CopyString(resource, length, encoding, ref index);
		}

		public static void CopyString2(this byte[] obj, string resource, Encoding encoding, int index)
		{
			obj.CopyString2(resource, encoding, ref index);
		}

		public static byte[] GetBytes(this byte[] obj, int length, ref int index)
		{
			byte[] array = new byte[length];
			Array.Copy(obj, index, array, 0, length);
			index += length;
			return array;
		}

		public static byte GetInt8(this byte[] obj, ref int index)
		{
			return obj[index++];
		}

		public static short GetInt16(this byte[] obj, ref int index)
		{
			return BitConverter.ToInt16(obj.GetBytes(2, ref index), 0).ToNetWorkNumber(NetWorkEnum.NetWorkToHost);
		}

		public static ushort GetUInt16(this byte[] obj, ref int index)
		{
			return BitConverter.ToUInt16(obj.GetBytes(2, ref index), 0).ToNetWorkNumber(NetWorkEnum.NetWorkToHost);
		}

		public static int GetInt32(this byte[] obj, ref int index)
		{
			return BitConverter.ToInt32(obj.GetBytes(4, ref index), 0).ToNetWorkNumber(NetWorkEnum.NetWorkToHost);
		}

		public static uint GetUInt32(this byte[] obj, ref int index)
		{
			return BitConverter.ToUInt32(obj.GetBytes(4, ref index), 0).ToNetWorkNumber(NetWorkEnum.NetWorkToHost);
		}

		public static long GetInt64(this byte[] obj, ref int index)
		{
			return BitConverter.ToInt64(obj.GetBytes(8, ref index), 0).ToNetWorkNumber(NetWorkEnum.NetWorkToHost);
		}

		public static ulong GetUInt64(this byte[] obj, ref int index)
		{
			return BitConverter.ToUInt64(obj.GetBytes(8, ref index), 0).ToNetWorkNumber(NetWorkEnum.NetWorkToHost);
		}

		public static string GetString(this byte[] obj, int length, Encoding encoding, ref int index)
		{
			return encoding.GetString(obj.GetBytes(length, ref index));
		}

		public static DateTime GetDateTime(this byte[] obj, ref int index)
		{
			int index2 = 0;
			byte[] bytes = obj.GetBytes(7, ref index);
			int uInt = bytes.GetUInt16(ref index2);
			int @int = bytes.GetInt8(ref index2);
			int int2 = bytes.GetInt8(ref index2);
			int int3 = bytes.GetInt8(ref index2);
			int int4 = bytes.GetInt8(ref index2);
			int int5 = bytes.GetInt8(ref index2);
			return new DateTime(uInt, @int, int2, int3, int4, int5);
		}

		public static byte[] GetBytes(this byte[] obj, int length, int index = 0)
		{
			return obj.GetBytes(length, ref index);
		}

		public static byte GetInt8(this byte[] obj, int index = 0)
		{
			return obj.GetInt8(ref index);
		}

		public static short GetInt16(this byte[] obj, int index = 0)
		{
			return obj.GetInt16(ref index);
		}

		public static ushort GetUInt16(this byte[] obj, int index = 0)
		{
			return obj.GetUInt16(ref index);
		}

		public static int GetInt32(this byte[] obj, int index = 0)
		{
			return obj.GetInt32(ref index);
		}

		public static uint GetUInt32(this byte[] obj, int index = 0)
		{
			return obj.GetUInt32(ref index);
		}

		public static long GetInt64(this byte[] obj, int index = 0)
		{
			return obj.GetInt64(ref index);
		}

		public static ulong GetUInt64(this byte[] obj, int index = 0)
		{
			return obj.GetUInt64(ref index);
		}

		public static string GetString(this byte[] obj, int length, Encoding encoding, int index = 0)
		{
			return obj.GetString(length, encoding, ref index);
		}

		public static DateTime GetDateTime(this byte[] obj, int index = 0)
		{
			return obj.GetDateTime(ref index);
		}

		public static byte[] StringToBytes(this string str, int format = 16)
		{
			IList<byte> list = new List<byte>();
			for (int i = 0; i < str.Length; i += 2)
			{
				list.Add(Convert.ToByte(str.Substring(i, 2), format));
			}
			return list.ToArray();
		}

		public static string BytesToString(this IEnumerable<byte> buff, string splitChar = "")
		{
			return BitConverter.ToString(buff.ToArray()).Replace("-", splitChar);
		}

		public static string BytesToString(this IEnumerable<byte> buff, string splitChar, string format)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (byte item in buff)
			{
				string value = item.ToString(format);
				stringBuilder.Append(value);
				stringBuilder.Append(splitChar);
			}
			return stringBuilder.ToStr();
		}

		public static byte[] ToNetWorkBuff(this short val)
		{
			return BitConverter.GetBytes(val.ToNetWorkNumber(NetWorkEnum.HostToNetWork));
		}

		public static byte[] ToNetWorkBuff(this int val)
		{
			return BitConverter.GetBytes(val.ToNetWorkNumber(NetWorkEnum.HostToNetWork));
		}

		public static byte[] ToNetWorkBuff(this long val)
		{
			return BitConverter.GetBytes(val.ToNetWorkNumber(NetWorkEnum.HostToNetWork));
		}

		public static byte[] ToNetWorkBuff(this ushort val)
		{
			return BitConverter.GetBytes(val.ToNetWorkNumber(NetWorkEnum.HostToNetWork));
		}

		public static byte[] ToNetWorkBuff(this uint val)
		{
			return BitConverter.GetBytes(val.ToNetWorkNumber(NetWorkEnum.HostToNetWork));
		}

		public static byte[] ToNetWorkBuff(this ulong val)
		{
			return BitConverter.GetBytes(val.ToNetWorkNumber(NetWorkEnum.HostToNetWork));
		}
	}
}
