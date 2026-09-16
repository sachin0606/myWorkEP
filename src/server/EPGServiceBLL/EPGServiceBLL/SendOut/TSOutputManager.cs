using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Model;
using DeXin.EPG.Resource;
using DeXin.EPG.Tools;
using EPGServiceBLL.Configuration;
using EPGServiceBLL.EPGException;
using EPGServiceBLL.SendOut.NetSocket;
using EPGServiceNotify;

namespace EPGServiceBLL.SendOut
{
	public static class TSOutputManager
	{
		private const int MMSecondOfSuspend = 30000;

		private const int MMSecondOfEmptyThread = 5000;

		private const int MMSecondOfOutofSpeed = 10;

		private const int MMSecondOfCheckSetting = 30000;

		private const int MMSecondOfRetryOpenTSPlayer = 5000;

		private const int MMSecondOfUpdateEIT_PF = 60000;

		private const int MMSecondOfCheckEachDayReload = 3600000;

		private const int MMSecondOfNotifyIcon = 5000;

		private static Dictionary<IPEndPoint, DateTime> LastSpeedTagTable;

		private static Dictionary<IPEndPoint, long> TotalSendBufferSizeTable;

		public static Dictionary<IPEndPoint, decimal> CurrentSpeedTable;

		private static AutoShowtimerConfiguration config;

		private static EPGWindowsFolderWatcher watcher;

		private static Dictionary<long, IList<byte[]>> PATSectionListTable;

		private static Dictionary<long, IList<byte[]>> CATSectionListTable;

		private static Dictionary<long, Dictionary<long, IList<byte[]>>> PMTSectionListTable;

		private static Dictionary<long, IList<byte[]>> NIT_ACTSectionListTable;

		private static Dictionary<long, IList<byte[]>> NIT_OTHSectionListTable;

		private static Dictionary<long, IList<byte[]>> BATSectionListTable;

		private static Dictionary<long, IList<byte[]>> SDT_ACTSectionListTable;

		private static Dictionary<long, IList<byte[]>> SDT_OTHSectionListTable;

		private static Dictionary<long, Dictionary<long, IList<byte[]>>> EIT_ACT_PFSectionListTable;

		private static Dictionary<long, Dictionary<long, IList<byte[]>>> EIT_ACT_SCSectionListTable;

		private static IList<byte[]> TDTSectionList;

		private static IList<byte[]> TOTSectionList;

		private static Dictionary<long, IPEndPoint> OutputIPSettingTable;

		private static IList<BATInfo> BATOutputList;

		private static IList<TsInfo> BAT_TSOutputList;

		private static IList<TsInfo> NIT_TSOutputList;

		private static IList<TsInfo> TSOutputList;

		private static IList<ProgramInfo> ProgramOutputList;

		private static IList<Multimedia> MultimediaOutputList;

		private static IList<EitInfo> EITOutputList;

		private static IList<MultipleipSettings> OutputIPSettingList;

		private static SimpleipSettings SingleIPSetting;

		private static IList<PidOutputInfo> PIDRemappingSettingList;

		private static IList<PmtPidOutputInfo> PMTPIDRemappingSettingList;

		private static IList<SendSpeedInfo> SendSpeedSettingList;

		private static IList<SendOrNotInfo> SendOrNotSettingList;

		private static DescriptorManagement desManager;

		public static IList<DescriptorSigleBase> BAT_TSDescriptorList;

		public static IList<DescriptorSigleBase> NIT_TSDescriptorList;

		public static IList<DescriptorSigleBase> EITDescriptorList;

		public static IList<DescriptorSigleBase> BATDescriptorList;

		public static IList<DescriptorSigleBase> CATDescriptorList;

		public static IList<DescriptorSigleBase> NITDescriptorList;

		public static IList<DescriptorSigleBase> PMTDescriptorList;

		public static IList<DescriptorSigleBase> MultimediaDescriptorList;

		public static IList<DescriptorSigleBase> SDTDescriptorList;

		public static DescriptorSigleBase TOTDescriptor;

		private static readonly SendSpeedInfo DefaultSendSpeed;

		private static readonly SendOrNotInfo DefaultSendOrNotSetting;

		private static List<Thread> TaskThreadPool;

		private static List<Delegate> EventPool;

		private static object lockFileObject;

		private static object lockRePreparTSPlayer;

		private static bool suspendSendTableThread;

		public static bool ServiceRunningState { get; private set; }

		private static event TableReloadHandller TOTDescriptorReloading;

		private static event TableReloadHandller SDTReloading;

		private static event TableReloadHandller PMTReloading;

		private static event TableReloadHandller PATReloading;

		public static event ServiceExitEventHandeller ServiceExiting;

		private static event TableReloadHandller EITReloading;

		private static event TableReloadWithKeyHandller EITPFReloading;

		private static event TableReloadHandller NITReloading;

		private static event TableReloadHandller BATReloading;

		private static event TableReloadHandller CATReloading;

		public static event ProcessChangedHandller ServerStopProcessChanged;

		private static void HandellEPGServiceException(Exception ex)
		{
			if (ex is ThreadAbortException)
			{
				throw ex;
			}
			try
			{
				Tool.WriteTXTLog(ex);
				BalloonInfo balloonInfo = new BalloonInfo();
				balloonInfo.BalloonType = ToolTipIcon.Error;
				balloonInfo.Content = ex.Message;
				balloonInfo.Timeout = 5000;
				balloonInfo.Title = "EPG Service";
				NotifyFormManage.StartShowMessageUI(balloonInfo);
				if (ex is EPGRegistorException)
				{
					throw ex;
				}
				if (ex is HTBDogErrorException)
				{
					throw ex;
				}
				if (ex is NullOrErrorAccreditException)
				{
					throw ex;
				}
				if (ex is SettingErrorException)
				{
					if (!suspendSendTableThread)
					{
						Tool.WriteTXTLog(string.Format("Warning : {0}", ResourceManagement.GetLocationString("TaskSuspend")));
						suspendSendTableThread = true;
					}
					BalloonInfo balloonInfo2 = new BalloonInfo();
					balloonInfo2.BalloonType = ToolTipIcon.Error;
					balloonInfo2.Content = string.Format("{0}:{1}", ResourceManagement.GetLocationString("TaskSuspend"), ex.Message);
					balloonInfo2.Timeout = 10000;
					balloonInfo2.Title = "EPG Service";
					NotifyFormManage.StartShowMessageUI(balloonInfo2);
				}
				else
				{
					if (ex is OutOfMemoryException)
					{
						throw ex;
					}
					Thread.Sleep(30000);
				}
			}
			catch
			{
				suspendSendTableThread = true;
				if (TSOutputManager.ServiceExiting != null)
				{
					TSOutputManager.ServiceExiting();
				}
			}
		}

		private static void CalculateSpeed(object point)
		{
			IPEndPoint iPEndPoint = point as IPEndPoint;
			if (iPEndPoint != null)
			{
				int millisecondsTimeout = new Random(DateTime.Now.Millisecond).Next(100, CurrentSpeedTable.Count * 1000 / 2);
				Thread.Sleep(millisecondsTimeout);
			}
			int num = 5000;
			int num2 = 1;
			bool isAddEmptyPackage = EPGSettings.Instatce.IsAddEmptyPackage;
			decimal num3 = EPGSettings.Instatce.TargetRate * 1024m / 1000m;
			while (ServiceRunningState)
			{
				if (iPEndPoint != null)
				{
					TimeSpan timeSpan = DateTime.Now - LastSpeedTagTable[iPEndPoint];
					if (timeSpan.TotalMilliseconds == 0.0)
					{
						Thread.Sleep(1);
						continue;
					}
					decimal num4 = (decimal)((double)TotalSendBufferSizeTable[iPEndPoint] / timeSpan.TotalMilliseconds * 8.0);
					if (num4 == 0m)
					{
						Thread.Sleep(1);
						continue;
					}
					CurrentSpeedTable[iPEndPoint] = num4;
					decimal num5 = (num3 - num4) * (decimal)num2 / 8m;
					if (timeSpan.TotalMilliseconds > (double)num || timeSpan.TotalMilliseconds < 0.0)
					{
						LastSpeedTagTable[iPEndPoint] = DateTime.Now;
						TotalSendBufferSizeTable[iPEndPoint] = 0L;
					}
					if (isAddEmptyPackage && num5 > 0m)
					{
						List<byte[]> sendBufferList = new SectionToPackageBLL().GenerateEmptyPackage(num5, 1);
						SendTSData(sendBufferList, iPEndPoint);
					}
					if (num5 <= 0m || !isAddEmptyPackage)
					{
						Thread.Sleep(num2);
					}
					continue;
				}
				try
				{
					IEnumerable<IPAddress> source = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where((IPAddress x) => x.AddressFamily == AddressFamily.InterNetwork);
					UdpClientManager.Send(BitConverter.GetBytes((double)(CurrentSpeedTable.Values.ToList().Sum() / 1024m / 1024m * 1000m)), new IPEndPoint(source.First(), 9098));
					Thread.Sleep(500);
				}
				catch
				{
				}
			}
		}

		private static void SendTOT()
		{
			Dictionary<long, byte> idxTOTTable = new Dictionary<long, byte>();
			Dictionary<long, DateTime> dictionary = new Dictionary<long, DateTime>();
			bool flag = false;
			TOTDescriptorReloading += delegate
			{
				lock (idxTOTTable)
				{
					idxTOTTable.Clear();
				}
			};
			PATReloading += delegate
			{
				lock (idxTOTTable)
				{
					idxTOTTable.Clear();
				}
			};
			Thread thread = new Thread(ClearupSendtimeDictionary);
			thread.Name = "Clearup TOT sendtime";
			thread.Priority = ThreadPriority.Lowest;
			thread.IsBackground = true;
			thread.CurrentCulture = Thread.CurrentThread.CurrentCulture;
			thread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
			Thread thread2 = thread;
			TaskThreadPool.Add(thread2);
			thread2.Start(new List<Dictionary<long, DateTime>> { dictionary });
			DateTime now = DateTime.Now;
			while (ServiceRunningState)
			{
				if (suspendSendTableThread)
				{
					Thread.Sleep(30000);
					continue;
				}
				if (TSOutputList.Count < 1)
				{
					Thread.Sleep(5000);
					continue;
				}
				try
				{
					if (flag)
					{
						Thread.Sleep(100);
					}
					else
					{
						GenerateSections(EnumTableType.TOT);
					}
					flag = true;
					for (int num = 0; num < TSOutputList.Count; num++)
					{
						long tsKey = TSOutputList[num].Identity;
						SendSpeedInfo sendSpeedInfo = SendSpeedSettingList.SingleOrDefault((SendSpeedInfo p) => p.TsTableId == tsKey);
						if (sendSpeedInfo == null)
						{
							sendSpeedInfo = DefaultSendSpeed;
						}
						SendOrNotInfo sendOrNotInfo = SendOrNotSettingList.SingleOrDefault((SendOrNotInfo p) => p.TsTableId == tsKey);
						if (sendOrNotInfo == null)
						{
							sendOrNotInfo = DefaultSendOrNotSetting;
						}
						if (sendOrNotInfo.Tot != 1)
						{
							continue;
						}
						DateTime dateTime;
						lock (dictionary)
						{
							dateTime = (dictionary.Keys.Contains(tsKey) ? dictionary[tsKey] : DateTimeHelper.GetUTCDateTimeNow().Date);
						}
						DateTime uTCDateTimeNow = DateTimeHelper.GetUTCDateTimeNow();
						if ((uTCDateTimeNow - dateTime).TotalMilliseconds < (double)(sendSpeedInfo.Tot / TOTSectionList.Count))
						{
							continue;
						}
						flag = false;
						SetRecordValue(dictionary, tsKey, uTCDateTimeNow);
						lock (TOTSectionList)
						{
							byte b;
							lock (idxTOTTable)
							{
								b = (byte)(idxTOTTable.Keys.Contains(tsKey) ? idxTOTTable[tsKey] : 0);
							}
							SetRecordValueIndex(idxTOTTable, tsKey, TOTSectionList.Count, b);
							ushort tablePID = GetTablePID(EnumTableType.TOT, tsKey);
							PackageGenerators.Instance[tsKey, tablePID].SendOutSectionData(TOTSectionList[b], tablePID, GetSendIPEndPoint(tsKey));
						}
					}
				}
				catch (Exception ex)
				{
					HandellEPGServiceException(ex);
				}
			}
		}

		private static void SendNIT()
		{
			Dictionary<long, byte> idxNIT_ACTTable = new Dictionary<long, byte>();
			Dictionary<long, byte> idxNIT_OTHTable = new Dictionary<long, byte>();
			Dictionary<long, DateTime> dictionary = new Dictionary<long, DateTime>();
			Dictionary<long, DateTime> dictionary2 = new Dictionary<long, DateTime>();
			bool flag = false;
			NITReloading += delegate
			{
				bool lockTaken7 = false;
				Dictionary<long, byte> obj4 = default(Dictionary<long, byte>);
				try
				{
					Monitor.Enter(obj4 = idxNIT_ACTTable, ref lockTaken7);
					idxNIT_ACTTable.Clear();
				}
				finally
				{
					if (lockTaken7)
					{
						Monitor.Exit(obj4);
					}
				}
				bool lockTaken8 = false;
				try
				{
					Monitor.Enter(obj4 = idxNIT_OTHTable, ref lockTaken8);
					idxNIT_OTHTable.Clear();
				}
				finally
				{
					if (lockTaken8)
					{
						Monitor.Exit(obj4);
					}
				}
			};
			Thread thread = new Thread(ClearupSendtimeDictionary);
			thread.Name = "Clearup NIT sendtime";
			thread.Priority = ThreadPriority.Lowest;
			thread.IsBackground = true;
			thread.CurrentCulture = Thread.CurrentThread.CurrentCulture;
			thread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
			Thread thread2 = thread;
			TaskThreadPool.Add(thread2);
			thread2.Start(new List<Dictionary<long, DateTime>> { dictionary, dictionary2 });
			DateTime now = DateTime.Now;
			Dictionary<long, IList<byte[]>> obj = default(Dictionary<long, IList<byte[]>>);
			Dictionary<long, DateTime> obj2 = default(Dictionary<long, DateTime>);
			Dictionary<long, byte> obj3 = default(Dictionary<long, byte>);
			while (ServiceRunningState)
			{
				if (suspendSendTableThread)
				{
					Thread.Sleep(30000);
					continue;
				}
				if (TSOutputList.Count < 1)
				{
					Thread.Sleep(5000);
					continue;
				}
				if (flag)
				{
					Thread.Sleep(10);
				}
				try
				{
					flag = true;
					for (int num = 0; num < TSOutputList.Count; num++)
					{
						long tsKey = TSOutputList[num].Identity;
						ushort tablePID = GetTablePID(EnumTableType.NIT, tsKey);
						SendSpeedInfo sendSpeedInfo = SendSpeedSettingList.SingleOrDefault((SendSpeedInfo p) => p.TsTableId == tsKey);
						if (sendSpeedInfo == null)
						{
							sendSpeedInfo = DefaultSendSpeed;
						}
						SendOrNotInfo sendOrNotInfo = SendOrNotSettingList.SingleOrDefault((SendOrNotInfo p) => p.TsTableId == tsKey);
						if (sendOrNotInfo == null)
						{
							sendOrNotInfo = DefaultSendOrNotSetting;
						}
						bool lockTaken = false;
						DateTime dateTime;
						try
						{
							Monitor.Enter(obj = NIT_ACTSectionListTable, ref lockTaken);
							if (sendOrNotInfo.NitAct == 1 && NIT_ACTSectionListTable.Keys.Contains(tsKey))
							{
								bool lockTaken2 = false;
								try
								{
									Monitor.Enter(obj2 = dictionary, ref lockTaken2);
									dateTime = (dictionary.Keys.Contains(tsKey) ? dictionary[tsKey] : DateTimeHelper.GetUTCDateTimeNow().Date);
								}
								finally
								{
									if (lockTaken2)
									{
										Monitor.Exit(obj2);
									}
								}
								DateTime uTCDateTimeNow = DateTimeHelper.GetUTCDateTimeNow();
								TimeSpan timeSpan = uTCDateTimeNow - dateTime;
								IList<byte[]> list = NIT_ACTSectionListTable[tsKey];
								if (timeSpan.TotalMilliseconds < (double)(sendSpeedInfo.NitAct / list.Count))
								{
									continue;
								}
								flag = false;
								SetRecordValue(dictionary, tsKey, uTCDateTimeNow);
								bool lockTaken3 = false;
								byte b;
								try
								{
									Monitor.Enter(obj3 = idxNIT_ACTTable, ref lockTaken3);
									b = (byte)(idxNIT_ACTTable.Keys.Contains(tsKey) ? idxNIT_ACTTable[tsKey] : 0);
								}
								finally
								{
									if (lockTaken3)
									{
										Monitor.Exit(obj3);
									}
								}
								SetRecordValueIndex(idxNIT_ACTTable, tsKey, list.Count, b);
								SendTSData(PackageGenerators.Instance[tsKey, tablePID].SectionConvertPackage(list[b], tablePID), GetSendIPEndPoint(tsKey));
							}
						}
						finally
						{
							if (lockTaken)
							{
								Monitor.Exit(obj);
							}
						}
						bool lockTaken4 = false;
						try
						{
							Monitor.Enter(obj = NIT_OTHSectionListTable, ref lockTaken4);
							if (sendOrNotInfo.NitOth != 1 || !NIT_OTHSectionListTable.Keys.Contains(tsKey))
							{
								continue;
							}
							bool lockTaken5 = false;
							try
							{
								Monitor.Enter(obj2 = dictionary2, ref lockTaken5);
								dateTime = (dictionary2.Keys.Contains(tsKey) ? dictionary2[tsKey] : DateTimeHelper.GetUTCDateTimeNow().Date);
							}
							finally
							{
								if (lockTaken5)
								{
									Monitor.Exit(obj2);
								}
							}
							DateTime uTCDateTimeNow = DateTimeHelper.GetUTCDateTimeNow();
							TimeSpan timeSpan = uTCDateTimeNow - dateTime;
							IList<byte[]> list2 = NIT_OTHSectionListTable[tsKey];
							if (timeSpan.TotalMilliseconds < (double)(sendSpeedInfo.NitOth / list2.Count))
							{
								continue;
							}
							flag = false;
							SetRecordValue(dictionary2, tsKey, uTCDateTimeNow);
							bool lockTaken6 = false;
							byte b2;
							try
							{
								Monitor.Enter(obj3 = idxNIT_OTHTable, ref lockTaken6);
								b2 = (byte)(idxNIT_OTHTable.Keys.Contains(tsKey) ? idxNIT_OTHTable[tsKey] : 0);
							}
							finally
							{
								if (lockTaken6)
								{
									Monitor.Exit(obj3);
								}
							}
							SetRecordValueIndex(idxNIT_OTHTable, tsKey, list2.Count, b2);
							SendTSData(PackageGenerators.Instance[tsKey, tablePID].SectionConvertPackage(list2[b2], tablePID), GetSendIPEndPoint(tsKey));
						}
						finally
						{
							if (lockTaken4)
							{
								Monitor.Exit(obj);
							}
						}
					}
				}
				catch (Exception ex)
				{
					HandellEPGServiceException(ex);
				}
			}
		}

