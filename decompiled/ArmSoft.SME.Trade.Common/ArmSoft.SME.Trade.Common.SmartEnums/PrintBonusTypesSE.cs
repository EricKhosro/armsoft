using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "PrintBonusAs")]
public class PrintBonusTypesSE : ASSmartEnum<PrintBonusTypesSE, string>
{
	public static readonly PrintBonusTypesSE Cash = new PrintBonusTypesSE("Cash", "1", () => ArmSoft.SME.Common.Resources.CashSum);

	public static readonly PrintBonusTypesSE DiscountAndPrePayment = new PrintBonusTypesSE("DiscountAndPrePayment", "2", () => Resources.DiscountAndPrePayment);

	public const string CashConst = "1";

	public const string DiscountAndPrePaymentConst = "2";

	public PrintBonusTypesSE(string name, string value, Func<string> captionDelegate)
		: base(name, value, captionDelegate)
	{
	}
}
