using System.Collections.Generic;
using DeXin.EPG.DALFactory;
using DeXin.EPG.Model;

namespace DeXin.EPG.BLL
{
	public class SendSpeedInfoManager
	{
		private AbstractFactory db = AbstractFactory.CreateFactory();

		public IList<SendSpeedInfo> GetOutPutSpeedSetting()
		{
			return db.CreateSendSpeedInfoDAL().GetOutPutSpeedSetting();
		}

		public SendSpeedInfo GetByTSIdentity(long TSIdentity)
		{
			return db.CreateSendSpeedInfoDAL().GetByTSIdentity(TSIdentity);
		}

		public IList<SendSpeedInfo> GetByNITIdentity(long NITIdentity)
		{
			return db.CreateSendSpeedInfoDAL().GetByNITIdentity(NITIdentity);
		}

		public SendSpeedInfo GetByIdentity(long Identity)
		{
			return db.CreateSendSpeedInfoDAL().GetByIdentity(Identity);
		}

		public int Save(SendSpeedInfo controller)
		{
			return db.CreateSendSpeedInfoDAL().Save(controller);
		}

		public int Delete(long Identity)
		{
			return db.CreateSendSpeedInfoDAL().Delete(Identity);
		}

		public int Delete(IList<long> ListIdentity)
		{
			return db.CreateSendSpeedInfoDAL().Delete(ListIdentity);
		}
	}
}
