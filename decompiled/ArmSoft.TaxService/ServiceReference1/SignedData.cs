using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class SignedData
{
	private Data dataField;

	private Signature[] signatureField;

	private InvoiceMetadata invoiceMetadataField;

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
	public InvoiceMetadata InvoiceMetadata
	{
		get
		{
			return invoiceMetadataField;
		}
		set
		{
			invoiceMetadataField = value;
		}
	}
}
