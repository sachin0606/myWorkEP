using System.Collections.Generic;
using DeXin.EPG.Model;

namespace DeXin.EPG.IDAL
{
	public interface IGenerator
	{
		IList<NitInfo> GetSendInfo();
	}
}
