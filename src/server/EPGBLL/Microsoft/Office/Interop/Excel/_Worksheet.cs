using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Office.Interop.Excel
{
	[ComImport]
	[CompilerGenerated]
	[TypeIdentifier]
	[Guid("000208D8-0000-0000-C000-000000000046")]
	public interface _Worksheet
	{
		void _VtblGap1_110();

		Range UsedRange
		{
			[DispId(412)]
			[LCIDConversion(0)]
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}
	}
}
