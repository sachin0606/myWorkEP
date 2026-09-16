using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Util.Common.ExtensionHelper
{
	public static class StringHelper
	{
		public static List<string> RegexMatch(this string infomation, Regex regex)
		{
			List<string> list = new List<string>();
			string text = infomation;
			while (true)
			{
				Match match = regex.Match(text);
				if (match.Groups.Count <= 0)
				{
					break;
				}
				string value = match.Groups[0].Value;
				if (value.IsNullOrEmpty())
				{
					break;
				}
				list.Add(value);
				int num = text.IndexOf(value);
				int startIndex = num + value.Length;
				text = text.Substring(startIndex);
			}
			return list;
		}
	}
}
