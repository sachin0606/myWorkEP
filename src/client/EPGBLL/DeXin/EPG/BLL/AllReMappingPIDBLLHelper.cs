using System;
using System.Collections.Generic;
using System.Linq;
using DeXin.EPG.Model;

namespace DeXin.EPG.BLL
{
	public class AllReMappingPIDBLLHelper : IComparer<KeyValuePair<string, string>>
	{
		protected const int NULL_PK_PID = 8191;

		public Dictionary<string, string> GetAllPid()
		{
			try
			{
				ReMappingPIDBLL reMappingPIDBLL = new ReMappingPIDBLL();
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				IList<PidOutputInfo> data = reMappingPIDBLL.GetData();
				foreach (PidOutputInfo item in data)
				{
					if (item.Pat != 8191)
					{
						dictionary.Add(item.Pat.ToString(), item.Pat.ToString());
					}
					if (item.Cat != 8191)
					{
						dictionary.Add(item.Cat.ToString(), item.Cat.ToString());
					}
					if (item.SdtOrBat != 8191)
					{
						dictionary.Add(item.SdtOrBat.ToString(), item.SdtOrBat.ToString());
					}
					if (item.Nit != 8191)
					{
						dictionary.Add(item.Nit.ToString(), item.Nit.ToString());
					}
					if (item.TotOrTdt != 8191)
					{
						dictionary.Add(item.TotOrTdt.ToString(), item.TotOrTdt.ToString());
					}
					if (item.Eit != 8191)
					{
						dictionary.Add(item.Eit.ToString(), item.Eit.ToString());
					}
				}
				PMTPIDReMappingBLL pMTPIDReMappingBLL = new PMTPIDReMappingBLL();
				IList<PmtPidOutputInfo> list = (from x in pMTPIDReMappingBLL.GetData()
					where x.OutputPid != 8191
					select x).ToList();
				foreach (PmtPidOutputInfo item2 in list)
				{
					dictionary.Add(item2.OutputPid.ToString(), item2.OutputPid.ToString());
				}
				return dictionary;
			}
			catch (ArgumentException ex)
			{
				throw ex;
			}
			catch (Exception ex2)
			{
				throw ex2;
			}
		}

		public int Compare(KeyValuePair<string, string> x, KeyValuePair<string, string> y)
		{
			try
			{
				int num = int.Parse(x.Key);
				int num2 = int.Parse(y.Key);
				if (num >= num2)
				{
					return 1;
				}
				if (num == num2)
				{
					return 0;
				}
				return -1;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
