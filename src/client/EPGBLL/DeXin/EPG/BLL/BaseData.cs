using System;
using System.Collections.Generic;
using System.Linq;
using Com.Cn.Tool.Model;
using DeXin.EPG.Model;
using DeXin.EPG.Tools;

namespace DeXin.EPG.BLL
{
	public class BaseData
	{
		private DescriptorTypeManager desTypeManager = new DescriptorTypeManager();

		private SystemSettingBLL sysManager = new SystemSettingBLL();

		private static BaseData _Ins = null;

		private static IList<SysFunction> _FunctionItems = null;

		private IList<BaseKey> _ServiceTypes = null;

		private IList<BaseKey> _Polarizations = null;

		private IList<BaseKey> _InnerFEC = null;

		private IList<BaseKey> _OuterFEC = null;

		private IList<BaseKey> _ModulationSchemeForSatellites = null;

		private IList<BaseKey> _ModulationSchemeForCable = null;

		private IList<BaseKey> _Bandwidth = null;

		private IList<BaseKey> _Constellation = null;

		private IList<BaseKey> _Hierarchy_information = null;

		private IList<BaseKey> _CodeRate = null;

		private IList<BaseKey> _GuardInterval = null;

		private IList<BaseKey> _TransmissionMode = null;

		public static BaseData Ins
		{
			get
			{
				if (_Ins == null)
				{
					_Ins = new BaseData();
				}
				return _Ins;
			}
		}

		public IList<SysFunction> FunctionItems
		{
			get
			{
				if (_FunctionItems == null)
				{
					_FunctionItems = new List<SysFunction>();
					foreach (EnumFunctionName value in Enum.GetValues(typeof(EnumFunctionName)))
					{
						SysFunction sysFunction = new SysFunction();
						sysFunction.FunctionKeyEnum = value;
						_FunctionItems.Add(sysFunction);
					}
				}
				return _FunctionItems;
			}
		}

		public IList<BaseKey> ServiceTypes
		{
			get
			{
				if (_ServiceTypes == null)
				{
					_ServiceTypes = new List<BaseKey>();
					_ServiceTypes.Add(new BaseKey("01", "digital television service", true));
					_ServiceTypes.Add(new BaseKey("02", "digital radio sound service", true));
					_ServiceTypes.Add(new BaseKey("03", "Teletext service", true));
					_ServiceTypes.Add(new BaseKey("04", "NVOD reference service", true));
					_ServiceTypes.Add(new BaseKey("05", "NVOD time-shifted service", true));
					_ServiceTypes.Add(new BaseKey("06", "mosaic service", true));
					_ServiceTypes.Add(new BaseKey("07", "PAL coded signal", true));
					_ServiceTypes.Add(new BaseKey("08", "SECAM coded signal", true));
					_ServiceTypes.Add(new BaseKey("09", "D/D2-MAC", true));
					_ServiceTypes.Add(new BaseKey("0A", "FM Radio", true));
					_ServiceTypes.Add(new BaseKey("0B", "NTSC coded signal", true));
					_ServiceTypes.Add(new BaseKey("0C", "data broadcast service", true));
					_ServiceTypes.Add(new BaseKey("0D", "reserved for Common Interface Usage (EN 50221 [16])", true));
					_ServiceTypes.Add(new BaseKey("0E", "RCS Map (see EN 301 790 [34])", true));
					_ServiceTypes.Add(new BaseKey("0F", "RCS FLS (see EN 301 790 [34])", true));
					_ServiceTypes.Add(new BaseKey("10", "DVB MHP service", true));
				}
				return _ServiceTypes;
			}
		}

		public IList<BaseKey> Polarizations
		{
			get
			{
				if (_Polarizations == null)
				{
					_Polarizations = new List<BaseKey>();
					_Polarizations.Add(new BaseKey("00", "linear - horizontal", true));
					_Polarizations.Add(new BaseKey("01", "linear - vertical", true));
					_Polarizations.Add(new BaseKey("02", "linear - horizontal", true));
					_Polarizations.Add(new BaseKey("03", "linear - horizontal", true));
				}
				return _Polarizations;
			}
		}

