using System;
using System.Collections.Generic;
using System.Data;
using DeXin.EPG.DALFactory;
using DeXin.EPG.Model;

namespace DeXin.EPG.BLL
{
	public class PermitGroupManagementBLL
	{
		private AbstractFactory af = AbstractFactory.CreateFactory();

		public IList<PermitGroup> GetDataByGroupIDAndGroupStatus(int groupID, bool status)
		{
			try
			{
				return af.CreatePermitGroupManagementDAL().GetDataByGroupIDAndGroupStatus(groupID, status);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public IList<PermitGroup> GetPermitGroupDataByPaging(PagingSearch pagingSearch, string groupName, string LocFunctionName)
		{
			try
			{
				return af.CreatePermitGroupManagementDAL().GetPermitGroupDataByPaging(pagingSearch, groupName, LocFunctionName);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public IList<PermitGroup> GetPermitGroupDataByPaging(PagingSearch pagingSearch)
		{
			try
			{
				return af.CreatePermitGroupManagementDAL().GetPermitGroupDataByPaging(pagingSearch);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public IList<PermitGroup> GetDataByGroupID(int groupID)
		{
			try
			{
				return af.CreatePermitGroupManagementDAL().GetDataByGroupID(groupID);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public IList<PermitGroup> GetAllData()
		{
			try
			{
				return af.CreatePermitGroupManagementDAL().GetAllData();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public DataSet GetDataSet()
		{
			try
			{
				return af.CreatePermitGroupManagementDAL().GetDataSet();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public IList<PermitGroup> GetDataByGroupIDAndFunctionID(int groupID, string functionID)
		{
			try
			{
				return af.CreatePermitGroupManagementDAL().GetDataByGroupIDAndFunctionID(groupID, functionID);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public IList<PermitGroup> GetPermitList(string currentFunctionID, string userID)
		{
			UserInfoManager userInfoManager = new UserInfoManager();
			IList<PermitGroup> list = null;
			try
			{
				return GetDataByGroupIDAndFunctionID(userInfoManager.GetGroupIDByUserID(userID), currentFunctionID);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int AddWithSqlTrans(List<PermitGroup> permitlst)
		{
			int num = 0;
			try
			{
				return af.CreatePermitGroupManagementDAL().AddWithSqlTrans(permitlst);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int AddWithSqlTrans(IList<PermitGroup> lstPermitInfo, IList<PermitGroup> lstPermitInfoOriginal)
		{
			int num = 0;
			try
			{
				return af.CreatePermitGroupManagementDAL().AddWithSqlTrans(lstPermitInfo, lstPermitInfoOriginal);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Update(PermitGroup permit)
		{
			int num = 0;
			try
			{
				return af.CreatePermitGroupManagementDAL().Update(permit);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Delete(PermitGroup permit)
		{
			int num = 0;
			try
			{
				return af.CreatePermitGroupManagementDAL().Delete(permit);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int DeleteWithSqlTrans(List<PermitGroup> permitlst)
		{
			int num = 0;
			try
			{
				return af.CreatePermitGroupManagementDAL().DeleteWithSqlTrans(permitlst);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
