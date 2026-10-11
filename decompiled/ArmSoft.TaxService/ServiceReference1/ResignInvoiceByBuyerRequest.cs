using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class ResignInvoiceByBuyerRequest : GenericAuthenticatedRequest
{
	private DateTime buyerSignDateField;

	private bool buyerSignDateFieldSpecified;

	private byte[] xmlFileField;

	private string clientTypeField;

	private string clientDescriptionField;

	[XmlElement(DataType = "date", Order = 0)]
	public DateTime BuyerSignDate
	{
		get
		{
			return buyerSignDateField;
		}
		set
		{
			buyerSignDateField = value;
		}
	}

	[XmlIgnore]
	public bool BuyerSignDateSpecified
	{
		get
		{
			return buyerSignDateFieldSpecified;
		}
		set
		{
			buyerSignDateFieldSpecified = value;
		}
	}

	[XmlElement(DataType = "base64Binary", Order = 1)]
	public byte[] xmlFile
	{
		get
		{
			return xmlFileField;
		}
		set
		{
			xmlFileField = value;
		}
	}

	[XmlElement(Order = 2)]
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

	[XmlElement(Order = 3)]
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
