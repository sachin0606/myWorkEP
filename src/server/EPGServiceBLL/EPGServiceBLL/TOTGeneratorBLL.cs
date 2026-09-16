using System;
using System.Collections.Generic;
using DeXin.EPG.BLL;
using DeXin.EPG.Tools;
using EPGServiceBLL.SendOut;

namespace EPGServiceBLL
{
	public class TOTGeneratorBLL
	{
		public static IList<byte[]> GetSectionData(DateTime dt)
		{
			IList<byte[]> list = new List<byte[]>();
			byte[] array = new byte[1024];
			int num = 0;
			int num2 = 0;
			int i = 0;
			int num3 = 1;
			int num4 = 0;
			while (true)
			{
				bool flag = true;
				array = new byte[1024];
				if (num >= 1)
				{
					break;
				}
				num++;
				array[0] = 115;
				array[1] = 112;
				array[2] = 0;
				byte[] array2 = CommTool.TimeToMJDBuf(dt);
				array2.CopyTo(array, 3);
				array[8] = 240;
				array[9] = 0;
				num4 = 10;
				for (; i < num3; i++)
				{
					byte[] array3 = new DescriptorManagement().ToTChageDate(TSOutputManager.TOTDescriptor.GetData(), DateTimeHelper.GetUTCDateTimeNow());
					int num5 = array3.Length;
					if (num4 + num5 + 4 > 1024)
					{
						break;
					}
					for (int j = 0; j < num5; j++)
					{
						array[num4 + j] = array3[j];
					}
					num4 += num5;
				}
				int num6 = num4 - 10;
				array[8] &= 240;
				array[8] |= (byte)(num6 >> 8);
				array[9] = (byte)num6;
				int num7 = num4 + 4 - 3;
				array[1] &= 240;
				array[1] |= (byte)((num7 >> 8) & 0xF);
				array[2] = (byte)(num7 & 0xFF);
				CRCGeneratorBLL.SetSectionCRC(array);
				list.Add(array);
			}
			return list;
		}
	}
}
