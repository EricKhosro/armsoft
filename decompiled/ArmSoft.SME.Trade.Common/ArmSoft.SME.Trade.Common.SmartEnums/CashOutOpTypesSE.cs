using System;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "OpType")]
public class CashOutOpTypesSE : ASSmartEnum<CashOutOpTypesSE, string>
{
	public static readonly CashOutOpTypesSE ToPartner = new CashOutOpTypesSE("ToPartner", "1", () => Resources.OpTypeToPartner);

	public static readonly CashOutOpTypesSE RevenueOutput = new CashOutOpTypesSE("RevenueOutput", "2", () => Resources.CashOutOpTypeAmountOutput);

	public static readonly CashOutOpTypesSE ToSettlementAcc = new CashOutOpTypesSE("ToSettlementAcc", "3", () => Resources.OpTypeToSettlAcc);

	public static readonly CashOutOpTypesSE ByPlCard = new CashOutOpTypesSE("ByPlCard", "4", () => Resources.CashOutOpTypeByPlCard);

	public const string ToPartnerConst = "1";

	public const string RevenueOutputConst = "2";

	public const string ToSettlementAccConst = "3";

	public const string ByPlCardConst = "4";

	public CashOutOpTypesSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
