using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ArmSoft.TaxService.Models.XML.AccDoc;

[Serializable]
[GeneratedCode("xsd", "4.8.3928.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class Signature
{
	private string signatureTypeField;

	private byte[] signatureValueField;

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

	[XmlElement(DataType = "base64Binary")]
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
