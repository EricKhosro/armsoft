namespace ArmSoft.SME.Trade.Client.Models;

public class Material
{
	public int MTID { get; set; }

	public string MTCode { get; set; }

	public string Caption { get; set; }

	public string FullCaption { get; set; }

	public MTGroup MTGroup { get; set; }

	public string MTGroupPath { get; set; }

	public string Unit { get; set; }

	public string UnitBrief { get; set; }

	public string AltUnit { get; set; }

	public string AltUnitBrief { get; set; }

	public string BarCode { get; set; }

	public string CPACode { get; set; }

	public string Type { get; set; }

	public string Description { get; set; }

	public string ExternalCode { get; set; }

	public bool ShowInPriceList { get; set; }

	public string ExpenseMethod { get; set; }

	public bool VAT { get; set; }

	public decimal EcoFeeRate { get; set; }

	public string Producer { get; set; }

	public string Country { get; set; }

	public string Property1 { get; set; }

	public string Property2 { get; set; }

	public string SubstituteItemsGroup { get; set; }

	public bool IsWeight { get; set; }

	public decimal Coeff { get; set; }

	public decimal Extra { get; set; }

	public decimal Bonus { get; set; }

	public decimal Point { get; set; }

	public decimal Discount { get; set; }

	public int? PLUCode { get; set; }

	public decimal MinQty { get; set; }

	public decimal MaxQty { get; set; }

	public string Body { get; set; }

	public string CaptionEn { get; set; }

	public string CaptionRu { get; set; }

	public string AdditionalDescr1 { get; set; }

	public string AdditionalDescr2 { get; set; }

	public bool ImageExists { get; set; }
}