		private static void SetRecordValue(Dictionary<long, long> recorder, long recorderKey, IList<long> sourceValues, long previewValue)
		{
			int num = sourceValues.IndexOf(previewValue);
			num++;
			try
			{
				Dictionary<long, long> obj = default(Dictionary<long, long>);
				if (num >= sourceValues.Count)
				{
					bool lockTaken = false;
					try
					{
						Monitor.Enter(obj = recorder, ref lockTaken);
						if (recorder.Keys.Contains(recorderKey))
						{
							recorder.Remove(recorderKey);
						}
						return;
					}
					finally
					{
						if (lockTaken)
						{
							Monitor.Exit(obj);
						}
					}
				}
				bool lockTaken2 = false;
				try
				{
					Monitor.Enter(obj = recorder, ref lockTaken2);
					recorder[recorderKey] = sourceValues[num];
				}
				finally
				{
					if (lockTaken2)
					{
						Monitor.Exit(obj);
					}
				}
			}
			catch (IndexOutOfRangeException)
			{
				ClearMemory(recorder);
			}
		}

		private static void SetRecordValue(Dictionary<long, Dictionary<long, long>> recorder, long firstKey, long secondKey, IList<long> sourceValues, long previewValue)
		{
			int num = sourceValues.IndexOf(previewValue);
			num++;
			long value = ((num >= sourceValues.Count) ? sourceValues.First() : sourceValues[num]);
			try
			{
				lock (recorder)
				{
					if (recorder.Keys.Contains(firstKey))
					{
						recorder[firstKey][secondKey] = value;
						return;
					}
					Dictionary<long, long> dictionary = new Dictionary<long, long>();
					dictionary.Add(secondKey, value);
					recorder.Add(firstKey, dictionary);
				}
			}
			catch (IndexOutOfRangeException)
			{
				ClearMemory(recorder);
			}
		}

		private static void SetRecordValueIndex(Dictionary<long, byte> recorder, long recorderKey, int sourceIndexCount, byte valueIndex)
		{
			byte b = (byte)(valueIndex + 1);
			try
			{
				Dictionary<long, byte> obj = default(Dictionary<long, byte>);
				if (b >= sourceIndexCount)
				{
					bool lockTaken = false;
					try
					{
						Monitor.Enter(obj = recorder, ref lockTaken);
						if (recorder.Keys.Contains(recorderKey))
						{
							recorder.Remove(recorderKey);
						}
						return;
					}
					finally
					{
						if (lockTaken)
						{
							Monitor.Exit(obj);
						}
					}
				}
				bool lockTaken2 = false;
				try
				{
					Monitor.Enter(obj = recorder, ref lockTaken2);
					recorder[recorderKey] = b;
				}
				finally
				{
					if (lockTaken2)
					{
						Monitor.Exit(obj);
					}
				}
			}
			catch (IndexOutOfRangeException)
			{
				ClearMemory(recorder);
			}
		}

		private static void SetRecordValueIndex(Dictionary<long, Dictionary<long, Dictionary<long, byte>>> recorder, long firstKey, long secondKey, long thirdKey, int sourceIndexCount, byte valueIndex)
		{
			byte b = (byte)(valueIndex + 1);
			b = (byte)((b < sourceIndexCount) ? b : 0);
			try
			{
				lock (recorder)
				{
					if (recorder.Keys.Contains(firstKey))
					{
						if (recorder[firstKey].Keys.Contains(secondKey))
						{
							recorder[firstKey][secondKey][thirdKey] = b;
							return;
						}
						Dictionary<long, byte> dictionary = new Dictionary<long, byte>();
						dictionary.Add(thirdKey, b);
						recorder[firstKey].Add(secondKey, dictionary);
					}
					else
					{
						Dictionary<long, byte> dictionary2 = new Dictionary<long, byte>();
						dictionary2.Add(thirdKey, b);
						Dictionary<long, Dictionary<long, byte>> dictionary3 = new Dictionary<long, Dictionary<long, byte>>();
						dictionary3.Add(secondKey, dictionary2);
						recorder.Add(firstKey, dictionary3);
					}
				}
			}
			catch (IndexOutOfRangeException)
			{
				ClearMemory(recorder);
			}
		}

		private static void SetRecordValue(Dictionary<long, DateTime> recorder, long recorderKey, DateTime recorderValue)
		{
			try
			{
				lock (recorder)
				{
					recorder[recorderKey] = recorderValue;
				}
			}
			catch (IndexOutOfRangeException)
			{
				ClearMemory(recorder);
			}
		}

		private static void SetRecordValue(Dictionary<long, Dictionary<long, DateTime>> recorder, long firstKey, long secondKey, DateTime recoderValue)
		{
			try
			{
				lock (recorder)
				{
					if (recorder.Keys.Contains(firstKey))
					{
						recorder[firstKey][secondKey] = recoderValue;
						return;
					}
					Dictionary<long, DateTime> dictionary = new Dictionary<long, DateTime>();
					dictionary.Add(secondKey, recoderValue);
					recorder.Add(firstKey, dictionary);
				}
			}
			catch (IndexOutOfRangeException)
			{
				ClearMemory(recorder);
			}
		}

		private static ushort GetTablePID(EnumTableType TableType, long tsIdentity)
		{
			ushort result = 8191;
			PidOutputInfo pidOutputInfo = PIDRemappingSettingList.Where((PidOutputInfo p) => p.TsTableId == tsIdentity).FirstOrDefault();
			switch (TableType)
			{
			case EnumTableType.BAT:
			case EnumTableType.SDT:
				result = (ushort)((pidOutputInfo == null) ? 17 : ((ushort)pidOutputInfo.SdtOrBat));
				break;
			case EnumTableType.CAT:
				result = (ushort)((pidOutputInfo == null) ? 1 : ((ushort)pidOutputInfo.Cat));
				break;
			case EnumTableType.EIT:
				result = (ushort)((pidOutputInfo == null) ? 18 : ((ushort)pidOutputInfo.Eit));
				break;
			case EnumTableType.NIT:
				result = (ushort)((pidOutputInfo == null) ? 16 : ((ushort)pidOutputInfo.Nit));
				break;
			case EnumTableType.PAT:
				result = (ushort)((pidOutputInfo != null) ? ((ushort)pidOutputInfo.Pat) : 0);
				break;
			case EnumTableType.TDT:
			case EnumTableType.TOT:
				result = (ushort)((pidOutputInfo == null) ? 20 : ((ushort)pidOutputInfo.TotOrTdt));
				break;
			}
			return result;
		}

		private static ushort GetPMTPID(long programIdentity)
		{
			ProgramInfo programInfo = ProgramOutputList.FirstOrDefault((ProgramInfo p) => p.Identity == programIdentity);
			return (ushort)((programInfo == null) ? 8191 : programInfo.PmtId);
		}

		private static ushort GetRemappingPMTPID(long programIdentity)
		{
			ProgramInfo programInfo = ProgramOutputList.FirstOrDefault((ProgramInfo p) => p.Identity == programIdentity);
			PmtPidOutputInfo pmtPidOutputInfo = PMTPIDRemappingSettingList.Where((PmtPidOutputInfo p) => p.ProgramTableId == programIdentity).FirstOrDefault();
			return (pmtPidOutputInfo == null) ? ((ushort)((programInfo == null) ? 8191 : programInfo.PmtId)) : ((ushort)pmtPidOutputInfo.OutputPid);
		}

		public static IPEndPoint GetSendIPEndPoint(long tsIdentity)
		{
			if (OutputIPSettingTable.Keys.Contains(tsIdentity))
			{
				return OutputIPSettingTable[tsIdentity];
			}
			IPEndPoint iPEndPoint = null;
			MultipleipSettings multipleipSettings = OutputIPSettingList.SingleOrDefault((MultipleipSettings p) => p.TsTableId == tsIdentity);
			iPEndPoint = ((multipleipSettings == null) ? ((SingleIPSetting == null) ? null : new IPEndPoint(IPAddress.Parse(SingleIPSetting.Ip), SingleIPSetting.Port)) : new IPEndPoint(IPAddress.Parse(multipleipSettings.Ip), multipleipSettings.Port));
			OutputIPSettingTable.Add(tsIdentity, iPEndPoint);
			return iPEndPoint;
		}

		private static void ClearupSendtimeDictionary(object paras)
		{
			while (ServiceRunningState)
			{
				if (suspendSendTableThread)
				{
					Thread.Sleep(30000);
					continue;
				}
				try
				{
					Thread.Sleep(3600000);
					Action<List<Dictionary<long, DateTime>>> action = delegate(List<Dictionary<long, DateTime>> times)
					{
						if (times.Count != 0)
						{
							for (int i = 0; i < times.Count; i++)
							{
								Dictionary<long, DateTime> dictionary3 = times[i];
								long[] array = dictionary3.Keys.ToArray();
								for (int j = 0; j < dictionary3.Count; j++)
								{
									long key = array[j];
									if ((DateTimeHelper.GetUTCDateTimeNow() - dictionary3[key]).Milliseconds >= 3600000)
									{
										dictionary3.Remove(key);
									}
								}
							}
						}
					};
					lock (paras)
					{
						if (paras is Dictionary<long, DateTime>)
						{
							Dictionary<long, DateTime> item = paras as Dictionary<long, DateTime>;
							action(new List<Dictionary<long, DateTime>> { item });
						}
						else if (paras is IList<Dictionary<long, DateTime>>)
						{
							List<Dictionary<long, DateTime>> obj = paras as List<Dictionary<long, DateTime>>;
							action(obj);
						}
						else
						{
							if (!(paras is Dictionary<long, Dictionary<long, DateTime>>))
							{
								continue;
							}
							Dictionary<long, Dictionary<long, DateTime>> dictionary = paras as Dictionary<long, Dictionary<long, DateTime>>;
							foreach (long key2 in dictionary.Keys)
							{
								Dictionary<long, DateTime> dictionary2 = dictionary[key2];
								if (dictionary2.Count == 0)
								{
									dictionary.Remove(key2);
									continue;
								}
								action(new List<Dictionary<long, DateTime>> { dictionary2 });
							}
							continue;
						}
					}
				}
				catch (Exception ex)
				{
					HandellEPGServiceException(ex);
				}
			}
		}

		private static void ClearMemory(IDictionary recorder)
		{
			lock (recorder)
			{
				recorder.Clear();
			}
			GC.Collect();
			Tool.WriteTXTLog("Clear memory completed");
			BalloonInfo balloonInfo = new BalloonInfo();
			balloonInfo.BalloonType = ToolTipIcon.Warning;
			balloonInfo.Content = ResourceManagement.GetLocationString("MemoryWarn");
			balloonInfo.Timeout = 5000;
			balloonInfo.Title = "EPG Service";
			NotifyFormManage.StartShowMessageUI(balloonInfo);
		}

		private static void AutoUpdateShowtime()
		{
			try
			{
				if (!ServiceRunningState || !config.IsAuto)
				{
					return;
				}
				while (true)
				{
					bool flag = true;
					try
					{
						SystemSetting systemSetting = new SystemSettingBLL().GetSystemSetting(SystemSettingCode.EPGEncoding);
						if (systemSetting == null)
						{
							CommTool.SetEncoder("DEFAULT");
						}
						else
						{
							CommTool.SetEncoder(systemSetting.Value);
						}
					}
					catch (Exception ex)
					{
						Tool.WriteTXTLog(string.Format("get epg encoding error:{0}", ex.Message));
						Thread.Sleep(30000);
						continue;
					}
					break;
				}
				watcher = new EPGWindowsFolderWatcher();
				watcher.Start(config.ShowtimPath);
				new Thread(Watch).Start();
			}
			catch (Exception ex)
			{
				HandellEPGServiceException(ex);
			}
		}

		private static void Watch()
		{
			while (true)
			{
				bool flag = true;
				if (!ServiceRunningState || !config.IsAuto)
				{
					break;
				}
				EPGWindowsFolderWatcher.WatcherFile watcherFile = watcher.Get();
				int num = (int)(DateTime.Now - watcherFile.UpdateTime).TotalSeconds;
				if (num < 5)
				{
					watcher.Set(watcherFile);
					continue;
				}
				StartWatchEPGFile(watcherFile);
				if (watcher.Count() == 0)
				{
					try
					{
						new SendNoticesManager().SetAllNoticeOn();
					}
					catch (Exception)
					{
					}
				}
			}
		}

		private static void StartWatchEPGFile(EPGWindowsFolderWatcher.WatcherFile file)
		{
			if (!ServiceRunningState || !config.IsAuto || file == null)
			{
				return;
			}
			try
			{
				Tool.WriteTXTLog(LogLevel.DEBUG, string.Format("File:{0} changed, begin auto update epg", file.Path));
				IList<ProgramInfo> byEitFile = new ProgramInfoManager().GetByEitFile(file.Name);
				if (byEitFile.Count == 0)
				{
					Tool.WriteTXTLog(LogLevel.DEBUG, string.Format("not found such programs infected with file:{0}", file.Path));
					return;
				}
				try
				{
					bool isAllImportSucc = true;
					int affectedNums = 0;
					new EITInfoManager().SaveImportEITAndLog(byEitFile.ToList(), null, config.DateFormat, ref isAllImportSucc, ref affectedNums, file.Path);
				}
				catch
				{
				}
				finally
				{
				}
				Tool.WriteTXTLog(LogLevel.DEBUG, string.Format("complete update epg from file:{0}", file.Path));
			}
			catch
			{
			}
		}

