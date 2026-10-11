using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class OverdueInvalidateAccDocRequest : GenericAuthenticatedRequest
{
	private byte[] invalidationRequestXmlField;

	private string clientTypeField;

	private string clientDescriptionField;

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

	[XmlElement(Order = 1)]
	public string ClientType
	{
		get
		{
			return clientTypeField;
		}
		set
		{
			clientTypeField = value;
		}
	}

	[XmlElement(Order = 2)]
	public string ClientDescription
	{
		get
		{
			return clientDescriptionField;
		}
		set
		{
			clientDescriptionField = value;
		}
	}
}
