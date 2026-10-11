using System;

namespace ArmSoft.SME.Trade.Common.CustomTypes;

public class DiscountLogRow
{
	public Guid ISN { get; set; } = Guid.Empty;

	public string DocNum { get; set; } = string.Empty;

	public DateOnly DocDate { get; set; }

	public short RowNumber { get; set; }

	public int GoodId { get; set; } = -1;

	public string GoodCode { get; set; } = string.Empty;

	public string GoodCaption { get; set; } = string.Empty;

	public string DiscountGroupCode { get; set; } = string.Empty;

	public byte DiscountGroupPriority { get; set; }

	public string GroupMergeMethod { get; set; } = string.Empty;

	public string GroupMergeMethodCaption { get; set; } = string.Empty;

	public string DiscountCode { get; set; } = string.Empty;

	public string DiscountCaption { get; set; } = string.Empty;

	public byte DiscountPriority { get; set; }

	public decimal DiscountedSum { get; set; }

	public decimal Percent { get; set; }
}