		private static void SendPMT()
		{
			Dictionary<long, long> prgIdxTable = new Dictionary<long, long>();
			Dictionary<long, byte> idxPrgPMTTable = new Dictionary<long, byte>();
			Dictionary<long, DateTime> dictionary = new Dictionary<long, DateTime>();
			bool flag = false;
			PMTReloading += delegate
			{
				lock (prgIdxTable)
				{
					prgIdxTable.Clear();
				}
				lock (idxPrgPMTTable)
				{
					idxPrgPMTTable.Clear();
				}
			};
			Thread thread = new Thread(ClearupSendtimeDictionary);
			thread.Name = "Clearup PMT sendtime";
			thread.Priority = ThreadPriority.Lowest;
			thread.IsBackground = true;
			thread.CurrentCulture = Thread.CurrentThread.CurrentCulture;
			thread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
			Thread thread2 = thread;
			TaskThreadPool.Add(thread2);
			thread2.Start(new List<Dictionary<long, DateTime>> { dictionary });
			DateTime now = DateTime.Now;
			while (ServiceRunningState)
			{
				if (suspendSendTableThread)
				{
					Thread.Sleep(30000);
					continue;
				}
				if (TSOutputList.Count < 1)
				{
					Thread.Sleep(5000);
					continue;
				}
				if (flag)
				{
					Thread.Sleep(10);
				}
				try
				{
					flag = true;
					for (int num = 0; num < TSOutputList.Count; num++)
					{
						long tsKey = TSOutputList[num].Identity;
						SendSpeedInfo sendSpeedInfo = SendSpeedSettingList.SingleOrDefault((SendSpeedInfo p) => p.TsTableId == tsKey);
						if (sendSpeedInfo == null)
						{
							sendSpeedInfo = DefaultSendSpeed;
						}
						SendOrNotInfo sendOrNotInfo = SendOrNotSettingList.SingleOrDefault((SendOrNotInfo p) => p.TsTableId == tsKey);
						if (sendOrNotInfo == null)
						{
							sendOrNotInfo = DefaultSendOrNotSetting;
						}
						lock (PMTSectionListTable)
						{
							if (sendOrNotInfo.Pmt != 1 || !PMTSectionListTable.Keys.Contains(tsKey))
							{
								continue;
							}
							Dictionary<long, IList<byte[]>> dictionary2 = PMTSectionListTable[tsKey];
							long num2;
							lock (prgIdxTable)
							{
								num2 = (prgIdxTable.Keys.Contains(tsKey) ? prgIdxTable[tsKey] : dictionary2.Keys.First());
							}
							DateTime dateTime;
							lock (dictionary)
							{
								dateTime = (dictionary.Keys.Contains(num2) ? dictionary[num2] : DateTimeHelper.GetUTCDateTimeNow().Date);
							}
							DateTime uTCDateTimeNow = DateTimeHelper.GetUTCDateTimeNow();
							TimeSpan timeSpan = uTCDateTimeNow - dateTime;
							IList<byte[]> list = dictionary2[num2];
							if (timeSpan.TotalMilliseconds < (double)(sendSpeedInfo.Pmt / list.Count))
							{
								continue;
							}
							flag = false;
							ushort pMTPID = GetPMTPID(num2);
							ushort remappingPMTPID = GetRemappingPMTPID(num2);
							SetRecordValue(dictionary, num2, uTCDateTimeNow);
							SetRecordValue(prgIdxTable, tsKey, dictionary2.Keys.ToList(), num2);
							byte b;
							lock (idxPrgPMTTable)
							{
								b = (byte)(idxPrgPMTTable.Keys.Contains(num2) ? idxPrgPMTTable[num2] : 0);
							}
							SetRecordValueIndex(idxPrgPMTTable, num2, list.Count, b);
							IList<byte[]> list2 = PackageGenerators.Instance[tsKey, pMTPID].SectionConvertPackage(list[b], pMTPID);
							if (remappingPMTPID != pMTPID)
							{
								foreach (byte[] item in list2)
								{
									PackageGenerators.Instance[tsKey, pMTPID].ReSetPackagePID(item, remappingPMTPID);
								}
							}
							SendTSData(list2, GetSendIPEndPoint(tsKey));
						}
					}
				}
				catch (Exception ex)
				{
					HandellEPGServiceException(ex);
				}
			}
		}

		private static void SendCAT()
		{
			Dictionary<long, byte> idxCATTable = new Dictionary<long, byte>();
			Dictionary<long, DateTime> dictionary = new Dictionary<long, DateTime>();
			bool flag = false;
			CATReloading += delegate
			{
				lock (idxCATTable)
				{
					idxCATTable.Clear();
				}
			};
			Thread thread = new Thread(ClearupSendtimeDictionary);
			thread.Name = "Clearup CAT sendtime";
			thread.Priority = ThreadPriority.Lowest;
			thread.IsBackground = true;
			thread.CurrentCulture = Thread.CurrentThread.CurrentCulture;
			thread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
			Thread thread2 = thread;
			TaskThreadPool.Add(thread2);
			thread2.Start(new List<Dictionary<long, DateTime>> { dictionary });
			DateTime now = DateTime.Now;
			while (ServiceRunningState)
			{
				if (suspendSendTableThread)
				{
					Thread.Sleep(30000);
					continue;
				}
				if (CATSectionListTable.Count == 0)
				{
					Thread.Sleep(5000);
					continue;
				}
				if (flag)
				{
					Thread.Sleep(10);
				}
				try
				{
					flag = true;
					for (int num = 0; num < TSOutputList.Count; num++)
					{
						long tsKey = TSOutputList[num].Identity;
						SendSpeedInfo sendSpeedInfo = SendSpeedSettingList.SingleOrDefault((SendSpeedInfo p) => p.TsTableId == tsKey);
						if (sendSpeedInfo == null)
						{
							sendSpeedInfo = DefaultSendSpeed;
						}
						SendOrNotInfo sendOrNotInfo = SendOrNotSettingList.SingleOrDefault((SendOrNotInfo p) => p.TsTableId == tsKey);
						if (sendOrNotInfo == null)
						{
							sendOrNotInfo = DefaultSendOrNotSetting;
						}
						lock (CATSectionListTable)
						{
							if (sendOrNotInfo.Cat != 1 || !CATSectionListTable.Keys.Contains(tsKey))
							{
								continue;
							}
							DateTime dateTime;
							lock (dictionary)
							{
								dateTime = (dictionary.Keys.Contains(tsKey) ? dictionary[tsKey] : DateTimeHelper.GetUTCDateTimeNow().Date);
							}
							DateTime uTCDateTimeNow = DateTimeHelper.GetUTCDateTimeNow();
							TimeSpan timeSpan = uTCDateTimeNow - dateTime;
							IList<byte[]> list = CATSectionListTable[tsKey];
							if (!(timeSpan.TotalMilliseconds < (double)(sendSpeedInfo.Cat / list.Count)))
							{
								flag = false;
								SetRecordValue(dictionary, tsKey, uTCDateTimeNow);
								byte b;
								lock (idxCATTable)
								{
									b = (byte)(idxCATTable.Keys.Contains(tsKey) ? idxCATTable[tsKey] : 0);
								}
								SetRecordValueIndex(idxCATTable, tsKey, list.Count, b);
								ushort tablePID = GetTablePID(EnumTableType.CAT, tsKey);
								SendTSData(PackageGenerators.Instance[tsKey, tablePID].SectionConvertPackage(list[b], tablePID), GetSendIPEndPoint(tsKey));
							}
						}
					}
				}
				catch (Exception ex)
				{
					HandellEPGServiceException(ex);
				}
			}
		}

		private static void SendBAT()
		{
			Dictionary<long, byte> idxBATTable = new Dictionary<long, byte>();
			Dictionary<long, DateTime> dictionary = new Dictionary<long, DateTime>();
			bool flag = false;
			BATReloading += delegate
			{
				lock (idxBATTable)
				{
					idxBATTable.Clear();
				}
			};
			Thread thread = new Thread(ClearupSendtimeDictionary);
			thread.Name = "Clearup BAT sendtime";
			thread.Priority = ThreadPriority.Lowest;
			thread.IsBackground = true;
			thread.CurrentCulture = Thread.CurrentThread.CurrentCulture;
			thread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
			Thread thread2 = thread;
			TaskThreadPool.Add(thread2);
			thread2.Start(new List<Dictionary<long, DateTime>> { dictionary });
			DateTime now = DateTime.Now;
			while (ServiceRunningState)
			{
				if (suspendSendTableThread)
				{
					Thread.Sleep(30000);
					continue;
				}
				if (BATSectionListTable.Count == 0)
				{
					Thread.Sleep(5000);
					continue;
				}
				if (flag)
				{
					Thread.Sleep(10);
				}
				try
				{
					flag = true;
					for (int num = 0; num < TSOutputList.Count; num++)
					{
						long tsKey = TSOutputList[num].Identity;
						SendSpeedInfo sendSpeedInfo = SendSpeedSettingList.SingleOrDefault((SendSpeedInfo p) => p.TsTableId == tsKey);
						if (sendSpeedInfo == null)
						{
							sendSpeedInfo = DefaultSendSpeed;
						}
						SendOrNotInfo sendOrNotInfo = SendOrNotSettingList.SingleOrDefault((SendOrNotInfo p) => p.TsTableId == tsKey);
						if (sendOrNotInfo == null)
						{
							sendOrNotInfo = DefaultSendOrNotSetting;
						}
						lock (BATSectionListTable)
						{
							if (sendOrNotInfo.Bat != 1 || !BATSectionListTable.Keys.Contains(tsKey))
							{
								continue;
							}
							DateTime dateTime;
							lock (dictionary)
							{
								dateTime = (dictionary.Keys.Contains(tsKey) ? dictionary[tsKey] : DateTimeHelper.GetUTCDateTimeNow().Date);
							}
							DateTime uTCDateTimeNow = DateTimeHelper.GetUTCDateTimeNow();
							TimeSpan timeSpan = uTCDateTimeNow - dateTime;
							IList<byte[]> list = BATSectionListTable[tsKey];
							if (!(timeSpan.TotalMilliseconds < (double)(sendSpeedInfo.Bat / list.Count)))
							{
								flag = false;
								SetRecordValue(dictionary, tsKey, uTCDateTimeNow);
								byte b;
								lock (idxBATTable)
								{
									b = (byte)(idxBATTable.Keys.Contains(tsKey) ? idxBATTable[tsKey] : 0);
								}
								SetRecordValueIndex(idxBATTable, tsKey, list.Count, b);
								ushort tablePID = GetTablePID(EnumTableType.BAT, tsKey);
								PackageGenerators.Instance[tsKey, tablePID].SendOutSectionData(list[b], tablePID, GetSendIPEndPoint(tsKey));
							}
						}
					}
				}
				catch (Exception ex)
				{
					HandellEPGServiceException(ex);
				}
			}
		}

		private static void SendTDT()
		{
			Dictionary<long, byte> idxTDTTable = new Dictionary<long, byte>();
			Dictionary<long, DateTime> dictionary = new Dictionary<long, DateTime>();
			bool flag = false;
			PATReloading += delegate
			{
				lock (idxTDTTable)
				{
					idxTDTTable.Clear();
				}
			};
			Thread thread = new Thread(ClearupSendtimeDictionary);
			thread.Name = "Clearup TDT sendtime";
			thread.Priority = ThreadPriority.Lowest;
			thread.IsBackground = true;
			thread.CurrentCulture = Thread.CurrentThread.CurrentCulture;
			thread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
			Thread thread2 = thread;
			TaskThreadPool.Add(thread2);
			thread2.Start(new List<Dictionary<long, DateTime>> { dictionary });
			DateTime now = DateTime.Now;
			while (ServiceRunningState)
			{
				if (suspendSendTableThread)
				{
					Thread.Sleep(30000);
					continue;
				}
				if (TSOutputList.Count < 1)
				{
					Thread.Sleep(5000);
					continue;
				}
				try
				{
					if (flag)
					{
						Thread.Sleep(100);
					}
					else
					{
						GenerateSections(EnumTableType.TDT);
					}
					flag = true;
					for (int num = 0; num < TSOutputList.Count; num++)
					{
						long tsKey = TSOutputList[num].Identity;
						SendSpeedInfo sendSpeedInfo = SendSpeedSettingList.SingleOrDefault((SendSpeedInfo p) => p.TsTableId == tsKey);
						if (sendSpeedInfo == null)
						{
							sendSpeedInfo = DefaultSendSpeed;
						}
						SendOrNotInfo sendOrNotInfo = SendOrNotSettingList.SingleOrDefault((SendOrNotInfo p) => p.TsTableId == tsKey);
						if (sendOrNotInfo == null)
						{
							sendOrNotInfo = DefaultSendOrNotSetting;
						}
						if (sendOrNotInfo.Tdt != 1)
						{
							continue;
						}
						DateTime dateTime;
						lock (dictionary)
						{
							dateTime = (dictionary.Keys.Contains(tsKey) ? dictionary[tsKey] : DateTimeHelper.GetUTCDateTimeNow().Date);
						}
						DateTime uTCDateTimeNow = DateTimeHelper.GetUTCDateTimeNow();
						if ((uTCDateTimeNow - dateTime).TotalMilliseconds < (double)(sendSpeedInfo.Tdt / TDTSectionList.Count))
						{
							continue;
						}
						flag = false;
						SetRecordValue(dictionary, tsKey, uTCDateTimeNow);
						lock (TDTSectionList)
						{
							byte b;
							lock (idxTDTTable)
							{
								b = (byte)(idxTDTTable.Keys.Contains(tsKey) ? idxTDTTable[tsKey] : 0);
							}
							SetRecordValueIndex(idxTDTTable, tsKey, TDTSectionList.Count, b);
							ushort tablePID = GetTablePID(EnumTableType.TDT, tsKey);
							PackageGenerators.Instance[tsKey, tablePID].SendOutSectionData(TDTSectionList[b], tablePID, GetSendIPEndPoint(tsKey));
						}
					}
				}
				catch (Exception ex)
				{
					HandellEPGServiceException(ex);
				}
			}
		}

		private static void CheckSendNotice()
		{
			SendNoticesManager sendNoticesManager = new SendNoticesManager();
			while (ServiceRunningState)
			{
				IList<SendNotices> all = sendNoticesManager.GetAll();
				if (DateTime.Now >= DateTime.Now.Date && DateTime.Now <= DateTime.Now.Date.AddMilliseconds(5000.0))
				{
					GenerateAllSendTable(true);
					Thread.Sleep(5000);
					continue;
				}
				if (all.Count == 0)
				{
					Thread.Sleep(5000);
					continue;
				}
				IList<IGrouping<int, SendNotices>> list = (from p in all
					group p by p.Code).ToList();
				foreach (IGrouping<int, SendNotices> item in list)
				{
					bool flag = false;
					EnumSendNoticesCodes key = (EnumSendNoticesCodes)item.Key;
					switch (key)
					{
					case EnumSendNoticesCodes.PMTPIDRemapping:
						LoadeDataFromDatabase(EnumSendNoticesCodes.PMTPIDRemapping);
						break;
					case EnumSendNoticesCodes.ReMapping:
						LoadeDataFromDatabase(EnumSendNoticesCodes.ReMapping);
						break;
					case EnumSendNoticesCodes.SendOrNot:
						LoadeDataFromDatabase(EnumSendNoticesCodes.SendOrNot);
						break;
					case EnumSendNoticesCodes.SendSpeedInfo:
						LoadeDataFromDatabase(EnumSendNoticesCodes.SendSpeedInfo);
						break;
					case EnumSendNoticesCodes.SingleIP:
						LoadeDataFromDatabase(EnumSendNoticesCodes.SingleIP);
						break;
					case EnumSendNoticesCodes.MultiIP:
						LoadeDataFromDatabase(EnumSendNoticesCodes.MultiIP);
						break;
					case EnumSendNoticesCodes.DES_TOT:
						LoadeDataFromDatabase(EnumSendNoticesCodes.DES_TOT);
						break;
					default:
						GenerateAllSendTable(true);
						flag = true;
						break;
					}
					if (flag)
					{
						break;
					}
					switch (key)
					{
					case EnumSendNoticesCodes.DES_TOT:
					case EnumSendNoticesCodes.SendOrNot:
					case EnumSendNoticesCodes.PMTPIDRemapping:
					case EnumSendNoticesCodes.ReMapping:
					case EnumSendNoticesCodes.MultiIP:
					case EnumSendNoticesCodes.SingleIP:
					case EnumSendNoticesCodes.SendSpeedInfo:
						foreach (SendNotices item2 in item)
						{
							sendNoticesManager.Delete(item2.Id);
						}
						break;
					}
				}
			}
		}

