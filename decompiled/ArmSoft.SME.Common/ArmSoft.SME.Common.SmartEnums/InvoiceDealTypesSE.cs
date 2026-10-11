using System;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(1, "DealTypes")]
public class InvoiceDealTypesSE : ASSmartEnum<InvoiceDealTypesSE, short>
{
	public static readonly InvoiceDealTypesSE VatOther = new InvoiceDealTypesSE("VatOther", 0, "3", () => Resources.Other);

	public static readonly InvoiceDealTypesSE VatZeroRate = new InvoiceDealTypesSE("VatZeroRate", 1, "1", () => Resources.VatZeroRate);

	public static readonly InvoiceDealTypesSE VatExempt = new InvoiceDealTypesSE("VatExempt", 2, "2", () => Resources.VatExempt);

	public string ValueUI { get; set; }

	public InvoiceDealTypesSE(string name, short value, string valueUI, Func<string> caption)
		: base(name, value, caption)
	{
		ValueUI = valueUI;
	}
}
