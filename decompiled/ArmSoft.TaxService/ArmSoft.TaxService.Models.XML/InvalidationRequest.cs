using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ArmSoft.TaxService.Models.XML;

[Serializable]
[GeneratedCode("xsd", "4.8.3928.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class InvalidationRequest : SignableData
{
	private InvoiceNumber invoiceNumberField;

	private string tINField;

	private string nameField;

	private string reasonField;

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
