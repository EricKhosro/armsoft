using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(2, "TaxInvoices")]
public class TaxInvTypesOldSE : ASSmartEnum<TaxInvTypesOldSE, string>
{
	public static readonly TaxInvTypesOldSE Goods = new TaxInvTypesOldSE("Goods", "1", () => ArmSoft.SME.Common.Resources.TaxAccountOfGoods);

	public static readonly TaxInvTypesOldSE Excise = new TaxInvTypesOldSE("Excise", "2", () => ArmSoft.SME.Common.Resources.TaxInvExcise);

	public static readonly TaxInvTypesOldSE Services = new TaxInvTypesOldSE("Services", "3", () => ArmSoft.SME.Common.Resources.TaxAccountOfServices);

	public static readonly TaxInvTypesOldSE AccDocGoods = new TaxInvTypesOldSE("AccDocGoods", "4", () => Resources.TaxInvTypeAccDoc + "(" + Resources.Goods + ")");

	public static readonly TaxInvTypesOldSE B2BDoc = new TaxInvTypesOldSE("B2BDoc", "5", () => ArmSoft.SME.Common.Resources.LogCommentImportedFromB2B);

	public static readonly TaxInvTypesOldSE AccDocServices = new TaxInvTypesOldSE("AccDocServices", "6", () => Resources.TaxInvTypeAccDoc + "(" + ArmSoft.SME.Common.Resources.Services + ")");

	public const string GoodsConst = "1";

	public const string ExciseConst = "2";

	public const string ServicesConst = "3";

	public const string AccDocGoodsConst = "4";

	public const string B2BDocConst = "5";

	public const string AccDocServicesConst = "6";

	public TaxInvTypesOldSE(string name, string value, Func<string> captionDelegate)
		: base(name, value, captionDelegate)
	{
	}
}
