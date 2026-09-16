using System.Collections.Generic;
using System.Linq;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Model;
using DeXin.EPG.Tools;
using EPGServiceBLL.SendOut;

namespace EPGServiceBLL
{
	public static class NITGeneratorBLL
	{
		private static Dictionary<long, int> versionpool = new Dictionary<long, int>();

		private static Dictionary<long, List<uint>> crcpool = new Dictionary<long, List<uint>>();

		private static int GetVersion(long identity, int tableversion)
		{
			int result = tableversion;
			if (tableversion == -1)
			{
				if (versionpool.ContainsKey(identity))
				{
					versionpool[identity] = (versionpool[identity] + 1) % 32;
				}
				else
				{
					versionpool[identity] = 0;
				}
				result = versionpool[identity];
			}
			return result;
		}

		private static int FirstGetVersion(long identity, int tableversion)
		{
			int result = tableversion;
			if (tableversion == -1)
			{
				result = ((!versionpool.ContainsKey(identity)) ? GetVersion(identity, tableversion) : versionpool[identity]);
			}
			return result;
		}

		public static IList<byte[]> GenerateNITSections(NitInfo nit, IList<TsInfo> args)
		{
			bool ischanged = false;
			IList<byte[]> list = new List<byte[]>();
			int num = FirstGetVersion(nit.Identity, nit.VersionNumber);
			list = GenerateNITSections(nit, args, num, out ischanged);
			if (ischanged)
			{
				num = GetVersion(nit.Identity, nit.VersionNumber);
				list = GenerateNITSections(nit, args, num, out ischanged);
			}
			string msg = string.Format("NIT NetWork:[0x{0:X4}]{1}   {2}   Version:[{3}]", nit.NetworkId, nit.NetworkName, (nit.VersionNumber == -1) ? "Auto" : "", num);
			Tool.WriteTXTLog(LogLevel.DEBUG, msg);
			return list;
		}

		private static IList<byte[]> GenerateNITSections(NitInfo nit, IList<TsInfo> args, int version, out bool ischanged)
		{
			ischanged = false;
			IList<byte[]> list = new List<byte[]>();
			IList<DescriptorSigleBase> nit_des = TSOutputManager.NITDescriptorList.Where((DescriptorSigleBase p) => p.TableID == nit.Identity).ToList();
			TsInfo item;
			foreach (TsInfo arg in args)
			{
				item = arg;
				item.Descriptors = TSOutputManager.NIT_TSDescriptorList.Where((DescriptorSigleBase p) => p.TSTableID == item.Identity).ToList();
			}
			IList<byte[]> sections = GetSections(nit, nit_des, args, 1024);
			int count = sections.Count;
			count &= 0xFFF;
			List<uint> list2 = new List<uint>();
			for (int num = 0; num < count; num++)
			{
				byte[] array = sections[num];
				array[1] = 0;
				array[1] = (byte)(0xF0 | (array.Length - 3 >> 8));
				array[2] = (byte)((array.Length - 3) & 0xFF);
				array[3] = (byte)((nit.NetworkId >> 8) & 0xFF);
				array[4] = (byte)(nit.NetworkId & 0xFF);
				array[5] = (byte)(0xC0 | ((version & 0x1F) << 1) | 1);
				array[6] = (byte)num;
				array[7] = (byte)(count - 1);
				uint item2 = CRCGeneratorBLL.SetSectionCRC(array);
				list2.Add(item2);
				list.Add(array);
			}
			if (crcpool.ContainsKey(nit.Identity))
			{
				if (crcpool[nit.Identity].Count != list2.Count)
				{
					ischanged = true;
				}
				else
				{
					for (int num = 0; num < list2.Count; num++)
					{
						if (crcpool[nit.Identity][num] != list2[num])
						{
							ischanged = true;
							break;
						}
					}
				}
			}
			else
			{
				crcpool[nit.Identity] = new List<uint>();
				ischanged = false;
			}
			crcpool[nit.Identity].Clear();
			crcpool[nit.Identity].AddRange(list2);
			return list;
		}

