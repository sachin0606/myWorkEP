using System.Collections.Generic;
using System.Linq;

namespace Dexin.Util.Common.ExtensionHelper
{
	public static class IListExtension
	{
		public static void Remove<T>(this IList<T> source, IEnumerable<T> removing)
		{
			foreach (T item in removing.ToList())
			{
				source.Remove(item);
			}
		}
	}
}
