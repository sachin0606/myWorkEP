using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;

namespace DeXin.EPG.Resource
{
	public class ResourceManagement
	{
		public static string GetLocationString(string strKey)
		{
			string text = "";
			try
			{
				ResourceManager resourceManager = new ResourceManager("DeXin.EPG.Resource.ResourcesSpecialFiles.ResourceSpecial", Assembly.GetExecutingAssembly());
				CultureInfo currentUICulture = Thread.CurrentThread.CurrentUICulture;
				text = resourceManager.GetString(strKey, currentUICulture);
				text = ((text == null) ? "" : text.Replace("\\n", "\n"));
			}
			catch
			{
				return strKey;
			}
			if (string.IsNullOrEmpty(text))
			{
				text = strKey;
			}
			return text;
		}
	}
}
