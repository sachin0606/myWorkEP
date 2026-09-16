using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.Model;
using DeXin.EPG.Resource;
using DeXin.EPG.Tools;
using Dexin.Util.Common.Text;

namespace DeXin.EPG.BLL.EventFileHelper
{
	public class EITFielExpert
	{
		public IList<EitInfo> GetEITListFromEITFile(ProgramInfo program, EnumDateFormat? dateFormat, string useLanguage, EITFileImprotProcessChanged processChangedListener)
		{
			CultureInfo dateCulture = Thread.CurrentThread.CurrentCulture;
			switch (dateFormat)
			{
			case EnumDateFormat.yyyyMMdd:
				dateCulture = new CultureInfo("zh-CN");
				break;
			case EnumDateFormat.ddMMyyyy:
				dateCulture = new CultureInfo("hi-IN");
				break;
			case EnumDateFormat.MMddyyyy:
				dateCulture = new CultureInfo("en-US");
				break;
			}
			FileInfo fileInfo = new FileInfo(program.EITFileName);
			if (fileInfo.Exists)
			{
				try
				{
					if (fileInfo.Extension.ToLower().Equals(".txt"))
					{
						return TXTEITFileExpert(program, dateCulture, processChangedListener);
					}
					if (fileInfo.Extension.ToLower().Equals(".xls") || fileInfo.Extension.ToLower().Equals(".xlsx"))
					{
						return EXCELEITFileExpert(program, dateCulture, processChangedListener);
					}
					if (fileInfo.Extension.ToLower().Equals(".xml"))
					{
						return XMLEITFileExpert(program, null, useLanguage, processChangedListener);
					}
					return null;
				}
				catch (Exception ex)
				{
					if (ex is CustomException)
					{
						throw ex;
					}
					throw new CustomException(string.Format("{0} {1}", ResourceManagement.GetLocationString("EventFileFormatError"), program.EITFileName), MessageType.LOG);
				}
			}
			return null;
		}

