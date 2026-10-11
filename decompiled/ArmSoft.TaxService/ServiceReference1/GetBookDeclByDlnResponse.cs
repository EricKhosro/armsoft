using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class GetBookDeclByDlnResponse : GenericResponse
{
	private BookDecl bookDeclField;

	[XmlElement(Order = 0)]
	public BookDecl BookDecl
	{
		get
		{
			return bookDeclField;
		}
		set
		{
			bookDeclField = value;
		}
	}
}
