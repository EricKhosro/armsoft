using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class LabelRow
{
	[PrintDataMember("Code")]
	public string Code { get; set; }

	[PrintDataMember("BarCode")]
	public string BarCode { get; set; }

	[PrintDataMember("Name")]
	public string Caption { get; set; }

	[PrintDataMember("FullCaption")]
	public string FullCaption { get; set; }

	[PrintDataMember("Group")]
	public CodeCaption Group { get; set; }

	[PrintDataMember("Specification")]
	public string Description { get; set; }

	[PrintDataMember("Country")]
	public CodeCaption Country { get; set; }

	[PrintDataMember("Producer")]
	public CodeCaption Producer { get; set; }

	[PrintDataMember("AttributeValue1")]
	public CodeCaption Property1 { get; set; }

	[PrintDataMember("AttributeValue2")]
	public CodeCaption Property2 { get; set; }

	[PrintDataMember("AttributeType1")]
	public CodeCaption PropertyType1 { get; set; }

	[PrintDataMember("AttributeType2")]
	public CodeCaption PropertyType2 { get; set; }

	[PrintDataMember("AdditionalDescription1")]
	public string AdditionalDescr1 { get; set; }

	[PrintDataMember("AdditionalDescription2")]
	public string AdditionalDescr2 { get; set; }

	[PrintDataMember("UseLife")]
	public string UseLife { get; set; }

	[PrintDataMember("Price")]
	public decimal Price { get; set; }

	[PrintDataMember("DiscountedPrice")]
	public decimal DiscountPrice { get; set; }

	[PrintDataMember("AvgPrice")]
	public decimal AvgPrice { get; set; }

	[PrintDataMember("DiscountPercentSymbol")]
	public decimal Discount { get; set; }

	[PrintDataMember("BonusPercent")]
	public decimal Bonus { get; set; }

	[PrintDataMember("BonusPoint")]
	public decimal Point { get; set; }

	[PrintDataMember("PriceType")]
	public PriceTypeRow PriceType { get; set; }

	[PrintDataMember("DicountPriceType")]
	public PriceTypeRow DiscountPriceType { get; set; }

	[PrintDataMember("AvgPriceType")]
	public PriceTypeRow AvgPriceType { get; set; }

	[PrintDataMember("QntUnit")]
	public QntUnitRow QntUnit { get; set; }

	[PrintDataMember("DateWord")]
	public string Date { get; set; }

	[PrintDataMember("Time")]
	public string Time { get; set; }

	[PrintDataMember("Company")]
	public CompanyRequisites Company { get; set; }

	[PrintDataMember("PLUCodeInElScale")]
	public int? PLUCode { get; set; }

	[PrintDataMember("ExternalCode")]
	public string ExternalCode { get; set; }

	[PrintDataMember("LabelUnit")]
	public UnitPriceRow UnitPriceRow { get; set; }

	[PrintDataMember("MainSupplier")]
	public CodeCaption MainSupplierRow { get; set; }
}