		private IList<EitInfo> EXCELEITFileExpert(ProgramInfo program, CultureInfo dateCulture, EITFileImprotProcessChanged processChangedListener)
		{
			char c = '\t';
			IList<EitInfo> list = new List<EitInfo>();
			switch (EPGSettings.Instatce.UseRegion)
			{
			case EnumUseRegions.India:
			{
				if (processChangedListener != null)
				{
					processChangedListener(string.Format(ResourceManagement.GetLocationString("ReadingEITFile"), program.EITFileName));
				}
				List<string> list4 = ExcelReader.Read(program.EITFileName, 1)[0].ToList();
				if (list4.Count < 2)
				{
					throw new CustomException(string.Format("{0} {1}", ResourceManagement.GetLocationString("EventFileFormatError"), program.EITFileName), MessageType.LOG);
				}
				try
				{
					Convert.ToDateTime(list4[1].Split('\t')[0].Trim('\t', ' '), dateCulture);
				}
				catch
				{
					list4.RemoveAt(1);
				}
				List<DateTime> list5 = new List<DateTime>();
				string[] array4 = list4[0].Split(c);
				foreach (string value in array4)
				{
					if (!string.IsNullOrEmpty(value))
					{
						try
						{
							list5.Add(Convert.ToDateTime(value, dateCulture));
						}
						catch
						{
						}
					}
				}
				int num = -1;
				for (int j = 1; j < list4.Count; j++)
				{
					string[] array5 = list4[j].Split(c);
					string[] array6 = ((j + 1 < list4.Count) ? list4[j + 1].Split(c) : list4[1].Split(c));
					for (int k = 1; k <= list5.Count; k++)
					{
						if (num > 0 && string.IsNullOrEmpty(array5[k]))
						{
							continue;
						}
						DateTime dateTime6 = Convert.ToDateTime(array5[0]);
						DateTime dateTime7 = Convert.ToDateTime(array6[0]);
						DateTime dateTime8 = ((num > 0 && j >= num) ? list5[k - 1].AddDays(1.0) : list5[k - 1]);
						DateTime dateTime9 = new DateTime(dateTime8.Year, dateTime8.Month, dateTime8.Day, dateTime6.Hour, dateTime6.Minute, dateTime6.Second);
						DateTime dateTime10 = new DateTime(dateTime8.Year, dateTime8.Month, dateTime8.Day, dateTime7.Hour, dateTime7.Minute, dateTime7.Second);
						if (dateTime10 < dateTime9)
						{
							dateTime10 = dateTime10.AddDays(1.0);
							num = j + 1;
						}
						if (!string.IsNullOrEmpty(array5[k]))
						{
							EitInfo eitInfo3 = new EitInfo();
							eitInfo3.EventName = array5[k];
							eitInfo3.StartTime = dateTime9;
							eitInfo3.EndTime = dateTime10;
							eitInfo3.Program = program;
							eitInfo3.ProgramTableId = program.Identity;
							EitInfo eitInfo4 = eitInfo3;
							if (processChangedListener != null)
							{
								processChangedListener(string.Format(ResourceManagement.GetLocationString("ImportingEventofProgram"), program.ProgramName, eitInfo4.EventName));
							}
							list.Add(eitInfo4);
						}
					}
				}
				break;
			}
			case EnumUseRegions.Philippines:
			{
				if (processChangedListener != null)
				{
					processChangedListener(string.Format(ResourceManagement.GetLocationString("ReadingEITFile"), program.EITFileName));
				}
				List<string[]> list2 = ExcelReader.Read(program.EITFileName, null);
				foreach (string[] item in list2)
				{
					List<string> list3 = item.ToList();
					if (list3.Count < 5)
					{
						throw new CustomException(string.Format("{0} {1}", ResourceManagement.GetLocationString("EventFileFormatError"), program.EITFileName), MessageType.LOG);
					}
					list3.RemoveAt(0);
					list3.RemoveAt(0);
					list3.RemoveAt(1);
					list3.RemoveAt(1);
					string[] array = list3[0].Split('-');
					DateTime dateTime = Convert.ToDateTime(array[0], dateCulture);
					DateTime dateTime2 = Convert.ToDateTime(array[1], dateCulture);
					if (dateTime.AddDays(6.0) > dateTime2)
					{
						throw new CustomException(string.Format("{0} {1}", ResourceManagement.GetLocationString("EventFileFormatError"), program.EITFileName), MessageType.LOG);
					}
					list3.RemoveAt(0);
					for (int i = 0; i < 7; i++)
					{
						DateTime dateTime3 = dateTime.AddDays(i);
						for (int j = 0; j < list3.Count; j++)
						{
							string[] array2 = list3[j].Split('\t');
							DateTime dateTime4;
							try
							{
								dateTime4 = Convert.ToDateTime(array2[0]);
							}
							catch
							{
								break;
							}
							if (string.IsNullOrEmpty(array2[i + 1]))
							{
								continue;
							}
							DateTime dateTime5 = dateTime4;
							for (int k = j + 1; k < list3.Count; k++)
							{
								string[] array3 = list3[k].Split('\t');
								if (!string.IsNullOrEmpty(array3[i + 1]) || string.IsNullOrEmpty(array3[0]))
								{
									break;
								}
								dateTime5 = Convert.ToDateTime(array3[0]);
							}
							EitInfo eitInfo = new EitInfo();
							eitInfo.EventName = array2[i + 1];
							eitInfo.StartTime = new DateTime(dateTime3.Year, dateTime3.Month, dateTime3.Day, dateTime4.Hour, dateTime4.Minute, dateTime4.Second);
							eitInfo.EndTime = new DateTime(dateTime3.Year, dateTime3.Month, dateTime3.Day, dateTime5.Hour, dateTime5.Minute, dateTime5.Second);
							eitInfo.Program = program;
							eitInfo.ProgramTableId = program.Identity;
							EitInfo eitInfo2 = eitInfo;
							if (processChangedListener != null)
							{
								processChangedListener(string.Format(ResourceManagement.GetLocationString("ImportingEventofProgram"), program.ProgramName, eitInfo2.EventName));
							}
							list.Add(eitInfo2);
						}
					}
				}
				break;
			}
			}
			if (processChangedListener != null)
			{
				processChangedListener(string.Format(ResourceManagement.GetLocationString("SavingEventProgram"), program.ProgramName));
			}
			return list.Where((EitInfo p) => p.EndTime > DateTime.Now).ToList();
		}

