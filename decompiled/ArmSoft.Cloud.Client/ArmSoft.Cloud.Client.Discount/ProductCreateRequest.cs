using System.Collections.Generic;

namespace ArmSoft.Cloud.Client.Discount;

public class ProductCreateRequest
{
	public string Code { get; set; }

	public string VariantCode { get; set; }

	public int VariantID { get; set; }

	public string Type { get; set; }

	public string UnitCode { get; set; }

	public string UnitName { get; set; }

	public string AltUnitCode { get; set; }

	public string AltUnitName { get; set; }

	public decimal Coeff { get; set; }

	public List<Caption> Captions { get; set; }

	public bool IsWeigth { get; set; }

	public string GroupCode { get; set; }

	public List<Caption> GroupCaptions { get; set; }

	public string Description { get; set; }

	public string AdditionalDescription1 { get; set; }

	public string AdditionalDescription2 { get; set; }

	public string Producer { get; set; }

	public string Country { get; set; }

	public decimal DiscountPercent { get; set; }

	public decimal RegularPrice { get; set; }

	public decimal DiscountedPrice { get; set; }

	public decimal RegularAltUnitPrice { get; set; }

	public decimal DiscountedAltUnitPrice { get; set; }

	public decimal Quantity { get; set; }

	public long Hash { get; set; }

	public bool IsHidden { get; set; }
}
