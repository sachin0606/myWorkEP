using System;
using System.Collections.Generic;
using DeXin.EPG.DALFactory;
using DeXin.EPG.Model;

namespace DeXin.EPG.BLL
{
	public class SendOrNotInfoBLL
	{
		private AbstractFactory abstractfacory = AbstractFactory.CreateFactory();

		public IList<SendOrNotInfo> GetData()
		{
			try
			{
				return abstractfacory.CreateSendOrNotInfoDAL().GetData();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public IList<SendOrNotInfo> GetDataByID(long id)
		{
			try
			{
				return abstractfacory.CreateSendOrNotInfoDAL().GetDataByID(id);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public IList<SendOrNotInfo> GetDataByTSTableID(long tsTableId)
		{
			try
			{
				return abstractfacory.CreateSendOrNotInfoDAL().GetDataByTSTableID(tsTableId);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public IList<SendOrNotInfo> GetDataByNetworkTableID(long networkTableId)
		{
			try
			{
				return abstractfacory.CreateSendOrNotInfoDAL().GetDataByNetworkTableID(networkTableId);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Add(SendOrNotInfo multileIP)
		{
			try
			{
				return abstractfacory.CreateSendOrNotInfoDAL().Add(multileIP);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Update(SendOrNotInfo multileIP)
		{
			try
			{
				return abstractfacory.CreateSendOrNotInfoDAL().Update(multileIP);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private int Delete(SendOrNotInfo multileIP)
		{
			try
			{
				return abstractfacory.CreateSendOrNotInfoDAL().Delete(multileIP);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Delete(IList<SendOrNotInfo> multileIPs)
		{
			try
			{
				return abstractfacory.CreateSendOrNotInfoDAL().Delete(multileIPs);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
