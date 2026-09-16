using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dexin.Util.Common.ExtensionHelper;

namespace Dexin.Util.Common
{
	public static class ArrayHelper
	{
		public static bool CompareBytes(byte[] source, byte[] comparison)
		{
			int num = source.Length;
			if (source.Length != comparison.Length)
			{
				return false;
			}
			for (int i = 0; i < num; i++)
			{
				if (source[i] != comparison[i])
				{
					return false;
				}
			}
			return true;
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

		public static string BytesToString(this byte[] buff, string splitChar = "")
		{
			return BitConverter.ToString(buff).Replace("-", splitChar);
		}

		public static string BytesToString(this byte[] buff, string splitChar, string format)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (byte b in buff)
			{
				string value = b.ToString(format);
				stringBuilder.Append(value);
			}
			return BitConverter.ToString(buff).Replace("-", splitChar);
		}

		public static void Copy(this byte[] obj, byte resource, ref int index)
		{
			obj[index++] = resource;
		}

		public static void Copy(this byte[] obj, short resource, ref int index, bool toNetBuffer = true)
		{
			byte[] bytes = BitConverter.GetBytes(toNetBuffer ? resource.ToNetWorkNumber() : resource);
			obj.CopyBytes(bytes, ref index);
		}

		public static void Copy(this byte[] obj, ushort resource, ref int index, bool toNetBuffer = true)
		{
			byte[] bytes = BitConverter.GetBytes(toNetBuffer ? resource.ToNetWorkNumber() : resource);
			obj.CopyBytes(bytes, ref index);
		}

		public static void Copy(this byte[] obj, int resource, ref int index, bool toNetBuffer = true)
		{
			byte[] bytes = BitConverter.GetBytes(toNetBuffer ? resource.ToNetWorkNumber() : resource);
			obj.CopyBytes(bytes, ref index);
		}

		public static void Copy(this byte[] obj, uint resource, ref int index, bool toNetBuffer = true)
		{
			byte[] bytes = BitConverter.GetBytes(toNetBuffer ? resource.ToNetWorkNumber() : resource);
			obj.CopyBytes(bytes, ref index);
		}

		public static void Copy(this byte[] obj, long resource, ref int index, bool toNetBuffer = true)
		{
			byte[] bytes = BitConverter.GetBytes(toNetBuffer ? resource.ToNetWorkNumber() : resource);
			obj.CopyBytes(bytes, ref index);
		}

		public static void Copy(this byte[] obj, ulong resource, ref int index, bool toNetBuffer = true)
		{
			byte[] bytes = BitConverter.GetBytes(toNetBuffer ? resource.ToNetWorkNumber() : resource);
			obj.CopyBytes(bytes, ref index);
		}

		public static void Copy(this byte[] obj, byte resource, int index = 0, bool toNetBuffer = true)
		{
			obj.Copy(resource, ref index, toNetBuffer);
		}

		public static void Copy(this byte[] obj, short resource, int index = 0, bool toNetBuffer = true)
		{
			obj.Copy(resource, ref index, toNetBuffer);
		}

		public static void Copy(this byte[] obj, ushort resource, int index = 0, bool toNetBuffer = true)
		{
			obj.Copy(resource, ref index, toNetBuffer);
		}

		public static void Copy(this byte[] obj, int resource, int index = 0, bool toNetBuffer = true)
		{
			obj.Copy(resource, ref index, toNetBuffer);
		}

		public static void Copy(this byte[] obj, uint resource, int index = 0, bool toNetBuffer = true)
		{
			obj.Copy(resource, ref index, toNetBuffer);
		}

		public static void Copy(this byte[] obj, long resource, int index = 0, bool toNetBuffer = true)
		{
			obj.Copy(resource, ref index, toNetBuffer);
		}

		public static void Copy(this byte[] obj, ulong resource, int index = 0, bool toNetBuffer = true)
		{
			obj.Copy(resource, ref index, toNetBuffer);
		}

		public static void CopyBytes(this byte[] obj, byte[] resource, ref int index)
		{
			obj.CopyBytes(resource, resource.Length, ref index);
		}

		public static void CopyBytes(this byte[] obj, byte[] resource, int length, ref int index)
		{
			obj.CopyBytes(resource, 0, length, ref index);
		}

		public static void CopyBytes(this byte[] obj, byte[] resource, int sourceindex, int length, ref int index)
		{
			Array.Copy(resource, sourceindex, obj, index, length);
			index += length;
		}

