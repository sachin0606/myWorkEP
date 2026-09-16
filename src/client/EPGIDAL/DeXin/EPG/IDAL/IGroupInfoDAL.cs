using System.Collections.Generic;
using DeXin.EPG.Model;

namespace DeXin.EPG.IDAL
{
	public interface IGroupInfoDAL
	{
		IList<GroupInfo> GetAllData();

		IList<GroupInfo> GetDataByGroupID(int intGroupID);

		int Add(GroupInfo myUserInfo);

		int Delete(GroupInfo myGroupInfo);

		int DeleteWithTrans(List<GroupInfo> lstPermitInfo);

		int Update(GroupInfo myGroupInfo);
	}
}
