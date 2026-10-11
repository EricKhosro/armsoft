using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class InvalidateInvoicesRequest : GenericAuthenticatedRequest
{
	private SignedData signedDataField;

	private string clientTypeField;

	private string clientDescriptionField;

	[XmlElement(Order = 0)]
	public SignedData SignedData
	{
		get
		{
			return signedDataField;
		}
		set
		{
			signedDataField = value;
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
