using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class InvoiceGridRow : CodeCaption
{
	[PrintDataMember("NameForECR")]
	public string NameForECR { get; set; }

	[PrintDataMember("Code")]
	public string CodeForECR { get; set; }

	[PrintDataMember("CPAClassification")]
	public string CPACode { get; set; }

	[PrintDataMember("NameFromDirectory")]
	public string NameDir { get; set; }

	[PrintDataMember("FullNameFromDirectory")]
	public string FullNameDir { get; set; }

	[PrintDataMember("UnitCode")]
	public string UnitCode { get; set; }

	[PrintDataMember("Unit")]
	public string UnitBrief { get; set; }

	[PrintDataMember("Quantity", DecimalPrecisionType.Quantity)]
	public decimal Quantity { get; set; }

	[PrintDataMember("RowN")]
	public int RowNumber { get; set; }

	[PrintDataMember("Price")]
	public decimal Price { get; set; }

	[PrintDataMember("Summa")]
	public decimal Summ { get; set; }

	[PrintDataMember("OptionsVAT")]
	public bool VAT { get; set; }

	[PrintDataMember("Specification")]
	public string Description { get; set; }

	[PrintDataMember("BarCode")]
	public string BarCode { get; set; }

	[PrintDataMember("Description")]
	public string RowDescription { get; set; }

	[PrintDataMember("Country")]
	public CodeCaption Country { get; set; }

	[PrintDataMember("Producer")]
	public CodeCaption Producer { get; set; }

	[PrintDataMember("AttributeValue1")]
	public CodeCaption Property1 { get; set; }

	[PrintDataMember("AttributeValue2")]
	public CodeCaption Property2 { get; set; }

	[PrintDataMember("MTNameRu")]
	public string NameDirRu { get; set; }

	[PrintDataMember("MTNameEn")]
	public string NameDirEn { get; set; }
}
