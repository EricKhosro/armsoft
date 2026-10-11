using System;
using System.Collections.Generic;

namespace ArmSoft.SME.Trade.Common.Models.Directories;

public class Discount : TradeDirectory
{
	public class DiscountScaleRow
	{
		public short ID { get; set; }

		public decimal BeginAmount { get; set; }

		public decimal EndAmount { get; set; }

		public decimal DiscountScalePercent { get; set; }

		public static DiscountScaleRow Clone(DiscountScaleRow source)
		{
			return new DiscountScaleRow
			{
				ID = source.ID,
				BeginAmount = source.BeginAmount,
				EndAmount = source.EndAmount,
				DiscountScalePercent = source.DiscountScalePercent
			};
		}
	}

	public class Condition
	{
		public string Scope { get; set; }

		public string Type { get; set; } = string.Empty;

		public decimal Limit { get; set; }

		public DateOnly? ConditionStart { get; set; }

		public DateOnly? ConditionEnd { get; set; }

		public bool IncludeCurrentSale { get; set; }

		public string PartnerGroup { get; set; } = string.Empty;

		public string Segment { get; set; } = string.Empty;

		public bool SegmentNegation { get; set; }

		public bool IsBirthDayDiscount { get; set; }

		public byte BeforBDay { get; set; }

		public byte AfterBDay { get; set; }

		public Dictionary<int, ConditionDaysAndHours> DaysHours { get; set; }

		public bool UseExtension { get; set; }

		public string UserScript { get; set; } = string.Empty;
	}

	public class ConditionDaysAndHours
	{
		public bool Active { get; set; }

		public TimeSpan? FirstStartTime { get; set; }

		public TimeSpan? FirstEndTime { get; set; }

		public TimeSpan? SecondStartTime { get; set; }

		public TimeSpan? SecondEndTime { get; set; }
	}

	public string DiscountGroupCode { get; set; } = string.Empty;

	public DateOnly StartDate { get; set; }

	public DateOnly? EndDate { get; set; }

	public bool IsActive { get; set; } = true;

	public string Storage { get; set; } = string.Empty;

	public string DiscountDocType { get; set; } = string.Empty;

	public string UseType { get; set; } = "1";

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

	public string Segment { get; set; } = string.Empty;

	public bool SegmentNegation { get; set; }

	public int GiftGoodID { get; set; }

	public string GiftGoodCode { get; set; }

	public string Description { get; set; } = string.Empty;

	public byte Priority { get; set; }

	public List<DiscountScaleRow> DiscountScale { get; set; }

	public bool UseDayHours { get; set; }

	public Condition DiscountCondition { get; set; } = new Condition
	{
		Type = "1"
	};
}
