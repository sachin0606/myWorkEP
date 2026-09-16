using System;
using System.Collections.Generic;
using System.Linq;
using DeXin.EPG.Model;
using DeXin.EPG.Tools;
using EPGServiceBLL.SendOut;

namespace EPGServiceBLL
{
	public static class EITGeneratorBLL
	{
		private struct TableIDSegmentNumber
		{
			public byte TableID;

			public byte SegmentNumber;
		}

		private static Dictionary<long, byte> _EITVersionNumberTable = new Dictionary<long, byte>();

		private static Dictionary<long, int> _PrgEIE_P_EventIDTable = new Dictionary<long, int>();

		public static int GetEIT_P_EventID(long programKey)
		{
			return _PrgEIE_P_EventIDTable.Keys.Contains(programKey) ? _PrgEIE_P_EventIDTable[programKey] : (-1);
		}

		public static byte GetEITVersionNumber(long ProgramIdentity)
		{
			byte b = (byte)(_EITVersionNumberTable.Keys.Contains(ProgramIdentity) ? ((uint)(_EITVersionNumberTable[ProgramIdentity] + 1)) : 0u);
			b %= 32;
			_EITVersionNumberTable[ProgramIdentity] = b;
			return b;
		}

		private static byte SetEITCommData(byte[] SectionBuffer, byte TableID, ProgramInfo CurrProgram, byte VersionNumber)
		{
			if (SectionBuffer.Length == 0)
			{
				return 0;
			}
			SectionBuffer[0] = TableID;
			SectionBuffer[1] = 240;
			SectionBuffer[2] = 0;
			SectionBuffer[3] = (byte)(CurrProgram.ServiceId >> 8);
			SectionBuffer[4] = (byte)(CurrProgram.ServiceId & 0xFF);
			SectionBuffer[5] = (byte)(0xC0 | ((VersionNumber << 1) & 0x3E) | 1);
			SectionBuffer[6] = 0;
			SectionBuffer[7] = 0;
			SectionBuffer[8] = (byte)(CurrProgram.TS.TsId >> 8);
			SectionBuffer[9] = (byte)(CurrProgram.TS.TsId & 0xFF);
			SectionBuffer[10] = (byte)(CurrProgram.TS.OriginalNetworkId >> 8);
			SectionBuffer[11] = (byte)(CurrProgram.TS.OriginalNetworkId & 0xFF);
			SectionBuffer[12] = 0;
			SectionBuffer[13] = 0;
			return 14;
		}

		public static byte[] GenerateEITPFSection(EitInfo EIT, bool IsPresentSection, ProgramInfo CurrProgram, byte versionNumber)
		{
			if (IsPresentSection)
			{
				int value = ((EIT == null) ? (-1) : EIT.EventId);
				_PrgEIE_P_EventIDTable[CurrProgram.Identity] = value;
			}
			ushort num = 0;
			ushort num2 = 0;
			byte[] array = new byte[4096];
			num2 += SetEITCommData(array, 78, CurrProgram, versionNumber);
			array[6] = ((!IsPresentSection) ? ((byte)1) : ((byte)0));
			array[7] = 1;
			array[12] = 1;
			array[13] = 78;
			ushort num3 = 0;
			if (EIT != null)
			{
				array[num2++] = (byte)(EIT.EventId >> 8);
				array[num2++] = (byte)(EIT.EventId & 0xFF);
				byte[] array2 = CommTool.TimeToMJDBuf(EIT.StartTime);
				array2.CopyTo(array, num2);
				num2 += 5;
				byte[] array3 = CRCGeneratorBLL.TimeSpanToBCD(EIT.EndTime - EIT.StartTime);
				array3.CopyTo(array, num2);
				num2 += 3;
				num3 = num2;
				byte b = (byte)((!IsPresentSection) ? 1u : 4u);
				array[num2++] = (byte)(((b & 7) << 5) | ((CurrProgram.PrgSdtFreecamod & 1) << 4));
				array[num2++] = 0;
				ushort num4 = 0;
				IList<DescriptorSigleBase> list = TSOutputManager.EITDescriptorList.Where((DescriptorSigleBase p) => p.TableID == EIT.Identity).ToList();
				while (num < list.Count)
				{
					DescriptorSigleBase descriptorSigleBase = list[num];
					byte[] data = descriptorSigleBase.GetData();
					if (num2 + data.Length + 4 > 4096)
					{
						break;
					}
					data.CopyTo(array, num2);
					num2 += (ushort)data.Length;
					num4 += (ushort)data.Length;
					num++;
				}
				array[num3++] |= (byte)((num4 >> 8) & 0xF);
				array[num3] = (byte)(num4 & 0xFF);
			}
			num2 += 4;
			int num5 = num2 - 3;
			array[1] &= 240;
			array[1] |= (byte)((num5 >> 8) & 0xF);
			array[2] = (byte)(num5 & 0xFF);
			CRCGeneratorBLL.SetSectionCRC(array);
			byte[] array4 = new byte[num2];
			Array.Copy(array, array4, num2);
			return array4;
		}

