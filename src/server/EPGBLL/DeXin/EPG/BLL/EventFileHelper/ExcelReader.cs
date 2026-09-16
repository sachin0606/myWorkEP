using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Office.Interop.Excel;

namespace DeXin.EPG.BLL.EventFileHelper
{
	public class ExcelReader
	{
		public static List<string[]> Read(string filePath, int? sheetIndex)
		{
			Application application = null;
			Workbooks workbooks = null;
			Workbook workbook = null;
			Worksheet worksheet = null;
			Range range = null;
			object value = Missing.Value;
			List<string[]> list = new List<string[]>();
			try
			{
				application = (Application)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("00024500-0000-0000-C000-000000000046")));
				application.UserControl = true;
				application.DisplayAlerts = false;
				application.Application.Workbooks.Open(filePath, value, value, value, value, value, value, value, value, value, value, value, value, Missing.Value, Missing.Value);
				workbooks = application.Workbooks;
				workbook = workbooks[1];
				for (int i = 1; i <= workbook.Worksheets.Count; i++)
				{
					worksheet = (Worksheet)(dynamic)workbook.Worksheets[sheetIndex ?? new int?(i)];
					StringBuilder stringBuilder = new StringBuilder();
					for (int j = 1; j <= worksheet.UsedRange.Rows.Count; j++)
					{
						for (int k = 1; k <= worksheet.UsedRange.Columns.Count; k++)
						{
							range = (dynamic)worksheet.UsedRange[j, k];
							object text = range.Text;
							if (text != null)
							{
								string value2 = text.ToString().Replace("\n", "").Replace("\r", "")
									.Replace("\t", "");
								stringBuilder.Append(value2);
								stringBuilder.Append("\t");
							}
						}
						if (j < worksheet.UsedRange.Rows.Count)
						{
							stringBuilder.Append("\n");
						}
					}
					list.Add((from p in stringBuilder.ToString().Split('\n')
						where !string.IsNullOrEmpty(p)
						select p).ToArray());
					if (sheetIndex.HasValue)
					{
						break;
					}
				}
				return list;
			}
			finally
			{
				if (application != null)
				{
					if (workbooks != null)
					{
						if (workbook != null)
						{
							if (worksheet != null)
							{
								if (range != null)
								{
									try
									{
										Marshal.ReleaseComObject(range);
										range = null;
									}
									catch
									{
									}
								}
								try
								{
									Marshal.ReleaseComObject(worksheet);
									worksheet = null;
								}
								catch
								{
								}
							}
							try
							{
								workbook.Close(false, value, value);
								Marshal.ReleaseComObject(workbook);
								workbook = null;
							}
							catch
							{
							}
						}
						try
						{
							workbooks.Close();
							Marshal.ReleaseComObject(workbooks);
							workbooks = null;
						}
						catch
						{
						}
					}
					try
					{
						application.Application.Workbooks.Close();
						application.Quit();
						Marshal.ReleaseComObject(application);
						application = null;
					}
					catch
					{
					}
					GC.Collect();
				}
			}
		}
	}
}
