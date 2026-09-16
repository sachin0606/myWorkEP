using System.Collections.Generic;
using DeXin.EPG.Model;

namespace DeXin.EPG.IDAL
{
	public interface IBATInfoDAL
	{
		IList<BATInfo> GetByNetworkIdentity(long NetworkIdentity);

		BATInfo GetByIdentity(long Identity);

		void Save(BATToProgram BATToPrgList, bool updateServiceList = true);

		void Delete(IList<long> ListIdentity);

		BATInfo GetByBATId(long NetworkIdentity, int BATId);

		IList<BATInfo> GetOutputBAT();
	}
}
