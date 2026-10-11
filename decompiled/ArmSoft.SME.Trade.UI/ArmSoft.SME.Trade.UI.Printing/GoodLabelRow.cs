using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Printing;
using ArmSoft.SME.Trade.Common.Models.Caches;

namespace ArmSoft.SME.Trade.UI.Printing;

public class GoodLabelRow
{
	[PrintDataMember]
	public MaterialCacheRow MaterialeRow { get; set; }

	[PrintDataMember("BarCode")]
	public string BarCode { get; set; }

	[PrintDataMember]
	public string BarTitle { get; set; }

	[PrintDataMember]
	public string AltUnitBarCode { get; set; }

	[PrintDataMember("Quantity", DecimalPrecisionType.Quantity)]
	public decimal Quantity { get; set; }

	[PrintDataMember("AttributeValue1")]
	public CodeCaption Property1Value { get; set; }

	[PrintDataMember("AttributeValue2")]
	public CodeCaption Property2Value { get; set; }
}
