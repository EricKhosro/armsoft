using System;
using System.Collections.Generic;
using ArmSoft.SME.Trade.Common.Models.Directories;

namespace ArmSoft.SME.Trade.Client.Models;

public class Discount
{
	public string Code { get; set; }

	public string Caption { get; set; }

	public string DiscountGroupCode { get; set; }

	public DateOnly DiscountStart { get; set; }

	public DateOnly? DiscountEnd { get; set; }

	public bool IsActive { get; set; }

	public string Storage { get; set; }

	public string DiscountDocType { get; set; }

	public string UseType { get; set; }

	public decimal Percent { get; set; }

	public decimal Amount { get; set; }

	public decimal GiftQuantity { get; set; }

	public bool IsMultiple { get; set; }

	public bool SelectGift { get; set; }

	public decimal GiftPrice { get; set; }

	public string PriceType { get; set; }

	public string Message { get; set; }

	public string SegmentCode { get; set; }

	public bool SegmentNegation { get; set; }

	public int GiftGoodID { get; set; }

	public string Description { get; set; }

	public byte Priority { get; set; }

	public List<ArmSoft.SME.Trade.Common.Models.Directories.Discount.DiscountScaleRow> DiscountScale { get; set; }

	public bool UseDayHours { get; set; }

	public ArmSoft.SME.Trade.Common.Models.Directories.Discount.Condition DiscountCondition { get; set; }
}
