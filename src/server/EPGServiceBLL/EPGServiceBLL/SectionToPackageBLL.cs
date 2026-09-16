using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using DeXin.EPG.Tools;
using EPGServiceBLL.SendOut;

namespace EPGServiceBLL
{
	public class SectionToPackageBLL
	{
		public static readonly byte[] PackageFF;

		public static readonly ushort NullPkgPID;

		private byte continuity_counter = 0;

		static SectionToPackageBLL()
		{
			PackageFF = new byte[184];
			NullPkgPID = 8191;
			for (byte b = 0; b < 184; b++)
			{
				PackageFF[b] = byte.MaxValue;
			}
			NullPkgPID = EPGSettings.Instatce.NullPkgPID;
		}

		private byte[] GeneratePackageHeader(ushort PID, bool StartPackage)
		{
			byte[] array = new byte[4] { 71, 0, 0, 0 };
			byte b = 0;
			byte b2 = (byte)((StartPackage ? 1 : 0) << 6);
			byte b3 = (byte)(((PID != 8191) ? 1 : 0) << 5);
			byte b4 = (byte)((PID & 0x1FFF) >> 8);
			array[1] = (byte)(b | b2 | b3 | b4);
			array[2] = (byte)(PID & 0xFF);
			byte b5 = 0;
			byte b6 = 16;
			byte b7 = (byte)(continuity_counter++ & 0xF);
			if (continuity_counter > 15)
			{
				continuity_counter = 0;
			}
			array[3] = (byte)(b5 | b6 | b7);
			return array;
		}

		public byte[] GenerateEmptyPackage()
		{
			byte[] first = GeneratePackageHeader(NullPkgPID, false);
			return first.Concat(PackageFF).ToArray();
		}

		public List<byte[]> GenerateEmptyPackage(decimal bufferSize, int batchSize)
		{
			if (bufferSize > 8192m)
			{
				bufferSize = 8192m;
			}
			List<byte[]> list = new List<byte[]>();
			int num = (int)Math.Ceiling(bufferSize / 188m);
			if (batchSize > num)
			{
				batchSize = num;
			}
			byte[] array = new byte[batchSize * 188];
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				byte[] array2 = GenerateEmptyPackage();
				Array.Copy(array2, 0, array, num2, 188);
				num2 += array2.Length;
				if (num2 >= array.Length)
				{
					list.Add(array);
					array = new byte[batchSize * 188];
					num2 = 0;
				}
			}
			return list;
		}

		public void ReSetPackagePID(byte[] package, ushort newPID)
		{
			byte b = (byte)((newPID & 0x1FFF) >> 8);
			package[1] = (byte)((package[1] & 0xE0) | b);
			package[2] = (byte)(newPID & 0xFF);
		}

		public IList<byte[]> SectionConvertPackage(byte[] SectionData, ushort PID)
		{
			IList<byte[]> list = new List<byte[]>();
			if (SectionData == null || SectionData.Length == 0)
			{
				return list;
			}
			ushort sectionLength = CRCGeneratorBLL.GetSectionLength(SectionData);
			sectionLength += 3;
			int num = 0;
			while (num < sectionLength)
			{
				byte[] array = new byte[188];
				if (list.Count == 0)
				{
					byte[] array2 = GeneratePackageHeader(PID, true);
					if (array2.Length != 4)
					{
						throw new Exception("Generate package header error, error length : " + array2.Length);
					}
					Array.Copy(array2, array, 4);
					array[4] = 0;
					byte b = (byte)((sectionLength > 183) ? 183 : sectionLength);
					Array.Copy(SectionData, num, array, 5, b);
					num += b;
					Array.Copy(PackageFF, 0, array, b + 5, 183 - b);
				}
				else
				{
					byte[] array2 = GeneratePackageHeader(PID, false);
					if (array2.Length != 4)
					{
						throw new Exception("Generate package header error, error length : " + array2.Length);
					}
					Array.Copy(array2, array, 4);
					byte b = (byte)((num + 184 > sectionLength) ? ((uint)(sectionLength - num)) : 184u);
					Array.Copy(SectionData, num, array, 4, b);
					num += b;
					Array.Copy(PackageFF, 0, array, b + 4, 184 - b);
				}
				list.Add(array);
			}
			return list;
		}

		public void SendOutSectionData(byte[] sectionData, ushort PID, IPEndPoint serverPiont)
		{
			lock (serverPiont)
			{
				TSOutputManager.SendTSData(SectionConvertPackage(sectionData, PID), serverPiont);
			}
		}
	}
}
