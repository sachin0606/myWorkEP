using System.Collections.Generic;
using DeXin.EPG.Model;

namespace DeXin.EPG.IDAL
{
	public interface IMultimediaDAL
	{
		IList<Multimedia> GetByProgramIdentity(long ProgramIdentity);

		int Save(Multimedia Media);

		void Delete(IList<long> ListIdentity);

		IList<Multimedia> GetOutput();
	}
}
