using System;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(2, "TaxInvoices")]
public class TaxAccAndInvoiceOldDocsTypeSE : ASSmartEnum<TaxAccAndInvoiceOldDocsTypeSE, string>
{
	public static readonly TaxAccAndInvoiceOldDocsTypeSE Goods = new TaxAccAndInvoiceOldDocsTypeSE("Goods", "1", () => Resources.TaxAccountOfGoods);

	public static readonly TaxAccAndInvoiceOldDocsTypeSE Excise = new TaxAccAndInvoiceOldDocsTypeSE("Excise", "2", () => Resources.TaxInvExcise);

	public static readonly TaxAccAndInvoiceOldDocsTypeSE Services = new TaxAccAndInvoiceOldDocsTypeSE("Services", "3", () => Resources.TaxAccountOfServices);

	public static readonly TaxAccAndInvoiceOldDocsTypeSE AccDocGoods = new TaxAccAndInvoiceOldDocsTypeSE("AccDocGoods", "4", () => Resources.AccDocGoods);

	public static readonly TaxAccAndInvoiceOldDocsTypeSE AccDocServices = new TaxAccAndInvoiceOldDocsTypeSE("AccDocServices", "5", () => Resources.AccDocServices);

	public static readonly TaxAccAndInvoiceOldDocsTypeSE B2BDoc = new TaxAccAndInvoiceOldDocsTypeSE("B2BDoc", "6", () => Resources.LogCommentImportedFromB2B);

	public TaxAccAndInvoiceOldDocsTypeSE(string name, string value, Func<string> captionDelegate)
		: base(name, value, captionDelegate)
	{
	}
}
