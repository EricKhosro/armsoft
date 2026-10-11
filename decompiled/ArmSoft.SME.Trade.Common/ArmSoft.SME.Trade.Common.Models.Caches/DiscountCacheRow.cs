using System;
using System.Collections.Generic;
using System.Text.Json;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Directories;

namespace ArmSoft.SME.Trade.Common.Models.Caches;

public class DiscountCacheRow : CacheRowBase<string>
{
	public string Code { get; set; }

	public string Caption { get; set; }

	public string DiscountGroupCode { get; set; } = string.Empty;

	public string DiscountGroupCaption { get; set; }

	public DateOnly StartDate { get; set; }

	public DateOnly? EndDate { get; set; }

	public bool IsActive { get; set; } = true;

	public string Storage { get; set; } = string.Empty;

	public string DiscountDocType { get; set; } = string.Empty;

	public string DiscountDocTypeCaption { get; set; }

	public string UseType { get; set; } = "1";

	public string UseTypeCaption { get; set; }

	public decimal Percent { get; set; }

	public decimal Amount { get; set; }

	public decimal GiftQuantity { get; set; }

	public decimal GiftDiscountPercent { get; set; } = 100m;

	public bool IsDistrInProportion { get; set; }

	public bool IsMultiple { get; set; }

	public bool SelectGift { get; set; }

	public decimal GiftPrice { get; set; }

	public string PriceType { get; set; } = string.Empty;

	public string Message { get; set; } = string.Empty;

	public string SegmentCode { get; set; } = string.Empty;

	public string SegmentCaption { get; set; }

	public bool SegmentNegation { get; set; }

	public int GiftGoodID { get; set; }

	public string Description { get; set; } = string.Empty;

	public byte Priority { get; set; }

	public List<Discount.DiscountScaleRow> DiscountScale { get; set; }

	public bool UseDayHours { get; set; }

	public Discount.Condition DiscountCondition { get; set; } = new Discount.Condition
	{
		Type = "1"
	};

	public override string KeyJSON => JsonSerializer.Serialize(Code);

	public DiscountCacheRow(string code, string caption, string discountGroupCode, string discountGroupCaption, DateOnly dateStart, DateOnly? dateEnd, bool isActive, byte priority, string useType, string useTypeCaption, string discountDocType, string discountDocTypeCaption, string segmentCode, string segmentCaption, string description, byte[] ts)
	{
		Code = code;
		Caption = caption;
		DiscountGroupCode = discountGroupCode;
		DiscountGroupCaption = discountGroupCaption;
		StartDate = dateStart;
		EndDate = dateEnd;
		IsActive = isActive;
		Priority = priority;
		UseType = useType;
		UseTypeCaption = useTypeCaption;
		DiscountDocType = discountDocType;
		DiscountDocTypeCaption = discountDocTypeCaption;
		SegmentCode = segmentCode;
		SegmentCaption = segmentCaption;
		Description = description;
		TS = ts;
	}

	public DiscountCacheRow()
	{
	}
}
