using System.Collections.Generic;
using DeXin.EPG.DALFactory;
using DeXin.EPG.Model;

namespace DeXin.EPG.BLL
{
	public class NITInfoManager
	{
		private AbstractFactory db = AbstractFactory.CreateFactory();

		public IList<NitInfo> GetAllNITInfos()
		{
			return db.CreateNITInfoDAL().GetAllNITInfos();
		}

		public IList<NitInfo> GetOutputNIT()
		{
			return db.CreateNITInfoDAL().GetOutputNIT();
		}

		public IList<NitInfo> GetNITInfoByNetworkID(int NetworkID)
		{
			return db.CreateNITInfoDAL().GetNITInfoByNetworkID(NetworkID);
		}

		public NitInfo GetNITInfoByNetworkIdentity(long NetworkIdentity)
		{
			return db.CreateNITInfoDAL().GetNITInfoByNetworkIdentity(NetworkIdentity);
		}

		public bool CanSaveNITInfo(NitInfo NIT)
		{
			return db.CreateNITInfoDAL().CanSaveNITInfo(NIT);
		}

		public int SaveNITInfo(NitInfo NIT)
		{
			return db.CreateNITInfoDAL().SaveNITInfo(NIT);
		}

		public int DeleteNITInfo(long NetworkIdentity)
		{
			return db.CreateNITInfoDAL().DeleteNITInfo(NetworkIdentity);
		}
	}
}