		private static IList<byte[]> SectionToPackages(byte[] section, int pid)
		{
			IList<byte[]> result = new List<byte[]>();
			IList<byte> returnLst = new List<byte>();
			byte[] resource = new byte[4];
			CATGeneratorBLL.ArrayToList(resource, returnLst);
			return result;
		}

		private static IList<byte[]> GetSections(NitInfo nit, IList<DescriptorSigleBase> nit_des, IList<TsInfo> args, int sectionLength)
		{
			IList<byte[]> list = new List<byte[]>();
			IList<byte> list2 = new List<byte>();
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			int num8 = 0;
			byte b = 0;
			byte[] array = new byte[10];
			do
			{
				bool flag = true;
				list2 = new List<byte>();
				CATGeneratorBLL.ArrayToList(new byte[10] { 64, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, list2);
				num4 = 0;
				DescriptorType networkNameDescriptorType = new DescriptorTypeManager().GetDescriptorTypesByLoaction(EnumPossibleLoaction.NIT).Single((DescriptorType x) => x.DesTag == 64);
				DescriptorSigleBase descriptorSigleBase = nit_des.SingleOrDefault((DescriptorSigleBase x) => x.DesTypeID == networkNameDescriptorType.Id);
				if (descriptorSigleBase != null)
				{
					CATGeneratorBLL.ArrayToList(descriptorSigleBase.GetData(), list2);
					num4 += descriptorSigleBase.GetData().Length;
				}
				for (; num < nit_des.Count; num++)
				{
					if (descriptorSigleBase == null || nit_des[num].Identity != descriptorSigleBase.Identity)
					{
						if (list2.Count + nit_des[num].GetData().Length + 2 + 4 > sectionLength)
						{
							break;
						}
						CATGeneratorBLL.ArrayToList(nit_des[num].GetData(), list2);
						num4 += nit_des[num].GetData().Length;
					}
				}
				list2[8] = (byte)(0xF0 | ((num4 >> 8) & 0xF));
				list2[9] = (byte)(num4 & 0xFF);
				num7 = list2.Count;
				num5 = 0;
				b = 240;
				list2.Add(b);
				b = 0;
				list2.Add(b);
				while (num2 < args.Count)
				{
					if (args[num2].Descriptors.Sum((DescriptorSigleBase x) => x.DataLength) > sectionLength)
					{
						if (list2.Count + 2 + 4 + 6 > sectionLength)
						{
							break;
						}
					}
					else if (list2.Count + 2 + 4 + 6 + args[num2].Descriptors.Sum((DescriptorSigleBase x) => x.GetData().Length) > sectionLength)
					{
						break;
					}
					b = (byte)((args[num2].TsId >> 8) & 0xFF);
					list2.Add(b);
					b = (byte)(args[num2].TsId & 0xFF);
					list2.Add(b);
					b = (byte)((args[num2].OriginalNetworkId >> 8) & 0xFF);
					list2.Add(b);
					b = (byte)(args[num2].OriginalNetworkId & 0xFF);
					list2.Add(b);
					num8 = list2.Count;
					list2.Add(0);
					list2.Add(0);
					num5 += 6;
					num6 = 0;
					for (; num3 < args[num2].Descriptors.Count && list2.Count + 4 + args[num2].Descriptors[num3].GetData().Length <= sectionLength; num3++)
					{
						CATGeneratorBLL.ArrayToList(args[num2].Descriptors[num3].GetData(), list2);
						num6 += args[num2].Descriptors[num3].GetData().Length;
					}
					num5 += num6;
					list2[num8] = (byte)(0xF0 | ((num6 >> 8) & 0xF));
					list2[num8 + 1] = (byte)(num6 & 0xFF);
					if (num3 < args[num2].Descriptors.Count)
					{
						break;
					}
					num2++;
					num3 = 0;
				}
				list2[num7] = (byte)(0xFF0 | ((num5 >> 8) & 0xF));
				list2[num7 + 1] = (byte)(num5 & 0xFF);
				CATGeneratorBLL.AddCRC(list, list2);
			}
			while (num2 < args.Count || num < nit_des.Count);
			return list;
		}
	}
}
