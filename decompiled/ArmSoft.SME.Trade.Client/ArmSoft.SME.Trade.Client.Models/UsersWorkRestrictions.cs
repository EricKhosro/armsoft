using System;

namespace ArmSoft.SME.Trade.Client.Models;

public class UsersWorkRestrictions
{
	public DateOnly? DateBegin { get; set; }

	public DateOnly? DateEnd { get; set; }

	public bool UseCurrentDateInDocs { get; set; }

	public bool AllowChangeSalePrices { get; set; }

	public bool AllowChangeDiscountPrice { get; set; }

	public bool AllowChangePriceType { get; set; }

	public bool AllowChangeDiscountPriceType { get; set; }

	public bool CalcDiscountAuto { get; set; }

	public bool AlwaysCalcDiscountAuto { get; set; }

	public bool ShowMTInclInPrList { get; set; }

	public bool ShowGoodsCostInReportsAndDocs { get; set; }

	public bool AllowExceedDebtLimit { get; set; }

	public bool DenyAccessToOtherOwnerDocs { get; set; }

	public bool AllowEditCodeOfDirectories { get; set; }

	public bool AllowUse100PercentSale { get; set; }

	public bool AllowPurchaseWith0PriceOr100Percent { get; set; }

	public bool AddMaterialToPartnersMT { get; set; }

	public bool AllowGroupDelete { get; set; }

	public bool AllowOpFromHistoryPanel { get; set; }
}
