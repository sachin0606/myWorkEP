using System.IO;

namespace Dexin.Util.Common.Net
{
	public class HttpFile
	{
		public string Name { get; set; }

		public string FullFileName { get; set; }

		public string FileName
		{
			get
			{
				return new FileInfo(FullFileName).Name;
			}
		}

		public byte[] FileContent { get; set; }

		internal HttpFile(string name, string fileFullName, byte[] fileContent)
		{
			Name = name;
			FullFileName = fileFullName;
			FileContent = fileContent;
		}
	}
}
