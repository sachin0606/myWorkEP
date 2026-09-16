using System.Collections.Generic;
using System.Data;
using DeXin.EPG.Model;

namespace DeXin.EPG.IDAL
{
	public interface IPermitGroupManagementDAL
	{
		IList<PermitGroup> GetDataByGroupIDAndGroupStatus(int groupID, bool status);

		IList<PermitGroup> GetPermitGroupDataByPaging(PagingSearch pagingSearch);

		IList<PermitGroup> GetPermitGroupDataByPaging(PagingSearch pagingSearch, string groupName, string LocFunctionName);

		IList<PermitGroup> GetDataByGroupID(int groupID);

		IList<PermitGroup> GetAllData();

		DataSet GetDataSet();

		IList<PermitGroup> GetDataByGroupIDAndFunctionID(int groupID, string functionID);

		int Add(PermitGroup permitGroupInfo);

		int AddWithSqlTrans(List<PermitGroup> lstPermitInfo);

		int DeleteWithSqlTrans(List<PermitGroup> lstPermitInfo);

		int AddWithSqlTrans(IList<PermitGroup> lstPermitInfo, IList<PermitGroup> lstPermitInfoOriginal);

		int Update(PermitGroup permitGroupInfo);

		int Delete(PermitGroup permitGroupInfo);
	}
}
