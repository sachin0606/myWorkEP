using System;
using System.Collections.Generic;
using DeXin.EPG.DALFactory;
using DeXin.EPG.Model;

namespace DeXin.EPG.BLL
{
	public class GroupInfoBLL
	{
		private AbstractFactory af = AbstractFactory.CreateFactory();

		public IList<GroupInfo> GetAllData()
		{
			try
			{
				return af.CreateGroupInfoDAL().GetAllData();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public IList<GroupInfo> GetDataByGroupID(int intgroupid)
		{
			try
			{
				return af.CreateGroupInfoDAL().GetDataByGroupID(intgroupid);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Add(GroupInfo groupinfo)
		{
			int num = 0;
			try
			{
				return af.CreateGroupInfoDAL().Add(groupinfo);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Delete(GroupInfo groupinfo)
		{
			try
			{
				return af.CreateGroupInfoDAL().Delete(groupinfo);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int DeleteWithTrans(List<GroupInfo> lst)
		{
			try
			{
				return af.CreateGroupInfoDAL().DeleteWithTrans(lst);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Update(GroupInfo groupinfo)
		{
			try
			{
				return af.CreateGroupInfoDAL().Update(groupinfo);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