		private static int GetTableIDSegmentNumber(EitInfo EIT, out TableIDSegmentNumber ComplexParms)
		{
			DateTime date = DateTimeHelper.GetUTCDateTimeNow().Date;
			ComplexParms.TableID = 0;
			ComplexParms.SegmentNumber = 0;
			if (EIT.StartTime < date)
			{
				return 0;
			}
			TimeSpan timeSpan = EIT.StartTime - date;
			if (timeSpan.TotalDays >= 64.0)
			{
				return -1;
			}
			ComplexParms.TableID = (byte)(timeSpan.TotalHours / 96.0);
			ComplexParms.SegmentNumber = (byte)(timeSpan.TotalHours % 96.0 / 3.0);
			return 1;
		}

		private static void SaveEITSection(byte tableID, byte segmentNumber, byte[] sourceSection, Dictionary<byte, Dictionary<byte, IList<byte[]>>> destinationTable)
		{
			if (destinationTable == null)
			{
				destinationTable = new Dictionary<byte, Dictionary<byte, IList<byte[]>>>();
			}
			if (destinationTable.Keys.Contains(tableID))
			{
				Dictionary<byte, IList<byte[]>> dictionary = destinationTable[tableID];
				if (dictionary.Keys.Contains(segmentNumber))
				{
					IList<byte[]> list = dictionary[segmentNumber];
					list.Add(sourceSection);
				}
				else
				{
					dictionary.Add(segmentNumber, new List<byte[]> { sourceSection });
				}
			}
			else
			{
				Dictionary<byte, IList<byte[]>> dictionary = new Dictionary<byte, IList<byte[]>>();
				dictionary.Add(segmentNumber, new List<byte[]> { sourceSection });
				destinationTable.Add(tableID, dictionary);
			}
		}

