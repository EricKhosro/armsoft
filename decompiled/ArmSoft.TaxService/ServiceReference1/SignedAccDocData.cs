using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class SignedAccDocData
{
	private Data dataField;

	private Signature[] signatureField;

	private AccDocMetadata accDocMetadataField;

	[XmlElement(Order = 0)]
	public Data Data
	{
		get
		{
			return dataField;
		}
		set
		{
			dataField = value;
		}
	}

	[XmlElement("Signature", Order = 1)]
	public Signature[] Signature
	{
		get
		{
			return signatureField;
		}
		set
		{
			signatureField = value;
		}
	}

	[XmlElement(Order = 2)]
	public AccDocMetadata AccDocMetadata
	{
		get
		{
			return accDocMetadataField;
		}
		set
		{
			accDocMetadataField = value;
		}
	}
}
