using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using DeXin.EPG.BLL.EventFileHelper;
using DeXin.EPG.DALFactory;
using DeXin.EPG.IDAL;
using DeXin.EPG.Model;
using DeXin.EPG.Resource;
using DeXin.EPG.Tools;
using Dexin.Util.Common.Text;

namespace DeXin.EPG.BLL
{
	public class EITInfoManager
	{
		private AbstractFactory db = AbstractFactory.CreateFactory();

		public IList<EitInfo> GetByProgramIdentity(long ProgramIdentity)
		{
			return db.CreateEITInfoDAL().GetByProgramIdentity(ProgramIdentity);
		}

		public int GetEITIdFromEITInfo(ProgramInfo Program)
		{
			return db.CreateEITInfoDAL().GetEITIdFromEITInfo(Program);
		}

		public int Save(EitInfo EIT)
		{
			return db.CreateEITInfoDAL().Save(new List<EitInfo> { EIT }, false);
		}

		public int Save(IList<EitInfo> EITList, bool AllowIgnore)
		{
			try
			{
				foreach (EitInfo EIT in EITList)
				{
					int result = 255;
					int.TryParse(ConfigurationManager.AppSettings["EITShortRemarkValueInputExceedLimited"], out result);
					if (EIT.EventRemark.Length > result)
					{
						EIT.EventRemark = EIT.EventRemark.Substring(0, result);
					}
				}
			}
			catch
			{
			}
			if (EITList == null || EITList.Count == 0)
			{
				return 0;
			}
			return db.CreateEITInfoDAL().Save(EITList, AllowIgnore);
		}

		public void Delete(IList<long> ListIdentity)
		{
			db.CreateEITInfoDAL().Delete(ListIdentity);
		}

		public void DeleteOutofdate(DateTime utcNowTime)
		{
			db.CreateEITInfoDAL().DeleteOutofdate(utcNowTime);
		}

		public IList<EitInfo> GetOutputEIT(DateTime utcNowTime, int days)
		{
			return db.CreateEITInfoDAL().GetOutputEIT(utcNowTime, days);
		}

		public IList<EitInfo> GetByProgramIdentity(long programIdentity, int pageIndex, int pageSize, out int totalSize)
		{
			return db.CreateEITInfoDAL().GetByProgramIdentity(programIdentity, pageIndex, pageSize, out totalSize);
		}

		public EitInfo GetbyID(long Identity)
		{
			return db.CreateEITInfoDAL().GetbyID(Identity);
		}

