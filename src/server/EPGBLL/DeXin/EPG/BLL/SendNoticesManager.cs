using System.Collections.Generic;
using DeXin.EPG.DALFactory;
using DeXin.EPG.Model;

namespace DeXin.EPG.BLL
{
	public class SendNoticesManager
	{
		private AbstractFactory db = AbstractFactory.CreateFactory();

		public int SetAllNoticeOn()
		{
			return db.CreateSendNoticesDAL().SetAllNoticeOn();
		}

		public IList<SendNotices> GetAll()
		{
			return db.CreateSendNoticesDAL().GetAll();
		}

		public void Delete(long Identity)
		{
			db.CreateSendNoticesDAL().Delete(Identity);
		}

		public void DeleteAll()
		{
			db.CreateSendNoticesDAL().DeleteAll();
		}

		public void DeleteByCode(int locationCode)
		{
			db.CreateSendNoticesDAL().DeleteByCode(locationCode);
		}
	}
}