		private static void SmartCheckSendNotice()
		{
			SendNoticesManager sendNoticesManager = new SendNoticesManager();
			while (ServiceRunningState)
			{
				try
				{
					IList<SendNotices> all = sendNoticesManager.GetAll();
					if (all.Count == 0)
					{
						Thread.Sleep(5000);
						continue;
					}
					BalloonInfo balloonInfo = new BalloonInfo();
					balloonInfo.BalloonType = ToolTipIcon.Info;
					balloonInfo.Content = ResourceManagement.GetLocationString("ReceivedNotice");
					balloonInfo.Timeout = 5000;
					balloonInfo.Title = "EPG Service";
					NotifyFormManage.StartShowMessageUI(balloonInfo);
					bool flag = false;
					bool flag2 = false;
					bool flag3 = false;
					bool flag4 = false;
					bool flag5 = false;
					bool flag6 = false;
					bool flag7 = false;
					IList<IGrouping<int, SendNotices>> list = (from p in all
						group p by p.Code).ToList();
					foreach (IGrouping<int, SendNotices> item in list)
					{
						EnumSendNoticesCodes key = (EnumSendNoticesCodes)item.Key;
						switch (key)
						{
						case EnumSendNoticesCodes.DES_TOT:
						case EnumSendNoticesCodes.SendOrNot:
						case EnumSendNoticesCodes.PMTPIDRemapping:
						case EnumSendNoticesCodes.ReMapping:
						case EnumSendNoticesCodes.MultiIP:
						case EnumSendNoticesCodes.SingleIP:
						case EnumSendNoticesCodes.SendSpeedInfo:
							OutputIPSettingTable.Clear();
							LoadeDataFromDatabase(key);
							break;
						case EnumSendNoticesCodes.BAT:
						case EnumSendNoticesCodes.DES_BAT:
						case EnumSendNoticesCodes.DES_BAT_TS:
							LoadeDataFromDatabase(key);
							flag2 = true;
							break;
						case EnumSendNoticesCodes.NIT:
						case EnumSendNoticesCodes.DES_NIT:
						case EnumSendNoticesCodes.DES_NIT_TS:
							LoadeDataFromDatabase(key);
							flag3 = true;
							break;
						case EnumSendNoticesCodes.EIT:
						case EnumSendNoticesCodes.DES_EIT:
							LoadeDataFromDatabase(key);
							flag4 = true;
							break;
						case EnumSendNoticesCodes.multimedia:
						case EnumSendNoticesCodes.DES_PMT:
						case EnumSendNoticesCodes.DES_ES:
							LoadeDataFromDatabase(key);
							flag5 = true;
							break;
						case EnumSendNoticesCodes.DES_SDT:
							LoadeDataFromDatabase(key);
							flag6 = true;
							break;
						case EnumSendNoticesCodes.DES_CAT:
							LoadeDataFromDatabase(key);
							flag7 = true;
							break;
						default:
							GenerateAllSendTable(true);
							flag = true;
							break;
						}
						if (flag)
						{
							break;
						}
						sendNoticesManager.DeleteByCode((int)key);
					}
					if (flag)
					{
						BalloonInfo balloonInfo2 = new BalloonInfo();
						balloonInfo2.BalloonType = ToolTipIcon.Info;
						balloonInfo2.Content = ResourceManagement.GetLocationString("DealNoticeCompleted");
						balloonInfo2.Timeout = 5000;
						balloonInfo2.Title = "EPG Service";
						NotifyFormManage.StartShowMessageUI(balloonInfo2);
						continue;
					}
					if (flag2)
					{
						GenerateSections(EnumTableType.BAT);
					}
					if (flag7)
					{
						GenerateSections(EnumTableType.CAT);
					}
					if (flag4)
					{
						GenerateSections(EnumTableType.EIT);
					}
					if (flag3)
					{
						GenerateSections(EnumTableType.NIT);
					}
					if (flag5)
					{
						GenerateSections(EnumTableType.PMT);
					}
					if (flag6)
					{
						GenerateSections(EnumTableType.SDT);
					}
					BalloonInfo balloonInfo3 = new BalloonInfo();
					balloonInfo3.BalloonType = ToolTipIcon.Info;
					balloonInfo3.Content = ResourceManagement.GetLocationString("DealNoticeCompleted");
					balloonInfo3.Timeout = 5000;
					balloonInfo3.Title = "EPG Service";
					NotifyFormManage.StartShowMessageUI(balloonInfo3);
				}
				catch (Exception ex)
				{
					HandellEPGServiceException(ex);
				}
			}
		}

		private static void SendSDT_ACT()
		{
			Dictionary<long, byte> idxSDT_ACTTable = new Dictionary<long, byte>();
			Dictionary<long, DateTime> dictionary = new Dictionary<long, DateTime>();
			bool flag = false;
			SDTReloading += delegate
			{
				lock (idxSDT_ACTTable)
				{
					idxSDT_ACTTable.Clear();
				}
			};
			Thread thread = new Thread(ClearupSendtimeDictionary);
			thread.Name = "Clearup SDT sendtime";
			thread.Priority = ThreadPriority.Lowest;
			thread.IsBackground = true;
			thread.CurrentCulture = Thread.CurrentThread.CurrentCulture;
			thread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
			Thread thread2 = thread;
			TaskThreadPool.Add(thread2);
			thread2.Start(new List<Dictionary<long, DateTime>> { dictionary });
			DateTime now = DateTime.Now;
			while (ServiceRunningState)
			{
				if (suspendSendTableThread)
				{
					Thread.Sleep(30000);
					continue;
				}
				if (TSOutputList.Count < 1)
				{
					Thread.Sleep(5000);
					continue;
				}
				if (flag)
				{
					Thread.Sleep(10);
				}
				try
				{
					flag = true;
					for (int num = 0; num < TSOutputList.Count; num++)
					{
						long tsKey = TSOutputList[num].Identity;
						ushort tablePID = GetTablePID(EnumTableType.SDT, tsKey);
						SendSpeedInfo sendSpeedInfo = SendSpeedSettingList.SingleOrDefault((SendSpeedInfo p) => p.TsTableId == tsKey);
						if (sendSpeedInfo == null)
						{
							sendSpeedInfo = DefaultSendSpeed;
						}
						SendOrNotInfo sendOrNotInfo = SendOrNotSettingList.SingleOrDefault((SendOrNotInfo p) => p.TsTableId == tsKey);
						if (sendOrNotInfo == null)
						{
							sendOrNotInfo = DefaultSendOrNotSetting;
						}
						lock (SDT_ACTSectionListTable)
						{
							if (sendOrNotInfo.SdtAct != 1 || !SDT_ACTSectionListTable.Keys.Contains(tsKey))
							{
								continue;
							}
							DateTime dateTime;
							lock (dictionary)
							{
								dateTime = (dictionary.Keys.Contains(tsKey) ? dictionary[tsKey] : DateTimeHelper.GetUTCDateTimeNow().Date);
							}
							DateTime uTCDateTimeNow = DateTimeHelper.GetUTCDateTimeNow();
							TimeSpan timeSpan = uTCDateTimeNow - dateTime;
							IList<byte[]> list = SDT_ACTSectionListTable[tsKey];
							if (!(timeSpan.TotalMilliseconds < (double)(sendSpeedInfo.SdtAct / list.Count)))
							{
								flag = false;
								SetRecordValue(dictionary, tsKey, uTCDateTimeNow);
								byte b;
								lock (idxSDT_ACTTable)
								{
									b = (byte)(idxSDT_ACTTable.Keys.Contains(tsKey) ? idxSDT_ACTTable[tsKey] : 0);
								}
								SetRecordValueIndex(idxSDT_ACTTable, tsKey, list.Count, b);
								PackageGenerators.Instance[tsKey, tablePID].SendOutSectionData(list[b], tablePID, GetSendIPEndPoint(tsKey));
							}
						}
					}
				}
				catch (Exception ex)
				{
					HandellEPGServiceException(ex);
				}
			}
		}

		private static void SendSDT_OTH()
		{
			Dictionary<long, byte> idxSDT_OTHTable = new Dictionary<long, byte>();
			Dictionary<long, DateTime> dictionary = new Dictionary<long, DateTime>();
			bool flag = false;
			SDTReloading += delegate
			{
				lock (idxSDT_OTHTable)
				{
					idxSDT_OTHTable.Clear();
				}
			};
			Thread thread = new Thread(ClearupSendtimeDictionary);
			thread.Name = "Clearup SDT_OTH sendtime";
			thread.Priority = ThreadPriority.Lowest;
			thread.IsBackground = true;
			thread.CurrentCulture = Thread.CurrentThread.CurrentCulture;
			thread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
			Thread thread2 = thread;
			TaskThreadPool.Add(thread2);
			thread2.Start(new List<Dictionary<long, DateTime>> { dictionary });
			DateTime now = DateTime.Now;
			while (ServiceRunningState)
			{
				if (suspendSendTableThread)
				{
					Thread.Sleep(30000);
					continue;
				}
				if (TSOutputList.Count < 1)
				{
					Thread.Sleep(5000);
					continue;
				}
				if (flag)
				{
					Thread.Sleep(10);
				}
				try
				{
					flag = true;
					for (int num = 0; num < TSOutputList.Count; num++)
					{
						long tsKey = TSOutputList[num].Identity;
						ushort tablePID = GetTablePID(EnumTableType.SDT, tsKey);
						SendSpeedInfo sendSpeedInfo = SendSpeedSettingList.SingleOrDefault((SendSpeedInfo p) => p.TsTableId == tsKey);
						if (sendSpeedInfo == null)
						{
							sendSpeedInfo = DefaultSendSpeed;
						}
						SendOrNotInfo sendOrNotInfo = SendOrNotSettingList.SingleOrDefault((SendOrNotInfo p) => p.TsTableId == tsKey);
						if (sendOrNotInfo == null)
						{
							sendOrNotInfo = DefaultSendOrNotSetting;
						}
						lock (SDT_OTHSectionListTable)
						{
							if (sendOrNotInfo.SdtOth != 1 || !SDT_OTHSectionListTable.Keys.Contains(tsKey))
							{
								continue;
							}
							DateTime dateTime;
							lock (dictionary)
							{
								dateTime = (dictionary.Keys.Contains(tsKey) ? dictionary[tsKey] : DateTimeHelper.GetUTCDateTimeNow().Date);
							}
							DateTime uTCDateTimeNow = DateTimeHelper.GetUTCDateTimeNow();
							TimeSpan timeSpan = uTCDateTimeNow - dateTime;
							IList<byte[]> list = SDT_OTHSectionListTable[tsKey];
							if (!(timeSpan.TotalMilliseconds < (double)(sendSpeedInfo.SdtOth / list.Count)))
							{
								flag = false;
								SetRecordValue(dictionary, tsKey, uTCDateTimeNow);
								byte b;
								lock (idxSDT_OTHTable)
								{
									b = (byte)(idxSDT_OTHTable.Keys.Contains(tsKey) ? idxSDT_OTHTable[tsKey] : 0);
								}
								SetRecordValueIndex(idxSDT_OTHTable, tsKey, list.Count, b);
								PackageGenerators.Instance[tsKey, tablePID].SendOutSectionData(list[b], tablePID, GetSendIPEndPoint(tsKey));
							}
						}
					}
				}
				catch (Exception ex)
				{
					HandellEPGServiceException(ex);
				}
			}
		}

		private static void SendPAT()
		{
			Dictionary<long, byte> idxPATTable = new Dictionary<long, byte>();
			Dictionary<long, DateTime> dictionary = new Dictionary<long, DateTime>();
			bool flag = false;
			PATReloading += delegate
			{
				lock (idxPATTable)
				{
					idxPATTable.Clear();
				}
			};
			Thread thread = new Thread(ClearupSendtimeDictionary);
			thread.Name = "Clearup PAT sendtime";
			thread.Priority = ThreadPriority.Lowest;
			thread.IsBackground = true;
			thread.CurrentCulture = Thread.CurrentThread.CurrentCulture;
			thread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
			Thread thread2 = thread;
			TaskThreadPool.Add(thread2);
			thread2.Start(new List<Dictionary<long, DateTime>> { dictionary });
			DateTime now = DateTime.Now;
			while (ServiceRunningState)
			{
				if (suspendSendTableThread)
				{
					Thread.Sleep(30000);
					continue;
				}
				if (TSOutputList.Count < 1)
				{
					Thread.Sleep(5000);
					continue;
				}
				if (flag)
				{
					Thread.Sleep(10);
				}
				try
				{
					flag = true;
					for (int num = 0; num < TSOutputList.Count; num++)
					{
						long tsKey = TSOutputList[num].Identity;
						SendSpeedInfo sendSpeedInfo = SendSpeedSettingList.SingleOrDefault((SendSpeedInfo p) => p.TsTableId == tsKey);
						if (sendSpeedInfo == null)
						{
							sendSpeedInfo = DefaultSendSpeed;
						}
						SendOrNotInfo sendOrNotInfo = SendOrNotSettingList.SingleOrDefault((SendOrNotInfo p) => p.TsTableId == tsKey);
						if (sendOrNotInfo == null)
						{
							sendOrNotInfo = DefaultSendOrNotSetting;
						}
						lock (PATSectionListTable)
						{
							if (sendOrNotInfo.Pat != 1 || !PATSectionListTable.Keys.Contains(tsKey))
							{
								continue;
							}
							DateTime dateTime;
							lock (dictionary)
							{
								dateTime = (dictionary.Keys.Contains(tsKey) ? dictionary[tsKey] : DateTimeHelper.GetUTCDateTimeNow().Date);
							}
							IList<byte[]> list = PATSectionListTable[tsKey];
							DateTime uTCDateTimeNow = DateTimeHelper.GetUTCDateTimeNow();
							if (!((uTCDateTimeNow - dateTime).TotalMilliseconds < (double)(sendSpeedInfo.Pat / list.Count)))
							{
								flag = false;
								SetRecordValue(dictionary, tsKey, uTCDateTimeNow);
								ushort tablePID = GetTablePID(EnumTableType.PAT, tsKey);
								byte b;
								lock (idxPATTable)
								{
									b = (byte)(idxPATTable.Keys.Contains(tsKey) ? idxPATTable[tsKey] : 0);
								}
								SetRecordValueIndex(idxPATTable, tsKey, list.Count, b);
								SendTSData(PackageGenerators.Instance[tsKey, tablePID].SectionConvertPackage(list[b], tablePID), GetSendIPEndPoint(tsKey));
							}
						}
					}
				}
				catch (Exception ex)
				{
					HandellEPGServiceException(ex);
				}
			}
		}

		static TSOutputManager()
		{
			LastSpeedTagTable = new Dictionary<IPEndPoint, DateTime>();
			TotalSendBufferSizeTable = new Dictionary<IPEndPoint, long>();
			CurrentSpeedTable = new Dictionary<IPEndPoint, decimal>();
			config = AutoShowtimer.Get();
			watcher = null;
			PATSectionListTable = new Dictionary<long, IList<byte[]>>();
			CATSectionListTable = new Dictionary<long, IList<byte[]>>();
			PMTSectionListTable = new Dictionary<long, Dictionary<long, IList<byte[]>>>();
			NIT_ACTSectionListTable = new Dictionary<long, IList<byte[]>>();
			NIT_OTHSectionListTable = new Dictionary<long, IList<byte[]>>();
			BATSectionListTable = new Dictionary<long, IList<byte[]>>();
			SDT_ACTSectionListTable = new Dictionary<long, IList<byte[]>>();
			SDT_OTHSectionListTable = new Dictionary<long, IList<byte[]>>();
			EIT_ACT_PFSectionListTable = new Dictionary<long, Dictionary<long, IList<byte[]>>>();
			EIT_ACT_SCSectionListTable = new Dictionary<long, Dictionary<long, IList<byte[]>>>();
			TDTSectionList = new List<byte[]>();
			TOTSectionList = new List<byte[]>();
			OutputIPSettingTable = new Dictionary<long, IPEndPoint>();
			desManager = new DescriptorManagement();
			DefaultSendSpeed = new SendSpeedInfo
			{
				Bat = 3000,
				Cat = 1000,
				EitActPf = 1000,
				EitActSc = 3000,
				EitOthPf = 2000,
				EitOthSc = 4000,
				NitAct = 1000,
				NitOth = 4000,
				Pat = 300,
				Pmt = 300,
				SdtAct = 1000,
				SdtOth = 4000,
				Tdt = 2000,
				Tot = 2000
			};
			DefaultSendOrNotSetting = new SendOrNotInfo
			{
				Bat = 1,
				Cat = 1,
				EitActPf = 1,
				EitActSc = 1,
				EitOthPf = 1,
				EitOthSc = 1,
				NitAct = 1,
				NitOth = 1,
				Pat = 1,
				Pmt = 1,
				SdtAct = 1,
				SdtOth = 1,
				Tdt = 1,
				Tot = 1
			};
			TaskThreadPool = new List<Thread>();
			EventPool = new List<Delegate>();
			lockFileObject = new object();
			lockRePreparTSPlayer = new object();
			EventPool.Add(TSOutputManager.BATReloading);
			EventPool.Add(TSOutputManager.CATReloading);
			EventPool.Add(TSOutputManager.EITReloading);
			EventPool.Add(TSOutputManager.EITPFReloading);
			EventPool.Add(TSOutputManager.NITReloading);
			EventPool.Add(TSOutputManager.PATReloading);
			EventPool.Add(TSOutputManager.PMTReloading);
			EventPool.Add(TSOutputManager.SDTReloading);
			EventPool.Add(TSOutputManager.TOTDescriptorReloading);
			EventPool.Add(TSOutputManager.ServiceExiting);
		}

