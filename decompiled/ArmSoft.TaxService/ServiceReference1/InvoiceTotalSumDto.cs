using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class InvoiceTotalSumDto
{
	private long countField;

	private double totalPriceWithoutVatField;

	private double totalExciseField;

	private double totalVatField;

	private double totalEnvFeeField;

	[XmlElement(Order = 0)]
	public long Count
	{
		get
		{
			return countField;
		}
		set
		{
			countField = value;
		}
	}

	[XmlElement(Order = 1)]
	public double TotalPriceWithoutVat
	{
		get
		{
			return totalPriceWithoutVatField;
		}
		set
		{
			totalPriceWithoutVatField = value;
		}
	}

	[XmlElement(Order = 2)]
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

	[XmlElement(Order = 3)]
	public double TotalVat
	{
		get
		{
			return totalVatField;
		}
		set
		{
			totalVatField = value;
		}
	}

	[XmlElement(Order = 4)]
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
