using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class InvoiceBatch : SignableData
{
	private Invoice[] invoiceField;

	[XmlElement("Invoice", Order = 0)]
	public Invoice[] Invoice
	{
		get
		{
			return invoiceField;
		}
		set
		{
			invoiceField = value;
		}
	}
}
