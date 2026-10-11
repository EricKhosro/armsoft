using System;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(1, "ApplicationType")]
public class AppTypeSE : ASSmartEnum<AppTypeSE, short>
{
	public static readonly AppTypeSE OldDesktop = new AppTypeSE("OldDesktop", 0, () => Resources.Desktop7);

	public static readonly AppTypeSE Mobile = new AppTypeSE("Mobile", 1, () => Resources.MobileApp);

	public static readonly AppTypeSE Web = new AppTypeSE("Web", 2, () => Resources.WebSite);

	public static readonly AppTypeSE Desktop = new AppTypeSE("Desktop", 3, () => Resources.Desktop);

	public static readonly AppTypeSE Excel = new AppTypeSE("Excel", 4, () => "Excel");

	public static readonly AppTypeSE Api = new AppTypeSE("Api", 5, () => "Api");

	public static readonly AppTypeSE ACS = new AppTypeSE("ACS", 6, () => "ACS");

	public static readonly AppTypeSE FinMobile = new AppTypeSE("FinMobile", 7, () => Resources.FinMobile);

	public static readonly AppTypeSE Other = new AppTypeSE("Other", 100, () => Resources.Other);

	public AppTypeSE(string name, short value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
