using System;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(2, "TaxAccType")]
public class TaxAccAndInvoiceDocsTypeSE : ASSmartEnum<TaxAccAndInvoiceDocsTypeSE, string>
{
	public static readonly TaxAccAndInvoiceDocsTypeSE Goods = new TaxAccAndInvoiceDocsTypeSE("Goods", "1", () => Resources.TaxInvDocGoods);

	public static readonly TaxAccAndInvoiceDocsTypeSE Services = new TaxAccAndInvoiceDocsTypeSE("Services", "2", () => Resources.TaxInvDocService);

	public static readonly TaxAccAndInvoiceDocsTypeSE Excise = new TaxAccAndInvoiceDocsTypeSE("Excise", "3", () => Resources.TaxInvExcise);

	public static readonly TaxAccAndInvoiceDocsTypeSE Leasing = new TaxAccAndInvoiceDocsTypeSE("Leasing", "4", () => Resources.EInvTypeLeasing);

	public static readonly TaxAccAndInvoiceDocsTypeSE VatReturn = new TaxAccAndInvoiceDocsTypeSE("VatReturn", "5", () => Resources.EInvTypeVatRerutn);

	public static readonly TaxAccAndInvoiceDocsTypeSE AccDocGoods = new TaxAccAndInvoiceDocsTypeSE("AccDocGoods", "11", () => Resources.AccDocGoods);

	public static readonly TaxAccAndInvoiceDocsTypeSE AccDocServices = new TaxAccAndInvoiceDocsTypeSE("AccDocServices", "12", () => Resources.AccDocServices);

	public static readonly TaxAccAndInvoiceDocsTypeSE AccDocExportEEU = new TaxAccAndInvoiceDocsTypeSE("AccDocExportEEU", "14", () => Resources.AccDocExportEEU);

	public static readonly TaxAccAndInvoiceDocsTypeSE AccDocTraceableG = new TaxAccAndInvoiceDocsTypeSE("AccDocTraceableG", "15", () => Resources.AccDocTraceableG);

	public static readonly TaxAccAndInvoiceDocsTypeSE AccDocTransportation = new TaxAccAndInvoiceDocsTypeSE("AccDocTransportation", "13", () => Resources.AccDocTransportation);

	public const string GoodsConst = "1";

	public const string ServicesConst = "2";

	public const string ExciseConst = "3";

	public const string LeasingConst = "4";

	public const string VatReturnConst = "5";

	public const string AccDocGoodsConst = "11";

	public const string AccDocServicesConst = "12";

	public const string AccDocTransportationConst = "13";

	public const string AccDocExportEEUConst = "14";

	public const string AccDocTraceableGConst = "15";

	public TaxAccAndInvoiceDocsTypeSE(string name, string value, Func<string> captionDelegate)
		: base(name, value, captionDelegate)
	{
	}
}
