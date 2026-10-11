using System;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(2, "ElSupportSystems")]
public class BankingModesSE : ASSmartEnum<BankingModesSE, string>
{
	public static readonly BankingModesSE InternetBank = new BankingModesSE("InternetBank", "1", () => Resources.InternetBank);

	public static readonly BankingModesSE ClientBank = new BankingModesSE("ClientBank", "2", () => Resources.ClientBank);

	public const string InternetBankConst = "1";

	public const string ClientBankConst = "2";

	public BankingModesSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
