using System.Collections.Generic;
using System.Linq;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.Model;
using DeXin.EPG.Tools;
using EPGServiceBLL.SendOut;

namespace EPGServiceBLL
{
	public static class SDTGeneratorBLL
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

		public static IList<byte[]> GenerateSDTSections(IList<ProgramInfo> programs)
		{
			bool ischanged = false;
			IList<byte[]> result = new List<byte[]>();
			if (programs.Count == 0)
			{
				return result;
			}
			TsInfo tS = programs[0].TS;
			int num = FirstGetVersion(tS.Identity, tS.SdtVersionNumber);
			result = GenerateSDTSections(tS, programs, num, out ischanged);
			if (ischanged)
			{
				num = GetVersion(tS.Identity, tS.SdtVersionNumber);
				result = GenerateSDTSections(tS, programs, num, out ischanged);
			}
			string msg = string.Format("SDT NetWork:[0x{0:X4}]{1}    TS:[0x{2:X4}]{3}    {4}   Version:[{5}]", tS.NIT.NetworkId, tS.NIT.NetworkName, tS.TsId, tS.TsName, (tS.SdtVersionNumber == -1) ? "Auto" : "", num);
			Tool.WriteTXTLog(LogLevel.DEBUG, msg);
			return result;
		}

		private static IList<byte[]> GenerateSDTSections(TsInfo tsinfo, IList<ProgramInfo> programs, int version, out bool ischanged)
		{
			ischanged = false;
			IList<byte[]> list = new List<byte[]>();
			IList<byte[]> sections = GetSections1(tsinfo, programs, 1024);
			int count = sections.Count;
			count &= 0xFFF;
			List<uint> list2 = new List<uint>();
			for (int i = 0; i < count; i++)
			{
				byte[] array = sections[i];
				array[0] = 66;
				array[1] = 0;
				array[1] = (byte)(0xF0 | (array.Length - 3 >> 8));
				array[2] = (byte)((array.Length - 3) & 0xFF);
				array[3] = (byte)((tsinfo.TsId >> 8) & 0xFF);
				array[4] = (byte)(tsinfo.TsId & 0xFF);
				array[5] = (byte)(0xC0 | ((version & 0x1F) << 1) | 1);
				array[6] = (byte)i;
				array[7] = (byte)(count - 1);
				array[8] = (byte)((tsinfo.OriginalNetworkId >> 8) & 0xFF);
				array[9] = (byte)(tsinfo.OriginalNetworkId & 0xFF);
				array[10] = byte.MaxValue;
				uint item = CRCGeneratorBLL.SetSectionCRC(array);
				list2.Add(item);
				list.Add(array);
			}
			if (crcpool.ContainsKey(tsinfo.Identity))
			{
				if (crcpool[tsinfo.Identity].Count != list2.Count)
				{
					ischanged = true;
				}
				else
				{
					for (int i = 0; i < list2.Count; i++)
					{
						if (crcpool[tsinfo.Identity][i] != list2[i])
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

		private static IList<byte[]> GetSections(TsInfo ts, IList<ProgramInfo> programs, int sectionLength)
		{
			IList<byte[]> list = new List<byte[]>();
			IList<byte> list2 = new List<byte>();
			byte[] array = new byte[11];
			int pro_index = 0;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			byte b = 0;
			do
			{
				bool flag = true;
				list2 = new List<byte>();
				array = new byte[11];
				CATGeneratorBLL.ArrayToList(array, list2);
				while (pro_index < programs.Count && list2.Count + 9 <= sectionLength)
				{
					b = (byte)((programs[pro_index].ServiceId >> 8) & 0xFF);
					list2.Add(b);
					b = (byte)(programs[pro_index].ServiceId & 0xFF);
					list2.Add(b);
					b = (byte)(0xFC | ((programs[pro_index].PrgSdtEitscflag & 1) << 1) | (programs[pro_index].PrgSdtEitpfflag & 1));
					list2.Add(b);
					num2 = list2.Count;
					num3 = 0;
					list2.Add(0);
					list2.Add(0);
					List<DescriptorSigleBase> list3;
					for (list3 = TSOutputManager.SDTDescriptorList.Where((DescriptorSigleBase p) => p.TableID == programs[pro_index].Identity).ToList(); num < list3.Count && list2.Count + 4 + list3[num].GetData().Length <= sectionLength; num++)
					{
						CATGeneratorBLL.ArrayToList(list3[num].GetData(), list2);
						num3 += list3[num].GetData().Length;
					}
					list2[num2] = (byte)(((programs[pro_index].PrgSdtRunningstate & 7) << 5) | ((programs[pro_index].PrgSdtFreecamod & 1) << 4) | ((num3 >> 8) & 0xF));
					list2[num2 + 1] = (byte)(num3 & 0xFF);
					if (num < list3.Count)
					{
						break;
					}
					pro_index++;
					num = 0;
				}
				CATGeneratorBLL.AddCRC(list, list2);
			}
			while (pro_index < programs.Count);
			return list;
		}

		private static List<byte[]> GetSections1(TsInfo ts, IList<ProgramInfo> programs, int sectionLength)
		{
			List<byte[]> list = new List<byte[]>();
			List<byte> list2 = new List<byte>();
			byte[] array = new byte[11];
			int num = sectionLength - array.Length;
			int num2 = 0;
			do
			{
				bool flag = true;
				list2 = new List<byte>();
				array = new byte[11];
				CATGeneratorBLL.ArrayToList(array, list2);
				while (num2 < programs.Count)
				{
					ProgramInfo program = programs[num2];
					List<DescriptorSigleBase> sdt_des_data = TSOutputManager.SDTDescriptorList.Where((DescriptorSigleBase p) => p.TableID == program.Identity).ToList();
					List<byte> list3 = program_to_data(program, sdt_des_data);
					if (list3.Count > num)
					{
						if (list2.Count > array.Length)
						{
							CATGeneratorBLL.AddCRC(list, list2);
						}
						List<byte[]> collection = program_to_data(array, program, sdt_des_data, sectionLength);
						list.AddRange(collection);
						num2++;
						break;
					}
					if (list3.Count + list2.Count > sectionLength)
					{
						CATGeneratorBLL.AddCRC(list, list2);
						break;
					}
					list2.AddRange(list3);
					num2++;
					if (num2 >= programs.Count && list2.Count > 0)
					{
						CATGeneratorBLL.AddCRC(list, list2);
					}
				}
			}
			while (num2 < programs.Count);
			return list;
		}

		private static List<byte[]> program_to_data(byte[] head, ProgramInfo program, List<DescriptorSigleBase> sdt_des_data, int sectionLength)
		{
			List<byte[]> list = new List<byte[]>();
			int num = 0;
			int num2 = sectionLength - head.Length;
			do
			{
				bool flag = true;
				List<byte> list2 = new List<byte>();
				list2.AddRange(head);
				byte item = (byte)((program.ServiceId >> 8) & 0xFF);
				list2.Add(item);
				item = (byte)(program.ServiceId & 0xFF);
				list2.Add(item);
				item = (byte)(0xFC | ((program.PrgSdtEitscflag & 1) << 1) | (program.PrgSdtEitpfflag & 1));
				list2.Add(item);
				int count = list2.Count;
				list2.Add(0);
				list2.Add(0);
				int num3 = 0;
				List<byte> list3 = new List<byte>();
				while (num < sdt_des_data.Count)
				{
					byte[] data = sdt_des_data[num].GetData();
					if (list2.Count + list3.Count + 5 + data.Length > num2)
					{
						break;
					}
					num++;
					CATGeneratorBLL.ArrayToList(data, list3);
					num3 += data.Length;
				}
				list2.AddRange(list3);
				list2[count] = (byte)(((program.PrgSdtRunningstate & 7) << 5) | ((program.PrgSdtFreecamod & 1) << 4) | ((num3 >> 8) & 0xF));
				list2[count + 1] = (byte)(num3 & 0xFF);
				CATGeneratorBLL.AddCRC(list, list2);
			}
			while (num < sdt_des_data.Count);
			return list;
		}

		private static List<byte> program_to_data(ProgramInfo program, List<DescriptorSigleBase> sdt_des_data)
		{
			List<byte> list = new List<byte>();
			IList<byte> list2 = new List<byte>();
			int num = 0;
			foreach (DescriptorSigleBase sdt_des_datum in sdt_des_data)
			{
				byte[] data = sdt_des_datum.GetData();
				CATGeneratorBLL.ArrayToList(data, list2);
				num += data.Length;
			}
			byte item = (byte)((program.ServiceId >> 8) & 0xFF);
			list.Add(item);
			item = (byte)(program.ServiceId & 0xFF);
			list.Add(item);
			item = (byte)(0xFC | ((program.PrgSdtEitscflag & 1) << 1) | (program.PrgSdtEitpfflag & 1));
			list.Add(item);
			item = (byte)(((program.PrgSdtRunningstate & 7) << 5) | ((program.PrgSdtFreecamod & 1) << 4) | ((num >> 8) & 0xF));
			list.Add(item);
			item = (byte)(num & 0xFF);
			list.Add(item);
			list.AddRange(list2);
			return list;
		}
	}
}
