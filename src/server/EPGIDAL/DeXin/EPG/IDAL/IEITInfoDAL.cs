using System;
using System.Collections.Generic;
using DeXin.EPG.Model;

namespace DeXin.EPG.IDAL
{
	public interface IEITInfoDAL
	{
		EitInfo GetbyID(long Identity);

		IList<EitInfo> GetByProgramIdentity(long ProgramIdentity);

		int GetEITIdFromEITInfo(ProgramInfo Program);

		int Save(IList<EitInfo> EITList, bool AllowIgnore);

		void Delete(IList<long> ListIdentity);

		IList<EitInfo> GetOutputEIT(DateTime utcNowTime, int days);

		void DeleteOutofdate(DateTime utcNowTime);

		void DeleteByProgramTableId(long prgtableid, DateTime utcNowTime);

		IList<EitInfo> GetByProgramIdentity(long programIdentity, int pageIndex, int pageSize, out int totalSize);

		void DeleteAll();
	}
}
