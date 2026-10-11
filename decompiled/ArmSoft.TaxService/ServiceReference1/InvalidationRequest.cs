using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class InvalidationRequest : SignableData
{
	private string userField;

	private InvoiceNumber invoiceNumberField;

	private string tINField;

	private string nameField;

	private string reasonField;

	[XmlElement(Order = 0)]
	public string User
	{
		get
		{
			return userField;
		}
		set
		{
			userField = value;
		}
	}

	[XmlElement(Order = 1)]
	public InvoiceNumber InvoiceNumber
	{
		get
		{
			return invoiceNumberField;
		}
		set
		{
			invoiceNumberField = value;
		}
	}

	[XmlElement(Order = 2)]
	public string TIN
	{
		get
		{
			return tINField;
		}
		set
		{
			tINField = value;
		}
	}

	[XmlElement(Order = 3)]
	public string Name
	{
		get
		{
			return nameField;
		}
		set
		{
			nameField = value;
		}
	}

	[XmlElement(Order = 4)]
	public string Reason
	{
		get
		{
			return reasonField;
		}
		set
		{
			reasonField = value;
		}
	}
}
