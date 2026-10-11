using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class PrintLabelWindowGridRow
{
	[PrintDataMember]
	public bool Include { get; set; }

	[PrintDataMember]
	public int MTID { get; set; }

	[PrintDataMember]
	public int RowID { get; set; }

	[PrintDataMember]
	public string MTCode { get; set; }

	[PrintDataMember]
	public string MTName { get; set; }

	[PrintDataMember]
	public string Unit { get; set; }

	[PrintDataMember("Unit")]
	public string UnitBrief { get; set; }

	[PrintDataMember]
	public string UnitFromDoc { get; set; }

	[PrintDataMember]
	public string UnitBriefFromDoc { get; set; }

	[PrintDataMember("Price")]
	public decimal Price { get; set; }

	[PrintDataMember("Quantity", DecimalPrecisionType.Quantity)]
	public decimal Quantity { get; set; }

	[PrintDataMember("UseLife")]
	public DateOnly? UseLife { get; set; }

	[PrintDataMember("Country")]
	public string Country { get; set; }

	[PrintDataMember("Producer")]
	public string Producer { get; set; }

	[PrintDataMember("AttributeValue1")]
	public string Property1 { get; set; }

	[PrintDataMember]
	public string Property1Caption { get; set; }

	[PrintDataMember("AttributeValue2")]
	public string Property2 { get; set; }

	[PrintDataMember]
	public string Property2Caption { get; set; }
}
