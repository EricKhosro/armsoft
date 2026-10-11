using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class CheckInvalidateInvoiceRequest : GenericAuthenticatedRequest
{
	private InvoiceNumber invoiceNumberField;

	private int byBuyerFlagField;

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
	public int ByBuyerFlag
	{
		get
		{
			return byBuyerFlagField;
		}
		set
		{
			byBuyerFlagField = value;
		}
	}
}
