using System;
using System.Collections.Generic;
using System.Linq;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Tools;

// Same path as Bouquet Management delete in the Dexin client:
// BATInfoManager.Delete -> notice (op=3) + descriptors + bat_to_program + bat_info.
// Then SetAllNoticeOn submits those notices (state 0 -> 1).
internal static class RemoveAllBouquets
{
	public static int Main()
	{
		try
		{
			SqlHelper.SetConnectionString("Data Source=localhost;Initial Catalog=EPG2;Integrated Security=True");
			var loc = LocationControl.Ins;

			var nit = new NITInfoManager().GetAllNITInfos()[0];
			var mgr = new BATInfoManager();
			var bats = mgr.GetByNetworkIdentity(nit.Identity).ToList();
			Console.WriteLine("Bouquets before: {0}", bats.Count);
			if (bats.Count == 0)
			{
				Console.WriteLine("Nothing to delete");
				return 0;
			}

			var ids = new List<long>();
			foreach (var b in bats.OrderBy(x => x.BATId))
			{
				Console.WriteLine("  id={0} bat_id={1} {2}", b.Identity, b.BATId, b.BATName);
				ids.Add(b.Identity);
			}

			mgr.Delete(ids);
			int n = new SendNoticesManager().SetAllNoticeOn();
			Console.WriteLine("Deleted {0}; SetAllNoticeOn {1}", ids.Count, n);

			var left = mgr.GetByNetworkIdentity(nit.Identity).ToList();
			Console.WriteLine("Bouquets after: {0}", left.Count);
			return left.Count == 0 ? 0 : 2;
		}
		catch (Exception ex)
		{
			Console.WriteLine("ERROR: " + ex);
			return 1;
		}
	}
}
