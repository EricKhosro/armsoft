using System;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "ScannerReadModes")]
public class ScanerModesSE : ASSmartEnum<ScanerModesSE, string>
{
	public static readonly ScanerModesSE MergeWithFirstMatchedRow = new ScanerModesSE("MergeWithFirstMatchedRow", "1", () => Resources.MergeWithFirstMatchedRow);

	public static readonly ScanerModesSE MergeWithCurrentRow = new ScanerModesSE("MergeWithCurrentRow", "2", () => Resources.MergeWithCurrentRow);

	public static readonly ScanerModesSE AddNewRow = new ScanerModesSE("AddNewRow", "3", () => Resources.AddNewRow);

	public const string MergeWithFirstMatchedRowConst = "1";

	public const string MergeWithCurrentRowConst = "2";

	public const string AddNewRowConst = "3";

	public ScanerModesSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
