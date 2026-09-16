using System;
using System.Collections.Generic;
using System.Linq;
using Com.Cn.Tool.Model;
using DeXin.EPG.DALFactory;
using DeXin.EPG.IDAL;
using DeXin.EPG.Model;
using DeXin.EPG.Tools;

namespace DeXin.EPG.BLL
{
	public class DescriptorManagement
	{
		private IDescriptor pageService = null;

		public DescriptorManagement()
		{
			try
			{
				pageService = AbstractFactory.CreateFactory().CreateDescriptorService();
			}
			catch (Exception)
			{
			}
		}

		public IList<DescriptorSigleBase> GetDescriptors(EnumPossibleLoaction location, long tableIdentity)
		{
			return pageService.GetDescriptors(location, tableIdentity);
		}

		public IList<DescriptorSigleBase> GetDescriptors(long batID, long tsID)
		{
			return pageService.GetDescriptors(batID, tsID);
		}

		public DescriptorSigleBase GetDescriptorByID(EnumPossibleLoaction location, long id)
		{
			return pageService.GetDescriptorByID(location, id);
		}

		public IList<DescriptorSigleBase> GetBAT_TS_DescriptorsByBATIDEntity(long batIdEntity)
		{
			return pageService.GetBAT_TS_DescriptorsByBATIDEntity(batIdEntity);
		}

		public IList<DescriptorSigleBase> GetNIT_TS_DescriptorsByNetWorkIDEntity(long netWorkIdentity)
		{
			return pageService.GetNIT_TS_DescriptorsByNetWorkIDEntity(netWorkIdentity);
		}

		public bool AddDescriptor(DescriptorBase item, out string msg)
		{
			bool flag = true;
			msg = LocationControl.Ins.LocationItems[LacationEnum.SaveSucceed];
			if (item is ExtendedEventDescriptorBase)
			{
				ExtendedEventDescriptorBase subDes = item as ExtendedEventDescriptorBase;
				flag = pageService.AddDescriptor(subDes);
			}
			else if (item is ParentalRatingBase)
			{
				ParentalRatingBase subDes2 = item as ParentalRatingBase;
				flag = pageService.AddDescriptor(subDes2);
			}
			else if (item is DescriptorSigleBase)
			{
				flag = pageService.AddDescriptor(item.Location, (DescriptorSigleBase)item);
			}
			if (!flag)
			{
				msg = LocationControl.Ins.LocationItems[LacationEnum.SaveFailure];
			}
			return flag;
		}

		public bool ModifyDescriptor(DescriptorBase item, out string msg)
		{
			msg = LocationControl.Ins.LocationItems[LacationEnum.EditSucceed];
			bool flag = true;
			if (item is ExtendedEventDescriptorBase)
			{
				ExtendedEventDescriptorBase subDes = item as ExtendedEventDescriptorBase;
				flag = pageService.AddDescriptor(subDes);
			}
			else if (item is ParentalRatingBase)
			{
				ParentalRatingBase subDes2 = item as ParentalRatingBase;
				flag = pageService.AddDescriptor(subDes2);
			}
			else if (item is DescriptorSigleBase)
			{
				flag = pageService.ModifyDescriptor(item.Location, (DescriptorSigleBase)item);
			}
			if (!flag)
			{
				flag = false;
				msg = LocationControl.Ins.LocationItems[LacationEnum.EditFailure];
			}
			return flag;
		}

		public bool DeleteDescriptor(EnumPossibleLoaction location, IList<long> args, out string msg)
		{
			msg = LocationControl.Ins.LocationItems[LacationEnum.DeleteFailure];
			bool result = false;
			if (pageService.DeleteDescriptorByIDEntity(location, args.ToArray()))
			{
				result = true;
				msg = LocationControl.Ins.LocationItems[LacationEnum.DeleteSucceed];
			}
			return result;
		}

		public IList<BaseKey> GetContentNibbleLevelByParentID(int pid)
		{
			return pageService.GetContentNibbleLevelByParentID(pid);
		}

		public IList<BaseKey> GetCountryCode()
		{
			return pageService.GetCountryCode();
		}

		public IList<BaseKey> GetLanguageCode()
		{
			return (from x in pageService.GetLanguageCode()
				orderby x.BKValue
				select x).ToList();
		}

		public IList<DescriptorSigleBase> GetEITDescriptorsForSend()
		{
			return pageService.GetEITDescriptorsForSend();
		}

		public byte[] ToTChageDate(byte[] descriptor, DateTime utcDateTime)
		{
			return descriptor;
		}
	}
}