		public IList<BaseKey> InnerFEC
		{
			get
			{
				if (_InnerFEC == null)
				{
					_InnerFEC = new List<BaseKey>();
					_InnerFEC.Add(new BaseKey("00", "not defined", true));
					_InnerFEC.Add(new BaseKey("01", "1/2 conv. code rate", true));
					_InnerFEC.Add(new BaseKey("02", "2/3 conv. code rate", true));
					_InnerFEC.Add(new BaseKey("03", "3/4 conv. code rate", true));
					_InnerFEC.Add(new BaseKey("04", "5/6 conv. code rate", true));
					_InnerFEC.Add(new BaseKey("05", "7/8 conv. code rate", true));
					_InnerFEC.Add(new BaseKey("06", "8/9 conv. code rate", true));
					_InnerFEC.Add(new BaseKey("0F", "no conv. coding", true));
				}
				return _InnerFEC;
			}
		}

		public IList<BaseKey> OuterFEC
		{
			get
			{
				if (_OuterFEC == null)
				{
					_OuterFEC = new List<BaseKey>();
					_OuterFEC.Add(new BaseKey("00", "not defined", true));
					_OuterFEC.Add(new BaseKey("01", "no outer FEC coding", true));
					_OuterFEC.Add(new BaseKey("02", "RS(204/188)", true));
				}
				return _OuterFEC;
			}
		}

		public IList<BaseKey> ModulationSchemeForSatellite
		{
			get
			{
				_ModulationSchemeForSatellites = new List<BaseKey>();
				_ModulationSchemeForSatellites.Add(new BaseKey("00", "not defined", true));
				_ModulationSchemeForSatellites.Add(new BaseKey("01", "QPSK", true));
				_ModulationSchemeForSatellites.Add(new BaseKey("02", "8PSK", true));
				_ModulationSchemeForSatellites.Add(new BaseKey("03", "16-QAM", true));
				return _ModulationSchemeForSatellites;
			}
		}

		public IList<BaseKey> ModulationSchemeForCable
		{
			get
			{
				_ModulationSchemeForCable = new List<BaseKey>();
				_ModulationSchemeForCable.Add(new BaseKey("00", "not defined", true));
				_ModulationSchemeForCable.Add(new BaseKey("01", "16-QAM", true));
				_ModulationSchemeForCable.Add(new BaseKey("02", "32-QAM", true));
				_ModulationSchemeForCable.Add(new BaseKey("03", "64-QAM", true));
				_ModulationSchemeForCable.Add(new BaseKey("04", "128-QAM", true));
				_ModulationSchemeForCable.Add(new BaseKey("05", "256-QAM", true));
				return _ModulationSchemeForCable;
			}
		}

		public IList<BaseKey> Bandwidth
		{
			get
			{
				_Bandwidth = new List<BaseKey>();
				_Bandwidth.Add(new BaseKey("00", "8 MHz", true));
				_Bandwidth.Add(new BaseKey("01", "7 MHz", true));
				_Bandwidth.Add(new BaseKey("02", "6 MHz", true));
				_Bandwidth.Add(new BaseKey("03", "5 MHz", true));
				return _Bandwidth;
			}
		}

		public IList<BaseKey> Constellation
		{
			get
			{
				_Constellation = new List<BaseKey>();
				_Constellation.Add(new BaseKey("00", "QPSK", true));
				_Constellation.Add(new BaseKey("01", "16 QAM", true));
				_Constellation.Add(new BaseKey("02", "64 QAM", true));
				_Constellation.Add(new BaseKey("03", "reserved for future use ", true));
				return _Constellation;
			}
		}

		public IList<BaseKey> Hierarchy_information
		{
			get
			{
				_Hierarchy_information = new List<BaseKey>();
				_Hierarchy_information.Add(new BaseKey("00", "non-hierarchical, native interleaver", true));
				_Hierarchy_information.Add(new BaseKey("01", "α = 1, native interleaver", true));
				_Hierarchy_information.Add(new BaseKey("02", "α = 2, native interleaver", true));
				_Hierarchy_information.Add(new BaseKey("03", "α = 4, native interleaver", true));
				_Hierarchy_information.Add(new BaseKey("04", "non-hierarchical, in-depth interleaver", true));
				_Hierarchy_information.Add(new BaseKey("05", "α = 1, in-depth interleaver", true));
				_Hierarchy_information.Add(new BaseKey("06", "α = 2, in-depth interleaver", true));
				_Hierarchy_information.Add(new BaseKey("07", "α = 4, in-depth interleaver", true));
				return _Hierarchy_information;
			}
		}

