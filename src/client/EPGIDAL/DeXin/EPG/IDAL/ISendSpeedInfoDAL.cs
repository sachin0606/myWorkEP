using System.Collections.Generic;
using DeXin.EPG.Model;

namespace DeXin.EPG.IDAL
{
	public interface ISendSpeedInfoDAL
	{
		SendSpeedInfo GetByTSIdentity(long TSIdentity);

		IList<SendSpeedInfo> GetByNITIdentity(long NITIdentity);

		SendSpeedInfo GetByIdentity(long Identity);

		int Save(SendSpeedInfo controller);

		int Delete(long Identity);

		int Delete(IList<long> ListIdentity);

		IList<SendSpeedInfo> GetOutPutSpeedSetting();
	}
}
