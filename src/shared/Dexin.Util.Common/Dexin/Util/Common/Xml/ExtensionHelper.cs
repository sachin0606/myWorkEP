using System.IO;
using System.Text;
using System.Xml.Linq;

namespace Dexin.Util.Common.Xml
{
	public static class ExtensionHelper
	{
		public static void SaveXmlWithUTF8NotBOM(this XDocument xml, string savePath)
		{
			using (StreamWriter streamWriter = new StreamWriter(savePath, false, new UTF8Encoding(false)))
			{
				xml.Save((TextWriter)streamWriter);
				streamWriter.WriteLine();
				streamWriter.Close();
			}
		}
	}
}