		private static void LoadeAllDataFromDatabase()
		{
			LoadeDataFromDatabase(EnumSendNoticesCodes.TS);
			LoadeDataFromDatabase(EnumSendNoticesCodes.BAT);
			LoadeDataFromDatabase(EnumSendNoticesCodes.PROGRAM);
			LoadeDataFromDatabase(EnumSendNoticesCodes.multimedia);
			LoadeDataFromDatabase(EnumSendNoticesCodes.EIT);
			LoadeDataFromDatabase(EnumSendNoticesCodes.MultiIP);
			LoadeDataFromDatabase(EnumSendNoticesCodes.SingleIP);
			LoadeDataFromDatabase(EnumSendNoticesCodes.ReMapping);
			LoadeDataFromDatabase(EnumSendNoticesCodes.PMTPIDRemapping);
			LoadeDataFromDatabase(EnumSendNoticesCodes.SendSpeedInfo);
			LoadeDataFromDatabase(EnumSendNoticesCodes.SendOrNot);
			LoadeDataFromDatabase(EnumSendNoticesCodes.DES_NIT_TS);
			LoadeDataFromDatabase(EnumSendNoticesCodes.DES_BAT_TS);
			LoadeDataFromDatabase(EnumSendNoticesCodes.DES_EIT);
			LoadeDataFromDatabase(EnumSendNoticesCodes.DES_BAT);
			LoadeDataFromDatabase(EnumSendNoticesCodes.DES_CAT);
			LoadeDataFromDatabase(EnumSendNoticesCodes.DES_NIT);
			LoadeDataFromDatabase(EnumSendNoticesCodes.DES_PMT);
			LoadeDataFromDatabase(EnumSendNoticesCodes.DES_ES);
			LoadeDataFromDatabase(EnumSendNoticesCodes.DES_SDT);
			LoadeDataFromDatabase(EnumSendNoticesCodes.DES_TOT);
			new SendNoticesManager().DeleteAll();
		}

		private static void LoadeDataFromDatabase(EnumSendNoticesCodes location)
		{
			try
			{
				TSInfoManager tSInfoManager = new TSInfoManager();
				DescriptorManagement descriptorManagement = new DescriptorManagement();
				switch (location)
				{
				case EnumSendNoticesCodes.TS:
					TSOutputList = tSInfoManager.GetOutputTS();
					break;
				case EnumSendNoticesCodes.PROGRAM:
					ProgramOutputList = new ProgramInfoManager().GetOutputProgram();
					break;
				case EnumSendNoticesCodes.multimedia:
					MultimediaOutputList = new MultimediaManager().GetOutput();
					break;
				case EnumSendNoticesCodes.EIT:
				{
					EITInfoManager eITInfoManager = new EITInfoManager();
					eITInfoManager.DeleteOutofdate(DateTimeHelper.GetUTCDateTimeNow());
					bool flag = false;
					EITOutputList = eITInfoManager.GetOutputEIT(DateTimeHelper.GetUTCDateTimeNow(), EPGSettings.Instatce.EventLoadeDays);
					break;
				}
				case EnumSendNoticesCodes.BAT:
					BATOutputList = new BATInfoManager().GetOutputBAT();
					break;
				case EnumSendNoticesCodes.MultiIP:
					OutputIPSettingList = new MultiIPOutputBLL().GetData();
					break;
				case EnumSendNoticesCodes.SingleIP:
					SingleIPSetting = new SingleIPOutputBLL().GetData().FirstOrDefault();
					break;
				case EnumSendNoticesCodes.ReMapping:
					PIDRemappingSettingList = new ReMappingPIDBLL().GetData();
					break;
				case EnumSendNoticesCodes.PMTPIDRemapping:
					PMTPIDRemappingSettingList = new PMTPIDReMappingBLL().GetData();
					break;
				case EnumSendNoticesCodes.SendSpeedInfo:
					SendSpeedSettingList = new SendSpeedInfoManager().GetOutPutSpeedSetting();
					break;
				case EnumSendNoticesCodes.SendOrNot:
					SendOrNotSettingList = new SendOrNotInfoBLL().GetData();
					break;
				case EnumSendNoticesCodes.DES_NIT_TS:
				{
					NIT_TSOutputList = new List<TsInfo>();
					NIT_TSDescriptorList = new List<DescriptorSigleBase>();
					List<long> list = (from p1 in (from p in TSOutputList
							group p by p.NitTableId).ToList()
						select p1.Key).ToList();
					{
						foreach (long item in list)
						{
							NIT_TSDescriptorList = NIT_TSDescriptorList.Concat(descriptorManagement.GetNIT_TS_DescriptorsByNetWorkIDEntity(item)).ToList();
							foreach (DescriptorSigleBase nIT_TSDescriptor in NIT_TSDescriptorList)
							{
								if (!NIT_TSOutputList.Select((TsInfo p) => p.Identity).Contains(nIT_TSDescriptor.TSTableID))
								{
									NIT_TSOutputList.Add(tSInfoManager.GetTSInfoByTSIdentity(nIT_TSDescriptor.TSTableID));
								}
							}
						}
						break;
					}
				}
				case EnumSendNoticesCodes.DES_BAT_TS:
					BAT_TSOutputList = new List<TsInfo>();
					BAT_TSDescriptorList = new List<DescriptorSigleBase>();
					{
						foreach (BATInfo bATOutput in BATOutputList)
						{
							BAT_TSDescriptorList = BAT_TSDescriptorList.Concat(descriptorManagement.GetBAT_TS_DescriptorsByBATIDEntity(bATOutput.Identity)).ToList();
							foreach (DescriptorSigleBase bAT_TSDescriptor in BAT_TSDescriptorList)
							{
								if (!BAT_TSOutputList.Select((TsInfo p) => p.Identity).Contains(bAT_TSDescriptor.TSTableID))
								{
									BAT_TSOutputList.Add(tSInfoManager.GetTSInfoByTSIdentity(bAT_TSDescriptor.TSTableID));
								}
							}
						}
						break;
					}
				case EnumSendNoticesCodes.DES_EIT:
				{
					bool flag = false;
					EITDescriptorList = descriptorManagement.GetEITDescriptorsForSend();
					break;
				}
				case EnumSendNoticesCodes.DES_BAT:
					BATDescriptorList = new List<DescriptorSigleBase>();
					{
						foreach (BATInfo bATOutput2 in BATOutputList)
						{
							BATDescriptorList = BATDescriptorList.Concat(descriptorManagement.GetDescriptors(EnumPossibleLoaction.BAT, bATOutput2.Identity)).ToList();
						}
						break;
					}
				case EnumSendNoticesCodes.DES_CAT:
					CATDescriptorList = new List<DescriptorSigleBase>();
					{
						foreach (TsInfo tSOutput in TSOutputList)
						{
							CATDescriptorList = CATDescriptorList.Concat(descriptorManagement.GetDescriptors(EnumPossibleLoaction.CAT, tSOutput.Identity)).ToList();
						}
						break;
					}
				case EnumSendNoticesCodes.DES_NIT:
					NITDescriptorList = new List<DescriptorSigleBase>();
					{
						foreach (long item2 in (from p1 in (from p in TSOutputList
								group p by p.NitTableId).ToList()
							select p1.Key).ToList())
						{
							NITDescriptorList = NITDescriptorList.Concat(descriptorManagement.GetDescriptors(EnumPossibleLoaction.NIT, item2)).ToList();
						}
						break;
					}
				case EnumSendNoticesCodes.DES_PMT:
					PMTDescriptorList = new List<DescriptorSigleBase>();
					{
						foreach (ProgramInfo programOutput in ProgramOutputList)
						{
							PMTDescriptorList = PMTDescriptorList.Concat(descriptorManagement.GetDescriptors(EnumPossibleLoaction.PMT, programOutput.Identity)).ToList();
						}
						break;
					}
				case EnumSendNoticesCodes.DES_ES:
					MultimediaDescriptorList = new List<DescriptorSigleBase>();
					{
						foreach (Multimedia multimediaOutput in MultimediaOutputList)
						{
							MultimediaDescriptorList = MultimediaDescriptorList.Concat(descriptorManagement.GetDescriptors(EnumPossibleLoaction.ES, multimediaOutput.Id)).ToList();
						}
						break;
					}
				case EnumSendNoticesCodes.DES_SDT:
					SDTDescriptorList = new List<DescriptorSigleBase>();
					{
						foreach (ProgramInfo programOutput2 in ProgramOutputList)
						{
							SDTDescriptorList = SDTDescriptorList.Concat(descriptorManagement.GetDescriptors(EnumPossibleLoaction.SDT, programOutput2.Identity)).ToList();
						}
						break;
					}
				case EnumSendNoticesCodes.DES_TOT:
					if (TSOutputManager.TOTDescriptorReloading != null)
					{
						TSOutputManager.TOTDescriptorReloading();
					}
					TOTDescriptor = descriptorManagement.GetDescriptorByID(EnumPossibleLoaction.TOT, 0L);
					break;
				case EnumSendNoticesCodes.PAT:
				case EnumSendNoticesCodes.NIT:
					break;
				}
			}
			catch (Exception ex)
			{
				Tool.WriteTXTLog(string.Format("LoadeDataFromDatabase [{0}] failed! Exception is below:", location.ToString()));
				throw ex;
			}
		}

		private static void GenerateAllSendTable(object prams)
		{
			if (prams == null)
			{
				throw new NullReferenceException();
			}
			if ((bool)prams)
			{
				LoadeAllDataFromDatabase();
			}
			GenerateSections(EnumTableType.PAT);
			GenerateSections(EnumTableType.PMT);
			GenerateSections(EnumTableType.CAT);
			GenerateSections(EnumTableType.NIT);
			GenerateSections(EnumTableType.BAT);
			GenerateSections(EnumTableType.SDT);
			bool flag = false;
			GenerateSections(EnumTableType.EIT);
			GenerateSections(EnumTableType.TDT);
			GenerateSections(EnumTableType.TOT);
		}

