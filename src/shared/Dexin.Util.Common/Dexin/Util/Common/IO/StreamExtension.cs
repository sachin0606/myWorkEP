using System.Collections.Generic;
using System.IO;

namespace Dexin.Util.Common.IO
{
	public static class StreamExtension
	{
		public static byte[] ReadLineAsBytes(this Stream SourceStream)
		{
			MemoryStream memoryStream = new MemoryStream();
			int num;
			do
			{
				bool flag = true;
				num = SourceStream.ReadByte();
				if (num == -1)
				{
					break;
				}
				memoryStream.WriteByte((byte)num);
			}
			while (num != 10);
			return memoryStream.ToArray();
		}

		public static byte[] ReadAllBytes(this Stream SourceStream, int receivebuffsize)
		{
			List<byte> list = new List<byte>();
			while (SourceStream.CanRead)
			{
				byte[] array = new byte[receivebuffsize];
				int num = SourceStream.Read(array, 0, receivebuffsize);
				if (num == 0)
				{
					break;
				}
				if (num != array.Length)
				{
					array = array.GetBytes(num);
				}
				list.AddRange(array);
			}
			return list.ToArray();
		}
	}
}
