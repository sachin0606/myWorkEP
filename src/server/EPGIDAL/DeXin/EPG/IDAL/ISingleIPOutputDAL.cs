using System.Collections.Generic;
using DeXin.EPG.Model;

namespace DeXin.EPG.IDAL
{
	public interface ISingleIPOutputDAL
	{
		IList<SimpleipSettings> GetData();

		int Add(SimpleipSettings singleIP);

		int Update(SimpleipSettings singleIP);

		int Delete(SimpleipSettings singleIP);

		int DeleteWithTrans(IList<SimpleipSettings> lstPermitInfo);
	}
}
