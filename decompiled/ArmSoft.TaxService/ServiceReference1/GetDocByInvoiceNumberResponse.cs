using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class GetDocByInvoiceNumberResponse : GenericResponse
{
	private byte[] docField;

	private int invoiceStatusField;

	private bool invoiceStatusFieldSpecified;

	[XmlElement(DataType = "base64Binary", Order = 0)]
	public byte[] Doc
	{
		get
		{
			return docField;
		}
		set
		{
			docField = value;
		}
	}

	[XmlElement(Order = 1)]
	public int InvoiceStatus
	{
		get
		{
			return invoiceStatusField;
		}
		set
		{
			invoiceStatusField = value;
		}
	}

	[XmlIgnore]
	public bool InvoiceStatusSpecified
	{
		get
		{
			return invoiceStatusFieldSpecified;
		}
		set
		{
			invoiceStatusFieldSpecified = value;
		}
	}
}
