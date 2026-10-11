using System;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(1, "TaxDocTypes")]
public class TaxDocTypeSE : ASSmartEnum<TaxDocTypeSE, short>
{
	public static readonly TaxDocTypeSE InvoiceDocs = new TaxDocTypeSE("InvoiceDocs", 1, () => Resources.TaxInvoices);

	public static readonly TaxDocTypeSE AccDocs = new TaxDocTypeSE("AccDocs", 2, () => Resources.TaxAccDocs);

	public TaxDocTypeSE(string name, short value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
