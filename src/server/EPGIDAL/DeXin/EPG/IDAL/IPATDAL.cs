using System.Collections.Generic;
using DeXin.EPG.Model;

namespace DeXin.EPG.IDAL
{
	public interface IPATDAL
	{
		IList<PAT> GetAll();

		void Save(List<long> ProgramIdentityList, string UserId);
	}
}
