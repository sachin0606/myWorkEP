using System.Collections.Generic;
using DeXin.EPG.Model;

namespace DeXin.EPG.IDAL
{
	public interface IStreamTypeInfoDAL
	{
		StreamTypeInfo GetByIdentity(long Identity);

		StreamTypeInfo GetByStreamTypeId(int StreamTypeId);

		IList<StreamTypeInfo> GetAll();
	}
}
