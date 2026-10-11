using System;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "Type")]
public class TerminalTypesSE : ASSmartEnum<TerminalTypesSE, string>
{
	public static readonly TerminalTypesSE Honeywell = new TerminalTypesSE("Honeywell", "1", () => "Honeywell");

	public static readonly TerminalTypesSE CipherLab = new TerminalTypesSE("CipherLab", "2", () => "CipherLab");

	public const string HoneywellConst = "1";

	public const string CipherLabConst = "2";

	public TerminalTypesSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
