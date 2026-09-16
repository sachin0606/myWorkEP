using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Dexin.Util.Common.Text
{
	public class TextHelper
	{
		public static void SaveTableToTXT(string path, DataTable dt, bool needTableHeader = true)
		{
			FileInfo fileInfo = new FileInfo(path);
			DirectoryInfo directory = fileInfo.Directory;
			if (fileInfo.Exists)
			{
				fileInfo.Delete();
			}
			IList<string> list = new List<string>();
			int count = dt.Columns.Count;
			if (needTableHeader)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (object column in dt.Columns)
				{
					stringBuilder.Append(column.ToString() + ",");
				}
				list.Add(stringBuilder.ToString().TrimEnd(','));
			}
			foreach (DataRow row in dt.Rows)
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < count; i++)
				{
					stringBuilder.Append(row[i].ToString() + ",");
				}
				list.Add(stringBuilder.ToString().TrimEnd(','));
			}
			using (StreamWriter streamWriter = fileInfo.CreateText())
			{
				foreach (string item in list)
				{
					streamWriter.WriteLine(item);
				}
			}
		}

		public static bool IsValidUrl(string url)
		{
			string pattern = "^((http|rtp)?://)(([0-9]{1,3}.){3}[0-9]{1,3}:[0-9]{1,4})$";
			return Regex.IsMatch(url, pattern);
		}

		public static string GetLogFolder()
		{
			DateTime now = DateTime.Now;
			string text = AppDomain.CurrentDomain.BaseDirectory + "TXTLog";
			string text2 = "";
			try
			{
				text2 = System.Configuration.ConfigurationManager.AppSettings["LogPath"].TrimEnd('\\');
			}
			catch
			{
				text2 = text;
			}
			if (string.IsNullOrEmpty(text2))
			{
				text2 = text;
			}
			return text2;
		}

		public static void WriteTXTLog(string msg, string folderName = "", string customFileName = "")
		{
			DateTime now = DateTime.Now;
			string logFolder = GetLogFolder();
			string text = (string.IsNullOrEmpty(folderName) ? (logFolder + "\\" + now.ToString("yyyyMM") + "\\") : string.Format(folderName.TrimEnd('\\') + "\\"));
			string text2 = ((!string.IsNullOrEmpty(customFileName)) ? customFileName : now.ToString("yyyy-MM-dd")) + ".txt";
			DirectoryInfo directoryInfo = new DirectoryInfo(text);
			if (!directoryInfo.Exists)
			{
				try
				{
					directoryInfo.Create();
				}
				catch
				{
					string path = string.Concat(new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Root, text.Remove(0, directoryInfo.Root.Name.Length));
					DirectoryInfo directoryInfo2 = new DirectoryInfo(path);
					text = directoryInfo2.FullName;
					directoryInfo2.Create();
				}
			}
			using (StreamWriter streamWriter = new StreamWriter(text + text2, true, Encoding.UTF8))
			{
				streamWriter.WriteLine("[" + now.ToString("yyyy-MM-dd HH:mm:ss") + "." + now.Millisecond + "] " + msg);
				streamWriter.AutoFlush = true;
			}
		}
	}
}
