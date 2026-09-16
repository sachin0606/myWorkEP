using System;
using System.Collections.Generic;
using DeXin.EPG.Tools;

namespace EPGServiceBLL
{
	public class TDTGeneratorBLL
	{
		public static IList<byte[]> GetSectionData(DateTime dt)
		{
			IList<byte[]> list = new List<byte[]>();
			byte[] array = new byte[8] { 112, 112, 5, 0, 0, 0, 0, 0 };
			byte[] array2 = CommTool.TimeToMJDBuf(dt);
			array2.CopyTo(array, 3);
			list.Add(array);
			return list;
		}
	}
}
