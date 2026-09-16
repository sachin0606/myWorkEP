using System.Collections.Generic;
using DeXin.EPG.Model;

namespace DeXin.EPG.IDAL
{
	public interface ITSInfoDAL
	{
		IList<TsInfo> GetTSInfoByNetWorkIdentity(long NetWorkIdentity);

		IList<TsInfo> GetOutputTS();

		IList<TsInfo> GetTSInfoByTSId(int TSId);

		bool CanSaveTSInfo(TsInfo TS);

		int SaveTSInfo(TsInfo TS);

		int DeleteTSInfo(long TSInfoIdentity);

		TsInfo GetTSInfoByTSIdentity(long TSIdentity);
	}
}
