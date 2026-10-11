using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class BatchError
{
	private InvoiceNumber invoiceNumberField;

	private Status errorField;

	[XmlElement(Order = 0)]
	public InvoiceNumber InvoiceNumber
	{
		get
		{
			return invoiceNumberField;
		}
		set
		{
			invoiceNumberField = value;
		}
	}

	[XmlElement(Order = 1)]
	public Status Error
	{
		get
		{
			return errorField;
		}
		set
		{
			errorField = value;
		}
	}
}
