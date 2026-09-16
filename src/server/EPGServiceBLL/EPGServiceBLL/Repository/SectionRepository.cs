using System.Collections.Generic;

namespace EPGServiceBLL.Repository
{
	public class SectionRepository
	{
		public Dictionary<long, IList<byte[]>> PATSectionListTable = new Dictionary<long, IList<byte[]>>();

		public Dictionary<long, IList<byte[]>> CATSectionListTable = new Dictionary<long, IList<byte[]>>();

		public Dictionary<long, Dictionary<long, IList<byte[]>>> PMTSectionListTable = new Dictionary<long, Dictionary<long, IList<byte[]>>>();

		public Dictionary<long, IList<byte[]>> NIT_ACTSectionListTable = new Dictionary<long, IList<byte[]>>();

		public Dictionary<long, IList<byte[]>> NIT_OTHSectionListTable = new Dictionary<long, IList<byte[]>>();

		public Dictionary<long, IList<byte[]>> BATSectionListTable = new Dictionary<long, IList<byte[]>>();

		public Dictionary<long, IList<byte[]>> SDT_ACTSectionListTable = new Dictionary<long, IList<byte[]>>();

		public Dictionary<long, IList<byte[]>> SDT_OTHSectionListTable = new Dictionary<long, IList<byte[]>>();

		public Dictionary<long, Dictionary<long, IList<byte[]>>> EIT_ACT_PFSectionListTable = new Dictionary<long, Dictionary<long, IList<byte[]>>>();

		public Dictionary<long, Dictionary<long, IList<byte[]>>> EIT_ACT_SCSectionListTable = new Dictionary<long, Dictionary<long, IList<byte[]>>>();

		public Dictionary<long, IList<byte[]>> TDTSectionListTable = new Dictionary<long, IList<byte[]>>();

		public Dictionary<long, IList<byte[]>> TOTSectionListTable = new Dictionary<long, IList<byte[]>>();
	}
}
