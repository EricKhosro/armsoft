using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class Total
{
	private decimal priceField;

	private decimal exciseField;

	private bool exciseFieldSpecified;

	private decimal vATField;

	private decimal totalPriceField;

	private decimal totalEnvironmentalFeeField;

	private bool totalEnvironmentalFeeFieldSpecified;

	private decimal totalExciseField;

	private bool totalExciseFieldSpecified;

	[XmlElement(Order = 0)]
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

	[XmlElement(Order = 1)]
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

	[XmlElement(Order = 2)]
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

	[XmlElement(Order = 3)]
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

	[XmlElement(Order = 4)]
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

	[XmlElement(Order = 5)]
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
}
