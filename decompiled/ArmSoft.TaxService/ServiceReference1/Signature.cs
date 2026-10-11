using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class Signature
{
	private string signatureTypeField;

	private byte[] signatureValueField;

	[XmlElement(Order = 0)]
	public string SignatureType
	{
		get
		{
			return signatureTypeField;
		}
		set
		{
			signatureTypeField = value;
		}
	}

	[XmlElement(DataType = "base64Binary", Order = 1)]
	public byte[] SignatureValue
	{
		get
		{
			return signatureValueField;
		}
		set
		{
			signatureValueField = value;
		}
	}
}
