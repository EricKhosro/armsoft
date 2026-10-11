using System;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "OpType")]
public class BankOutOpTypeSE : ASSmartEnum<BankOutOpTypeSE, string>
{
	public static readonly BankOutOpTypeSE ToPartner = new BankOutOpTypeSE("ToPartner", "1", () => Resources.OpTypeToPartner);

	public static readonly BankOutOpTypeSE ToSettlementAcc = new BankOutOpTypeSE("ToSettlementAcc", "2", () => Resources.OpTypeToSettlAcc);

	public const string ToPartnerConst = "1";

	public const string ToSettlementAccConst = "2";

	public BankOutOpTypeSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
