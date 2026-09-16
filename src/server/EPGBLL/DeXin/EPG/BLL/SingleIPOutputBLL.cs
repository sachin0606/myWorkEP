using System;
using System.Collections.Generic;
using DeXin.EPG.DALFactory;
using DeXin.EPG.Model;

namespace DeXin.EPG.BLL
{
	public class SingleIPOutputBLL
	{
		private AbstractFactory abstractfactory = AbstractFactory.CreateFactory();

		public IList<SimpleipSettings> GetData()
		{
			try
			{
				return abstractfactory.CreateSingleIPOutputDAL().GetData();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Add(SimpleipSettings singleIPSetting)
		{
			try
			{
				return abstractfactory.CreateSingleIPOutputDAL().Add(singleIPSetting);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Update(SimpleipSettings singleIPSetting)
		{
			try
			{
				return abstractfactory.CreateSingleIPOutputDAL().Update(singleIPSetting);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Delete(SimpleipSettings singleIPSetting)
		{
			try
			{
				return abstractfactory.CreateSingleIPOutputDAL().Delete(singleIPSetting);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int DeleteWithTrans(IList<SimpleipSettings> lstPermitInfo)
		{
			try
			{
				return abstractfactory.CreateSingleIPOutputDAL().DeleteWithTrans(lstPermitInfo);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