		private static void GenerateSections(EnumTableType TableType)
		{
			try
			{
				Dictionary<long, IList<byte[]>> obj3 = default(Dictionary<long, IList<byte[]>>);
				Dictionary<long, Dictionary<long, IList<byte[]>>> obj2 = default(Dictionary<long, Dictionary<long, IList<byte[]>>>);
				IList<byte[]> obj = default(IList<byte[]>);
				switch (TableType)
				{
				case EnumTableType.PAT:
				{
					bool lockTaken11 = false;
					try
					{
						Monitor.Enter(obj3 = PATSectionListTable, ref lockTaken11);
						PATSectionListTable.Clear();
					}
					finally
					{
						if (lockTaken11)
						{
							Monitor.Exit(obj3);
						}
					}
					if (TSOutputManager.PATReloading != null)
					{
						TSOutputManager.PATReloading();
					}
					TsInfo iTS3;
					foreach (TsInfo tSOutput in TSOutputList)
					{
						iTS3 = tSOutput;
						IList<ProgramInfo> programs = ProgramOutputList.Where((ProgramInfo p) => p.TsTableId == iTS3.Identity).ToList();
						IList<byte[]> value2 = PATGeneratorBLL.GeneratePATSections(programs);
						bool lockTaken12 = false;
						try
						{
							Monitor.Enter(obj3 = PATSectionListTable, ref lockTaken12);
							PATSectionListTable[iTS3.Identity] = value2;
						}
						finally
						{
							if (lockTaken12)
							{
								Monitor.Exit(obj3);
							}
						}
					}
					break;
				}
				case EnumTableType.PMT:
				{
					bool lockTaken9 = false;
					try
					{
						Monitor.Enter(obj2 = PMTSectionListTable, ref lockTaken9);
						PMTSectionListTable.Clear();
					}
					finally
					{
						if (lockTaken9)
						{
							Monitor.Exit(obj2);
						}
					}
					if (TSOutputManager.PMTReloading != null)
					{
						TSOutputManager.PMTReloading();
					}
					TsInfo iTS2;
					foreach (TsInfo tSOutput2 in TSOutputList)
					{
						iTS2 = tSOutput2;
						Dictionary<long, IList<byte[]>> dictionary3 = new Dictionary<long, IList<byte[]>>();
						ProgramInfo iPrg;
						foreach (ProgramInfo item in ProgramOutputList.Where((ProgramInfo p) => p.TsTableId == iTS2.Identity).ToList())
						{
							iPrg = item;
							IList<byte[]> sectionData = new PMTGeneratorBLL().GetSectionData(iPrg, MultimediaOutputList.Where((Multimedia p) => p.ProgramTableId == iPrg.Identity).ToList());
							dictionary3[iPrg.Identity] = sectionData;
						}
						bool lockTaken10 = false;
						try
						{
							Monitor.Enter(obj2 = PMTSectionListTable, ref lockTaken10);
							PMTSectionListTable[iTS2.Identity] = dictionary3;
						}
						finally
						{
							if (lockTaken10)
							{
								Monitor.Exit(obj2);
							}
						}
					}
					break;
				}
				case EnumTableType.CAT:
				{
					bool lockTaken7 = false;
					try
					{
						Monitor.Enter(obj3 = CATSectionListTable, ref lockTaken7);
						CATSectionListTable.Clear();
					}
					finally
					{
						if (lockTaken7)
						{
							Monitor.Exit(obj3);
						}
					}
					if (TSOutputManager.CATReloading != null)
					{
						TSOutputManager.CATReloading();
					}
					foreach (TsInfo tSOutput3 in TSOutputList)
					{
						IList<byte[]> list5 = new List<byte[]>();
						IList<byte[]> value = CATGeneratorBLL.GenerateCATSections(tSOutput3);
						bool lockTaken8 = false;
						try
						{
							Monitor.Enter(obj3 = CATSectionListTable, ref lockTaken8);
							CATSectionListTable[tSOutput3.Identity] = value;
						}
						finally
						{
							if (lockTaken8)
							{
								Monitor.Exit(obj3);
							}
						}
					}
					break;
				}
				case EnumTableType.NIT:
				{
					Dictionary<long, IList<byte[]>> dictionary4 = new Dictionary<long, IList<byte[]>>();
					IList<long> list6 = (from p1 in (from p in TSOutputList
							group p by p.NitTableId).ToList()
						select p1.Key).ToList();
					long nitKey;
					foreach (long item2 in list6)
					{
						nitKey = item2;
						IList<TsInfo> args = NIT_TSOutputList.Where((TsInfo p) => p.NitTableId == nitKey).ToList();
						NitInfo nit = (from p1 in TSOutputList.Select((TsInfo p) => p.NIT).ToList()
							where p1.Identity == nitKey
							select p1).First();
						IList<byte[]> value3 = NITGeneratorBLL.GenerateNITSections(nit, args);
						dictionary4[nitKey] = value3;
					}
					bool lockTaken13 = false;
					try
					{
						Monitor.Enter(obj3 = NIT_ACTSectionListTable, ref lockTaken13);
						NIT_ACTSectionListTable.Clear();
					}
					finally
					{
						if (lockTaken13)
						{
							Monitor.Exit(obj3);
						}
					}
					bool lockTaken14 = false;
					try
					{
						Monitor.Enter(obj3 = NIT_OTHSectionListTable, ref lockTaken14);
						NIT_OTHSectionListTable.Clear();
					}
					finally
					{
						if (lockTaken14)
						{
							Monitor.Exit(obj3);
						}
					}
					if (TSOutputManager.NITReloading != null)
					{
						TSOutputManager.NITReloading();
					}
					foreach (TsInfo tSOutput4 in TSOutputList)
					{
						IList<byte[]> list7 = new List<byte[]>();
						foreach (long key in dictionary4.Keys)
						{
							IList<byte[]> list8 = dictionary4[key];
							if (tSOutput4.NitTableId == key)
							{
								bool lockTaken15 = false;
								try
								{
									Monitor.Enter(obj3 = NIT_ACTSectionListTable, ref lockTaken15);
									NIT_ACTSectionListTable[tSOutput4.Identity] = list8;
								}
								finally
								{
									if (lockTaken15)
									{
										Monitor.Exit(obj3);
									}
								}
								continue;
							}
							foreach (byte[] item3 in list8)
							{
								byte[] array = (byte[])item3.Clone();
								array[0] = 65;
								CRCGeneratorBLL.SetSectionCRC(array);
								list7.Add(array);
							}
						}
						if (list7.Count <= 0)
						{
							continue;
						}
						bool lockTaken16 = false;
						try
						{
							Monitor.Enter(obj3 = NIT_OTHSectionListTable, ref lockTaken16);
							NIT_OTHSectionListTable[tSOutput4.Identity] = list7;
						}
						finally
						{
							if (lockTaken16)
							{
								Monitor.Exit(obj3);
							}
						}
					}
					break;
				}
				case EnumTableType.BAT:
				{
					Dictionary<long, List<byte[]>> dictionary5 = new Dictionary<long, List<byte[]>>();
					IList<IGrouping<long, BATInfo>> list11 = (from p in BATOutputList
						group p by p.NitTableId).ToList();
					IGrouping<long, BATInfo> iBATGroup;
					foreach (IGrouping<long, BATInfo> item4 in list11)
					{
						iBATGroup = item4;
						List<TsInfo> bAT_TSList = BAT_TSOutputList.Where((TsInfo p) => p.NitTableId == iBATGroup.Key).ToList();
						List<byte[]> list12 = new List<byte[]>();
						foreach (BATInfo item5 in iBATGroup.ToList())
						{
							list12 = list12.Concat(BATGeneratorBLL.GenerateBATSections(item5, bAT_TSList)).ToList();
						}
						dictionary5[iBATGroup.Key] = list12;
					}
					bool lockTaken21 = false;
					try
					{
						Monitor.Enter(obj3 = BATSectionListTable, ref lockTaken21);
						BATSectionListTable.Clear();
					}
					finally
					{
						if (lockTaken21)
						{
							Monitor.Exit(obj3);
						}
					}
					if (TSOutputManager.BATReloading != null)
					{
						TSOutputManager.BATReloading();
					}
					foreach (TsInfo tSOutput5 in TSOutputList)
					{
						if (!dictionary5.Keys.Contains(tSOutput5.NitTableId))
						{
							continue;
						}
						bool lockTaken22 = false;
						try
						{
							Monitor.Enter(obj3 = BATSectionListTable, ref lockTaken22);
							BATSectionListTable[tSOutput5.Identity] = dictionary5[tSOutput5.NitTableId];
						}
						finally
						{
							if (lockTaken22)
							{
								Monitor.Exit(obj3);
							}
						}
					}
					break;
				}
				case EnumTableType.SDT:
				{
					bool lockTaken17 = false;
					try
					{
						Monitor.Enter(obj3 = SDT_ACTSectionListTable, ref lockTaken17);
						SDT_ACTSectionListTable.Clear();
					}
					finally
					{
						if (lockTaken17)
						{
							Monitor.Exit(obj3);
						}
					}
					if (TSOutputManager.SDTReloading != null)
					{
						TSOutputManager.SDTReloading();
					}
					TsInfo iTS4;
					foreach (TsInfo tSOutput6 in TSOutputList)
					{
						iTS4 = tSOutput6;
						IList<ProgramInfo> programs = ProgramOutputList.Where((ProgramInfo p) => p.TsTableId == iTS4.Identity).ToList();
						IList<byte[]> value4 = SDTGeneratorBLL.GenerateSDTSections(programs);
						bool lockTaken18 = false;
						try
						{
							Monitor.Enter(obj3 = SDT_ACTSectionListTable, ref lockTaken18);
							SDT_ACTSectionListTable[iTS4.Identity] = value4;
						}
						finally
						{
							if (lockTaken18)
							{
								Monitor.Exit(obj3);
							}
						}
					}
					bool lockTaken19 = false;
					try
					{
						Monitor.Enter(obj3 = SDT_OTHSectionListTable, ref lockTaken19);
						SDT_OTHSectionListTable.Clear();
					}
					finally
					{
						if (lockTaken19)
						{
							Monitor.Exit(obj3);
						}
					}
					foreach (TsInfo tSOutput7 in TSOutputList)
					{
						IList<byte[]> list9 = new List<byte[]>();
						long tsKey;
						foreach (long key2 in SDT_ACTSectionListTable.Keys)
						{
							tsKey = key2;
							IList<byte[]> list10 = SDT_ACTSectionListTable[tsKey];
							if (tSOutput7.Identity == tsKey)
							{
								continue;
							}
							TsInfo tsInfo = TSOutputList.Where((TsInfo p) => p.Identity == tsKey).FirstOrDefault();
							if (tSOutput7.NitTableId != tsInfo.NitTableId)
							{
								continue;
							}
							foreach (byte[] item6 in list10)
							{
								byte[] array2 = (byte[])item6.Clone();
								array2[0] = 70;
								CRCGeneratorBLL.SetSectionCRC(array2);
								list9.Add(array2);
							}
						}
						if (list9.Count <= 0)
						{
							continue;
						}
						bool lockTaken20 = false;
						try
						{
							Monitor.Enter(obj3 = SDT_OTHSectionListTable, ref lockTaken20);
							SDT_OTHSectionListTable[tSOutput7.Identity] = list9;
						}
						finally
						{
							if (lockTaken20)
							{
								Monitor.Exit(obj3);
							}
						}
					}
					break;
				}
				case EnumTableType.EIT:
				{
					bool flag = false;
					DateTime DTNow = DateTimeHelper.GetUTCDateTimeNow();
					bool lockTaken3 = false;
					try
					{
						Monitor.Enter(obj2 = EIT_ACT_PFSectionListTable, ref lockTaken3);
						EIT_ACT_PFSectionListTable.Clear();
					}
					finally
					{
						if (lockTaken3)
						{
							Monitor.Exit(obj2);
						}
					}
					bool lockTaken4 = false;
					try
					{
						Monitor.Enter(obj2 = EIT_ACT_SCSectionListTable, ref lockTaken4);
						EIT_ACT_SCSectionListTable.Clear();
					}
					finally
					{
						if (lockTaken4)
						{
							Monitor.Exit(obj2);
						}
					}
					if (TSOutputManager.EITReloading != null)
					{
						TSOutputManager.EITReloading();
					}
					TsInfo iTS;
					foreach (TsInfo tSOutput8 in TSOutputList)
					{
						iTS = tSOutput8;
						Dictionary<long, IList<byte[]>> dictionary = new Dictionary<long, IList<byte[]>>();
						Dictionary<long, IList<byte[]>> dictionary2 = new Dictionary<long, IList<byte[]>>();
						IList<ProgramInfo> list = ProgramOutputList.Where((ProgramInfo p) => p.TsTableId == iTS.Identity).ToList();
						ProgramInfo iProgram;
						foreach (ProgramInfo item7 in list)
						{
							iProgram = item7;
							IList<EitInfo> source = EITOutputList.Where((EitInfo p) => p.ProgramTableId == iProgram.Identity).ToList();
							EitInfo eIT = source.Where((EitInfo p) => DTNow >= p.StartTime && DTNow < p.EndTime).FirstOrDefault();
							EitInfo eIT2 = source.Where((EitInfo p) => DTNow < p.StartTime).FirstOrDefault();
							byte eITVersionNumber = EITGeneratorBLL.GetEITVersionNumber(iProgram.Identity);
							IList<byte[]> list2 = new List<byte[]>();
							list2.Add(EITGeneratorBLL.GenerateEITPFSection(eIT, true, iProgram, eITVersionNumber));
							list2.Add(EITGeneratorBLL.GenerateEITPFSection(eIT2, false, iProgram, eITVersionNumber));
							dictionary[iProgram.Identity] = list2;
							IList<EitInfo> list3 = source.Where((EitInfo p) => p.StartTime > DTNow).ToList();
							if (list3.Count != 0)
							{
								IList<byte[]> list4 = EITGeneratorBLL.GenerateEITSCSections(list3, iProgram);
								if (list4 != null)
								{
									dictionary2[iProgram.Identity] = list4;
								}
							}
						}
						bool lockTaken5 = false;
						try
						{
							Monitor.Enter(obj2 = EIT_ACT_PFSectionListTable, ref lockTaken5);
							EIT_ACT_PFSectionListTable[iTS.Identity] = dictionary;
						}
						finally
						{
							if (lockTaken5)
							{
								Monitor.Exit(obj2);
							}
						}
						if (dictionary2.Count <= 0)
						{
							continue;
						}
						bool lockTaken6 = false;
						try
						{
							Monitor.Enter(obj2 = EIT_ACT_SCSectionListTable, ref lockTaken6);
							EIT_ACT_SCSectionListTable[iTS.Identity] = dictionary2;
						}
						finally
						{
							if (lockTaken6)
							{
								Monitor.Exit(obj2);
							}
						}
					}
					break;
				}
				case EnumTableType.TDT:
				{
					bool lockTaken2 = false;
					try
					{
						Monitor.Enter(obj = TDTSectionList, ref lockTaken2);
						TDTSectionList = TDTGeneratorBLL.GetSectionData(DateTimeHelper.GetUTCDateTimeNow());
					}
					finally
					{
						if (lockTaken2)
						{
							Monitor.Exit(obj);
						}
					}
					break;
				}
				case EnumTableType.TOT:
				{
					bool lockTaken = false;
					try
					{
						Monitor.Enter(obj = TOTSectionList, ref lockTaken);
						TOTSectionList = TOTGeneratorBLL.GetSectionData(DateTimeHelper.GetUTCDateTimeNow());
					}
					finally
					{
						if (lockTaken)
						{
							Monitor.Exit(obj);
						}
					}
					break;
				}
				case EnumTableType.TS:
					break;
				}
			}
			catch (Exception ex)
			{
				Tool.WriteTXTLog(string.Format("GenerateSections [{0}] failed! Exception is below:", TableType.ToString()));
				Tool.WriteTXTLog(ex);
				throw ex;
			}
			GC.Collect();
		}

		public static void StartSendTransprotStream()
		{
			if (ServiceRunningState)
			{
				return;
			}
			if (EPGSettings.Instatce.SendTSType == EnumSendType.TSPlayer)
			{
				if (!StreamPlayerManagement.Ins.OpenDevice())
				{
					Tool.WriteTXTLog(ResourceManagement.GetLocationString("OpenTSPlayerError"));
					BalloonInfo balloonInfo = new BalloonInfo();
					balloonInfo.BalloonType = ToolTipIcon.Error;
					balloonInfo.Content = ResourceManagement.GetLocationString("OpenTSPlayerError");
					balloonInfo.Timeout = 5000;
					balloonInfo.Title = "EPG Service";
					NotifyFormManage.StartShowMessageUI(balloonInfo);
					StopSendTransprotStream();
					if (TSOutputManager.ServiceExiting != null)
					{
						TSOutputManager.ServiceExiting();
					}
					return;
				}
				StreamPlayerManagement.Ins.SetRate(EPGSettings.Instatce.TSPlayerRate);
			}
			try
			{
				LoadeAllDataFromDatabase();
				Thread thread = new Thread(GenerateAllSendTable);
				thread.Name = "All section generater";
				thread.IsBackground = true;
				thread.Start(false);
			}
			catch (Exception ex)
			{
				HandellEPGServiceException(ex);
				StartSendTransprotStream();
				return;
			}
			ServiceRunningState = true;
			suspendSendTableThread = false;
			try
			{
				foreach (TsInfo tSOutput in TSOutputList)
				{
					GetSendIPEndPoint(tSOutput.Identity);
				}
				foreach (IPEndPoint item in from x in OutputIPSettingTable.Values.Distinct()
					where x != null
					select x)
				{
					LastSpeedTagTable[item] = DateTime.Now;
					TotalSendBufferSizeTable[item] = 0L;
					CurrentSpeedTable[item] = 0m;
				}
				foreach (IPEndPoint item2 in CurrentSpeedTable.Keys.ToList())
				{
					Thread thread2 = new Thread(CalculateSpeed);
					thread2.Name = string.Format("tCalculateSpeed[{0}:{1}]", item2.Address, item2.Port);
					thread2.Priority = ThreadPriority.Lowest;
					thread2.IsBackground = true;
					thread2.CurrentCulture = Thread.CurrentThread.CurrentCulture;
					thread2.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
					Thread thread3 = thread2;
					TaskThreadPool.Add(thread3);
					thread3.Start(item2);
				}
				Thread thread4 = new Thread(CalculateSpeed);
				thread4.Name = "tCalculateSpeedTotal";
				thread4.Priority = ThreadPriority.Lowest;
				thread4.IsBackground = true;
				thread4.CurrentCulture = Thread.CurrentThread.CurrentCulture;
				thread4.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
				Thread thread5 = thread4;
				TaskThreadPool.Add(thread5);
				thread5.Start();
				Thread thread6 = new Thread(SendTDT);
				thread6.Name = "Send TDT";
				thread6.Priority = ThreadPriority.Lowest;
				thread6.IsBackground = true;
				thread6.CurrentCulture = Thread.CurrentThread.CurrentCulture;
				thread6.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
				Thread thread7 = thread6;
				TaskThreadPool.Add(thread7);
				thread7.Start();
				Thread thread8 = new Thread(SendTOT);
				thread8.Name = "Send TOT";
				thread8.Priority = ThreadPriority.Lowest;
				thread8.IsBackground = true;
				thread8.CurrentCulture = Thread.CurrentThread.CurrentCulture;
				thread8.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
				Thread thread9 = thread8;
				TaskThreadPool.Add(thread9);
				thread9.Start();
				Thread thread10 = new Thread(SendPAT);
				thread10.Name = "Send PAT";
				thread10.Priority = ThreadPriority.Normal;
				thread10.IsBackground = true;
				thread10.CurrentCulture = Thread.CurrentThread.CurrentCulture;
				thread10.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
				Thread thread11 = thread10;
				TaskThreadPool.Add(thread11);
				thread11.Start();
				Thread thread12 = new Thread(SendPMT);
				thread12.Name = "Send PMT";
				thread12.Priority = ThreadPriority.Normal;
				thread12.IsBackground = true;
				thread12.CurrentCulture = Thread.CurrentThread.CurrentCulture;
				thread12.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
				Thread thread13 = thread12;
				TaskThreadPool.Add(thread13);
				thread13.Start();
				Thread thread14 = new Thread(SendCAT);
				thread14.Name = "Send CAT";
				thread14.Priority = ThreadPriority.AboveNormal;
				thread14.IsBackground = true;
				thread14.CurrentCulture = Thread.CurrentThread.CurrentCulture;
				thread14.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
				Thread thread15 = thread14;
				TaskThreadPool.Add(thread15);
				thread15.Start();
				Thread thread16 = new Thread(SendNIT);
				thread16.Name = "Send NIT";
				thread16.Priority = ThreadPriority.Normal;
				thread16.IsBackground = true;
				thread16.CurrentCulture = Thread.CurrentThread.CurrentCulture;
				thread16.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
				Thread thread17 = thread16;
				TaskThreadPool.Add(thread17);
				thread17.Start();
				Thread thread18 = new Thread(SendSDT_ACT);
				thread18.Name = "Send SDT_ACT";
				thread18.Priority = ThreadPriority.Normal;
				thread18.IsBackground = true;
				thread18.CurrentCulture = Thread.CurrentThread.CurrentCulture;
				thread18.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
				Thread thread19 = thread18;
				TaskThreadPool.Add(thread19);
				thread19.Start();
				Thread thread20 = new Thread(SendSDT_OTH);
				thread20.Name = "Send SDT_OTH";
				thread20.Priority = ThreadPriority.Normal;
				thread20.IsBackground = true;
				thread20.CurrentCulture = Thread.CurrentThread.CurrentCulture;
				thread20.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
				Thread thread21 = thread20;
				TaskThreadPool.Add(thread21);
				thread21.Start();
				Thread thread22 = new Thread(SendBAT);
				thread22.Name = "Send BAT";
				thread22.Priority = ThreadPriority.BelowNormal;
				thread22.IsBackground = true;
				thread22.CurrentCulture = Thread.CurrentThread.CurrentCulture;
				thread22.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
				Thread thread23 = thread22;
				TaskThreadPool.Add(thread23);
				thread23.Start();
				bool flag = false;
				Thread thread24 = new Thread(SendEIT_ACT_PF);
				thread24.Name = "Send EIT_ACT_PF";
				thread24.Priority = ThreadPriority.AboveNormal;
				thread24.IsBackground = true;
				thread24.CurrentCulture = Thread.CurrentThread.CurrentCulture;
				thread24.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
				Thread thread25 = thread24;
				TaskThreadPool.Add(thread25);
				thread25.Start();
				Thread thread26 = new Thread(SendEIT_ACT_SC);
				thread26.Name = "Send EIT_ACT_SC";
				thread26.Priority = ThreadPriority.AboveNormal;
				thread26.IsBackground = true;
				thread26.CurrentCulture = Thread.CurrentThread.CurrentCulture;
				thread26.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
				Thread thread27 = thread26;
				TaskThreadPool.Add(thread27);
				thread27.Start();
				Thread thread28 = new Thread(SendEIT_OTH_PF);
				thread28.Name = "Send EIT_OTH_PF";
				thread28.Priority = ThreadPriority.AboveNormal;
				thread28.IsBackground = true;
				thread28.CurrentCulture = Thread.CurrentThread.CurrentCulture;
				thread28.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
				Thread thread29 = thread28;
				TaskThreadPool.Add(thread29);
				thread29.Start();
				Thread thread30 = new Thread(SendEIT_OTH_SC);
				thread30.Name = "Send EIT_OTH_SC";
				thread30.Priority = ThreadPriority.AboveNormal;
				thread30.IsBackground = true;
				thread30.CurrentCulture = Thread.CurrentThread.CurrentCulture;
				thread30.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
				Thread thread31 = thread30;
				TaskThreadPool.Add(thread31);
				thread31.Start();
				Thread thread32 = new Thread(UpdateEIT_PF);
				thread32.Name = "UpdateEIT_PF";
				thread32.Priority = ThreadPriority.Lowest;
				thread32.IsBackground = true;
				thread32.CurrentCulture = Thread.CurrentThread.CurrentCulture;
				thread32.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
				Thread thread33 = thread32;
				TaskThreadPool.Add(thread33);
				thread33.Start();
				Thread thread34 = new Thread(SmartCheckSendNotice);
				thread34.Name = "tSmartNotice";
				thread34.Priority = ThreadPriority.Lowest;
				thread34.IsBackground = true;
				thread34.CurrentCulture = Thread.CurrentThread.CurrentCulture;
				thread34.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
				Thread thread35 = thread34;
				TaskThreadPool.Add(thread35);
				thread35.Start();
				Thread thread36 = new Thread(CheckServiceSetting);
				thread36.Name = "tCheckSetting";
				thread36.Priority = ThreadPriority.Lowest;
				thread36.IsBackground = true;
				thread36.CurrentCulture = Thread.CurrentThread.CurrentCulture;
				thread36.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
				Thread thread37 = thread36;
				TaskThreadPool.Add(thread37);
				thread37.Start();
				Thread thread38 = new Thread(ReloadeAllDataOnceEachDay);
				thread38.Name = "tEachdayLoader";
				thread38.Priority = ThreadPriority.Lowest;
				thread38.IsBackground = true;
				thread38.CurrentCulture = Thread.CurrentThread.CurrentCulture;
				thread38.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
				Thread thread39 = thread38;
				TaskThreadPool.Add(thread39);
				thread39.Start();
				flag = false;
				AutoUpdateShowtime();
				BalloonInfo balloonInfo2 = new BalloonInfo();
				balloonInfo2.BalloonType = ToolTipIcon.Info;
				balloonInfo2.Content = ResourceManagement.GetLocationString("ServiceStartOK");
				balloonInfo2.Timeout = 5000;
				balloonInfo2.Title = "EPG Service";
				NotifyFormManage.StartShowMessageUI(balloonInfo2);
			}
			catch (Exception ex)
			{
				Tool.WriteTXTLog(ex);
				throw ex;
			}
		}

