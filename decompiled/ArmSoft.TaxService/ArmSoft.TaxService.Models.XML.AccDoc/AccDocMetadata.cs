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
public class AccDocMetadata
{
	private string submissionDateField;

	private string coSignDateField;

	private int invoiceDocStatusField;

	private bool invoiceDocStatusFieldSpecified;

	public string SubmissionDate
	{
		get
		{
			return submissionDateField;
		}
		set
		{
			submissionDateField = value;
		}
	}

	public string CoSignDate
	{
		get
		{
			return coSignDateField;
		}
		set
		{
			coSignDateField = value;
		}
	}

	public int InvoiceDocStatus
	{
		get
		{
			return invoiceDocStatusField;
		}
		set
		{
			invoiceDocStatusField = value;
		}
	}

	[XmlIgnore]
	public bool InvoiceDocStatusSpecified
	{
		get
		{
			return invoiceDocStatusFieldSpecified;
		}
		set
		{
			invoiceDocStatusFieldSpecified = value;
		}
	}
}
