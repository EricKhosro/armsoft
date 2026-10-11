using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Constants;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "PaymentSystem")]
public class NonCashPaymentMethodsSE : ASSmartEnum<NonCashPaymentMethodsSE, short>
{
	public static readonly NonCashPaymentMethodsSE None = new NonCashPaymentMethodsSE("None", 0, () => string.Empty, null);

	public static readonly NonCashPaymentMethodsSE CardPayment = new NonCashPaymentMethodsSE("CardPayment", 11, () => ArmSoft.SME.Common.Resources.CardPayment, "creditcard_16.svg");

	public static readonly NonCashPaymentMethodsSE ARCA = new NonCashPaymentMethodsSE("ARCA", 1, ((object)ArmSoft.SME.Common.Constants.Common.NonCashPaymentType.ARCA).ToString, "arca_16.svg");

	public static readonly NonCashPaymentMethodsSE VISA = new NonCashPaymentMethodsSE("VISA", 2, ((object)ArmSoft.SME.Common.Constants.Common.NonCashPaymentType.VISA).ToString, "visa_16.svg");

	public static readonly NonCashPaymentMethodsSE MASTER = new NonCashPaymentMethodsSE("MASTER", 3, ((object)ArmSoft.SME.Common.Constants.Common.NonCashPaymentType.MASTER).ToString, "mastercard_16.svg");

	public static readonly NonCashPaymentMethodsSE AMEX = new NonCashPaymentMethodsSE("AMEX", 4, ((object)ArmSoft.SME.Common.Constants.Common.NonCashPaymentType.AMEX).ToString, "amex_16.svg");

	public static readonly NonCashPaymentMethodsSE MIR = new NonCashPaymentMethodsSE("MIR", 10, ((object)ArmSoft.SME.Common.Constants.Common.NonCashPaymentType.MIR).ToString, "mir_16.svg");

	public static readonly NonCashPaymentMethodsSE PayX = new NonCashPaymentMethodsSE("PayX", 5, () => ArmSoft.SME.Common.Resources.PayX, "fav_payx_16.svg");

	public static readonly NonCashPaymentMethodsSE Idram = new NonCashPaymentMethodsSE("Idram", 6, () => ArmSoft.SME.Common.Resources.Idram, "fav_idram_16.svg");

	public static readonly NonCashPaymentMethodsSE InecoPay = new NonCashPaymentMethodsSE("InecoPay", 7, () => ArmSoft.SME.Common.Resources.InecoPay, "fav_inecopay_16.svg");

	public static readonly NonCashPaymentMethodsSE Telcell = new NonCashPaymentMethodsSE("Telcell", 9, () => ArmSoft.SME.Common.Resources.Telcell, "fav_telcell_16.svg");

	public string ImagePath { get; }

	public NonCashPaymentMethodsSE(string name, short value, Func<string> captionDelegate, string imagePath)
		: base(name, value, captionDelegate)
	{
		ImagePath = imagePath;
	}
}
