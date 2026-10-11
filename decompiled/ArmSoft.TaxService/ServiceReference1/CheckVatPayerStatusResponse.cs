using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class CheckVatPayerStatusResponse : GenericResponse
{
	private bool vatPayerField;

	private bool hasSubmittedInvoicesField;

	private bool hasSubmittedInvoicesFieldSpecified;

	private DateTime firstInvoiceDateField;

	private bool firstInvoiceDateFieldSpecified;

	[XmlElement(Order = 0)]
	public bool VatPayer
	{
		get
		{
			return vatPayerField;
		}
		set
		{
			vatPayerField = value;
		}
	}

	[XmlElement(Order = 1)]
	public bool HasSubmittedInvoices
	{
		get
		{
			return hasSubmittedInvoicesField;
		}
		set
		{
			hasSubmittedInvoicesField = value;
		}
	}

	[XmlIgnore]
	public bool HasSubmittedInvoicesSpecified
	{
		get
		{
			return hasSubmittedInvoicesFieldSpecified;
		}
		set
		{
			hasSubmittedInvoicesFieldSpecified = value;
		}
	}

	[XmlElement(Order = 2)]
	public DateTime FirstInvoiceDate
	{
		get
		{
			return firstInvoiceDateField;
		}
		set
		{
			firstInvoiceDateField = value;
		}
	}

	[XmlIgnore]
	public bool FirstInvoiceDateSpecified
	{
		get
		{
			return firstInvoiceDateFieldSpecified;
		}
		set
		{
			firstInvoiceDateFieldSpecified = value;
		}
	}
}