		public IList<BaseKey> CodeRate
		{
			get
			{
				_CodeRate = new List<BaseKey>();
				_CodeRate.Add(new BaseKey("00", "1/2", true));
				_CodeRate.Add(new BaseKey("01", "2/3", true));
				_CodeRate.Add(new BaseKey("02", "3/4", true));
				_CodeRate.Add(new BaseKey("03", "5/6", true));
				_CodeRate.Add(new BaseKey("04", "7/8", true));
				return _CodeRate;
			}
		}

		public IList<BaseKey> GuardInterval
		{
			get
			{
				_GuardInterval = new List<BaseKey>();
				_GuardInterval.Add(new BaseKey("00", "1/32", true));
				_GuardInterval.Add(new BaseKey("01", "2/16", true));
				_GuardInterval.Add(new BaseKey("02", "1/8", true));
				_GuardInterval.Add(new BaseKey("03", "1/4", true));
				return _GuardInterval;
			}
		}

		public IList<BaseKey> TransmissionMode
		{
			get
			{
				_TransmissionMode = new List<BaseKey>();
				_TransmissionMode.Add(new BaseKey("00", "2k mode", true));
				_TransmissionMode.Add(new BaseKey("01", "8k mode", true));
				_TransmissionMode.Add(new BaseKey("02", "4k mode", true));
				_TransmissionMode.Add(new BaseKey("03", "reserved for future use", true));
				return _TransmissionMode;
			}
		}

		public IList<BaseKey> ContentNibbleLevel1
		{
			get
			{
				IList<BaseKey> list = new List<BaseKey>();
				list.Add(new BaseKey("00", "", true));
				return list;
			}
		}

		public IList<BaseKey> LogicalCellPresentationInfo
		{
			get
			{
				IList<BaseKey> list = new List<BaseKey>();
				list.Add(new BaseKey("00", "undefined", true));
				list.Add(new BaseKey("01", "video", true));
				list.Add(new BaseKey("02", "still picture (see note)", true));
				list.Add(new BaseKey("03", "graphics/text", true));
				return list;
			}
		}

		public IList<BaseKey> CellLinkageInfo
		{
			get
			{
				IList<BaseKey> list = new List<BaseKey>();
				list.Add(new BaseKey("00", "undefined", true));
				list.Add(new BaseKey("01", "bouquet related", true));
				list.Add(new BaseKey("02", "service related", true));
				list.Add(new BaseKey("03", "other mosaic related", true));
				list.Add(new BaseKey("04", "event related", true));
				return list;
			}
		}

		public IList<BaseKey> NumberOfElementaryEClls
		{
			get
			{
				IList<BaseKey> list = new List<BaseKey>();
				list.Add(new BaseKey("00", "one cells", true));
				list.Add(new BaseKey("01", "two cells", true));
				list.Add(new BaseKey("02", "three cells", true));
				list.Add(new BaseKey("03", "four cells", true));
				list.Add(new BaseKey("04", "five cells", true));
				list.Add(new BaseKey("05", "six cells", true));
				list.Add(new BaseKey("06", "seven cells", true));
				list.Add(new BaseKey("07", "eight cells", true));
				return list;
			}
		}

		public LcnType lcntype
		{
			get
			{
				SystemSetting systemSetting = sysManager.GetSystemSetting(SystemSettingCode.LCNLENGTH);
				if (systemSetting == null)
				{
					return LcnType.BIT_10;
				}
				return TypeManagent<LcnType>.GetEnum(systemSetting.Value);
			}
		}

		public IList<BaseKey> SmoothingBufferSize
		{
			get
			{
				IList<BaseKey> list = new List<BaseKey>();
				list.Add(new BaseKey("00", "DVB_reserved", true));
				list.Add(new BaseKey("01", "1 536", true));
				list.Add(new BaseKey("02", "DVB_reserved", true));
				list.Add(new BaseKey("03", "DVB_reserved", true));
				return list;
			}
		}

