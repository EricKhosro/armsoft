using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ArmSoft.TaxService.Models.XML;

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

	private string classifierCodeField;

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

	private decimal tarePriceField;

	private bool tarePriceFieldSpecified;

	private decimal exciseTaxField;

	private bool exciseTaxFieldSpecified;

	private decimal exciseRateField;

	private bool exciseRateFieldSpecified;

	private decimal exciseField;

	private bool exciseFieldSpecified;

	private decimal vATRateField;

	private bool vATRateFieldSpecified;

	private decimal vATField;

	private bool vATFieldSpecified;

	private DealType dealTypeField;

	private bool dealTypeFieldSpecified;

	private decimal environmentalFeeField;

	private bool environmentalFeeFieldSpecified;

	private bool isVatEditableInAdjustingAccountField;

	private bool isVatEditableInAdjustingAccountFieldSpecified;

	private decimal totalPriceField;

	private bool totalPriceFieldSpecified;

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
