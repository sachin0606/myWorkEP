using DeXin.EPG.DALFactory;
using DeXin.EPG.Model;

namespace DeXin.EPG.BLL
{
	public class BATToProgramManager
	{
		private AbstractFactory db = AbstractFactory.CreateFactory();

		public BATToProgram GetByBAT(long BATIdtntity)
		{
			return db.CreateBATToProgramDAL().GetByBAT(BATIdtntity);
		}

		public BATToProgram GetByProgram(long ProgramIdentity)
		{
			return db.CreateBATToProgramDAL().GetByProgram(ProgramIdentity);
		}
	}
}
