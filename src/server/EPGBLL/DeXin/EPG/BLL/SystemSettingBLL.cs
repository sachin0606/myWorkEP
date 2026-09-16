using System.Collections.Generic;
using DeXin.EPG.DALFactory;
using DeXin.EPG.Model;

namespace DeXin.EPG.BLL
{
	public class SystemSettingBLL
	{
		private AbstractFactory db = AbstractFactory.CreateFactory();

		public SystemSetting GetSystemSetting(SystemSettingCode key)
		{
			return db.CreateSystemSettingDAL().GetSystemSetting(key);
		}

		public void Save(SystemSetting item)
		{
			db.CreateSystemSettingDAL().Save(item);
		}

		public List<EitRateSetting> GetEitRateSetting()
		{
			return db.CreateSystemSettingDAL().GetEitRateSetting();
		}

		public void SaveOrUpdate(List<EitRateSetting> items)
		{
			db.CreateSystemSettingDAL().SaveOrUpdate(items);
		}
	}
}
