using System;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "ReportCurr")]
public class ReportCurrTypesSE : ASSmartEnum<ReportCurrTypesSE, string>
{
	public static readonly ReportCurrTypesSE NCOnly = new ReportCurrTypesSE("NCOnly", "1", () => Resources.ParTAccRepCurNC);

	public static readonly ReportCurrTypesSE FCOnly = new ReportCurrTypesSE("FCOnly", "2", () => Resources.ParTAccRepCurFC);

	public static readonly ReportCurrTypesSE NCAndFC = new ReportCurrTypesSE("NCAndFC", "3", () => Resources.ParTAccRepCurNFC);

	public ReportCurrTypesSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
