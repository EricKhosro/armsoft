using System;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "OpType")]
public class BankInOpTypeSE : ASSmartEnum<BankInOpTypeSE, string>
{
	public static readonly BankInOpTypeSE FromPartner = new BankInOpTypeSE("FromPartner", "1", () => Resources.OpTypeFromPartner);

	public static readonly BankInOpTypeSE FromSettlementAcc = new BankInOpTypeSE("FromSettlementAcc", "2", () => Resources.OpTypeFromSettlAcc);

	public const string FromPartnerConst = "1";

	public const string FromSettlementAccConst = "2";

	public BankInOpTypeSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
