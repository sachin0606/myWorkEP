using System.ComponentModel;

namespace DeXin.EPG.Model
{
	[TypeDescriptionProvider(typeof(MyTypeDescriptionProvider))]
	public class PmtPidOutputInfo
	{
		private long _Id = 0L;

		private long _ProgramTableId = 0L;

		private int _OutputPid = 0;

		private ProgramInfo _Program;

		public long Id
		{
			get
			{
				return _Id;
			}
			set
			{
				_Id = value;
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

		public int OutputPid
		{
			get
			{
				return _OutputPid;
			}
			set
			{
				_OutputPid = value;
			}
		}

		public ProgramInfo Program
		{
			get
			{
				return _Program;
			}
			set
			{
				_Program = value;
			}
		}
	}
}
