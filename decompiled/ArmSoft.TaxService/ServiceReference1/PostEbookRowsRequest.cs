using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class PostEbookRowsRequest : GenericAuthenticatedRequest
{
	private string bookNumberField;

	private TpBookDetails[] bookDetailsField;

	[XmlElement(Order = 0)]
	public string BookNumber
	{
		get
		{
			return bookNumberField;
		}
		set
		{
			bookNumberField = value;
		}
	}

	[XmlElement("BookDetails", Order = 1)]
	public TpBookDetails[] BookDetails
	{
		get
		{
			return bookDetailsField;
		}
		set
		{
			bookDetailsField = value;
		}
	}
}
