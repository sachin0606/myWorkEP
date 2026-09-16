using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;

namespace Dexin.Util.Common.Resource
{
	public class SatelliteResource
	{
		private IList<DictionaryEntry> _ResourceList;

		public IList<DictionaryEntry> ResourceList
		{
			get
			{
				return _ResourceList;
			}
			set
			{
				_ResourceList = value;
			}
		}

		public SatelliteResource(Assembly assembly)
		{
			_ResourceList = new List<DictionaryEntry>();
			Assembly satelliteAssembly = assembly.GetSatelliteAssembly(CultureInfo.CurrentUICulture);
			string[] manifestResourceNames = satelliteAssembly.GetManifestResourceNames();
			foreach (string text in manifestResourceNames)
			{
				ResourceLocation resourceLocation = satelliteAssembly.GetManifestResourceInfo(text).ResourceLocation;
				if ((resourceLocation & ResourceLocation.ContainedInAnotherAssembly) == 0)
				{
					Stream manifestResourceStream = satelliteAssembly.GetManifestResourceStream(text);
					using (ResourceReader reader = new ResourceReader(manifestResourceStream))
					{
						EnumerateResources(reader, text);
					}
				}
			}
		}

		private void EnumerateResources(ResourceReader reader, string resourceName)
		{
			foreach (DictionaryEntry item in reader)
			{
				_ResourceList.Add(item);
			}
		}
	}
}
