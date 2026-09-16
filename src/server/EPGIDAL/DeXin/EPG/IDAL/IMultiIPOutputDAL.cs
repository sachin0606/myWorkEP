using System.Collections.Generic;
using DeXin.EPG.Model;

namespace DeXin.EPG.IDAL
{
	public interface IMultiIPOutputDAL
	{
		IList<MultipleipSettings> GetData();

		IList<MultipleipSettings> GetDataByID(long id);

		IList<MultipleipSettings> GetDataByNetworkTableID(long id);

		int Add(MultipleipSettings multiIP);

		int Update(MultipleipSettings multiIP);

		int Delete(MultipleipSettings multiIP);

		int Delete(IList<MultipleipSettings> multipleIPs);
	}
}