		public static void CopyBytes(this byte[] obj, byte[] resource, int length, int index = 0)
		{
			obj.CopyBytes(resource, length, ref index);
		}

		public static void CopyString(this byte[] obj, string resource, Encoding encoding, ref int index)
		{
			byte[] bytes = encoding.GetBytes(resource);
			obj.CopyBytes(bytes, ref index);
		}

		public static void CopyString(this byte[] obj, string resource, Encoding encoding, int index)
		{
			obj.CopyString(resource, encoding, ref index);
		}

		public static byte[] GetBytes(this byte[] obj, int length, int index = 0)
		{
			return obj.GetBytes(length, ref index);
		}

		public static byte[] GetBytes(this byte[] obj, int length, ref int index)
		{
			byte[] array = new byte[length];
			Array.Copy(obj, index, array, 0, length);
			index += length;
			return array;
		}

		public static byte[] GetBytes(this byte[] obj, long length, long index = 0L)
		{
			return obj.GetBytes(length, ref index);
		}

		public static byte[] GetBytes(this byte[] obj, long length, ref long index)
		{
			byte[] array = new byte[length];
			Array.Copy(obj, index, array, 0L, length);
			index += length;
			return array;
		}

		public static byte GetInt8(this byte[] obj, ref int index)
		{
			return obj[index++];
		}

		public static short GetInt16(this byte[] obj, ref int index, bool toHostBuffer = true)
		{
			short num = BitConverter.ToInt16(obj.GetBytes(2, ref index), 0);
			return toHostBuffer ? num.ToHostNumber() : num;
		}

		public static ushort GetUInt16(this byte[] obj, ref int index, bool toHostBuffer = true)
		{
			ushort num = BitConverter.ToUInt16(obj.GetBytes(2, ref index), 0);
			return toHostBuffer ? num.ToHostNumber() : num;
		}

		public static int GetInt32(this byte[] obj, ref int index, bool toHostBuffer = true)
		{
			int num = BitConverter.ToInt32(obj.GetBytes(4, ref index), 0);
			return toHostBuffer ? num.ToHostNumber() : num;
		}

		public static uint GetUInt32(this byte[] obj, ref int index, bool toHostBuffer = true)
		{
			uint num = BitConverter.ToUInt32(obj.GetBytes(4, ref index), 0);
			return toHostBuffer ? num.ToHostNumber() : num;
		}

		public static long GetInt64(this byte[] obj, ref int index, bool toHostBuffer = true)
		{
			long num = BitConverter.ToInt64(obj.GetBytes(8, ref index), 0);
			return toHostBuffer ? num.ToHostNumber() : num;
		}

		public static ulong GetUInt64(this byte[] obj, ref int index, bool toHostBuffer = true)
		{
			ulong num = BitConverter.ToUInt64(obj.GetBytes(8, ref index), 0);
			return toHostBuffer ? num.ToHostNumber() : num;
		}

		public static byte GetInt8(this byte[] obj, int index = 0)
		{
			return obj.GetInt8(ref index);
		}

		public static short GetInt16(this byte[] obj, int index = 0, bool toHostBuffer = true)
		{
			return obj.GetInt16(ref index, toHostBuffer);
		}

		public static ushort GetUInt16(this byte[] obj, int index = 0, bool toHostBuffer = true)
		{
			return obj.GetUInt16(ref index, toHostBuffer);
		}

		public static int GetInt32(this byte[] obj, int index = 0, bool toHostBuffer = true)
		{
			return obj.GetInt32(ref index, toHostBuffer);
		}

		public static uint GetUInt32(this byte[] obj, int index = 0, bool toHostBuffer = true)
		{
			return obj.GetUInt32(ref index, toHostBuffer);
		}

		public static long GetInt64(this byte[] obj, int index = 0, bool toHostBuffer = true)
		{
			return obj.GetInt64(ref index, toHostBuffer);
		}

		public static ulong GetUInt64(this byte[] obj, int index = 0, bool toHostBuffer = true)
		{
			return obj.GetUInt64(ref index, toHostBuffer);
		}

		public static string GetString(this byte[] obj, int length, Encoding encoding, ref int index)
		{
			return encoding.GetString(obj.GetBytes(length, ref index));
		}

		public static string GetString(this byte[] obj, int length, Encoding encoding, int index = 0)
		{
			return obj.GetString(length, encoding, ref index);
		}
	}
}
