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
[XmlRoot(Namespace = "http://www.taxservice.am/tp3/invoice/definitions", IsNullable = false)]
public class SignedAccDocData
{
	private Data dataField;

	private Signature[] signatureField;

	private AccDocMetadata accDocMetadataField;

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

	[XmlElement("Signature")]
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
