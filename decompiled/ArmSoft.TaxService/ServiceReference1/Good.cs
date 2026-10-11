using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class Good
{
	private string descriptionField;

	private string classificationField;

	private string unitField;

	private decimal volumeField;

	private bool volumeFieldSpecified;

	private decimal amountField;

	private bool amountFieldSpecified;

	private decimal pricePerUnitField;

	private bool pricePerUnitFieldSpecified;

	private decimal priceField;

	private bool priceFieldSpecified;

	private decimal discountField;

	private bool discountFieldSpecified;

	private decimal exciseTaxField;

	private bool exciseTaxFieldSpecified;

	private decimal tarePriceField;

	private bool tarePriceFieldSpecified;

	private decimal exciseRateField;

	private bool exciseRateFieldSpecified;

	private decimal exciseField;

	private bool exciseFieldSpecified;

	private decimal vATRateField;

	private bool vATRateFieldSpecified;

	private decimal vATField;

	private bool vATFieldSpecified;

	private decimal environmentalFeeField;

	private bool environmentalFeeFieldSpecified;

	private bool isVatEditableInAdjustingAccountField;

	private bool isVatEditableInAdjustingAccountFieldSpecified;

	private decimal totalPriceField;

	private decimal totalExciseTaxField;

	private bool totalExciseTaxFieldSpecified;

	private decimal totalEnvironmentalFeeField;

	private bool totalEnvironmentalFeeFieldSpecified;

	[XmlElement(Order = 0)]
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

	[XmlElement(Order = 1)]
	public string Classification
	{
		get
		{
			return classificationField;
		}
		set
		{
			classificationField = value;
		}
	}

	[XmlElement(Order = 2)]
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

	[XmlElement(Order = 3)]
	public decimal Volume
	{
		get
		{
			return volumeField;
		}
		set
		{
			volumeField = value;
		}
	}

	[XmlIgnore]
	public bool VolumeSpecified
	{
		get
		{
			return volumeFieldSpecified;
		}
		set
		{
			volumeFieldSpecified = value;
		}
	}

	[XmlElement(Order = 4)]
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

	[XmlElement(Order = 5)]
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

	[XmlElement(Order = 6)]
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

	[XmlElement(Order = 7)]
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

	[XmlElement(Order = 8)]
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

	[XmlElement(Order = 9)]
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

	[XmlElement(Order = 10)]
	public decimal ExciseRate
	{
		get
		{
			return exciseRateField;
		}
		set
		{
			exciseRateField = value;
		}
	}

	[XmlIgnore]
	public bool ExciseRateSpecified
	{
		get
		{
			return exciseRateFieldSpecified;
		}
		set
		{
			exciseRateFieldSpecified = value;
		}
	}

	[XmlElement(Order = 11)]
	public decimal Excise
	{
		get
		{
			return exciseField;
		}
		set
		{
			exciseField = value;
		}
	}

	[XmlIgnore]
	public bool ExciseSpecified
	{
		get
		{
			return exciseFieldSpecified;
		}
		set
		{
			exciseFieldSpecified = value;
		}
	}

	[XmlElement(Order = 12)]
	public decimal VATRate
	{
		get
		{
			return vATRateField;
		}
		set
		{
			vATRateField = value;
		}
	}

	[XmlIgnore]
	public bool VATRateSpecified
	{
		get
		{
			return vATRateFieldSpecified;
		}
		set
		{
			vATRateFieldSpecified = value;
		}
	}

	[XmlElement(Order = 13)]
	public decimal VAT
	{
		get
		{
			return vATField;
		}
		set
		{
			vATField = value;
		}
	}

	[XmlIgnore]
	public bool VATSpecified
	{
		get
		{
			return vATFieldSpecified;
		}
		set
		{
			vATFieldSpecified = value;
		}
	}

	[XmlElement(Order = 14)]
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

	[XmlElement(Order = 15)]
	public bool IsVatEditableInAdjustingAccount
	{
		get
		{
			return isVatEditableInAdjustingAccountField;
		}
		set
		{
			isVatEditableInAdjustingAccountField = value;
		}
	}

	[XmlIgnore]
	public bool IsVatEditableInAdjustingAccountSpecified
	{
		get
		{
			return isVatEditableInAdjustingAccountFieldSpecified;
		}
		set
		{
			isVatEditableInAdjustingAccountFieldSpecified = value;
		}
	}

	[XmlElement(Order = 16)]
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

	[XmlElement(Order = 17)]
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

	[XmlElement(Order = 18)]
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
}
