using System;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "DocumentTypes")]
public class DiscountDocTypesSE : ASSmartEnum<DiscountDocTypesSE, short>
{
	public static readonly DiscountDocTypesSE MTInvoice = new DiscountDocTypesSE("MTInvoice", 1, () => Resources.DocInvoice);

	public static readonly DiscountDocTypesSE MTBill = new DiscountDocTypesSE("MTBill", 2, () => Resources.DocMTBill);

	public DiscountDocTypesSE(string name, short value, Func<string> captionDelegate)
		: base(name, value, captionDelegate)
	{
	}
}
