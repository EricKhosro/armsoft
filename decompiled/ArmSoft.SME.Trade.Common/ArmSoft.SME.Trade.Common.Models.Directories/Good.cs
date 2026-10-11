using System.Collections.Generic;

namespace ArmSoft.SME.Trade.Common.Models.Directories;

public class Good : TradeDirectory
{
	public class GoodQntUnit
	{
		public string Code { get; set; }

		public string Brief { get; set; }

		public decimal Coeff { get; set; }

		public bool IsDefault { get; set; }

		public byte[] Image { get; set; }
	}

	public class GoodBarCode
	{
		public string UnitCode { get; set; }

		public string Brief { get; set; }

		public string BarCode { get; set; }
	}

	public string FullName { get; set; }

	public string Type { get; set; }

	public string Group { get; set; }

	public string MainSupplier { get; set; }

	public int MainSupplierID { get; set; }

	public string SubstituteItemsGroup { get; set; }

	public string CPACode { get; set; }

	public string Unit { get; set; }

	public string UnitCaption { get; set; }

	public string UnitBrief { get; set; }

	public bool UnitFraction { get; set; }

	public string AltUnit { get; set; }

	public string AltUnitCaption { get; set; }

	public string AltUnitBrief { get; set; }

	public bool AltUnitFraction { get; set; }

	public string ExpenseMethod { get; set; }

	public bool ShowInPriceList { get; set; }

	public string Description { get; set; }

	public string ExternalCode { get; set; }

	public string LabelUnit { get; set; }

	public decimal LabelCoeff { get; set; }

	public string Country { get; set; }

	public string Producer { get; set; }

	public string Property1 { get; set; }

	public string Property2 { get; set; }

	public decimal MinQTY { get; set; }

	public decimal MaxQTY { get; set; }

	public decimal Extra { get; set; }

	public decimal Bonus { get; set; }

	public bool IgnoreBonus { get; set; }

	public decimal Point { get; set; }

	public bool VAT { get; set; }

	public decimal EcoFeeRate { get; set; }

	public bool IsWeight { get; set; }

	public bool CheckMarking { get; set; }

	public decimal Coeff { get; set; }

	public decimal Discount { get; set; }

	public int? PLUCode { get; set; }

	public byte[] Img { get; set; }

	public bool ImageEdited { get; set; }

	public string BarCode { get; set; }

	public string UnitOld { get; set; }

	public string BarCodeOld { get; set; }

	public string CaptionEN { get; set; }

	public string CaptionRU { get; set; }

	public string AdditionalDescr1 { get; set; }

	public string AdditionalDescr2 { get; set; }

	public bool CheckDBCacheRelevance { get; set; }

	public List<GoodQntUnit> GoodQntUnits { get; set; }

	public List<GoodQntUnit> OldGoodQntUnits { get; set; }

	public List<GoodBarCode> GoodBarCodes { get; set; }
}
