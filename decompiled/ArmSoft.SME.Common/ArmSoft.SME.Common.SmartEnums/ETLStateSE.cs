using System;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(2, "ETLStates")]
public class ETLStateSE : ASSmartEnum<ETLStateSE, string>
{
	public static readonly ETLStateSE NotForExport = new ETLStateSE("NotForExport", "0", () => Resources.NotForETLExport);

	public static readonly ETLStateSE ForExport = new ETLStateSE("ForExport", "1", () => Resources.ForETLExport);

	public static readonly ETLStateSE Exported = new ETLStateSE("Exported", "2", () => Resources.ETLExported);

	public const string NotForExportConst = "0";

	public const string ForExportConst = "1";

	public const string ExportedConst = "2";

	protected ETLStateSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
