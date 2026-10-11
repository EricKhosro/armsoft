using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class AccDocTotalSumDto
{
	private double totalPriceField;

	private double totalExciseField;

	private double totalEnvFeeField;

	[XmlElement(Order = 0)]
	public double TotalPrice
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
	public double TotalExcise
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

	[XmlElement(Order = 2)]
	public double TotalEnvFee
	{
		get
		{
			return totalEnvFeeField;
		}
		set
		{
			totalEnvFeeField = value;
		}
	}
}
