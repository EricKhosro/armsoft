using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class GetInvoicesRequest : GenericAuthenticatedRequest
{
	private InvoiceFilter invoiceFilterField;

	[XmlElement(Order = 0)]
	public InvoiceFilter InvoiceFilter
	{
		get
		{
			return invoiceFilterField;
		}
		set
		{
			invoiceFilterField = value;
		}
	}
}
