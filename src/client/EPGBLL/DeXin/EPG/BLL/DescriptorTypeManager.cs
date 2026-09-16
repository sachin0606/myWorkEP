using System.Collections.Generic;
using System.Linq;
using DeXin.EPG.DALFactory;
using DeXin.EPG.IDAL;
using DeXin.EPG.Model;
using DeXin.EPG.Tools;

namespace DeXin.EPG.BLL
{
	public class DescriptorTypeManager
	{
		private IDescriptorType pageService = null;

		public DescriptorTypeManager()
		{
			pageService = AbstractFactory.CreateFactory().CreateDescriptorTypeService();
		}

		public IList<DescriptorType> GetDescriptorTypesByLoaction(EnumPossibleLoaction location)
		{
			return pageService.GetDescriptorTypesByLoaction(location.ToString());
		}

		public DescriptorType GetDescriptorTypeByID(long args)
		{
			return pageService.GetDescriptorTypeByID(args);
		}

		public bool AddDescriptorType(DescriptorType args, out string msg)
		{
			msg = "TAG Repeat" + LocationControl.Ins.LocationItems[LacationEnum.SaveFailure];
			bool flag = pageService.AddDescriptorType(args);
			if (flag)
			{
				msg = LocationControl.Ins.LocationItems[LacationEnum.SaveSucceed];
			}
			return flag;
		}

		public bool ModifyDescriptorType(DescriptorType args, out string msg)
		{
			msg = LocationControl.Ins.LocationItems[LacationEnum.EditFailure];
			bool flag = pageService.ModifyDescriptorType(args);
			if (flag)
			{
				msg = LocationControl.Ins.LocationItems[LacationEnum.EditSucceed];
			}
			return flag;
		}

		public bool DeleteDescriptorType(IList<long> args, out string msg)
		{
			msg = LocationControl.Ins.LocationItems[LacationEnum.DeleteFailure];
			bool flag = pageService.DeleteDescriptorType(args.ToArray());
			if (flag)
			{
				msg = LocationControl.Ins.LocationItems[LacationEnum.DeleteSucceed];
			}
			return flag;
		}
	}
}
