using System.Collections.Generic;
using System.Data.SqlClient;
using DeXin.EPG.Model;

namespace DeXin.EPG.IDAL
{
	public interface ILogDAL
	{
		IList<LogInfo> GetLogDataByPaging(PagingSearch pagingSearch);

		int AddLog(LogInfo logInfo);

		int AddLog(LogInfo logInfo, SqlTransaction sqltrans);

		bool AddLogs(IList<LogInfo> logs);
	}
}
