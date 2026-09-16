using System.Collections.Generic;
using DeXin.EPG.DALFactory;
using DeXin.EPG.Model;

namespace DeXin.EPG.BLL
{
	public class PATManager
	{
		private AbstractFactory db = AbstractFactory.CreateFactory();

		public IList<PAT> GetAll()
		{
			return db.CreatePATDAL().GetAll();
		}

		public void Save(List<long> ProgramIdentityList, string UserId)
		{
			db.CreatePATDAL().Save(ProgramIdentityList, UserId);
		}
	}
}
