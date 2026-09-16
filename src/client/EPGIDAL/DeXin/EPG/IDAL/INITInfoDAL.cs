using System.Collections.Generic;
using DeXin.EPG.Model;

namespace DeXin.EPG.IDAL
{
	public interface INITInfoDAL
	{
		IList<NitInfo> GetAllNITInfos();

		IList<NitInfo> GetOutputNIT();

		IList<NitInfo> GetNITInfoByNetworkID(int NetworkID);

		NitInfo GetNITInfoByNetworkIdentity(long NetworkIdentity);

		bool CanSaveNITInfo(NitInfo NIT);

		int SaveNITInfo(NitInfo NIT);

		int DeleteNITInfo(long NetworkIdentity);
	}
}