		public static void StopSendTransprotStream()
		{
			if (!ServiceRunningState)
			{
				return;
			}
			ServiceRunningState = false;
			if (EPGSettings.Instatce.SendTSType == EnumSendType.TSPlayer)
			{
				StreamPlayerManagement.Ins.CloseDevice();
			}
			UdpClientManager.Instance.Dispose();
			if (watcher != null)
			{
				watcher.Dispose();
			}
			int num = TaskThreadPool.Count + EventPool.Count;
			for (int i = 0; i < TaskThreadPool.Count; i++)
			{
				Thread thread = TaskThreadPool[i];
				if (thread != null && thread.ThreadState == ThreadState.Running)
				{
					thread.Abort();
				}
				if (TSOutputManager.ServerStopProcessChanged != null)
				{
					double num2 = ((double)i + 1.0) / (double)num * 100.0;
					if (num2 >= 100.0)
					{
						num2 = 99.0;
					}
					TSOutputManager.ServerStopProcessChanged((int)num2);
				}
			}
			for (int i = 0; i < EventPool.Count; i++)
			{
				EventPool[i] = null;
				if (TSOutputManager.ServerStopProcessChanged != null)
				{
					double num2 = ((double)(i + TaskThreadPool.Count) + 1.0) / (double)num * 100.0;
					if (num2 >= 100.0)
					{
						num2 = 99.0;
					}
					TSOutputManager.ServerStopProcessChanged((int)num2);
				}
			}
			TaskThreadPool.Clear();
			EventPool.Clear();
			TSOutputManager.ServerStopProcessChanged(100);
		}

		public static void SendTSData(IList<byte[]> sendBufferList, IPEndPoint sendPoint)
		{
			switch (EPGSettings.Instatce.SendTSType)
			{
			case EnumSendType.NetworkUDP:
				if (sendPoint == null)
				{
					throw new NullSendOutIPSettingException();
				}
				UdpClientManager.Instance.Send(sendBufferList, sendPoint);
				break;
			case EnumSendType.TSPlayer:
				foreach (byte[] sendBuffer in sendBufferList)
				{
					if (!StreamPlayerManagement.Ins.WriteData(sendBuffer))
					{
						RePrepareTSPlayer();
					}
				}
				break;
			case EnumSendType.NetworkTCP:
				if (sendPoint == null)
				{
					throw new NullSendOutIPSettingException();
				}
				foreach (byte[] sendBuffer2 in sendBufferList)
				{
					TcpClientManager.Instance[sendPoint].SendSocketOnly(sendBuffer2);
				}
				break;
			case EnumSendType.File:
				lock (lockFileObject)
				{
					using (FileStream fileStream = new FileStream("c:\\test.ts", FileMode.Append, FileAccess.Write))
					{
						foreach (byte[] sendBuffer3 in sendBufferList)
						{
							fileStream.Write(sendBuffer3, 0, sendBuffer3.Length);
						}
					}
				}
				break;
			}
			if (!TotalSendBufferSizeTable.ContainsKey(sendPoint))
			{
				TotalSendBufferSizeTable[sendPoint] = 0L;
			}
			long num = sendBufferList.Sum((byte[] x) => x.LongLength);
			TotalSendBufferSizeTable[sendPoint] += num;
		}

		private static void RePrepareTSPlayer()
		{
			lock (lockRePreparTSPlayer)
			{
				if (StreamPlayerManagement.Ins.WriteData(SectionToPackageBLL.PackageFF))
				{
					return;
				}
				int num = 0;
				while (ServiceRunningState)
				{
					StreamPlayerManagement.Ins.CloseDevice();
					Thread.Sleep(1000);
					if (StreamPlayerManagement.Ins.OpenDevice())
					{
						StreamPlayerManagement.Ins.SetRate(EPGSettings.Instatce.TSPlayerRate);
						break;
					}
					Tool.WriteTXTLog(ResourceManagement.GetLocationString("OpenTSPlayerError"));
					BalloonInfo balloonInfo = new BalloonInfo();
					balloonInfo.BalloonType = ToolTipIcon.Warning;
					balloonInfo.Content = ResourceManagement.GetLocationString("OpenTSPlayerError");
					balloonInfo.Timeout = 5000;
					balloonInfo.Title = "EPG Service";
					NotifyFormManage.StartShowMessageUI(balloonInfo);
					Thread.Sleep(++num * 5000);
					if (num > 20)
					{
						num = 0;
					}
				}
			}
		}

