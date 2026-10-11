using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class RevokeSignatureRequest : GenericAuthenticatedRequest
{
	private byte[] invalidationRequestXmlField;

	[XmlElement(DataType = "base64Binary", Order = 0)]
	public byte[] InvalidationRequestXml
	{
		get
		{
			return invalidationRequestXmlField;
		}
		set
		{
			invalidationRequestXmlField = value;
		}
	}
}
