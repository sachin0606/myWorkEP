using System.Collections.Generic;
using DeXin.EPG.Model;

namespace DeXin.EPG.IDAL
{
	public interface IPMTPIDReMappingDAL
	{
		IList<PmtPidOutputInfo> GetData();

		IList<PmtPidOutputInfo> GetDataByID(long id);

		IList<PmtPidOutputInfo> GetDataByProgramTableID(long program_table_id);

		IList<PmtPidOutputInfo> GetDataByTSTableID(long tsTableId);

		int Add(PmtPidOutputInfo multiIP);

		int Update(PmtPidOutputInfo multiIP);

		int Delete(PmtPidOutputInfo multiIP);

		int Delete(IList<PmtPidOutputInfo> multipleIPs);
	}
}
