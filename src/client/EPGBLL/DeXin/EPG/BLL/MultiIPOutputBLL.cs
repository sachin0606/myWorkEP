using System;
using System.Collections.Generic;
using DeXin.EPG.DALFactory;
using DeXin.EPG.Model;

namespace DeXin.EPG.BLL
{
	public class MultiIPOutputBLL
	{
		private AbstractFactory abstractfacory = AbstractFactory.CreateFactory();

		public IList<MultipleipSettings> GetData()
		{
			try
			{
				return abstractfacory.CreateMultiIPOutputDAL().GetData();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public IList<MultipleipSettings> GetDataByID(long id)
		{
			try
			{
				return abstractfacory.CreateMultiIPOutputDAL().GetDataByID(id);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public IList<MultipleipSettings> GetDataByNetworkTableID(long id)
		{
			try
			{
				return abstractfacory.CreateMultiIPOutputDAL().GetDataByNetworkTableID(id);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Add(MultipleipSettings multileIP)
		{
			try
			{
				return abstractfacory.CreateMultiIPOutputDAL().Add(multileIP);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Update(MultipleipSettings multileIP)
		{
			try
			{
				return abstractfacory.CreateMultiIPOutputDAL().Update(multileIP);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Delete(MultipleipSettings multileIP)
		{
			try
			{
				return abstractfacory.CreateMultiIPOutputDAL().Delete(multileIP);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Delete(IList<MultipleipSettings> multileIPs)
		{
			try
			{
				return abstractfacory.CreateMultiIPOutputDAL().Delete(multileIPs);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
