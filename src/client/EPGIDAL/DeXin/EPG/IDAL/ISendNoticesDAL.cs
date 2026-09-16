using System.Collections.Generic;
using DeXin.EPG.Model;

namespace DeXin.EPG.IDAL
{
	public interface ISendNoticesDAL
	{
		int SetAllNoticeOn();

		IList<SendNotices> GetAll();

		void Delete(long Identity);

		void DeleteAll();

		void DeleteByCode(int locationCode);
	}
}
