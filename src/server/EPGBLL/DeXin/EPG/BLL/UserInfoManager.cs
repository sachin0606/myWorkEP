using System;
using System.Collections.Generic;
using System.Linq;
using Com.Cn.Tool;
using DeXin.EPG.DALFactory;
using DeXin.EPG.Model;
using DeXin.EPG.Resource;
using DeXin.EPG.Tools;

namespace DeXin.EPG.BLL
{
	public class UserInfoManager
	{
		private AbstractFactory factory = AbstractFactory.CreateFactory();

		public IList<UserInfo> SearchUserInfosByCondiction(string userID, string userName, string sex, string groupID, int PageIndex, int PageSize)
		{
			IList<UserInfo> list = null;
			try
			{
				return factory.CreateUserInfoService().SearchUserInfosByCondiction(userID, userName, sex, groupID, PageIndex, PageSize);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public UserInfo GetUserByID(string userId)
		{
			UserInfo userInfo = null;
			try
			{
				return factory.CreateUserInfoService().GetUserByID(userId);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public IList<UserInfo> GetUserByGroupID(string groupId)
		{
			IList<UserInfo> list = null;
			try
			{
				return factory.CreateUserInfoService().GetUserByGroupID(groupId);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public bool AddUserInfo(UserInfo user, out string msg)
		{
			bool result = false;
			try
			{
				UserInfo userByID = GetUserByID(user.UserID);
				if (userByID == null)
				{
					if (factory.CreateUserInfoService().Add_UpdateUserInfo(user) > 0)
					{
						result = true;
						msg = LocationControl.Ins.LocationItems[LacationEnum.SaveSucceed];
					}
					else
					{
						msg = LocationControl.Ins.LocationItems[LacationEnum.SaveFailure];
					}
				}
				else
				{
					msg = ResourceManagement.GetLocationString("CustomerExist");
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public UserInfo CheckLogin(UserInfo user, out string msg)
		{
			UserInfo userInfo = null;
			msg = LocationControl.Ins.LocationItems[LacationEnum.LoinInSucceed];
			try
			{
				userInfo = GetUserByID(user.UserID);
				if (userInfo == null)
				{
					user = (userInfo = null);
					msg = LocationControl.Ins.LocationItems[LacationEnum.AccountIsNotExist];
				}
				else if (!userInfo.Password.Equals(user.Password))
				{
					user = (userInfo = null);
					msg = LocationControl.Ins.LocationItems[LacationEnum.PasswordIswrong];
				}
				else if (!userInfo.Status)
				{
					user = (userInfo = null);
					msg = LocationControl.Ins.LocationItems[LacationEnum.UserDisEnabled];
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return userInfo;
		}

		public bool UndateUserInfo(UserInfo user, out string msg)
		{
			bool result = false;
			try
			{
				UserInfo userByID = GetUserByID(user.UserID);
				if (userByID != null)
				{
					if (factory.CreateUserInfoService().Add_UpdateUserInfo(user) > 0)
					{
						result = true;
						msg = LocationControl.Ins.LocationItems[LacationEnum.EditSucceed];
					}
					else
					{
						msg = LocationControl.Ins.LocationItems[LacationEnum.EditFailure];
					}
				}
				else
				{
					msg = ResourceManagement.GetLocationString("CustomerNotExist");
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public bool DeleteStatusUserInfo(string lastEditor, IList<string> users, out string msg)
		{
			msg = "";
			bool flag = false;
			try
			{
				flag = factory.CreateUserInfoService().DeleteStatusUserInfo(lastEditor, users.ToArray()) > 0;
				msg = (flag ? LocationControl.Ins.LocationItems[LacationEnum.OperateSucceed] : LocationControl.Ins.LocationItems[LacationEnum.OperateFailure]);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return flag;
		}

		public int GetGroupIDByUserID(string userID)
		{
			int num = 0;
			try
			{
				UserInfo userByID = GetUserByID(userID);
				return Tool.GetInt32(userByID.GroupID);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
