using System.Collections.Generic;
using DeXin.EPG.DALFactory;
using DeXin.EPG.Model;

namespace DeXin.EPG.BLL
{
	public class MultimediaManager
	{
		private AbstractFactory db = AbstractFactory.CreateFactory();

		public IList<Multimedia> GetByProgramIdentity(long ProgramIdentity)
		{
			return db.CreateMultimediaDAL().GetByProgramIdentity(ProgramIdentity);
		}

		public int Save(Multimedia Media)
		{
			return db.CreateMultimediaDAL().Save(Media);
		}

		public void Delete(IList<long> ListIdentity)
		{
			db.CreateMultimediaDAL().Delete(ListIdentity);
		}

		public IList<Multimedia> GetOutput()
		{
			return db.CreateMultimediaDAL().GetOutput();
		}
	}
}
