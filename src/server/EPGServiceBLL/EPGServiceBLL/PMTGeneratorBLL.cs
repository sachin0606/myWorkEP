using System.Collections.Generic;
using System.Linq;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.Model;
using DeXin.EPG.Tools;
using EPGServiceBLL.SendOut;

namespace EPGServiceBLL
{
	public class PMTGeneratorBLL
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

		private static int FirstGetVersion(long tsidentity, int tableversion)
		{
			int result = tableversion;
			if (tableversion == -1)
			{
				result = ((!versionpool.ContainsKey(tsidentity)) ? GetVersion(tsidentity, tableversion) : versionpool[tsidentity]);
			}
			return result;
		}

		public IList<byte[]> GetSectionData(ProgramInfo prg, IList<Multimedia> prgMultimediaList)
		{
			bool ischanged = false;
			IList<byte[]> list = new List<byte[]>();
			int num = FirstGetVersion(prg.Identity, prg.PmtVersionNumber);
			list = GetSectionData(prg, prgMultimediaList, num, out ischanged);
			if (ischanged)
			{
				num = GetVersion(prg.Identity, prg.PmtVersionNumber);
				list = GetSectionData(prg, prgMultimediaList, num, out ischanged);
			}
			string msg = string.Format("PMT NetWork:[0x{0:X4}]{1}    TS:[0x{2:X4}]{3}   Program:[0x{4:X4}]{5}    {6}   Version:[{7}]", prg.TS.NIT.NetworkId, prg.TS.NIT.NetworkName, prg.TS.TsId, prg.TS.TsName, prg.ServiceId, prg.ProgramName, (prg.PmtVersionNumber == -1) ? "Auto" : "", num);
			Tool.WriteTXTLog(LogLevel.DEBUG, msg);
			return list;
		}

		private IList<byte[]> GetSectionData(ProgramInfo prg, IList<Multimedia> prgMultimediaList, int version, out bool ischanged)
		{
			ischanged = false;
			IList<DescriptorSigleBase> list = TSOutputManager.PMTDescriptorList.Where((DescriptorSigleBase p) => p.TableID == prg.Identity).ToList();
			IList<byte[]> list2 = new List<byte[]>();
			byte[] array = new byte[1024];
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int count = list.Count;
			int num7 = 0;
			int count2 = prgMultimediaList.Count;
			int num8 = 0;
			int num9 = 0;
			do
			{
				bool flag = true;
				array = new byte[1024];
				if (num >= 256)
				{
					break;
				}
				num3 = num * 1024;
				num2 = num++;
				array[0] = 2;
				array[1] = 176;
				array[2] = 0;
				array[3] = (byte)(prg.ServiceId >> 8);
				array[4] = (byte)(prg.ServiceId & 0xFF);
				array[5] = 193;
				array[5] |= (byte)(version << 1);
				array[6] = (byte)num2;
				array[7] = 0;
				array[8] = 224;
				array[8] |= (byte)((prg.PCRPID & 0x1FFF) >> 8);
				array[9] = (byte)(prg.PCRPID & 0xFF);
				array[10] = 240;
				array[11] = 0;
				num4 = 12;
				for (; num6 < count; num6++)
				{
					DescriptorSigleBase descriptorSigleBase = list[num6];
					if (descriptorSigleBase != null)
					{
						byte[] data = descriptorSigleBase.GetData();
						int num10 = data.Length;
						if (num4 + num10 + 4 > 1024)
						{
							break;
						}
						for (int num11 = 0; num11 < num10; num11++)
						{
							array[num4 + num11] = data[num11];
						}
						num4 += num10;
					}
				}
				num5 = num4 - 10 - 2;
				array[10] &= 240;
				array[10] |= (byte)(num5 >> 8);
				array[11] = (byte)num5;
				while (num7 < count2)
				{
					Multimedia multimedia = prgMultimediaList[num7];
					if (multimedia != null)
					{
						if (num4 + 5 + 4 > 1024)
						{
							break;
						}
						array[num4++] = (byte)multimedia.PESType.StreamType;
						num5 = multimedia.PesPid & 0x1FFF;
						array[num4++] = (byte)(0xE0 | (num5 >> 8));
						array[num4++] = (byte)num5;
						int num12 = num4;
						array[num4++] = 240;
						array[num4++] = 0;
						IList<DescriptorSigleBase> list3 = TSOutputManager.MultimediaDescriptorList.Where((DescriptorSigleBase p) => p.TableID == multimedia.Id).ToList();
						for (num9 = list3.Count; num8 < num9; num8++)
						{
							DescriptorSigleBase descriptorSigleBase = list3[num8];
							if (descriptorSigleBase != null)
							{
								byte[] data = descriptorSigleBase.GetData();
								int num10 = data.Length;
								if (num4 + num10 + 4 > 1024)
								{
									break;
								}
								for (int num11 = 0; num11 < num10; num11++)
								{
									array[num4 + num11] = data[num11];
								}
								num4 += num10;
							}
						}
						num5 = num4 - num12 - 2;
						array[num12] &= 240;
						array[num12] |= (byte)(num5 >> 8);
						array[num12 + 1] = (byte)num5;
						if (num8 < num9)
						{
							break;
						}
					}
					num7++;
					num8 = 0;
				}
				int num13 = num4 + 4 - 3;
				array[1] &= 240;
				array[1] |= (byte)((num13 >> 8) & 0xF);
				array[2] = (byte)(num13 & 0xFF);
				list2.Add(array);
			}
			while (num7 < count2);
			int count3 = list2.Count;
			List<uint> list4 = new List<uint>();
			for (int num14 = 0; num14 < count3; num14++)
			{
				list2[num14][6] = (byte)num14;
				list2[num14][7] = (byte)(count3 - 1);
				uint item = CRCGeneratorBLL.SetSectionCRC(list2[num14]);
				list4.Add(item);
			}
			if (crcpool.ContainsKey(prg.Identity))
			{
				if (crcpool[prg.Identity].Count != list4.Count)
				{
					ischanged = true;
				}
				else
				{
					for (int num14 = 0; num14 < list4.Count; num14++)
					{
						if (crcpool[prg.Identity][num14] != list4[num14])
						{
							ischanged = true;
							break;
						}
					}
				}
			}
			else
			{
				crcpool[prg.Identity] = new List<uint>();
				ischanged = false;
			}
			crcpool[prg.Identity].Clear();
			crcpool[prg.Identity].AddRange(list4);
			return list2;
		}
	}
}
