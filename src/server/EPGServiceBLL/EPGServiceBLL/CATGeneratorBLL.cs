using System;
using System.Collections.Generic;
using System.Linq;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.Model;
using DeXin.EPG.Tools;
using EPGServiceBLL.SendOut;

namespace EPGServiceBLL
{
	public static class CATGeneratorBLL
	{
		private static Dictionary<long, int> versionpool = new Dictionary<long, int>();

		private static Dictionary<long, List<uint>> crcpool = new Dictionary<long, List<uint>>();

		private static int GetVersion(long tsidentity, int tableversion)
		{
			int result = tableversion;
			if (tableversion == -1)
			{
				if (versionpool.ContainsKey(tsidentity))
				{
					versionpool[tsidentity] = (versionpool[tsidentity] + 1) % 32;
				}
				else
				{
					versionpool[tsidentity] = 0;
				}
				result = versionpool[tsidentity];
			}
			return result;
		}

		private static int FirstGetVersion(long tsidentity, int tableversion)
		{
			int result = tableversion;
			if (tableversion == -1)
			{
				result = ((!versionpool.ContainsKey(tsidentity)) ? GetVersion(tsidentity, tableversion) : versionpool[tsidentity]);
			}
			return result;
		}

		public static IList<byte[]> GenerateCATSections(TsInfo tsinfo)
		{
			bool ischanged = false;
			IList<byte[]> list = new List<byte[]>();
			int num = FirstGetVersion(tsinfo.Identity, tsinfo.CatVersionNumber);
			list = GenerateCATSections(tsinfo, num, out ischanged);
			if (ischanged)
			{
				num = GetVersion(tsinfo.Identity, tsinfo.CatVersionNumber);
				list = GenerateCATSections(tsinfo, num, out ischanged);
			}
			string msg = string.Format("CAT NetWork:[0x{0:X4}]{1}    TS:[0x{2:X4}]{3}    {4}   Version:[{5}]", tsinfo.NIT.NetworkId, tsinfo.NIT.NetworkName, tsinfo.TsId, tsinfo.TsName, (tsinfo.CatVersionNumber == -1) ? "Auto" : "", num);
			Tool.WriteTXTLog(LogLevel.DEBUG, msg);
			return list;
		}

		private static IList<byte[]> GenerateCATSections(TsInfo tsinfo, int version, out bool ischanged)
		{
			ischanged = false;
			IList<byte[]> list = new List<byte[]>();
			IList<DescriptorSigleBase> descriptors = TSOutputManager.CATDescriptorList.Where((DescriptorSigleBase p) => p.TableID == tsinfo.Identity).ToList();
			byte[] array = new byte[8];
			IList<byte[]> sections = GetSections(descriptors, 1024);
			int count = sections.Count;
			count &= 0xFFF;
			List<uint> list2 = new List<uint>();
			for (int num = 0; num < count; num++)
			{
				byte[] array2 = sections[num];
				array2[0] = 1;
				array2[1] = (byte)(0xB0 | ((array2.Length - 3 >> 8) & 0xF));
				array2[2] = (byte)((array2.Length - 3) & 0xFF);
				array2[3] = byte.MaxValue;
				array2[4] = byte.MaxValue;
				array2[5] = (byte)(0x3FC0 | ((version & 0x1F) << 1) | 1);
				array2[6] = (byte)num;
				array2[7] = (byte)(count - 1);
				uint item = CRCGeneratorBLL.SetSectionCRC(array2);
				list2.Add(item);
				list.Add(array2);
			}
			if (crcpool.ContainsKey(tsinfo.Identity))
			{
				if (crcpool[tsinfo.Identity].Count != list2.Count)
				{
					ischanged = true;
				}
				else
				{
					for (int num = 0; num < list2.Count; num++)
					{
						if (crcpool[tsinfo.Identity][num] != list2[num])
						{
							ischanged = true;
							break;
						}
					}
				}
			}
			else
			{
				crcpool[tsinfo.Identity] = new List<uint>();
				ischanged = false;
			}
			crcpool[tsinfo.Identity].Clear();
			crcpool[tsinfo.Identity].AddRange(list2);
			return list;
		}

		private static IList<byte[]> GenerateCATPackages(byte[] section, int pid)
		{
			IList<byte[]> list = new List<byte[]>();
			byte[] fFByte = GetFFByte(188);
			int num = ((section[1] & 0xF) << 8) | section[2];
			int num2 = (section.Length + 1) / 184;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			byte[] array = new byte[5] { 71, 64, 0, 16, 0 };
			array[1] = (byte)(((array[1] & 0xF) << 4) | ((pid >> 8) & 0x1F));
			array[2] = (byte)(pid & 0xFF);
			num5 = 2;
			for (int i = 0; i < num2; i++)
			{
				num4 = 0;
				num3 = ((i + 1 == num2) ? (section.Length % 184) : 184);
				Array.Copy(array, 0, fFByte, num4, 4);
				num4 += 4;
				if (i == 0)
				{
					num3 = ((i + 1 == num2) ? (section.Length % 183) : 183);
					fFByte[num4] = 0;
					num4++;
				}
				if (i > 0)
				{
					fFByte[num4++] = (byte)((pid >> 8) & 0x1F);
				}
				Array.Copy(section, num5, fFByte, num4, num3);
				num5 += num3;
				fFByte[3] = (byte)(((num5 & 0xF) << 4) + (i & 0xF));
				list.Add(fFByte);
			}
			return list;
		}

		private static byte[] GetFFByte(int length)
		{
			byte[] array = new byte[length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = byte.MaxValue;
			}
			return array;
		}

		private static IList<byte[]> GetSections(IList<DescriptorSigleBase> descriptors, byte[] headData, int sectionLength)
		{
			IList<byte[]> list = new List<byte[]>();
			IList<byte> list2 = new List<byte>();
			if (descriptors.Count > 0)
			{
				for (int i = 0; i < descriptors.Count; i++)
				{
					if (list2.Count + headData.Length + descriptors[i].GetData().Length + 4 > sectionLength)
					{
						AddCRC(list, list2);
						list2 = new List<byte>();
					}
					if (list2.Count == 0)
					{
						ArrayToList(headData, list2);
					}
					ArrayToList(descriptors[i].GetData(), list2);
					if (i == descriptors.Count - 1)
					{
						AddCRC(list, list2);
					}
				}
			}
			else
			{
				ArrayToList(headData, list2);
				AddCRC(list, list2);
			}
			return list;
		}

		private static IList<byte[]> GetSections(IList<DescriptorSigleBase> descriptors, int sectionLength)
		{
			IList<byte[]> list = new List<byte[]>();
			IList<byte> list2 = new List<byte>();
			byte[] array = new byte[8];
			int i = 0;
			do
			{
				bool flag = true;
				array = new byte[8];
				list2 = new List<byte>();
				ArrayToList(array, list2);
				for (; i < descriptors.Count && list2.Count + descriptors[i].GetData().Length + 4 <= sectionLength; i++)
				{
					ArrayToList(descriptors[i].GetData(), list2);
				}
				AddCRC(list, list2);
			}
			while (i < descriptors.Count - 1);
			return list;
		}

		public static void AddCRC(IList<byte[]> resultValue, IList<byte> temp)
		{
			temp.Add(byte.MaxValue);
			temp.Add(byte.MaxValue);
			temp.Add(byte.MaxValue);
			temp.Add(byte.MaxValue);
			resultValue.Add(temp.ToArray());
		}

		public static void ArrayToList(byte[] resource, IList<byte> returnLst)
		{
			foreach (byte item in resource)
			{
				returnLst.Add(item);
			}
		}
	}
}
