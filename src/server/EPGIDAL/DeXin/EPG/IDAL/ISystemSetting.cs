using System.Collections.Generic;
using DeXin.EPG.Model;

namespace DeXin.EPG.IDAL
{
	public interface ISystemSetting
	{
		SystemSetting GetSystemSetting(SystemSettingCode key);

		void Save(SystemSetting item);

		List<EitRateSetting> GetEitRateSetting();

		void SaveOrUpdate(List<EitRateSetting> items);
	}
}
