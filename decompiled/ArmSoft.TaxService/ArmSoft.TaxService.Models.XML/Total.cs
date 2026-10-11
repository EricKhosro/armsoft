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
public class Total
{
	private decimal priceField;

	private bool priceFieldSpecified;

	private decimal exciseField;

	private bool exciseFieldSpecified;

	private decimal vATField;

	private bool vATFieldSpecified;

	private decimal totalPriceField;

	private bool totalPriceFieldSpecified;

	private decimal totalEnvironmentalFeeField;

	private bool totalEnvironmentalFeeFieldSpecified;

	private decimal totalTarePriceField;

	private bool totalTarePriceFieldSpecified;

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

	public decimal TotalTarePrice
	{
		get
		{
			return totalTarePriceField;
		}
		set
		{
			totalTarePriceField = value;
		}
	}

	[XmlIgnore]
	public bool TotalTarePriceSpecified
	{
		get
		{
			return totalTarePriceFieldSpecified;
		}
		set
		{
			totalTarePriceFieldSpecified = value;
		}
	}
}
