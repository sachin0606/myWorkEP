using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Office.Interop.Excel
{
	[ComImport]
	[CompilerGenerated]
	[TypeIdentifier]
	[Guid("000208DA-0000-0000-C000-000000000046")]
	public interface _Workbook
	{
		void _VtblGap1_20();

		[LCIDConversion(3)]
		[DispId(277)]
		void Close([Optional][In][MarshalAs(UnmanagedType.Struct)] object SaveChanges, [Optional][In][MarshalAs(UnmanagedType.Struct)] object Filename, [Optional][In][MarshalAs(UnmanagedType.Struct)] object RouteWorkbook);

		void _VtblGap2_103();

		Sheets Worksheets
		{
			[DispId(494)]
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}
	}
}
