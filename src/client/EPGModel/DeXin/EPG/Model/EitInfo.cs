using System;
using System.Collections.Generic;
using System.ComponentModel;
using DeXin.EPG.Tools;

namespace DeXin.EPG.Model
{
	[TypeDescriptionProvider(typeof(MyTypeDescriptionProvider))]
	public class EitInfo : BaseTable
	{
		private int _EventId = -1;

		private string _EventName = string.Empty;

		private string _EventRemark = string.Empty;

		private string _EventLongRemark = string.Empty;

		private long _ProgramTableId = 0L;

		private DateTime _StartTime = DateTime.Now;

		private DateTime _EndTime = DateTime.Now;

		private int? _EitRateValue;

		private EitRateSetting _EITRateSetting;

		private ProgramInfo _Program;

		private IList<DescriptorSigleBase> _Event_Descriptors = new List<DescriptorSigleBase>();

		public int EventId
		{
			get
			{
				return _EventId;
			}
			set
			{
				_EventId = value;
			}
		}

		public string EventName
		{
			get
			{
				return _EventName;
			}
			set
			{
				_EventName = value;
			}
		}

		public string EventRemark
		{
			get
			{
				return _EventRemark;
			}
			set
			{
				_EventRemark = value;
			}
		}

		public string EventLongRemark
		{
			get
			{
				return _EventLongRemark;
			}
			set
			{
				_EventLongRemark = value;
			}
		}

		public long ProgramTableId
		{
			get
			{
				return _ProgramTableId;
			}
			set
			{
				_ProgramTableId = value;
			}
		}

		public DateTime StartTime
		{
			get
			{
				return _StartTime;
			}
			set
			{
				_StartTime = value;
			}
		}

		public DateTime EndTime
		{
			get
			{
				return _EndTime;
			}
			set
			{
				_EndTime = value;
			}
		}

		public int? EitRateValue
		{
			get
			{
				return _EitRateValue;
			}
			set
			{
				_EitRateValue = value;
			}
		}

		public EitRateSetting EITRateSetting
		{
			get
			{
				if (_EITRateSetting == null && this.ReadEitRateSetting != null)
				{
					this.ReadEitRateSetting(this);
					if (_EITRateSetting == null && _EitRateValue.HasValue)
					{
						_EITRateSetting = new EitRateSetting
						{
							RateCode = _EitRateValue.ToString(),
							RateName = _EitRateValue.ToString(),
							RateValue = _EitRateValue.Value
						};
					}
				}
				return _EITRateSetting;
			}
			set
			{
				_EITRateSetting = value;
				if (_EITRateSetting != null)
				{
					_EitRateValue = _EITRateSetting.RateValue;
				}
			}
		}

		public ProgramInfo Program
		{
			get
			{
				if (_Program == null && this.ReadProgram != null)
				{
					this.ReadProgram(this);
				}
				return _Program;
			}
			set
			{
				_Program = value;
				if (_Program != null)
				{
					_ProgramTableId = _Program.Identity;
				}
			}
		}

		public IList<DescriptorSigleBase> Event_Descriptors
		{
			get
			{
				return _Event_Descriptors;
			}
			set
			{
				_Event_Descriptors = value;
			}
		}

		public event GetExtendPropertyHandeller ReadProgram;

		public event GetExtendPropertyHandeller ReadEitRateSetting;

		public EitInfo()
		{
			_TableType = EnumTableType.EIT;
		}

		public EitInfo(long identity, string tableName)
			: base(identity, tableName)
		{
		}

		public override string ToString()
		{
			return string.Format("event_id:{0}, event_name:{1}, start_time:{2}, end_time:{3}", EventId, EventName, StartTime, EndTime);
		}
	}
}
