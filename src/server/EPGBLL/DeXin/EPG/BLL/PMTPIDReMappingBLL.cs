using System;
using System.Collections.Generic;
using DeXin.EPG.DALFactory;
using DeXin.EPG.Model;

namespace DeXin.EPG.BLL
{
	public class PMTPIDReMappingBLL
	{
		private AbstractFactory abstractfacory = AbstractFactory.CreateFactory();

		public IList<PmtPidOutputInfo> GetData()
		{
			try
			{
				return abstractfacory.CreatePMTPIDReMappingDAL().GetData();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public IList<PmtPidOutputInfo> GetDataByID(long id)
		{
			try
			{
				return abstractfacory.CreatePMTPIDReMappingDAL().GetDataByID(id);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public IList<PmtPidOutputInfo> GetDataByProgramTableID(long program_table_id)
		{
			try
			{
				return abstractfacory.CreatePMTPIDReMappingDAL().GetDataByProgramTableID(program_table_id);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public IList<PmtPidOutputInfo> GetDataByTSTableID(long tsTableId)
		{
			try
			{
				return abstractfacory.CreatePMTPIDReMappingDAL().GetDataByTSTableID(tsTableId);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Add(PmtPidOutputInfo multileIP)
		{
			try
			{
				return abstractfacory.CreatePMTPIDReMappingDAL().Add(multileIP);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Update(PmtPidOutputInfo multileIP)
		{
			try
			{
				return abstractfacory.CreatePMTPIDReMappingDAL().Update(multileIP);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Delete(PmtPidOutputInfo multileIP)
		{
			try
			{
				return abstractfacory.CreatePMTPIDReMappingDAL().Delete(multileIP);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Delete(IList<PmtPidOutputInfo> multileIPs)
		{
			try
			{
				return abstractfacory.CreatePMTPIDReMappingDAL().Delete(multileIPs);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
