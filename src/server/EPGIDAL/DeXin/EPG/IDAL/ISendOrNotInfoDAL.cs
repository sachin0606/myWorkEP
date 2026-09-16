using System.Collections.Generic;
using DeXin.EPG.Model;

namespace DeXin.EPG.IDAL
{
	public interface ISendOrNotInfoDAL
	{
		IList<SendOrNotInfo> GetData();

		IList<SendOrNotInfo> GetDataByID(long id);

		IList<SendOrNotInfo> GetDataByTSTableID(long tsTableId);

		IList<SendOrNotInfo> GetDataByNetworkTableID(long networkTableId);

		int Add(SendOrNotInfo multiIP);

		int Update(SendOrNotInfo multiIP);

		int Delete(SendOrNotInfo multiIP);

		int Delete(IList<SendOrNotInfo> multipleIPs);
	}
}
