using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class GetTaxpayerInfoResponseExtended : GenericResponse
{
	private string vATNumberField;

	private TaxpayerInfo taxpayerInfoField;

	private string checkStatusField;

	[XmlElement(Order = 0)]
	public string VATNumber
	{
		get
		{
			return vATNumberField;
		}
		set
		{
			vATNumberField = value;
		}
	}

	[XmlElement(Order = 1)]
	public TaxpayerInfo TaxpayerInfo
	{
		get
		{
			return taxpayerInfoField;
		}
		set
		{
			taxpayerInfoField = value;
		}
	}

	[XmlElement(Order = 2)]
	public string checkStatus
	{
		get
		{
			return checkStatusField;
		}
		set
		{
			checkStatusField = value;
		}
	}
}
