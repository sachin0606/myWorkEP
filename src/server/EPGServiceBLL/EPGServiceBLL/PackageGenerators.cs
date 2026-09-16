using System.Collections.Generic;
using System.Linq;

namespace EPGServiceBLL
{
	public class PackageGenerators
	{
		private static object lockObject = new object();

		private static PackageGenerators _Instance = new PackageGenerators();

		private Dictionary<long, Dictionary<ushort, SectionToPackageBLL>> generatorsTable = new Dictionary<long, Dictionary<ushort, SectionToPackageBLL>>();

		public static PackageGenerators Instance
		{
			get
			{
				return _Instance;
			}
		}

		public SectionToPackageBLL this[long tsKey, ushort PID]
		{
			get
			{
				lock (lockObject)
				{
					if (!generatorsTable.Keys.Contains(tsKey))
					{
						generatorsTable.Add(tsKey, new Dictionary<ushort, SectionToPackageBLL>());
					}
					if (!generatorsTable[tsKey].Keys.Contains(PID))
					{
						generatorsTable[tsKey].Add(PID, new SectionToPackageBLL());
					}
					return generatorsTable[tsKey][PID];
				}
			}
		}
	}
}
