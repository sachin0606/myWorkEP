using System.Collections.Generic;
using DeXin.EPG.DALFactory;
using DeXin.EPG.Model;

namespace DeXin.EPG.BLL
{
	public class StreamTypeInfoManager
	{
		private AbstractFactory db = AbstractFactory.CreateFactory();

		public StreamTypeInfo GetByIdentity(long Identity)
		{
			return db.CreateStreamTypeInfoDAL().GetByIdentity(Identity);
		}

		public StreamTypeInfo GetByStreamTypeId(int StreamTypeId)
		{
			return db.CreateStreamTypeInfoDAL().GetByStreamTypeId(StreamTypeId);
		}

		public IList<StreamTypeInfo> GetAll()
		{
			return db.CreateStreamTypeInfoDAL().GetAll();
		}
	}
}