		private static void UpdateEIT_PF()
		{
			bool flag = true;
			while (ServiceRunningState)
			{
				if (suspendSendTableThread)
				{
					Thread.Sleep(30000);
					continue;
				}
				Thread.Sleep(60000);
				try
				{
					DateTime DTNow = DateTimeHelper.GetUTCDateTimeNow();
					for (int i = 0; i < TSOutputList.Count; i++)
					{
						TsInfo iTS = TSOutputList[i];
						IList<ProgramInfo> list = ProgramOutputList.Where((ProgramInfo p) => p.TsTableId == iTS.Identity).ToList();
						for (int num = 0; num < list.Count; num++)
						{
							ProgramInfo iProgram = list[num];
							IList<EitInfo> source = EITOutputList.Where((EitInfo p) => p.ProgramTableId == iProgram.Identity).ToList();
							EitInfo eitInfo = source.Where((EitInfo p) => DTNow >= p.StartTime && DTNow < p.EndTime).FirstOrDefault();
							if (((eitInfo == null) ? (-1) : eitInfo.EventId) == EITGeneratorBLL.GetEIT_P_EventID(iProgram.Identity))
							{
								continue;
							}
							if (TSOutputManager.EITPFReloading != null)
							{
								TSOutputManager.EITPFReloading(iProgram.Identity);
							}
							EitInfo eIT = source.Where((EitInfo p) => DTNow < p.StartTime).FirstOrDefault();
							byte eITVersionNumber = EITGeneratorBLL.GetEITVersionNumber(iProgram.Identity);
							IList<byte[]> list2 = new List<byte[]>();
							list2.Add(EITGeneratorBLL.GenerateEITPFSection(eitInfo, true, iProgram, eITVersionNumber));
							list2.Add(EITGeneratorBLL.GenerateEITPFSection(eIT, false, iProgram, eITVersionNumber));
							lock (EIT_ACT_PFSectionListTable)
							{
								if (EIT_ACT_PFSectionListTable.Keys.Contains(iTS.Identity) && EIT_ACT_PFSectionListTable[iTS.Identity].Keys.Contains(iProgram.Identity))
								{
									EIT_ACT_PFSectionListTable[iTS.Identity][iProgram.Identity] = list2;
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					HandellEPGServiceException(ex);
				}
			}
		}

		private static void SendEIT_ACT_PF()
		{
			bool flag = true;
			Dictionary<long, long> prgIdxEIT_ACT_PFTable = new Dictionary<long, long>();
			Dictionary<long, byte> idxPrgEIT_ACT_PFTable = new Dictionary<long, byte>();
			Dictionary<long, Dictionary<long, DateTime>> dictionary = new Dictionary<long, Dictionary<long, DateTime>>();
			EITReloading += delegate
			{
				lock (prgIdxEIT_ACT_PFTable)
				{
					prgIdxEIT_ACT_PFTable.Clear();
				}
				lock (idxPrgEIT_ACT_PFTable)
				{
					idxPrgEIT_ACT_PFTable.Clear();
				}
			};
			EITPFReloading += delegate(long key)
			{
				lock (idxPrgEIT_ACT_PFTable)
				{
					idxPrgEIT_ACT_PFTable.Remove(key);
				}
			};
			Thread thread = new Thread(ClearupSendtimeDictionary);
			thread.Name = "Clearup EIT_ACT_PF sendtime";
			thread.Priority = ThreadPriority.Lowest;
			thread.IsBackground = true;
			thread.CurrentCulture = Thread.CurrentThread.CurrentCulture;
			thread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
			Thread thread2 = thread;
			TaskThreadPool.Add(thread2);
			thread2.Start(dictionary);
			DateTime now = DateTime.Now;
			bool flag2 = true;
			while (ServiceRunningState)
			{
				if (suspendSendTableThread)
				{
					Thread.Sleep(30000);
					continue;
				}
				if (TSOutputList.Count < 1)
				{
					Thread.Sleep(5000);
					continue;
				}
				if (flag2)
				{
					Thread.Sleep(10);
				}
				try
				{
					flag2 = true;
					for (int num = 0; num < TSOutputList.Count; num++)
					{
						long tsKey = TSOutputList[num].Identity;
						SendOrNotInfo sendOrNotInfo = SendOrNotSettingList.SingleOrDefault((SendOrNotInfo p) => p.TsTableId == tsKey);
						if (sendOrNotInfo == null)
						{
							sendOrNotInfo = DefaultSendOrNotSetting;
						}
						SendSpeedInfo sendSpeedInfo = SendSpeedSettingList.SingleOrDefault((SendSpeedInfo p) => p.TsTableId == tsKey);
						if (sendSpeedInfo == null)
						{
							sendSpeedInfo = DefaultSendSpeed;
						}
						ushort tablePID = GetTablePID(EnumTableType.EIT, tsKey);
						lock (EIT_ACT_PFSectionListTable)
						{
							if (sendOrNotInfo.EitActPf != 1 || !EIT_ACT_PFSectionListTable.Keys.Contains(tsKey))
							{
								continue;
							}
							Dictionary<long, IList<byte[]>> dictionary2 = EIT_ACT_PFSectionListTable[tsKey];
							long num2;
							lock (prgIdxEIT_ACT_PFTable)
							{
								num2 = (prgIdxEIT_ACT_PFTable.Keys.Contains(tsKey) ? prgIdxEIT_ACT_PFTable[tsKey] : dictionary2.Keys.First());
							}
							DateTime dateTime;
							lock (dictionary)
							{
								dateTime = ((dictionary.Keys.Contains(tsKey) && dictionary[tsKey].Keys.Contains(num2)) ? dictionary[tsKey][num2] : DateTimeHelper.GetUTCDateTimeNow().Date);
							}
							DateTime uTCDateTimeNow = DateTimeHelper.GetUTCDateTimeNow();
							if (!((uTCDateTimeNow - dateTime).TotalMilliseconds < (double)(sendSpeedInfo.EitActPf / dictionary2[num2].Count)))
							{
								flag2 = false;
								SetRecordValue(dictionary, tsKey, num2, uTCDateTimeNow);
								SetRecordValue(prgIdxEIT_ACT_PFTable, tsKey, dictionary2.Keys.ToList(), num2);
								byte b;
								lock (idxPrgEIT_ACT_PFTable)
								{
									b = (byte)(idxPrgEIT_ACT_PFTable.Keys.Contains(num2) ? idxPrgEIT_ACT_PFTable[num2] : 0);
								}
								SetRecordValueIndex(idxPrgEIT_ACT_PFTable, num2, dictionary2[num2].Count, b);
								PackageGenerators.Instance[tsKey, tablePID].SendOutSectionData(dictionary2[num2][b], tablePID, GetSendIPEndPoint(tsKey));
							}
						}
					}
				}
				catch (Exception ex)
				{
					HandellEPGServiceException(ex);
				}
			}
		}

		private static void SendEIT_ACT_SC()
		{
			bool flag = true;
			Dictionary<long, long> prgIdxEIT_ACT_SCTable = new Dictionary<long, long>();
			Dictionary<long, byte> idxPrgEIT_ACT_SCTable = new Dictionary<long, byte>();
			Dictionary<long, Dictionary<long, DateTime>> dictionary = new Dictionary<long, Dictionary<long, DateTime>>();
			EITReloading += delegate
			{
				lock (prgIdxEIT_ACT_SCTable)
				{
					prgIdxEIT_ACT_SCTable.Clear();
				}
				lock (idxPrgEIT_ACT_SCTable)
				{
					idxPrgEIT_ACT_SCTable.Clear();
				}
			};
			Thread thread = new Thread(ClearupSendtimeDictionary);
			thread.Name = "Clearup EIT_ACT_SC sendtime";
			thread.Priority = ThreadPriority.Lowest;
			thread.IsBackground = true;
			thread.CurrentCulture = Thread.CurrentThread.CurrentCulture;
			thread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
			Thread thread2 = thread;
			TaskThreadPool.Add(thread2);
			thread2.Start(dictionary);
			DateTime now = DateTime.Now;
			bool flag2 = true;
			while (ServiceRunningState)
			{
				if (suspendSendTableThread)
				{
					Thread.Sleep(30000);
					continue;
				}
				if (EIT_ACT_SCSectionListTable.Count < 1)
				{
					Thread.Sleep(5000);
					continue;
				}
				if (flag2)
				{
					Thread.Sleep(10);
				}
				try
				{
					flag2 = true;
					for (int num = 0; num < TSOutputList.Count; num++)
					{
						long tsKey = TSOutputList[num].Identity;
						SendOrNotInfo sendOrNotInfo = SendOrNotSettingList.SingleOrDefault((SendOrNotInfo p) => p.TsTableId == tsKey);
						if (sendOrNotInfo == null)
						{
							sendOrNotInfo = DefaultSendOrNotSetting;
						}
						SendSpeedInfo sendSpeedInfo = SendSpeedSettingList.SingleOrDefault((SendSpeedInfo p) => p.TsTableId == tsKey);
						if (sendSpeedInfo == null)
						{
							sendSpeedInfo = DefaultSendSpeed;
						}
						ushort tablePID = GetTablePID(EnumTableType.EIT, tsKey);
						lock (EIT_ACT_SCSectionListTable)
						{
							if (sendOrNotInfo.EitActSc != 1 || !EIT_ACT_SCSectionListTable.Keys.Contains(tsKey))
							{
								continue;
							}
							Dictionary<long, IList<byte[]>> dictionary2 = EIT_ACT_SCSectionListTable[tsKey];
							long num2;
							lock (prgIdxEIT_ACT_SCTable)
							{
								num2 = (prgIdxEIT_ACT_SCTable.Keys.Contains(tsKey) ? prgIdxEIT_ACT_SCTable[tsKey] : dictionary2.Keys.First());
							}
							DateTime dateTime;
							lock (dictionary)
							{
								dateTime = ((dictionary.Keys.Contains(tsKey) && dictionary[tsKey].Keys.Contains(num2)) ? dictionary[tsKey][num2] : DateTimeHelper.GetUTCDateTimeNow().Date);
							}
							DateTime uTCDateTimeNow = DateTimeHelper.GetUTCDateTimeNow();
							if (!((uTCDateTimeNow - dateTime).TotalMilliseconds < (double)(sendSpeedInfo.EitActSc / dictionary2[num2].Count)))
							{
								flag2 = false;
								SetRecordValue(dictionary, tsKey, num2, uTCDateTimeNow);
								SetRecordValue(prgIdxEIT_ACT_SCTable, tsKey, dictionary2.Keys.ToList(), num2);
								byte b;
								lock (idxPrgEIT_ACT_SCTable)
								{
									b = (byte)(idxPrgEIT_ACT_SCTable.Keys.Contains(num2) ? idxPrgEIT_ACT_SCTable[num2] : 0);
								}
								SetRecordValueIndex(idxPrgEIT_ACT_SCTable, num2, dictionary2[num2].Count, b);
								PackageGenerators.Instance[tsKey, tablePID].SendOutSectionData(dictionary2[num2][b], tablePID, GetSendIPEndPoint(tsKey));
							}
						}
					}
				}
				catch (Exception ex)
				{
					HandellEPGServiceException(ex);
				}
			}
		}

		private static void SendEIT_OTH_PF()
		{
			bool flag = true;
			Dictionary<long, long> tsKeyToTsSendKey_OTH_PFTable = new Dictionary<long, long>();
			Dictionary<long, Dictionary<long, long>> prgIdxEIT_OTH_PFTable = new Dictionary<long, Dictionary<long, long>>();
			Dictionary<long, Dictionary<long, Dictionary<long, byte>>> idxPrgEIT_OTH_PFTable = new Dictionary<long, Dictionary<long, Dictionary<long, byte>>>();
			Dictionary<long, Dictionary<long, DateTime>> dictionary = new Dictionary<long, Dictionary<long, DateTime>>();
			EITReloading += delegate
			{
				lock (tsKeyToTsSendKey_OTH_PFTable)
				{
					tsKeyToTsSendKey_OTH_PFTable.Clear();
				}
				lock (prgIdxEIT_OTH_PFTable)
				{
					prgIdxEIT_OTH_PFTable.Clear();
				}
				lock (idxPrgEIT_OTH_PFTable)
				{
					idxPrgEIT_OTH_PFTable.Clear();
				}
			};
			EITPFReloading += delegate(long key)
			{
				lock (idxPrgEIT_OTH_PFTable)
				{
					idxPrgEIT_OTH_PFTable.Remove(key);
				}
			};
			Thread thread = new Thread(ClearupSendtimeDictionary);
			thread.Name = "Clearup EIT_OTH_PF sendtime";
			thread.Priority = ThreadPriority.Lowest;
			thread.IsBackground = true;
			thread.CurrentCulture = Thread.CurrentThread.CurrentCulture;
			thread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
			Thread thread2 = thread;
			TaskThreadPool.Add(thread2);
			thread2.Start(dictionary);
			DateTime now = DateTime.Now;
			bool flag2 = false;
			Dictionary<long, Dictionary<long, IList<byte[]>>> eIT_ACT_PFSectionListTable = default(Dictionary<long, Dictionary<long, IList<byte[]>>>);
			while (ServiceRunningState)
			{
				if (suspendSendTableThread)
				{
					Thread.Sleep(30000);
					continue;
				}
				if (TSOutputList.Count < 2)
				{
					Thread.Sleep(5000);
					continue;
				}
				if (flag2)
				{
					Thread.Sleep(10);
				}
				try
				{
					flag2 = true;
					for (int num = 0; num < TSOutputList.Count; num++)
					{
						long tsKey = TSOutputList[num].Identity;
						SendOrNotInfo sendOrNotInfo = SendOrNotSettingList.SingleOrDefault((SendOrNotInfo p) => p.TsTableId == tsKey);
						if (sendOrNotInfo == null)
						{
							sendOrNotInfo = DefaultSendOrNotSetting;
						}
						ushort tablePID = GetTablePID(EnumTableType.EIT, tsKey);
						bool lockTaken = false;
						try
						{
							Monitor.Enter(eIT_ACT_PFSectionListTable = EIT_ACT_PFSectionListTable, ref lockTaken);
							if (sendOrNotInfo.EitOthPf != 1 || EIT_ACT_PFSectionListTable.Count < 2)
							{
								continue;
							}
							TsInfo tsSend = TSOutputList.Single((TsInfo x) => x.Identity == tsKey);
							IList<long> list = (from x in TSOutputList
								where x.Identity != tsKey && x.NitTableId == tsSend.NitTableId
								select x.Identity).ToList();
							if (list.Count == 0)
							{
								continue;
							}
							SendSpeedInfo sendSpeedInfo = SendSpeedSettingList.SingleOrDefault((SendSpeedInfo p) => p.TsTableId == tsKey);
							if (sendSpeedInfo == null)
							{
								sendSpeedInfo = DefaultSendSpeed;
							}
							long num2;
							lock (tsKeyToTsSendKey_OTH_PFTable)
							{
								num2 = (tsKeyToTsSendKey_OTH_PFTable.Keys.Contains(tsKey) ? tsKeyToTsSendKey_OTH_PFTable[tsKey] : list.First());
							}
							if (!EIT_ACT_PFSectionListTable.Keys.Contains(num2))
							{
								continue;
							}
							Dictionary<long, IList<byte[]>> dictionary2 = EIT_ACT_PFSectionListTable[num2];
							long num3;
							lock (prgIdxEIT_OTH_PFTable)
							{
								num3 = ((prgIdxEIT_OTH_PFTable.Keys.Contains(tsKey) && prgIdxEIT_OTH_PFTable[tsKey].Keys.Contains(num2)) ? prgIdxEIT_OTH_PFTable[tsKey][num2] : dictionary2.Keys.First());
							}
							DateTime dateTime;
							lock (dictionary)
							{
								dateTime = ((dictionary.Keys.Contains(tsKey) && dictionary[tsKey].Keys.Contains(num3)) ? dictionary[tsKey][num3] : DateTimeHelper.GetUTCDateTimeNow().Date);
							}
							DateTime uTCDateTimeNow = DateTimeHelper.GetUTCDateTimeNow();
							TimeSpan timeSpan = uTCDateTimeNow - dateTime;
							double num4 = sendSpeedInfo.EitOthPf / dictionary2[num3].Count;
							if (!(timeSpan.TotalMilliseconds < num4))
							{
								flag2 = false;
								SetRecordValue(dictionary, tsKey, num3, uTCDateTimeNow);
								SetRecordValue(prgIdxEIT_OTH_PFTable, tsKey, num2, dictionary2.Keys.ToList(), num3);
								SetRecordValue(tsKeyToTsSendKey_OTH_PFTable, tsKey, list, num2);
								byte b;
								lock (idxPrgEIT_OTH_PFTable)
								{
									b = (byte)((idxPrgEIT_OTH_PFTable.Keys.Contains(tsKey) && idxPrgEIT_OTH_PFTable[tsKey].Keys.Contains(num2) && idxPrgEIT_OTH_PFTable[tsKey][num2].Keys.Contains(num3)) ? idxPrgEIT_OTH_PFTable[tsKey][num2][num3] : 0);
								}
								SetRecordValueIndex(idxPrgEIT_OTH_PFTable, tsKey, num2, num3, dictionary2[num3].Count, b);
								PackageGenerators.Instance[tsKey, tablePID].SendOutSectionData(EIT_ACTSectionToEIT_OTHSecttion(dictionary2[num3][b]), tablePID, GetSendIPEndPoint(tsKey));
							}
						}
						finally
						{
							if (lockTaken)
							{
								Monitor.Exit(eIT_ACT_PFSectionListTable);
							}
						}
					}
				}
				catch (Exception ex)
				{
					HandellEPGServiceException(ex);
				}
			}
		}

		private static void SendEIT_OTH_SC()
		{
			bool flag = true;
			Dictionary<long, long> tsKeyToTsSendKey_OTH_SCTable = new Dictionary<long, long>();
			Dictionary<long, Dictionary<long, long>> prgIdxEIT_OTH_SCTable = new Dictionary<long, Dictionary<long, long>>();
			Dictionary<long, Dictionary<long, Dictionary<long, byte>>> idxPrgEIT_OTH_SCTable = new Dictionary<long, Dictionary<long, Dictionary<long, byte>>>();
			Dictionary<long, Dictionary<long, DateTime>> dictionary = new Dictionary<long, Dictionary<long, DateTime>>();
			EITReloading += delegate
			{
				lock (tsKeyToTsSendKey_OTH_SCTable)
				{
					tsKeyToTsSendKey_OTH_SCTable.Clear();
				}
				lock (prgIdxEIT_OTH_SCTable)
				{
					prgIdxEIT_OTH_SCTable.Clear();
				}
				lock (idxPrgEIT_OTH_SCTable)
				{
					idxPrgEIT_OTH_SCTable.Clear();
				}
			};
			Thread thread = new Thread(ClearupSendtimeDictionary);
			thread.Name = "Clearup EIT_OTH_SC sendtime";
			thread.Priority = ThreadPriority.Lowest;
			thread.IsBackground = true;
			thread.CurrentCulture = Thread.CurrentThread.CurrentCulture;
			thread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
			Thread thread2 = thread;
			TaskThreadPool.Add(thread2);
			thread2.Start(dictionary);
			DateTime now = DateTime.Now;
			bool flag2 = false;
			Dictionary<long, Dictionary<long, IList<byte[]>>> eIT_ACT_SCSectionListTable = default(Dictionary<long, Dictionary<long, IList<byte[]>>>);
			while (ServiceRunningState)
			{
				if (suspendSendTableThread)
				{
					Thread.Sleep(30000);
					continue;
				}
				if (TSOutputList.Count < 2)
				{
					Thread.Sleep(5000);
					continue;
				}
				if (flag2)
				{
					Thread.Sleep(10);
				}
				try
				{
					flag2 = true;
					for (int num = 0; num < TSOutputList.Count; num++)
					{
						long tsKey = TSOutputList[num].Identity;
						SendOrNotInfo sendOrNotInfo = SendOrNotSettingList.SingleOrDefault((SendOrNotInfo p) => p.TsTableId == tsKey);
						if (sendOrNotInfo == null)
						{
							sendOrNotInfo = DefaultSendOrNotSetting;
						}
						SendSpeedInfo sendSpeedInfo = SendSpeedSettingList.SingleOrDefault((SendSpeedInfo p) => p.TsTableId == tsKey);
						if (sendSpeedInfo == null)
						{
							sendSpeedInfo = DefaultSendSpeed;
						}
						ushort tablePID = GetTablePID(EnumTableType.EIT, tsKey);
						bool lockTaken = false;
						try
						{
							Monitor.Enter(eIT_ACT_SCSectionListTable = EIT_ACT_SCSectionListTable, ref lockTaken);
							if (sendOrNotInfo.EitOthSc != 1 || EIT_ACT_SCSectionListTable.Count < 2)
							{
								continue;
							}
							TsInfo tsSend = TSOutputList.Single((TsInfo x) => x.Identity == tsKey);
							IList<long> list = (from x in TSOutputList
								where x.Identity != tsKey && x.NitTableId == tsSend.NitTableId
								select x.Identity).ToList();
							if (list.Count == 0)
							{
								continue;
							}
							long num2;
							lock (tsKeyToTsSendKey_OTH_SCTable)
							{
								num2 = (tsKeyToTsSendKey_OTH_SCTable.Keys.Contains(tsKey) ? tsKeyToTsSendKey_OTH_SCTable[tsKey] : list.First());
							}
							if (!EIT_ACT_SCSectionListTable.Keys.Contains(num2))
							{
								continue;
							}
							Dictionary<long, IList<byte[]>> dictionary2 = EIT_ACT_SCSectionListTable[num2];
							long num3;
							lock (prgIdxEIT_OTH_SCTable)
							{
								num3 = ((prgIdxEIT_OTH_SCTable.Keys.Contains(tsKey) && prgIdxEIT_OTH_SCTable[tsKey].Keys.Contains(num2)) ? prgIdxEIT_OTH_SCTable[tsKey][num2] : dictionary2.Keys.First());
							}
							DateTime dateTime;
							lock (dictionary)
							{
								dateTime = ((dictionary.Keys.Contains(tsKey) && dictionary[tsKey].Keys.Contains(num3)) ? dictionary[tsKey][num3] : DateTimeHelper.GetUTCDateTimeNow().Date);
							}
							DateTime uTCDateTimeNow = DateTimeHelper.GetUTCDateTimeNow();
							if (!((uTCDateTimeNow - dateTime).TotalMilliseconds < (double)(sendSpeedInfo.EitOthSc / dictionary2[num3].Count)))
							{
								flag2 = false;
								SetRecordValue(dictionary, tsKey, num3, uTCDateTimeNow);
								SetRecordValue(prgIdxEIT_OTH_SCTable, tsKey, num2, dictionary2.Keys.ToList(), num3);
								SetRecordValue(tsKeyToTsSendKey_OTH_SCTable, tsKey, list, num2);
								byte b;
								lock (idxPrgEIT_OTH_SCTable)
								{
									b = (byte)((idxPrgEIT_OTH_SCTable.Keys.Contains(tsKey) && idxPrgEIT_OTH_SCTable[tsKey].Keys.Contains(num2) && idxPrgEIT_OTH_SCTable[tsKey][num2].Keys.Contains(num3)) ? idxPrgEIT_OTH_SCTable[tsKey][num2][num3] : 0);
								}
								SetRecordValueIndex(idxPrgEIT_OTH_SCTable, tsKey, num2, num3, dictionary2[num3].Count, b);
								PackageGenerators.Instance[tsKey, tablePID].SendOutSectionData(EIT_ACTSectionToEIT_OTHSecttion(dictionary2[num3][b]), tablePID, GetSendIPEndPoint(tsKey));
							}
						}
						finally
						{
							if (lockTaken)
							{
								Monitor.Exit(eIT_ACT_SCSectionListTable);
							}
						}
					}
				}
				catch (Exception ex)
				{
					HandellEPGServiceException(ex);
				}
			}
		}

		private static byte[] EIT_ACTSectionToEIT_OTHSecttion(byte[] sectionBuffer)
		{
			byte[] array = (byte[])sectionBuffer.Clone();
			byte b = array[0];
			if (b == 78)
			{
				array[0] = 79;
			}
			else
			{
				if (b < 80 || b > 95)
				{
					throw new Exception("not set table_id");
				}
				array[0] = (byte)(b + 16);
			}
			CRCGeneratorBLL.SetSectionCRC(array);
			return array;
		}

		private static void CheckServiceSetting()
		{
			while (ServiceRunningState)
			{
				try
				{
					if (!EPGRegistor.CheckRegistServer())
					{
						throw new NullOrErrorAccreditException();
					}
					RegistInfo registS = EPGRegistor.GetRegistS();
					if (registS.TrialVersion && ProgramOutputList.Count > 10)
					{
						throw new VersionLimitedException();
					}
					if (!HTBDogBll.CheckHTBDogDesc(registS.TrialVersion))
					{
						throw new HTBDogErrorException();
					}
					TsInfo[] array = TSOutputList.Where((TsInfo p) => !OutputIPSettingList.Select((MultipleipSettings multipleipSettings) => multipleipSettings.TsTableId).Contains(p.Identity)).ToArray();
					TsInfo[] array2 = array.Where((TsInfo p) => !PIDRemappingSettingList.Select((PidOutputInfo pidOutputInfo) => pidOutputInfo.TsTableId).Contains(p.Identity)).ToArray();
					TsInfo[] array3 = TSOutputList.Where((TsInfo p) => !PIDRemappingSettingList.Select((PidOutputInfo x) => x.TsTableId).Contains(p.Identity)).ToArray();
					switch (EPGSettings.Instatce.SendTSType)
					{
					case EnumSendType.NetworkUDP:
					case EnumSendType.NetworkTCP:
						if (SingleIPSetting == null)
						{
							if (array.Length > 0)
							{
								throw new NullSendOutIPSettingException();
							}
						}
						else if (array2.Length > 1)
						{
							throw new NeedMorePIDRemappingException();
						}
						break;
					case EnumSendType.TSPlayer:
					case EnumSendType.File:
						if (array3.Length > 1)
						{
							throw new NeedMorePIDRemappingException();
						}
						break;
					}
					if (suspendSendTableThread)
					{
						Tool.WriteTXTLog(string.Format("Warning : {0}", ResourceManagement.GetLocationString("TaskResume")));
						BalloonInfo balloonInfo = new BalloonInfo();
						balloonInfo.BalloonType = ToolTipIcon.Info;
						balloonInfo.Content = ResourceManagement.GetLocationString("TaskResume");
						balloonInfo.Timeout = 5000;
						balloonInfo.Title = "EPG Service";
						NotifyFormManage.StartShowMessageUI(balloonInfo);
						suspendSendTableThread = false;
					}
				}
				catch (Exception ex)
				{
					HandellEPGServiceException(ex);
				}
				finally
				{
					GC.Collect();
					Thread.Sleep(30000);
				}
			}
		}

		private static void ReloadeAllDataOnceEachDay()
		{
			while (ServiceRunningState)
			{
				try
				{
					if (DateTime.Now >= DateTime.Now.Date && DateTime.Now < DateTime.Now.Date.AddMilliseconds(30000.0))
					{
						BalloonInfo balloonInfo = new BalloonInfo();
						balloonInfo.BalloonType = ToolTipIcon.Info;
						balloonInfo.Content = "Service is reloading database for epg";
						balloonInfo.Timeout = 5000;
						balloonInfo.Title = "EPG Service";
						NotifyFormManage.StartShowMessageUI(balloonInfo);
						GenerateAllSendTable(true);
						BalloonInfo balloonInfo2 = new BalloonInfo();
						balloonInfo2.BalloonType = ToolTipIcon.Info;
						balloonInfo2.Content = "Service completed database for epg";
						balloonInfo2.Timeout = 5000;
						balloonInfo2.Title = "EPG Service";
						NotifyFormManage.StartShowMessageUI(balloonInfo2);
						Thread.Sleep(82800000);
					}
					else
					{
						Thread.Sleep(30000);
					}
				}
				catch (Exception ex)
				{
					HandellEPGServiceException(ex);
				}
			}
		}
	}
}
