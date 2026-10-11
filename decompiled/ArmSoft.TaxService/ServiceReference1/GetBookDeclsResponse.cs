using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class GetBookDeclsResponse : GenericResponse
{
	private BookDeclsListElement[] bookDeclsListElementField;

	[XmlElement("BookDeclsListElement", Order = 0)]
	public BookDeclsListElement[] BookDeclsListElement
	{
		get
		{
			return bookDeclsListElementField;
		}
		set
		{
			bookDeclsListElementField = value;
		}
	}
}