		public IList<BaseKey> SmoothingBufferLeakRate
		{
			get
			{
				IList<BaseKey> list = new List<BaseKey>();
				list.Add(new BaseKey("00", "DVB_reserved", true));
				list.Add(new BaseKey("01", "0,0009", true));
				list.Add(new BaseKey("02", "0,0018", true));
				list.Add(new BaseKey("03", "0,0036", true));
				list.Add(new BaseKey("04", "0,0072", true));
				list.Add(new BaseKey("05", "0,0108", true));
				list.Add(new BaseKey("06", "0,0144", true));
				list.Add(new BaseKey("07", "0,0216", true));
				list.Add(new BaseKey("08", "0,0288", true));
				list.Add(new BaseKey("09", "0,0075", true));
				list.Add(new BaseKey("10", "0,5", true));
				list.Add(new BaseKey("11", "0,5625", true));
				list.Add(new BaseKey("12", "0,8437", true));
				list.Add(new BaseKey("13", "1,0", true));
				list.Add(new BaseKey("14", "1,1250", true));
				list.Add(new BaseKey("15", "1,5", true));
				list.Add(new BaseKey("16", "1,6875", true));
				list.Add(new BaseKey("17", "2,0", true));
				list.Add(new BaseKey("18", "2,2500", true));
				list.Add(new BaseKey("19", "2,5", true));
				list.Add(new BaseKey("20", "3,0", true));
				list.Add(new BaseKey("21", "3,3,3750", true));
				list.Add(new BaseKey("22", "3,5", true));
				list.Add(new BaseKey("23", "4,0", true));
				list.Add(new BaseKey("24", "4,5", true));
				list.Add(new BaseKey("25", "5,0", true));
				list.Add(new BaseKey("26", "5,5", true));
				list.Add(new BaseKey("27", "6,0", true));
				list.Add(new BaseKey("28", "6,5", true));
				list.Add(new BaseKey("29", "6,7500", true));
				list.Add(new BaseKey("30", "((value) - 16) × 0,5 (7,0 Mbit/s, 7,5 Mbit/s, 8,0 Mbit/s)", true));
				list.Add(new BaseKey("31", "((value) - 16) × 0,5 (7,0 Mbit/s, 7,5 Mbit/s, 8,0 Mbit/s)", true));
				list.Add(new BaseKey("32", "((value) - 16) × 0,5 (7,0 Mbit/s, 7,5 Mbit/s, 8,0 Mbit/s)", true));
				list.Add(new BaseKey("33", "((value) - 24) (9 Mbit/s, 10 Mbit/s, 11 Mbit/s, 12 Mbit/s, 13 Mbit/s)", true));
				list.Add(new BaseKey("34", "((value) - 24) (9 Mbit/s, 10 Mbit/s, 11 Mbit/s, 12 Mbit/s, 13 Mbit/s)", true));
				list.Add(new BaseKey("35", "((value) - 24) (9 Mbit/s, 10 Mbit/s, 11 Mbit/s, 12 Mbit/s, 13 Mbit/s)", true));
				list.Add(new BaseKey("36", "((value) - 24) (9 Mbit/s, 10 Mbit/s, 11 Mbit/s, 12 Mbit/s, 13 Mbit/s)", true));
				list.Add(new BaseKey("37", "((value) - 24) (9 Mbit/s, 10 Mbit/s, 11 Mbit/s, 12 Mbit/s, 13 Mbit/s)", true));
				list.Add(new BaseKey("38", "13,5", true));
				list.Add(new BaseKey("39", "((value) - 25) (14 Mbit/s, 15 Mbit/s, 16 Mbit/s, 17 Mbit/s, 18 Mbit/s)", true));
				list.Add(new BaseKey("40", "((value) - 25) (14 Mbit/s, 15 Mbit/s, 16 Mbit/s, 17 Mbit/s, 18 Mbit/s)", true));
				list.Add(new BaseKey("41", "((value) - 25) (14 Mbit/s, 15 Mbit/s, 16 Mbit/s, 17 Mbit/s, 18 Mbit/s)", true));
				list.Add(new BaseKey("42", "((value) - 25) (14 Mbit/s, 15 Mbit/s, 16 Mbit/s, 17 Mbit/s, 18 Mbit/s)", true));
				list.Add(new BaseKey("43", "((value) - 25) (14 Mbit/s, 15 Mbit/s, 16 Mbit/s, 17 Mbit/s, 18 Mbit/s)", true));
				list.Add(new BaseKey("44", "((value) - 34) × 2 (20 Mbit/s, 22 Mbit/s, 24 Mbit/s, 26 Mbit/s)", true));
				list.Add(new BaseKey("45", "((value) - 34) × 2 (20 Mbit/s, 22 Mbit/s, 24 Mbit/s, 26 Mbit/s)", true));
				list.Add(new BaseKey("46", "((value) - 34) × 2 (20 Mbit/s, 22 Mbit/s, 24 Mbit/s, 26 Mbit/s)", true));
				list.Add(new BaseKey("47", "((value) - 34) × 2 (20 Mbit/s, 22 Mbit/s, 24 Mbit/s, 26 Mbit/s)", true));
				list.Add(new BaseKey("48", "27", true));
				list.Add(new BaseKey("49", "((value) - 35) × 2 (28 Mbit/s, 30 Mbit/s, 32 Mbit/s to 40 Mbit/s)", true));
				list.Add(new BaseKey("50", "((value) - 35) × 2 (28 Mbit/s, 30 Mbit/s, 32 Mbit/s to 40 Mbit/s)", true));
				list.Add(new BaseKey("51", "((value) - 35) × 2 (28 Mbit/s, 30 Mbit/s, 32 Mbit/s to 40 Mbit/s)", true));
				list.Add(new BaseKey("52", "((value) - 35) × 2 (28 Mbit/s, 30 Mbit/s, 32 Mbit/s to 40 Mbit/s)", true));
				list.Add(new BaseKey("53", "((value) - 35) × 2 (28 Mbit/s, 30 Mbit/s, 32 Mbit/s to 40 Mbit/s)", true));
				list.Add(new BaseKey("54", "((value) - 35) × 2 (28 Mbit/s, 30 Mbit/s, 32 Mbit/s to 40 Mbit/s)", true));
				list.Add(new BaseKey("55", "((value) - 35) × 2 (28 Mbit/s, 30 Mbit/s, 32 Mbit/s to 40 Mbit/s)", true));
				list.Add(new BaseKey("56", "44", true));
				list.Add(new BaseKey("57 ", "48", true));
				list.Add(new BaseKey("58", "54", true));
				list.Add(new BaseKey("59", "72", true));
				list.Add(new BaseKey("60", "108", true));
				list.Add(new BaseKey("61", "DVB_reserved", true));
				list.Add(new BaseKey("62", "DVB_reserved", true));
				list.Add(new BaseKey("63", "DVB_reserved", true));
				return list;
			}
		}

