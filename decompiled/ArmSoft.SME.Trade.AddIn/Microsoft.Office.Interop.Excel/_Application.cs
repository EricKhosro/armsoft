using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Office.Interop.Excel;

[ComImport]
[CompilerGenerated]
[DefaultMember("_Default")]
[Guid("000208D5-0000-0000-C000-000000000046")]
[TypeIdentifier]
public interface _Application
{
	void _VtblGap1_11();

	[DispId(308)]
	Workbook ActiveWorkbook
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(308)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(549)]
	AddIns AddIns
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(549)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	void _VtblGap2_30();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(779)]
	[LCIDConversion(30)]
	[return: MarshalAs(UnmanagedType.Interface)]
	Range Union([In][MarshalAs(UnmanagedType.Interface)] Range Arg1, [In][MarshalAs(UnmanagedType.Interface)] Range Arg2, [Optional][In][MarshalAs(UnmanagedType.Struct)] object Arg3, [Optional][In][MarshalAs(UnmanagedType.Struct)] object Arg4, [Optional][In][MarshalAs(UnmanagedType.Struct)] object Arg5, [Optional][In][MarshalAs(UnmanagedType.Struct)] object Arg6, [Optional][In][MarshalAs(UnmanagedType.Struct)] object Arg7, [Optional][In][MarshalAs(UnmanagedType.Struct)] object Arg8, [Optional][In][MarshalAs(UnmanagedType.Struct)] object Arg9, [Optional][In][MarshalAs(UnmanagedType.Struct)] object Arg10, [Optional][In][MarshalAs(UnmanagedType.Struct)] object Arg11, [Optional][In][MarshalAs(UnmanagedType.Struct)] object Arg12, [Optional][In][MarshalAs(UnmanagedType.Struct)] object Arg13, [Optional][In][MarshalAs(UnmanagedType.Struct)] object Arg14, [Optional][In][MarshalAs(UnmanagedType.Struct)] object Arg15, [Optional][In][MarshalAs(UnmanagedType.Struct)] object Arg16, [Optional][In][MarshalAs(UnmanagedType.Struct)] object Arg17, [Optional][In][MarshalAs(UnmanagedType.Struct)] object Arg18, [Optional][In][MarshalAs(UnmanagedType.Struct)] object Arg19, [Optional][In][MarshalAs(UnmanagedType.Struct)] object Arg20, [Optional][In][MarshalAs(UnmanagedType.Struct)] object Arg21, [Optional][In][MarshalAs(UnmanagedType.Struct)] object Arg22, [Optional][In][MarshalAs(UnmanagedType.Struct)] object Arg23, [Optional][In][MarshalAs(UnmanagedType.Struct)] object Arg24, [Optional][In][MarshalAs(UnmanagedType.Struct)] object Arg25, [Optional][In][MarshalAs(UnmanagedType.Struct)] object Arg26, [Optional][In][MarshalAs(UnmanagedType.Struct)] object Arg27, [Optional][In][MarshalAs(UnmanagedType.Struct)] object Arg28, [Optional][In][MarshalAs(UnmanagedType.Struct)] object Arg29, [Optional][In][MarshalAs(UnmanagedType.Struct)] object Arg30);

	void _VtblGap3_1();

	[DispId(572)]
	Workbooks Workbooks
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(572)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	void _VtblGap4_19();

	[DispId(316)]
	XlCalculation Calculation
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(316)]
		[LCIDConversion(0)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[LCIDConversion(0)]
		[DispId(316)]
		[param: In]
		set;
	}

	void _VtblGap5_39();

	[DispId(0)]
	string _Default
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(0)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	void _VtblGap6_5();

	[DispId(343)]
	bool DisplayAlerts
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(343)]
		[LCIDConversion(0)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[LCIDConversion(0)]
		[DispId(343)]
		[param: In]
		set;
	}

	void _VtblGap7_122();

	[DispId(382)]
	bool ScreenUpdating
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[LCIDConversion(0)]
		[DispId(382)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(382)]
		[LCIDConversion(0)]
		[param: In]
		set;
	}

	void _VtblGap8_12();

	[DispId(386)]
	object StatusBar
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[LCIDConversion(0)]
		[DispId(386)]
		[return: MarshalAs(UnmanagedType.Struct)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(386)]
		[LCIDConversion(0)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Struct)]
		set;
	}

	void _VtblGap9_22();

	[DispId(392)]
	string Version
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[LCIDConversion(0)]
		[DispId(392)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	void _VtblGap10_136();

	[DispId(2775)]
	AddIns2 AddIns2
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2775)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}
}
