using System.Collections.Generic;

namespace ArmSoft.SME.Trade.Common.Models.Documents;

public interface ISaleRow
{
	int RowID { get; set; }

	string ItemType { get; set; }

	string Storage { get; set; }

	int ItemID { get; set; }

	string ItemCode { get; set; }

	string BarCode { get; set; }

	string ItemName { get; set; }

	string UnitCode { get; set; }

	string UnitBrief { get; set; }

	bool? VAT { get; set; }

	decimal Quantity { get; set; }

	decimal Price { get; set; }

	decimal Discount { get; set; }

	decimal DiscountPrice { get; set; }

	decimal BeginSumma { get; set; }

	decimal Summa { get; set; }

	decimal DiscountSumma { get; set; }

	decimal AccumulatedBonus { get; set; }

	decimal UsedBonus { get; set; }

	bool? IsPartySelected { get; set; }

	string RowDescription { get; set; }

	bool IsDeleted { get; set; }

	bool IsGift { get; set; }

	HashSet<string> MarkingList { get; set; }

	bool IsRowMarkable { get; set; }
}
