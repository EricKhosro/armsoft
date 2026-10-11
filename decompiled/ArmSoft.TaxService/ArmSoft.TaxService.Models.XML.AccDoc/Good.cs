using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ArmSoft.TaxService.Models.XML.AccDoc;

[Serializable]
[GeneratedCode("xsd", "4.8.3928.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class Good
{
	private string goodIdField;

	private string goodParentIdField;

	private string descriptionField;

	private string descriptionRuField;

	private string classifierCodeField;

	private string unitField;

	private string unitRuField;

	private decimal amountField;

	private bool amountFieldSpecified;

	private decimal pricePerUnitField;

	private bool pricePerUnitFieldSpecified;

	private decimal priceField;

	private bool priceFieldSpecified;

	private decimal discountField;

	private bool discountFieldSpecified;

	private DealType dealTypeField;

	private bool dealTypeFieldSpecified;

	private decimal tarePriceField;

	private bool tarePriceFieldSpecified;

	private decimal exciseTaxField;

	private bool exciseTaxFieldSpecified;

	private decimal environmentalFeeField;

	private bool environmentalFeeFieldSpecified;

	private decimal totalPriceField;

	private bool totalPriceFieldSpecified;

	private decimal totalEnvironmentalFeeField;

	private bool totalEnvironmentalFeeFieldSpecified;

	private decimal totalExciseTaxField;

	private bool totalExciseTaxFieldSpecified;

	private GoodTraceable goodTraceableInfoField;

	public string GoodId
	{
		get
		{
			return goodIdField;
		}
		set
		{
			goodIdField = value;
		}
	}

	public string GoodParentId
	{
		get
		{
			return goodParentIdField;
		}
		set
		{
			goodParentIdField = value;
		}
	}

	public string Description
	{
		get
		{
			return descriptionField;
		}
		set
		{
			descriptionField = value;
		}
	}

	public string DescriptionRu
	{
		get
		{
			return descriptionRuField;
		}
		set
		{
			descriptionRuField = value;
		}
	}

	public string ClassifierCode
	{
		get
		{
			return classifierCodeField;
		}
		set
		{
			classifierCodeField = value;
		}
	}

	public string Unit
	{
		get
		{
			return unitField;
		}
		set
		{
			unitField = value;
		}
	}

	public string UnitRu
	{
		get
		{
			return unitRuField;
		}
		set
		{
			unitRuField = value;
		}
	}

	public decimal Amount
	{
		get
		{
			return amountField;
		}
		set
		{
			amountField = value;
		}
	}

	[XmlIgnore]
	public bool AmountSpecified
	{
		get
		{
			return amountFieldSpecified;
		}
		set
		{
			amountFieldSpecified = value;
		}
	}

	public decimal PricePerUnit
	{
		get
		{
			return pricePerUnitField;
		}
		set
		{
			pricePerUnitField = value;
		}
	}

	[XmlIgnore]
	public bool PricePerUnitSpecified
	{
		get
		{
			return pricePerUnitFieldSpecified;
		}
		set
		{
			pricePerUnitFieldSpecified = value;
		}
	}

	public decimal Price
	{
		get
		{
			return priceField;
		}
		set
		{
			priceField = value;
		}
	}

	[XmlIgnore]
	public bool PriceSpecified
	{
		get
		{
			return priceFieldSpecified;
		}
		set
		{
			priceFieldSpecified = value;
		}
	}

	public decimal Discount
	{
		get
		{
			return discountField;
		}
		set
		{
			discountField = value;
		}
	}

	[XmlIgnore]
	public bool DiscountSpecified
	{
		get
		{
			return discountFieldSpecified;
		}
		set
		{
			discountFieldSpecified = value;
		}
	}

	public DealType DealType
	{
		get
		{
			return dealTypeField;
		}
		set
		{
			dealTypeField = value;
		}
	}

	[XmlIgnore]
	public bool DealTypeSpecified
	{
		get
		{
			return dealTypeFieldSpecified;
		}
		set
		{
			dealTypeFieldSpecified = value;
		}
	}

	public decimal TarePrice
	{
		get
		{
			return tarePriceField;
		}
		set
		{
			tarePriceField = value;
		}
	}

	[XmlIgnore]
	public bool TarePriceSpecified
	{
		get
		{
			return tarePriceFieldSpecified;
		}
		set
		{
			tarePriceFieldSpecified = value;
		}
	}

	public decimal ExciseTax
	{
		get
		{
			return exciseTaxField;
		}
		set
		{
			exciseTaxField = value;
		}
	}

	[XmlIgnore]
	public bool ExciseTaxSpecified
	{
		get
		{
			return exciseTaxFieldSpecified;
		}
		set
		{
			exciseTaxFieldSpecified = value;
		}
	}

	public decimal EnvironmentalFee
	{
		get
		{
			return environmentalFeeField;
		}
		set
		{
			environmentalFeeField = value;
		}
	}

	[XmlIgnore]
	public bool EnvironmentalFeeSpecified
	{
		get
		{
			return environmentalFeeFieldSpecified;
		}
		set
		{
			environmentalFeeFieldSpecified = value;
		}
	}

	public decimal TotalPrice
	{
		get
		{
			return totalPriceField;
		}
		set
		{
			totalPriceField = value;
		}
	}

	[XmlIgnore]
	public bool TotalPriceSpecified
	{
		get
		{
			return totalPriceFieldSpecified;
		}
		set
		{
			totalPriceFieldSpecified = value;
		}
	}

	public decimal TotalEnvironmentalFee
	{
		get
		{
			return totalEnvironmentalFeeField;
		}
		set
		{
			totalEnvironmentalFeeField = value;
		}
	}

	[XmlIgnore]
	public bool TotalEnvironmentalFeeSpecified
	{
		get
		{
			return totalEnvironmentalFeeFieldSpecified;
		}
		set
		{
			totalEnvironmentalFeeFieldSpecified = value;
		}
	}

	public decimal TotalExciseTax
	{
		get
		{
			return totalExciseTaxField;
		}
		set
		{
			totalExciseTaxField = value;
		}
	}

	[XmlIgnore]
	public bool TotalExciseTaxSpecified
	{
		get
		{
			return totalExciseTaxFieldSpecified;
		}
		set
		{
			totalExciseTaxFieldSpecified = value;
		}
	}

	public GoodTraceable GoodTraceableInfo
	{
		get
		{
			return goodTraceableInfoField;
		}
		set
		{
			goodTraceableInfoField = value;
		}
	}
}
