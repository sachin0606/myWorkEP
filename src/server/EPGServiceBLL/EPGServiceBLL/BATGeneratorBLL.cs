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
	public static class BATGeneratorBLL
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

		public static IList<byte[]> GenerateBATSections(BATInfo BAT, IList<TsInfo> BAT_TSList)
		{
			IList<byte[]> result = new List<byte[]>();
			if (BAT == null)
			{
				return result;
			}
			bool ischanged = false;
			int num = FirstGetVersion(BAT.Identity, BAT.BATVersionNumber);
			result = GenerateBATSections(BAT, BAT_TSList, num, out ischanged);
			if (ischanged)
			{
				num = GetVersion(BAT.Identity, BAT.BATVersionNumber);
				result = GenerateBATSections(BAT, BAT_TSList, num, out ischanged);
			}
			string msg = string.Format("BAT BAT:[0x{0:X4}]{1}    {2}   Version:[{3}]", BAT.BATId, BAT.BATName, (BAT.BATVersionNumber == -1) ? "Auto" : "", num);
			Tool.WriteTXTLog(LogLevel.DEBUG, msg);
			return result;
		}

		private static IList<byte[]> GenerateBATSections(BATInfo BAT, IList<TsInfo> BAT_TSList, int version, out bool ischanged)
		{
			ischanged = false;
			BAT_TSList = BAT_TSList.OrderBy((TsInfo x) => x.TsId).ToList();
			IList<DescriptorSigleBase> list = TSOutputManager.BATDescriptorList.Where((DescriptorSigleBase p) => p.TableID == BAT.Identity).ToList();
			IList<byte[]> list2 = new List<byte[]>();
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			do
			{
				bool flag = true;
				if (list2.Count > 255)
				{
					break;
				}
				ushort num4 = 0;
				byte[] array = new byte[1024];
				array[num4++] = 74;
				array[num4++] = 240;
				array[num4++] = 0;
				array[num4++] = (byte)(BAT.BATId >> 8);
				array[num4++] = (byte)(BAT.BATId & 0xFF);
				array[num4++] = (byte)(0xC0 | ((version & 0x1F) << 1) | 1);
				array[num4++] = 0;
				array[num4++] = 0;
				ushort num5 = num4;
				array[num4++] = 240;
				array[num4++] = 0;
				ushort num6 = 0;
				for (; num < list.Count; num++)
				{
					DescriptorSigleBase descriptorSigleBase = list[num];
					byte[] data = descriptorSigleBase.GetData();
					if (num4 + data.Length + 6 > 1024)
					{
						break;
					}
					Array.Copy(data, 0, array, num4, data.Length);
					num4 += (ushort)data.Length;
					num6 += (ushort)data.Length;
				}
				array[num5++] |= (byte)((num6 >> 8) & 0xF);
				array[num5] = (byte)(num6 & 0xFF);
				ushort num7 = num4;
				array[num4++] = 240;
				array[num4++] = 0;
				ushort num8 = 0;
				while (num2 < BAT_TSList.Count)
				{
					TsInfo iBAT_TS = BAT_TSList[num2];
					IList<DescriptorSigleBase> list3 = TSOutputManager.BAT_TSDescriptorList.Where((DescriptorSigleBase p) => p.BATTableID == BAT.Identity && p.TSTableID == iBAT_TS.Identity).ToList();
					if (list3.Count != 0)
					{
						if (list3.Sum((DescriptorSigleBase x) => x.DataLength) > 1024)
						{
							if (num4 + 6 + 4 > 1024)
							{
								break;
							}
						}
						else if (num4 + 6 + 4 + list3.Sum((DescriptorSigleBase x) => x.DataLength) > 1024)
						{
							break;
						}
						array[num4++] = (byte)(iBAT_TS.TsId >> 8);
						array[num4++] = (byte)(iBAT_TS.TsId & 0xFF);
						array[num4++] = (byte)(iBAT_TS.OriginalNetworkId >> 8);
						array[num4++] = (byte)(iBAT_TS.OriginalNetworkId & 0xFF);
						ushort num9 = num4;
						array[num4++] = 240;
						array[num4++] = 0;
						num8 += 6;
						ushort num10 = 0;
						for (; num3 < list3.Count; num3++)
						{
							DescriptorSigleBase descriptorSigleBase2 = list3[num3];
							byte[] data2 = descriptorSigleBase2.GetData();
							if (num4 + data2.Length + 4 > 1024)
							{
								break;
							}
							Array.Copy(data2, 0, array, num4, data2.Length);
							num4 += (ushort)data2.Length;
							num10 += (ushort)data2.Length;
							num8 += (ushort)data2.Length;
						}
						array[num9++] |= (byte)((num10 >> 8) & 0xF);
						array[num9] = (byte)(num10 & 0xFF);
						if (num3 < list3.Count)
						{
							break;
						}
					}
					num2++;
					num3 = 0;
				}
				array[num7++] |= (byte)((num8 >> 8) & 0xF);
				array[num7] = (byte)(num8 & 0xFF);
				num4 += 4;
				int num11 = num4 - 3;
				array[1] &= 240;
				array[1] |= (byte)((num11 >> 8) & 0xF);
				array[2] = (byte)(num11 & 0xFF);
				list2.Add(array);
			}
			while (num2 < BAT_TSList.Count || num < list.Count);
			List<uint> list4 = new List<uint>();
			for (int num12 = 0; num12 < list2.Count; num12++)
			{
				byte[] array = list2[num12];
				array[6] = (byte)num12;
				array[7] = (byte)(list2.Count - 1);
				uint item = CRCGeneratorBLL.SetSectionCRC(array);
				list4.Add(item);
			}
			if (crcpool.ContainsKey(BAT.Identity))
			{
				if (crcpool[BAT.Identity].Count != list4.Count)
				{
					ischanged = true;
				}
				else
				{
					for (int num12 = 0; num12 < list4.Count; num12++)
					{
						if (crcpool[BAT.Identity][num12] != list4[num12])
						{
							ischanged = true;
							break;
						}
					}
				}
			}
			else
			{
				crcpool[BAT.Identity] = new List<uint>();
				ischanged = false;
			}
			crcpool[BAT.Identity].Clear();
			crcpool[BAT.Identity].AddRange(list4);
			return list2;
		}
	}
}
