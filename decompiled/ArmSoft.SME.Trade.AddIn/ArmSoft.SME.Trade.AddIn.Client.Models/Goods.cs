using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

[ComVisible(true)]
[Guid("DEA76DA0-DAF8-4A34-8D11-4D138AC94CF2")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class Goods : ExchangeBase
{
	public List<QuantityUnitPart> GoodQuantityUnits;

	public List<MTBarCode> GoodBarCodes;

	public string Code { get; set; } = string.Empty;

	public string Name { get; set; } = string.Empty;

	public string LongName { get; set; } = string.Empty;

	public string Type { get; set; } = string.Empty;

	public string Group { get; set; } = string.Empty;

	public string SubstituteItemsGroup { get; set; } = string.Empty;

	public string MainSupplier { get; set; } = string.Empty;

	public string CPACode { get; set; } = string.Empty;

	public string Unit { get; set; } = string.Empty;

	public string AltUnit { get; set; } = string.Empty;

	public string ExpMethod { get; set; } = string.Empty;

	public bool? ShowInPriceList { get; set; }

	public string Description { get; set; } = string.Empty;

	public string Country { get; set; } = string.Empty;

	public string Producer { get; set; } = string.Empty;

	public string Property1 { get; set; } = string.Empty;

	public string Property2 { get; set; } = string.Empty;

	public decimal? MinQTY { get; set; }

	public decimal? MaxQTY { get; set; }

	public decimal? Extra { get; set; }

	public decimal? Bonus { get; set; }

	public bool? IgnoreBonus { get; set; }

	public decimal? Point { get; set; }

	public decimal? Coeff { get; set; }

	public decimal? Discount { get; set; }

	public int? PLUCode { get; set; }

	public bool? VAT { get; set; }

	public bool? IsWeight { get; set; }

	public bool? CheckMarking { get; set; }

	public string LabelUnit { get; set; }

	public decimal? LabelCoeff { get; set; }

	public string BarCode { get; set; } = string.Empty;

	public decimal? EnvironmentalFeePercent { get; set; }

	public string ExternalCode { get; set; } = string.Empty;

	public string Image { get; set; } = string.Empty;

	public string CaptionEN { get; set; } = string.Empty;

	public string CaptionRU { get; set; } = string.Empty;

	public string AdditionalDescr1 { get; set; } = string.Empty;

	public string AdditionalDescr2 { get; set; } = string.Empty;
}
