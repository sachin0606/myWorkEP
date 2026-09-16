using System.Collections.Generic;
using DeXin.EPG.Model;

namespace DeXin.EPG.IDAL
{
	public interface IProgramInfoDAL
	{
		IList<ProgramInfo> GetProgramInfosByTSIdentity(long TSIdentity);

		ProgramInfo GetByTSIdentityAndServiceId(long TSIdentity, int ServiceId);

		ProgramInfo Get(int NetworkID, int TSID, int ServiceID);

		int DeleteProgram(long ProgramIdentity);

		bool CanSaveProgramInfo(ProgramInfo Program);

		int Save(ProgramInfo Program);

		IList<ProgramInfo> GetOutputProgram();

		IList<ProgramInfo> GetHasEITFileProgram();

		ProgramInfo GetByIdentity(long Identity);

		IList<ProgramInfo> GetByEitFile(string fileName);
	}
}
