using System.Collections.Generic;
using DeXin.EPG.Tools;

namespace DeXin.EPG.Model
{
	public class BATToProgram : BaseTable
	{
		private BATInfo _BAT;

		private IList<ProgramInfo> _ProgramList;

		public BATInfo BAT
		{
			get
			{
				return _BAT;
			}
			set
			{
				_BAT = value;
			}
		}

		public IList<ProgramInfo> ProgramList
		{
			get
			{
				return _ProgramList;
			}
			set
			{
				_ProgramList = value;
			}
		}
	}
}
