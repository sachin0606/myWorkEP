using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Office.Interop.Excel
{
	[ComImport]
	[CompilerGenerated]
	[Guid("000208D5-0000-0000-C000-000000000046")]
	[DefaultMember("_Default")]
	[TypeIdentifier]
	public interface _Application
	{
		Application Application
		{
			[DispId(148)]
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		void _VtblGap1_44();

		Workbooks Workbooks
		{
			[DispId(572)]
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		void _VtblGap2_66();

		bool DisplayAlerts
		{
			[DispId(343)]
			[LCIDConversion(0)]
			get;
			[DispId(343)]
			[LCIDConversion(0)]
			[param: In]
			set;
		}

		void _VtblGap3_109();

		[DispId(302)]
		void Quit();

		void _VtblGap4_44();

		bool UserControl
		{
			[DispId(1210)]
			get;
			[DispId(1210)]
			[param: In]
			set;
		}
	}
}
