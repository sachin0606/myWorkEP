using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeXin.EPG.DALFactory;
using DeXin.EPG.Model;

namespace DeXin.EPG.BLL
{
	public class ReMappingPIDBLL
	{
		private AbstractFactory abstractfacory = AbstractFactory.CreateFactory();

		public IList<PidOutputInfo> GetData()
		{
			try
			{
				return abstractfacory.CreateReMappingPIDDAL().GetData();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public IList<PidOutputInfo> GetDataByID(long id)
		{
			try
			{
				return abstractfacory.CreateReMappingPIDDAL().GetDataByID(id);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public IList<PidOutputInfo> GetDataByTSTableID(long tsTableId)
		{
			try
			{
				return abstractfacory.CreateReMappingPIDDAL().GetDataByTSTableID(tsTableId);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public IList<PidOutputInfo> GetDataByNetworkTableID(long networkTableId)
		{
			try
			{
				return abstractfacory.CreateReMappingPIDDAL().GetDataByNetworkTableID(networkTableId);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Add(PidOutputInfo multileIP)
		{
			try
			{
				return abstractfacory.CreateReMappingPIDDAL().Add(multileIP);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Update(PidOutputInfo multileIP)
		{
			try
			{
				return abstractfacory.CreateReMappingPIDDAL().Update(multileIP);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Delete(PidOutputInfo multileIP)
		{
			try
			{
				return abstractfacory.CreateReMappingPIDDAL().Delete(multileIP);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Delete(IList<PidOutputInfo> multileIPs)
		{
			try
			{
				return abstractfacory.CreateReMappingPIDDAL().Delete(multileIPs);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public IList<string> ExportToCSVData()
		{
			List<string> list = new List<string>();
			IEnumerable<IGrouping<long, PidOutputInfo>> enumerable = from x in GetData()
				group x by x.TS.NitTableId;
			if (enumerable.Count() == 0)
			{
				return list;
			}
			list.Add("PID Remapping Configuration");
			list.Add("");
			StringBuilder stringBuilder = new StringBuilder();
			foreach (IGrouping<long, PidOutputInfo> item in enumerable)
			{
				stringBuilder.Append("Network ID,0x");
				stringBuilder.Append(item.First().TS.NIT.NetworkId.ToString("X2"));
				list.Add(stringBuilder.ToString());
				stringBuilder.Clear();
				stringBuilder.Append("Network Name,");
				stringBuilder.Append(item.First().TS.NIT.NetworkName);
				list.Add(stringBuilder.ToString());
				stringBuilder.Clear();
				list.Add("");
				list.Add("TS Name,PAT,CAT,SDT or BAT,NIT,TDT or TOD,EIT");
				foreach (PidOutputInfo item2 in item)
				{
					stringBuilder.Append(item2.TS.TsName);
					stringBuilder.Append(",0x");
					stringBuilder.Append(item2.Pat.ToString("X2"));
					stringBuilder.Append(",0x");
					stringBuilder.Append(item2.Cat.ToString("X2"));
					stringBuilder.Append(",0x");
					stringBuilder.Append(item2.SdtOrBat.ToString("X2"));
					stringBuilder.Append(",0x");
					stringBuilder.Append(item2.Nit.ToString("X2"));
					stringBuilder.Append(",0x");
					stringBuilder.Append(item2.TotOrTdt.ToString("X2"));
					stringBuilder.Append(",0x");
					stringBuilder.Append(item2.Eit.ToString("X2"));
					list.Add(stringBuilder.ToString());
					stringBuilder.Clear();
				}
				list.Add("");
				list.Add("");
				list.Add("");
			}
			return list;
		}
	}
}
