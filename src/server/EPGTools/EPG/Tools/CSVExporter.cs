using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace EPG.Tools
{
	public class CSVExporter
	{
		public static void Export(string fileName, IList<string> fileLines)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.DefaultExt = "csv";
			saveFileDialog.Filter = "Excel Worksheets|*.csv";
			saveFileDialog.FileName = fileName;
			if (saveFileDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			using (StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName, false, Encoding.UTF8))
			{
				foreach (string fileLine in fileLines)
				{
					streamWriter.WriteLine(fileLine);
				}
			}
		}
	}
}
