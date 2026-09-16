using System.ComponentModel;

namespace DeXin.EPG.Model
{
	[TypeDescriptionProvider(typeof(MyTypeDescriptionProvider))]
	public class Multimedia
	{
		private long _Id = 0L;

		private long _StreamTypeTableId = 0L;

		private long _ProgramTableId = 0L;

		private int _PesPid = 0;

		private ProgramInfo _Program;

		private StreamTypeInfo _PESType;

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

		public long StreamTypeTableId
		{
			get
			{
				return _StreamTypeTableId;
			}
			set
			{
				_StreamTypeTableId = value;
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

		public int PesPid
		{
			get
			{
				return _PesPid;
			}
			set
			{
				_PesPid = value;
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
			}
		}

		public StreamTypeInfo PESType
		{
			get
			{
				if (_PESType == null && this.ReadPESType != null)
				{
					this.ReadPESType(this);
				}
				return _PESType;
			}
			set
			{
				_PESType = value;
			}
		}

		public event GetExtendPropertyHandeller ReadProgram;

		public event GetExtendPropertyHandeller ReadPESType;
	}
}
