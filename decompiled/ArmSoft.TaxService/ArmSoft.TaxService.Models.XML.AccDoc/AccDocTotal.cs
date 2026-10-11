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
public class AccDocTotal
{
	private decimal totalPriceField;

	private bool totalPriceFieldSpecified;

	private decimal totalExciseField;

	private bool totalExciseFieldSpecified;

	private decimal totalEnvironmentalFeeField;

	private bool totalEnvironmentalFeeFieldSpecified;

	private decimal totalTarePriceField;

	private bool totalTarePriceFieldSpecified;

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

	public decimal TotalExcise
	{
		get
		{
			return totalExciseField;
		}
		set
		{
			totalExciseField = value;
		}
	}

	[XmlIgnore]
	public bool TotalExciseSpecified
	{
		get
		{
			return totalExciseFieldSpecified;
		}
		set
		{
			totalExciseFieldSpecified = value;
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