		public static IList<byte[]> GenerateEITSCSections(IList<EitInfo> EITList, ProgramInfo CurrProgram)
		{
			if (EITList == null || EITList.Count == 0)
			{
				return null;
			}
			byte eITVersionNumber = GetEITVersionNumber(CurrProgram.Identity);
			IList<byte[]> list = new List<byte[]>();
			Dictionary<byte, Dictionary<byte, IList<byte[]>>> dictionary = new Dictionary<byte, Dictionary<byte, IList<byte[]>>>();
			ushort num = 0;
			ushort num2 = 0;
			while (num < EITList.Count)
			{
				ushort num3 = 0;
				byte[] array = new byte[4096];
				TableIDSegmentNumber ComplexParms = default(TableIDSegmentNumber);
				int tableIDSegmentNumber = GetTableIDSegmentNumber(EITList[num], out ComplexParms);
				if (tableIDSegmentNumber < 0)
				{
					break;
				}
				if (tableIDSegmentNumber == 0)
				{
					num++;
					continue;
				}
				byte tableID = (byte)(80 + ComplexParms.TableID);
				num3 += SetEITCommData(array, tableID, CurrProgram, eITVersionNumber);
				ushort num4 = 0;
				while (num < EITList.Count)
				{
					EitInfo EIT = EITList[num];
					if (EIT == null)
					{
						break;
					}
					TableIDSegmentNumber ComplexParms2 = default(TableIDSegmentNumber);
					int tableIDSegmentNumber2 = GetTableIDSegmentNumber(EIT, out ComplexParms2);
					if (tableIDSegmentNumber2 < 0)
					{
						break;
					}
					if (tableIDSegmentNumber2 == 0)
					{
						num++;
					}
					else
					{
						if (ComplexParms2.TableID != ComplexParms.TableID || ComplexParms2.SegmentNumber != ComplexParms.SegmentNumber || num3 + 12 + 4 > 4096)
						{
							break;
						}
						array[num3++] = (byte)(EIT.EventId >> 8);
						array[num3++] = (byte)(EIT.EventId & 0xFF);
						byte[] array2 = CommTool.TimeToMJDBuf(EIT.StartTime);
						array2.CopyTo(array, num3);
						num3 += 5;
						byte[] array3 = CRCGeneratorBLL.TimeSpanToBCD(EIT.EndTime - EIT.StartTime);
						array3.CopyTo(array, num3);
						num3 += 3;
						num4 = num3;
						byte b = 0;
						array[num3++] = (byte)(((b & 7) << 5) | ((CurrProgram.PrgSdtFreecamod & 1) << 4));
						array[num3++] = 0;
						ushort num5 = 0;
						IList<DescriptorSigleBase> list2 = TSOutputManager.EITDescriptorList.Where((DescriptorSigleBase p) => p.TableID == EIT.Identity).ToList();
						while (num2 < list2.Count)
						{
							DescriptorSigleBase descriptorSigleBase = list2[num2];
							byte[] data = descriptorSigleBase.GetData();
							if (num3 + data.Length + 4 > 4096)
							{
								break;
							}
							data.CopyTo(array, num3);
							num3 += (ushort)data.Length;
							num5 += (ushort)data.Length;
							num2++;
						}
						array[num4++] |= (byte)((num5 >> 8) & 0xF);
						array[num4] = (byte)(num5 & 0xFF);
						if (num2 < list2.Count)
						{
							break;
						}
					}
					num++;
					num2 = 0;
				}
				num3 += 4;
				int num6 = num3 - 3;
				array[1] &= 240;
				array[1] |= (byte)((num6 >> 8) & 0xF);
				array[2] = (byte)(num6 & 0xFF);
				byte[] array4 = new byte[num3];
				Array.Copy(array, array4, num3);
				SaveEITSection(tableID, ComplexParms.SegmentNumber, array4, dictionary);
			}
			if (dictionary.Count == 0)
			{
				return null;
			}
			for (byte b2 = 80; b2 <= dictionary.Keys.Max(); b2++)
			{
				if (dictionary.Keys.Contains(b2))
				{
					Dictionary<byte, IList<byte[]>> dictionary2 = dictionary[b2];
					for (byte b3 = (byte)((b2 - 80) * 8); b3 < dictionary2.Keys.Max(); b3++)
					{
						if (!dictionary2.Keys.Contains(b3))
						{
							byte[] array5 = new byte[18];
							ushort num7 = SetEITCommData(array5, b2, CurrProgram, eITVersionNumber);
							num7 += 4;
							int num6 = num7 - 3;
							array5[1] &= 240;
							array5[1] |= (byte)((num6 >> 8) & 0xF);
							array5[2] = (byte)(num6 & 0xFF);
							dictionary2.Add(b3, new List<byte[]> { array5 });
						}
					}
				}
				else
				{
					byte[] array5 = new byte[18];
					ushort num7 = SetEITCommData(array5, b2, CurrProgram, eITVersionNumber);
					num7 += 4;
					int num6 = num7 - 3;
					array5[1] &= 240;
					array5[1] |= (byte)((num6 >> 8) & 0xF);
					array5[2] = (byte)(num6 & 0xFF);
					Dictionary<byte, IList<byte[]>> dictionary3 = new Dictionary<byte, IList<byte[]>>();
					dictionary3.Add((byte)((b2 - 80) * 32), new List<byte[]> { array5 });
					dictionary.Add(b2, dictionary3);
				}
			}
			List<byte> list3 = dictionary.Keys.ToList();
			list3.Sort();
			byte b4 = list3.Max();
			foreach (byte item in list3)
			{
				Dictionary<byte, IList<byte[]>> dictionary2 = dictionary[item];
				List<byte> list4 = dictionary2.Keys.ToList();
				list4.Sort();
				byte b5 = dictionary2.Keys.Max();
				byte b6 = (byte)dictionary2[b5].Count;
				byte b7 = (byte)(b5 * 8 + b6 - 1);
				foreach (byte item2 in list4)
				{
					IList<byte[]> list5 = dictionary2[item2];
					for (byte b8 = 0; b8 < list5.Count; b8++)
					{
						byte b9 = (byte)(item2 * 8 + b8);
						byte b10 = (byte)(item2 * 8 + list5.Count - 1);
						byte[] array = list5[b8];
						array[6] = b9;
						array[7] = b7;
						array[12] = b10;
						array[13] = b4;
						CRCGeneratorBLL.SetSectionCRC(array);
						list.Add(array);
					}
				}
			}
			return list;
		}
	}
}
