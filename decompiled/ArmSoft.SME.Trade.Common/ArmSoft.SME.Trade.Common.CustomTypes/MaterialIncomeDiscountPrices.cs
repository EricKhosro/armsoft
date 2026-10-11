namespace ArmSoft.SME.Trade.Common.CustomTypes;

public class MaterialIncomeDiscountPrices
{
	public bool MaterialHasPrice { get; set; }

	public string UnitCode { get; set; }

	public decimal Price { get; set; }

	public decimal DiscountPrice { get; set; }

	public decimal Discount { get; set; }

	public int MaterialID { get; set; }
}
