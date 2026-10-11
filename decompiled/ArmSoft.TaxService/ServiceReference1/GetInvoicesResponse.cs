using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class GetInvoicesResponse : GenericResponse
{
	private InvoiceListElement[] invoiceListElementField;

	[XmlElement("InvoiceListElement", Order = 0)]
	public InvoiceListElement[] InvoiceListElement
	{
		get
		{
			return invoiceListElementField;
		}
		set
		{
			invoiceListElementField = value;
		}
	}
}
