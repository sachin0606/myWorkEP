using System.Collections.Generic;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.Model;

namespace EPGServiceBLL
{
	public static class PATGeneratorBLL
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

		public static IList<byte[]> GeneratePATSections(IList<ProgramInfo> programs)
		{
			IList<byte[]> result = new List<byte[]>();
			if (programs.Count == 0)
			{
				return result;
			}
			bool ischanged = false;
			TsInfo tS = programs[0].TS;
			int num = FirstGetVersion(tS.Identity, tS.SdtVersionNumber);
			result = GeneratePATSections(tS, programs, num, out ischanged);
			if (ischanged)
			{
				num = GetVersion(tS.Identity, tS.PatVersionNumber);
				result = GeneratePATSections(tS, programs, num, out ischanged);
			}
			string msg = string.Format("PAT NetWork:[0x{0:X4}]{1}    TS:[0x{2:X4}]{3}    {4}   Version:[{5}]", tS.NIT.NetworkId, tS.NIT.NetworkName, tS.TsId, tS.TsName, (tS.PatVersionNumber == -1) ? "Auto" : "", num);
			Tool.WriteTXTLog(LogLevel.DEBUG, msg);
			return result;
		}

		private static IList<byte[]> GeneratePATSections(TsInfo tsinfo, IList<ProgramInfo> programs, int version, out bool ischanged)
		{
			ischanged = false;
			IList<byte[]> list = new List<byte[]>();
			int num = 0;
			do
			{
				bool flag = true;
				if (list.Count > 255)
				{
					break;
				}
				int num2 = 0;
				byte[] array = new byte[1024];
				array[num2++] = 0;
				array[num2++] = 176;
				array[num2++] = 0;
				array[num2++] = (byte)(tsinfo.TsId >> 8);
				array[num2++] = (byte)(tsinfo.TsId & 0xFF);
				array[num2] = 193;
				byte b = (byte)((version & 0x1F) << 1);
				array[num2] |= b;
				num2++;
				array[num2++] = 0;
				array[num2++] = 0;
				if (list.Count == 0)
				{
					array[num2++] = 0;
					array[num2++] = 0;
					array[num2++] = 224;
					array[num2++] = 16;
				}
				while (num < programs.Count && num2 + 4 + 4 <= 1024)
				{
					ProgramInfo programInfo = programs[num++];
					array[num2++] = (byte)((programInfo.ServiceId >> 8) & 0xFF);
					array[num2++] = (byte)(programInfo.ServiceId & 0xFF);
					array[num2++] = (byte)(0xE0 | ((programInfo.PmtId >> 8) & 0xFF));
					array[num2++] = (byte)(programInfo.PmtId & 0xFF);
				}
				num2 += 4;
				int num3 = num2 - 3;
				array[1] &= 240;
				array[1] |= (byte)((num3 >> 8) & 0xF);
				array[2] = (byte)(num3 & 0xFF);
				list.Add(array);
			}
			while (num != programs.Count);
			List<uint> list2 = new List<uint>();
			for (int i = 0; i < list.Count; i++)
			{
				byte[] array = list[i];
				array[6] = (byte)i;
				array[7] = (byte)(list.Count - 1);
				uint item = CRCGeneratorBLL.SetSectionCRC(array);
				list2.Add(item);
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
	}
}
