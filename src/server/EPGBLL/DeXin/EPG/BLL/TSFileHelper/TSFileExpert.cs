using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DeXin.EPG.BLL.TSFileHelper
{
	public class TSFileExpert
	{
		protected const byte SYNS_BYTE = 71;

		protected const int PACKAGE_SIZE = 188;

		protected Dictionary<int, int> ContinuityCounterTable = new Dictionary<int, int>();

		public Dictionary<int, byte[]> ConvertToSections(string filename, int PID, int limitSectionCount = 0, byte tableId = 0)
		{
			Dictionary<int, byte[]> dictionary = new Dictionary<int, byte[]>();
			List<byte> list = new List<byte>();
			using (FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read))
			{
				fileStream.Position = 0L;
				int num = 0;
				do
				{
					byte[] array = new byte[188];
					num = fileStream.Read(array, 0, 188);
					switch (num)
					{
					case 0:
						goto end_IL_0022;
					case 188:
					{
						if (array.Length != 188 || array[0] != 71)
						{
							break;
						}
						int num2 = ((array[1] & 0x1F) << 8) | array[2];
						if (num2 != PID)
						{
							break;
						}
						int num3 = array[3] & 0xF;
						if ((array[1] & 0x40) == 64)
						{
							ContinuityCounterTable[PID] = (num3 + 1) & 0xF;
							byte b = array[5];
							if (tableId != 0 && tableId != b)
							{
								break;
							}
							list.Clear();
							byte[] array2 = new byte[183];
							Array.Copy(array, 5, array2, 0, 183);
							list.AddRange(array2);
						}
						else
						{
							if (list.Count <= 0)
							{
								break;
							}
							if (ContinuityCounterTable.Keys.Contains(PID) && num3 != ContinuityCounterTable[PID])
							{
								list.Clear();
								break;
							}
							ContinuityCounterTable[PID] = (num3 + 1) & 0xF;
							byte[] array2 = new byte[184];
							Array.Copy(array, 4, array2, 0, 184);
							list.AddRange(array2);
						}
						int num4 = list[6];
						int num5 = list[7];
						if (dictionary.Keys.Contains(num4))
						{
							list.Clear();
							break;
						}
						int num6 = ((list[1] & 0xF) << 8) | list[2];
						num6 += 3;
						if (list.Count >= num6)
						{
							byte[] array2 = new byte[num6];
							Array.Copy(list.ToArray(), array2, num6);
							dictionary[num4] = array2;
							list.Clear();
						}
						if (num5 + 1 != dictionary.Count && (limitSectionCount <= 0 || dictionary.Count < limitSectionCount))
						{
							break;
						}
						goto end_IL_0022;
					}
					}
					continue;
					end_IL_0022:
					break;
				}
				while (num > 0);
			}
			return dictionary;
		}

		public Dictionary<int, Dictionary<int, byte[]>> ParseToBATSections(string filename)
		{
			Dictionary<int, Dictionary<int, byte[]>> dictionary = new Dictionary<int, Dictionary<int, byte[]>>();
			Dictionary<int, byte[]> dictionary2 = new Dictionary<int, byte[]>();
			List<byte> list = new List<byte>();
			using (FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read))
			{
				fileStream.Position = 0L;
				int num = 0;
				do
				{
					byte[] array = new byte[188];
					num = fileStream.Read(array, 0, 188);
					switch (num)
					{
					case 0:
						goto end_IL_0029;
					case 188:
					{
						if (array.Length != 188 || array[0] != 71)
						{
							break;
						}
						int num2 = ((array[1] & 0x1F) << 8) | array[2];
						int num3 = 17;
						int num4 = 74;
						if (num2 != num3)
						{
							break;
						}
						int num5 = array[3] & 0xF;
						if ((array[1] & 0x40) == 64)
						{
							ContinuityCounterTable[num3] = (num5 + 1) & 0xF;
							byte b = array[5];
							if (num4 != 0 && num4 != b)
							{
								break;
							}
							list.Clear();
							byte[] array2 = new byte[183];
							Array.Copy(array, 5, array2, 0, 183);
							int value = (array2[3] << 8) | array2[4];
							if (dictionary.Keys.Contains(value))
							{
								list.Clear();
								break;
							}
							list.AddRange(array2);
						}
						else
						{
							if (list.Count <= 0)
							{
								break;
							}
							if (ContinuityCounterTable.Keys.Contains(num3) && num5 != ContinuityCounterTable[num3])
							{
								list.Clear();
								break;
							}
							ContinuityCounterTable[num3] = (num5 + 1) & 0xF;
							byte[] array2 = new byte[184];
							Array.Copy(array, 4, array2, 0, 184);
							list.AddRange(array2);
						}
						int num6 = list[6];
						int num7 = list[7];
						if (dictionary2.Keys.Contains(num6))
						{
							list.Clear();
							break;
						}
						int num8 = ((list[1] & 0xF) << 8) | list[2];
						num8 += 3;
						if (list.Count >= num8)
						{
							byte[] array2 = new byte[num8];
							Array.Copy(list.ToArray(), array2, num8);
							dictionary2[num6] = array2;
							list.Clear();
						}
						if (num7 + 1 == dictionary2.Count)
						{
							byte[] value2 = dictionary2.First().Value;
							int value = (value2[3] << 8) | value2[4];
							if (!dictionary.Keys.Contains(value))
							{
								dictionary[value] = dictionary2;
							}
							dictionary2 = new Dictionary<int, byte[]>();
						}
						break;
					}
					}
					continue;
					end_IL_0029:
					break;
				}
				while (num > 0);
			}
			return dictionary;
		}
	}
}
