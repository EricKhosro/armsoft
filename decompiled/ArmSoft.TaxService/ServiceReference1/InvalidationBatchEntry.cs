using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class InvalidationBatchEntry
{
	private InvoiceNumber invoiceNumberField;

	private string reasonField;

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
	public string Reason
	{
		get
		{
			return reasonField;
		}
		set
		{
			reasonField = value;
		}
	}
}
