using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class AccDocTotal
{
	private decimal totalPriceField;

	private decimal totalExciseField;

	private bool totalExciseFieldSpecified;

	private decimal totalEnvironmentalFeeField;

	private bool totalEnvironmentalFeeFieldSpecified;

	[XmlElement(Order = 0)]
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

	[XmlElement(Order = 1)]
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

	[XmlElement(Order = 2)]
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
