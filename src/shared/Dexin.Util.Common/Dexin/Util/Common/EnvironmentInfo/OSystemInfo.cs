using System;

namespace Dexin.Util.Common.EnvironmentInfo
{
	public class OSystemInfo
	{
		public OSystemTypes GetCurrentOSystemType()
		{
			Version version = Environment.OSVersion.Version;
			if (version.Major == 5)
			{
				if (version.Minor == 0)
				{
					return OSystemTypes.Win2000;
				}
				if (version.Minor == 1)
				{
					return OSystemTypes.WinXP;
				}
				if (version.Minor == 2)
				{
					return OSystemTypes.Win2003;
				}
			}
			else if (version.Major == 6)
			{
				if (version.Minor == 0)
				{
					return OSystemTypes.WinVista;
				}
				if (version.Minor == 1)
				{
					return OSystemTypes.Win7Or2008;
				}
				if (version.Minor == 2)
				{
					return OSystemTypes.Win8Or10;
				}
			}
			return OSystemTypes.Unkonwn;
		}
	}
}
