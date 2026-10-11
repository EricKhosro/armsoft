using System;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "OpType")]
public class CashInOpTypesSE : ASSmartEnum<CashInOpTypesSE, string>
{
	public static readonly CashInOpTypesSE FromPartner = new CashInOpTypesSE("FromPartner", "1", () => Resources.OpTypeFromPartner);

	public static readonly CashInOpTypesSE RevenueInput = new CashInOpTypesSE("RevenueInput", "2", () => Resources.CashInOpTypeRevenueInput);

	public static readonly CashInOpTypesSE FromSettlementAcc = new CashInOpTypesSE("FromSettlementAcc", "3", () => Resources.OpTypeFromSettlAcc);

	public static readonly CashInOpTypesSE ByPlCard = new CashInOpTypesSE("ByPlCard", "4", () => Resources.CashInOpTypeByPlCard);

	public const string FromPartnerConst = "1";

	public const string RevenueInputConst = "2";

	public const string FromSettlementAccConst = "3";

	public const string ByPlCardConst = "4";

	public CashInOpTypesSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