		public IList<BaseKey> CodingTypeValues
		{
			get
			{
				IList<BaseKey> list = new List<BaseKey>();
				list.Add(new BaseKey("00", "not defined", true));
				list.Add(new BaseKey("01", "satellite", true));
				list.Add(new BaseKey("02", "cable", true));
				list.Add(new BaseKey("03", "terrestrial", true));
				return list;
			}
		}

		private BaseData()
		{
		}

		public IList<BaseKey> GetPosibleLocations()
		{
			IList<BaseKey> list = new List<BaseKey>();
			foreach (EnumPossibleLoaction value in Enum.GetValues(typeof(EnumPossibleLoaction)))
			{
				BaseKey baseKey = new PosibleLocation(value);
				baseKey.BKValue = baseKey.BKValue;
				list.Add(baseKey);
			}
			return list;
		}

		public IList<BaseKey> GetDescriptorTypesByLocation(EnumPossibleLoaction location, bool showAll)
		{
			IList<BaseKey> list = new List<BaseKey>();
			IList<DescriptorType> descriptorTypesByLoaction = desTypeManager.GetDescriptorTypesByLoaction(location);
			return showAll ? ((IEnumerable<BaseKey>)descriptorTypesByLoaction).ToList() : ((IEnumerable<BaseKey>)descriptorTypesByLoaction.Where((DescriptorType p) => p.BKStatus)).ToList();
		}

		public IList<BaseKey> GetCountryCode()
		{
			return new DescriptorManagement().GetCountryCode();
		}

		public IList<BaseKey> GetLanguageCode()
		{
			return new DescriptorManagement().GetLanguageCode();
		}

		public List<EitRateSetting> GetEitRateSetting()
		{
			return (from p in sysManager.GetEitRateSetting()
				orderby p.RateValue
				select p).ToList();
		}

		public string GetDefaultConuntryCode()
		{
			SystemSetting systemSetting = GetSystemSetting(SystemSettingCode.CountryCode);
			return (systemSetting == null) ? "CHN" : systemSetting.Value;
		}

		public SystemSetting GetSystemSetting(SystemSettingCode code)
		{
			return sysManager.GetSystemSetting(code);
		}

		public List<BaseKey> GetEncoders()
		{
			List<BaseKey> list = new List<BaseKey>();
			list.Add(new BaseKey("DEFAULT", "Default", true));
			list.Add(new BaseKey("UTF-8", "UTF-8", true));
			list.Add(new BaseKey("UNICODE", "UNICODE", true));
			list.Add(new BaseKey("GB2312", "GB2312", true));
			list.Add(new BaseKey("ASCII", "ASCII", true));
			return list;
		}
	}
}
