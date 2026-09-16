using DeXin.EPG.Model;

namespace DeXin.EPG.IDAL
{
	public interface IBATToProgramDAL
	{
		BATToProgram GetByBAT(long BATIdtntity);

		BATToProgram GetByProgram(long ProgramIdentity);
	}
}
