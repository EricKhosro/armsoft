using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class GetTpBookListResponse : GenericResponse
{
	private TpBookListElement[] tpBookListElementField;

	[XmlElement("TpBookListElement", Order = 0)]
	public TpBookListElement[] TpBookListElement
	{
		get
		{
			return tpBookListElementField;
		}
		set
		{
			tpBookListElementField = value;
		}
	}
}