		private IList<EitInfo> TXTEITFileExpert(ProgramInfo program, CultureInfo dateCulture, EITFileImprotProcessChanged processChangedListener)
		{
			IList<EitInfo> list = new List<EitInfo>();
			List<string> list2 = File.ReadAllText(program.EITFileName, Encoding.Default).Split(new string[2] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
			if (list2.Count < 2)
			{
				throw new CustomException(string.Format("{0} {1}", ResourceManagement.GetLocationString("EventFileFormatError") + " count<2", program.EITFileName), MessageType.LOG);
			}
			DateTime dateTime;
			try
			{
				dateTime = Convert.ToDateTime(list2[0].Trim('\t', ' '), dateCulture);
			}
			catch
			{
				throw new CustomException(string.Format(ResourceManagement.GetLocationString("EventFileFirstLineFormatError") + "{0}", program.EITFileName), MessageType.LOG);
			}
			int num = 0;
			int num2 = 0;
			EitInfo eitInfo = null;
			for (int i = 1; i < list2.Count; i++)
			{
				string text = list2[i].Trim('\t', ' ');
				if (!string.IsNullOrWhiteSpace(text))
				{
					try
					{
						dateTime = Convert.ToDateTime(text, dateCulture);
						num = 0;
						num2 = i;
					}
					catch
					{
						goto IL_0139;
					}
				}
				continue;
				IL_0139:
				EitInfo eitInfo2 = ConverToEITInfo(text, dateTime, program, '\t');
				if (eitInfo2 == null)
				{
					eitInfo2 = ConverToEITInfo(text, dateTime, program, ' ');
				}
				if (eitInfo2 == null)
				{
					string logFileName = CommTool.GetLogFileName(eitInfo.Program.EITFileName, false);
					DirectoryInfo directoryInfo = new DirectoryInfo(CommTool.GetFaildedReportFolder());
					if (!directoryInfo.Exists)
					{
						directoryInfo.Create();
					}
					string text2 = string.Format(ResourceManagement.GetLocationString("CannotRecognizeLine"), program.EITFileName, i + 1);
					TextHelper.WriteTXTLog(text2);
					TextHelper.WriteTXTLog(text2, directoryInfo.FullName, logFileName);
					throw new CustomException(text2, MessageType.LOG);
				}
				if (eitInfo != null && eitInfo.StartTime > eitInfo2.StartTime)
				{
					if (++num > 1 || i - 1 == num2)
					{
						string logFileName = CommTool.GetLogFileName(eitInfo.Program.EITFileName, false);
						DirectoryInfo directoryInfo = new DirectoryInfo(CommTool.GetFaildedReportFolder());
						if (!directoryInfo.Exists)
						{
							directoryInfo.Create();
						}
						string text2 = string.Format(ResourceManagement.GetLocationString("DateInvalidInTheSecondTime"), program.EITFileName, i + 1);
						TextHelper.WriteTXTLog(text2);
						TextHelper.WriteTXTLog(text2, directoryInfo.FullName, logFileName);
						throw new CustomException(text2, MessageType.LOG);
					}
					eitInfo2.StartTime = eitInfo2.StartTime.AddDays(1.0);
					eitInfo2.EndTime = eitInfo2.EndTime.AddDays(1.0);
					Tool.WriteTXTLog(string.Format(ResourceManagement.GetLocationString("TimeLessThanLastLine"), program.EITFileName, i + 1));
				}
				dateTime = eitInfo2.StartTime;
				if (eitInfo != null && eitInfo.StartTime == eitInfo.EndTime)
				{
					eitInfo.EndTime = dateTime;
				}
				list.Add(eitInfo2);
				eitInfo = eitInfo2;
				if (processChangedListener != null)
				{
					processChangedListener(string.Format(ResourceManagement.GetLocationString("ImportingEventofProgram"), program.ProgramName, eitInfo2.EventName));
				}
			}
			if (processChangedListener != null)
			{
				processChangedListener(string.Format(ResourceManagement.GetLocationString("SavingEventProgram"), program.ProgramName));
			}
			return list.Where((EitInfo p) => p.StartTime != p.EndTime && p.EndTime > DateTime.Now).ToList();
		}

		private EitInfo ConverToEITInfo(string eitFileLine, DateTime dateBase, ProgramInfo program, char separator)
		{
			EitInfo eitInfo = new EitInfo();
			eitInfo.Program = program;
			eitInfo.ProgramTableId = program.Identity;
			EitInfo eitInfo2 = eitInfo;
			List<string> list = eitFileLine.Split(separator).ToList();
			DateTime result;
			if (!DateTime.TryParse(list[0], out result))
			{
				return null;
			}
			DateTime endTime = (eitInfo2.StartTime = new DateTime(dateBase.Year, dateBase.Month, dateBase.Day, result.Hour, result.Minute, result.Second));
			int count = list.Count;
			if (count == 2)
			{
				eitInfo2.EventName = list[1];
				eitInfo2.EndTime = endTime;
				return eitInfo2;
			}
			if (count >= 3)
			{
				int result2 = -1;
				if (int.TryParse(list[1].Trim(), out result2))
				{
					eitInfo2.EndTime = endTime.AddMinutes(result2);
					if (separator.Equals(' '))
					{
						for (int i = 2; i < count; i++)
						{
							eitInfo2.EventName += list[i];
						}
					}
					else
					{
						eitInfo2.EventName = list[2];
						if (count > 3 && count >= 4)
						{
							eitInfo2.EventRemark = list[3];
							for (int i = 4; i < count; i++)
							{
								eitInfo2.EventLongRemark += list[i];
							}
						}
					}
					return eitInfo2;
				}
				eitInfo2.EndTime = endTime;
				if (separator.Equals(' '))
				{
					for (int i = 1; i < count; i++)
					{
						eitInfo2.EventName += list[i];
					}
				}
				else
				{
					eitInfo2.EventName = list[1];
					eitInfo2.EventRemark = list[2];
					if (count > 3)
					{
						for (int i = 3; i < count; i++)
						{
							eitInfo2.EventLongRemark += list[i];
						}
					}
				}
				return eitInfo2;
			}
			return null;
		}

		private IList<EitInfo> XMLEITFileExpert(ProgramInfo program, CultureInfo dateCulture, string useLanguage, EITFileImprotProcessChanged processChangedListener)
		{
			IList<EitRateSetting> rateSettings = new SystemSettingBLL().GetEitRateSetting();
			Func<XElement, string, string, string> GetElementValue = delegate(XElement element, string name, string useLan)
			{
				XElement xElement = ((!string.IsNullOrEmpty(useLan)) ? element.Elements(name).FirstOrDefault((XElement x) => x.Attribute("lang").Value == useLan) : element.Element(name));
				return (xElement == null) ? "" : xElement.Value;
			};
			Func<XElement, EitRateSetting> GetEventRate = delegate(XElement element)
			{
				EitRateSetting result = null;
				try
				{
					XElement xElement = element.Element("rating");
					if (xElement != null)
					{
						XElement xValue = xElement.Element("value");
						if (xValue != null)
						{
							result = rateSettings.SingleOrDefault((EitRateSetting x) => x.RateCode.Equals(xValue.Value));
						}
					}
				}
				catch (Exception ex)
				{
					Tool.WriteTXTLog(LogLevel.DEBUG, string.Format("try to get event rate failed:{0}", ex.Message));
				}
				return result;
			};
			XDocument xDocument = XDocument.Load(program.EITFileName);
			IEnumerable<EitInfo> source = from xEIT in xDocument.Element("tv").Elements("programme")
				select new EitInfo
				{
					EventName = GetElementValue(xEIT, "title", useLanguage),
					EventRemark = GetElementValue(xEIT, "desc", useLanguage),
					EventLongRemark = GetElementValue(xEIT, "extdesc", useLanguage),
					StartTime = DateTime.ParseExact(xEIT.Attribute("start").Value, new string[2] { "yyyyMMddHHmmss +ffff", "yyyyMMddHHmmss" }, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal),
					EndTime = DateTime.ParseExact(xEIT.Attribute("stop").Value, new string[2] { "yyyyMMddHHmmss +ffff", "yyyyMMddHHmmss" }, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal),
					EITRateSetting = GetEventRate(xEIT),
					Program = program,
					ProgramTableId = program.Identity
				};
			if (processChangedListener != null)
			{
				processChangedListener(string.Format(ResourceManagement.GetLocationString("SavingEventProgram"), program.ProgramName));
			}
			return source.Where((EitInfo p) => p.EndTime > DateTime.Now).ToList();
		}
	}
}
