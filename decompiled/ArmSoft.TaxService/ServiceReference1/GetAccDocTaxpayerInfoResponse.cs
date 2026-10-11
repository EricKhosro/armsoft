using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class GetAccDocTaxpayerInfoResponse : GenericResponse
{
	private AccDocTaxpayerInfo accDocTaxpayerInfoField;

	[XmlElement(Order = 0)]
	public AccDocTaxpayerInfo AccDocTaxpayerInfo
	{
		get
		{
			return accDocTaxpayerInfoField;
		}
		set
		{
			accDocTaxpayerInfoField = value;
		}
	}
}
