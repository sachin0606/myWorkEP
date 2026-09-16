using System.Collections.Generic;
using System.Linq;
using DeXin.EPG.DALFactory;
using DeXin.EPG.Model;

namespace DeXin.EPG.BLL
{
	public class TSInfoManager
	{
		private AbstractFactory db = AbstractFactory.CreateFactory();

		public IList<TsInfo> GetTSInfoByNetWorkIdentity(long NetWorkIdentity)
		{
			return (from p in db.CreateTSInofDAL().GetTSInfoByNetWorkIdentity(NetWorkIdentity)
				orderby p.TsId
				select p).ToList();
		}

		public IList<TsInfo> GetOutputTS()
		{
			return db.CreateTSInofDAL().GetOutputTS();
		}

		public IList<TsInfo> GetTSInfoByTSId(int TSId)
		{
			return db.CreateTSInofDAL().GetTSInfoByTSId(TSId);
		}

		public bool CanSaveTSInfo(TsInfo TS)
		{
			return db.CreateTSInofDAL().CanSaveTSInfo(TS);
		}

		public int SaveTSInfo(TsInfo TS)
		{
			return db.CreateTSInofDAL().SaveTSInfo(TS);
		}

		public int DeleteTSInfo(long TSInfoIdentity)
		{
			return db.CreateTSInofDAL().DeleteTSInfo(TSInfoIdentity);
		}

		public TsInfo GetTSInfoByTSIdentity(long TSIdentity)
		{
			return db.CreateTSInofDAL().GetTSInfoByTSIdentity(TSIdentity);
		}
	}
}
