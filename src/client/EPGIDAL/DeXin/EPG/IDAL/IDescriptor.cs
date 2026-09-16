using System.Collections.Generic;
using Com.Cn.Tool.Model;
using DeXin.EPG.Model;
using DeXin.EPG.Tools;

namespace DeXin.EPG.IDAL
{
	public interface IDescriptor
	{
		IList<DescriptorSigleBase> GetDescriptors(EnumPossibleLoaction location, long tableIdentity);

		IList<DescriptorSigleBase> GetDescriptors(long batId, long tsId);

		IList<DescriptorSigleBase> GetBAT_TS_DescriptorsByBATIDEntity(long batId);

		IList<DescriptorSigleBase> GetNIT_TS_DescriptorsByNetWorkIDEntity(long netWorkIdentity);

		DescriptorSigleBase GetDescriptorByID(EnumPossibleLoaction location, long id);

		bool AddDescriptor(EnumPossibleLoaction location, DescriptorSigleBase args);

		bool ModifyDescriptor(EnumPossibleLoaction location, DescriptorSigleBase args);

		bool DeleteDescriptorByIDEntity(EnumPossibleLoaction location, params long[] args);

		IList<BaseKey> GetContentNibbleLevelByParentID(int pid);

		IList<BaseKey> GetCountryCode();

		IList<BaseKey> GetLanguageCode();

		IList<DescriptorSigleBase> GetEITDescriptorsForSend();

		bool AddDescriptor(ExtendedEventDescriptorBase subDes);

		bool AddDescriptor(ParentalRatingBase subDes);
	}
}
