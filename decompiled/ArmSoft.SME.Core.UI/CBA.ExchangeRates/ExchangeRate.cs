using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace CBA.ExchangeRates;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.cba.am/")]
public class ExchangeRate
{
	private string iSOField;

	private string amountField;

	private string rateField;

	private string differenceField;

	[XmlElement(Order = 0)]
	public string ISO
	{
		get
		{
			return iSOField;
		}
		set
		{
			iSOField = value;
		}
	}

	[XmlElement(Order = 1)]
	public string Amount
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

	[XmlElement(Order = 2)]
	public string Rate
	{
		get
		{
			return rateField;
		}
		set
		{
			rateField = value;
		}
	}

	[XmlElement(Order = 3)]
	public string Difference
	{
		get
		{
			return differenceField;
		}
		set
		{
			differenceField = value;
		}
	}
}
