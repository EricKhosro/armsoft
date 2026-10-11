using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class OnBehalfOfSupplierInfo
{
	private string vATNumberField;

	private TaxpayerInfo taxpayerField;

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
	public TaxpayerInfo Taxpayer
	{
		get
		{
			return taxpayerField;
		}
		set
		{
			taxpayerField = value;
		}
	}
}
