using System.Collections.Generic;
using DeXin.EPG.Model;

namespace DeXin.EPG.IDAL
{
	public interface IDescriptorType
	{
		IList<DescriptorType> GetDescriptorTypesByLoaction(string args);

		DescriptorType GetDescriptorTypeByID(long args);

		bool AddDescriptorType(DescriptorType args);

		bool ModifyDescriptorType(DescriptorType args);

		bool DeleteDescriptorType(params long[] args);
	}
}
