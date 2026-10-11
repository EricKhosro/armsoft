using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumPartial(SmartEnumType.Special)]
[SmartEnumProviderSupport(8, "TreeDefs")]
[SmartEnumPartial(SmartEnumType.Base)]
public class SubsystemTypesSE : ASSmartEnum<SubsystemTypesSE, short>
{
	public static readonly SubsystemTypesSE Purchases = new SubsystemTypesSE("Purchases", 1, () => Resources.Purchases);

	public static readonly SubsystemTypesSE Sales = new SubsystemTypesSE("Sales", 2, () => ArmSoft.SME.Common.Resources.Sales);

	public static readonly SubsystemTypesSE Storage = new SubsystemTypesSE("Storage", 3, () => ArmSoft.SME.Common.Resources.Storage);

	public static readonly SubsystemTypesSE CashBank = new SubsystemTypesSE("CashBank", 4, () => Resources.CashBank);

	public static readonly SubsystemTypesSE AnalysisReports = new SubsystemTypesSE("AnalysisReports", 5, () => ArmSoft.SME.Common.Resources.AnalysisCaption);

	public static readonly SubsystemTypesSE PointOfSale = new SubsystemTypesSE("PointOfSale", 7, () => Resources.PointOfSalePOS);

	public static readonly SubsystemTypesSE EInvoicing = new SubsystemTypesSE("EInvoicing", 8, () => ArmSoft.SME.Common.Resources.EInvoicing);

	public static readonly SubsystemTypesSE B2B = new SubsystemTypesSE("B2B", 6, () => ArmSoft.SME.Common.Resources.b2bSystem);

	public static readonly SubsystemTypesSE AdminTools = new SubsystemTypesSE("AdminTools", 9, () => ArmSoft.SME.Common.Resources.AdministrativeTools);

	public static readonly SubsystemTypesSE UserAWP = new SubsystemTypesSE("UserAWP", 100, () => ArmSoft.SME.Common.Resources.UserAWP);

	public const short PurchasesConst = 1;

	public const short SalesConst = 2;

	public const short StorageConst = 3;

	public const short CashBankConst = 4;

	public const short AnalysisReportsConst = 5;

	public const short B2BConst = 6;

	public const short PointOfSaleConst = 7;

	public const short EInvoicingConst = 8;

	public const short AdminToolsConst = 9;

	public const short UserAWPConst = 100;

	protected SubsystemTypesSE(string name, short value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
