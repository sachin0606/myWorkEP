using System.Collections.Generic;
using Com.Cn.Tool;
using DeXin.EPG.Tools;

namespace DeXin.EPG.Model
{
	public static class DescriptorHelpers
	{
		public static byte[] KVtoBuff(this KeyValuePair<string, string> item)
		{
			byte[] array = CommTool.DescriptorStringToBytes(item.Key);
			byte[] array2 = CommTool.DescriptorStringToBytes(item.Value);
			byte[] array3 = new byte[2 + array.Length + array2.Length];
			int startIndex = 0;
			Tool.ToByteArray((byte)array.Length, array3, ref startIndex);
			Tool.ToByteArray(array, array3, ref startIndex);
			Tool.ToByteArray((byte)array2.Length, array3, ref startIndex);
			Tool.ToByteArray(array2, array3, ref startIndex);
			return array3;
		}
	}
}
