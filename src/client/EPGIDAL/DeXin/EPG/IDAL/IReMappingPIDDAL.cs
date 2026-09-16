using System.Collections.Generic;
using DeXin.EPG.Model;

namespace DeXin.EPG.IDAL
{
	public interface IReMappingPIDDAL
	{
		IList<PidOutputInfo> GetData();

		IList<PidOutputInfo> GetDataByID(long id);

		IList<PidOutputInfo> GetDataByTSTableID(long tsTableId);

		IList<PidOutputInfo> GetDataByNetworkTableID(long networkTableId);

		int Add(PidOutputInfo multiIP);

		int Update(PidOutputInfo multiIP);

		int Delete(PidOutputInfo multiIP);

		int Delete(IList<PidOutputInfo> multipleIPs);
	}
}
