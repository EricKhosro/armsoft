using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class ResignAccDocByBuyerRequest : GenericAuthenticatedRequest
{
	private DateTime buyerSignDateField;

	private bool buyerSignDateFieldSpecified;

	private byte[] xmlFileField;

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
}
