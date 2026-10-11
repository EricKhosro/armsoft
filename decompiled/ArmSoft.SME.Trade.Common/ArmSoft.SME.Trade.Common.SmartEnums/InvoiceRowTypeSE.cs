using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "TypeWord")]
public class InvoiceRowTypeSE : ASSmartEnum<InvoiceRowTypeSE, string>
{
	public static readonly InvoiceRowTypeSE Goods = new InvoiceRowTypeSE("Goods", "1", () => Resources.Goods);

	public static readonly InvoiceRowTypeSE Services = new InvoiceRowTypeSE("Services", "2", () => ArmSoft.SME.Common.Resources.Services);

	public const string GoodsConst = "1";

	public const string ServicesConst = "2";

	public InvoiceRowTypeSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
