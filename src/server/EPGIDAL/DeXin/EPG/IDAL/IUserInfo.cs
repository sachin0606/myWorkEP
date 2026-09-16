using System.Collections.Generic;
using DeXin.EPG.Model;

namespace DeXin.EPG.IDAL
{
	public interface IUserInfo
	{
		UserInfo GetUserInfoByLoginName(string loginName);

		UserInfo GetUserByID(string userId);

		IList<UserInfo> GetUserByGroupID(string groupId);

		int Add_UpdateUserInfo(UserInfo user);

		IList<UserInfo> SearchUserInfosByCondiction(string userID, string userName, string sex, string groupID, int PageIndex, int PageSize);

		int DeleteUserInfo(params string[] users);

		int DeleteStatusUserInfo(string lastEditor, params string[] users);
	}
}