		public int SaveImportEITAndLog(List<ProgramInfo> selectPrgList, EITFileImprotProcessChanged processChangedListener, EnumDateFormat? dateFormat, ref bool isAllImportSucc, ref int affectedNums, string eitFile = "", string lan = "")
		{
			IEITInfoDAL iEITInfoDAL = db.CreateEITInfoDAL();
			foreach (ProgramInfo selectPrg in selectPrgList)
			{
				string text = "";
				string customFileName = "";
				string faildcutomFileName = "";
				string text2 = "";
				string text3 = "";
				string text4 = "";
				int num = 0;
				string text5 = "";
				string text6 = "";
				string text7 = "";
				string text8 = "";
				string arg = "";
				IList<EitInfo> list = null;
				try
				{
					if (!string.IsNullOrWhiteSpace(eitFile))
					{
						selectPrg.EITFileName = eitFile;
					}
					DateTime now = DateTime.Now;
					string text9 = now.ToString("yyyy-MM-dd");
					FileInfo fileInfo = new FileInfo(selectPrg.EITFileName);
					if (!fileInfo.Exists)
					{
						selectPrg.EITFileName = "FileNotFound";
					}
					text = selectPrg.EITFileName;
					customFileName = CommTool.GetFileName(selectPrg.EITFileName, true);
					faildcutomFileName = CommTool.GetFileName(selectPrg.EITFileName, false);
					text2 = TextHelper.GetLogFolder() + "\\" + now.ToString("yyyyMM") + "\\" + text9 + "ImportingEventReport\\";
					text3 = text2 + "Succeeded\\";
					text4 = text2 + "Failded\\";
					DirectoryInfo directoryInfo = new DirectoryInfo(text3);
					if (!directoryInfo.Exists)
					{
						directoryInfo.Create();
					}
					DirectoryInfo directoryInfo2 = new DirectoryInfo(text4);
					if (!directoryInfo2.Exists)
					{
						directoryInfo2.Create();
					}
					TextHelper.WriteTXTLog(string.Format("                                     "));
					TextHelper.WriteTXTLog(string.Format("                                     "));
					TextHelper.WriteTXTLog(string.Format("                                     "));
					TextHelper.WriteTXTLog(string.Format("                                     "));
					TextHelper.WriteTXTLog(string.Format("#*************************************************************************************************************************"));
					TextHelper.WriteTXTLog(string.Format("#========================================================================================================================="));
					TextHelper.WriteTXTLog(string.Format("#"));
					text5 = string.Format("ts id:               [{0}]", selectPrg.TS.TsId.ToString("X4"));
					text6 = string.Format("service id:          [{0}]", selectPrg.ServiceId.ToString("X4"));
					text7 = string.Format("service name:        [{0}]", selectPrg.ProgramName);
					text8 = string.Format("event file :         [{0}]", text);
					arg = string.Format("{0} {1} {2} {3}", text5, text6, text7, text8);
					TextHelper.WriteTXTLog(string.Format("#      {0}", text5));
					TextHelper.WriteTXTLog(string.Format("#      {0}", text6));
					TextHelper.WriteTXTLog(string.Format("#      {0}", text7));
					TextHelper.WriteTXTLog(string.Format("#      {0}", text8));
					TextHelper.WriteTXTLog(string.Format("#      Start to import  at {0}", DateTime.Now));
					TextHelper.WriteTXTLog(string.Format("#"));
					TextHelper.WriteTXTLog(string.Format("#========================================================================================================================="));
					list = new List<EitInfo>();
					list = new EITFielExpert().GetEITListFromEITFile(selectPrg, dateFormat, lan, processChangedListener);
					if (list.Count > 0)
					{
						DateTime utcNowTime = list.Min((EitInfo p) => p.StartTime).ToUniversalTime();
						iEITInfoDAL.DeleteByProgramTableId(selectPrg.Identity, utcNowTime);
						num = Save(list, true);
						affectedNums += num;
						if (processChangedListener != null)
						{
							processChangedListener(string.Format(ResourceManagement.GetLocationString("ImportFinished") + " " + ResourceManagement.GetLocationString("Program") + "{0}", selectPrg.ProgramName));
						}
					}
				}
				catch (Exception ex)
				{
					isAllImportSucc = false;
					if (processChangedListener != null)
					{
						processChangedListener(string.Format(ResourceManagement.GetLocationString("ImportError") + " " + ResourceManagement.GetLocationString("Program") + "{0}" + ex.Message, selectPrg.ProgramName));
					}
					WriteFailedTXT(text, text4, faildcutomFileName);
					TextHelper.WriteTXTLog(string.Format("#Failed:{0}-------------------------------------------------------------", arg));
					TextHelper.WriteTXTLog("#" + ex.Message);
				}
				try
				{
					TextHelper.WriteTXTLog(string.Format("#=============================================================================="));
					TextHelper.WriteTXTLog(string.Format("#"));
					TextHelper.WriteTXTLog(string.Format("#                                  Report"));
					TextHelper.WriteTXTLog(string.Format("#      {0}", text5));
					TextHelper.WriteTXTLog(string.Format("#      {0}", text6));
					TextHelper.WriteTXTLog(string.Format("#      {0}", text7));
					TextHelper.WriteTXTLog(string.Format("#      {0}", text8));
					TextHelper.WriteTXTLog(string.Format("#    Finished importing at {0}", DateTime.Now));
					int count = list.Count;
					TextHelper.WriteTXTLog(string.Format("#"));
					TextHelper.WriteTXTLog(string.Format("#    Event Total: {0} Succeeded: {1} Failded: {2}", count, num, count - num));
					TextHelper.WriteTXTLog(string.Format("#"));
					TextHelper.WriteTXTLog(string.Format("#=============================================================================="));
					if (count == num && count > 0)
					{
						TextHelper.WriteTXTLog(string.Format("File has been uploaded successfully"), text3, customFileName);
					}
					else
					{
						WriteFailedTXT(text, text4, faildcutomFileName);
					}
				}
				catch (Exception ex)
				{
					TextHelper.WriteTXTLog(string.Format("#Failed:{0}-------------------------------------------------------------", arg));
					TextHelper.WriteTXTLog("#" + ex.Message);
				}
			}
			return affectedNums;
		}

		public void ClearEITInfos()
		{
			db.CreateEITInfoDAL().DeleteAll();
		}

		private void WriteFailedTXT(string eventFilePath, string faildDir, string faildcutomFileName)
		{
			string text = faildDir + faildcutomFileName;
			FileInfo fileInfo = new FileInfo(text);
			if (fileInfo.Exists)
			{
				File.Copy(eventFilePath, faildDir + faildcutomFileName.Remove(faildcutomFileName.LastIndexOf('.')) + "_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff") + fileInfo.Extension, true);
			}
			else
			{
				File.Copy(eventFilePath, text, true);
			}
		}
	}
}
