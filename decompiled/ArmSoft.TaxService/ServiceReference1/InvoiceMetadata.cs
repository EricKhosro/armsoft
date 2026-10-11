using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class InvoiceMetadata
{
	private DateTime submissionDateField;

	private bool submissionDateFieldSpecified;

	private DateTime coSignDateField;

	private bool coSignDateFieldSpecified;

	private BatchInfo batchInfoField;

	private int invoiceDocStatusField;

	private bool invoiceDocStatusFieldSpecified;

	[XmlElement(Order = 0)]
	public DateTime SubmissionDate
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

	[XmlIgnore]
	public bool SubmissionDateSpecified
	{
		get
		{
			return submissionDateFieldSpecified;
		}
		set
		{
			submissionDateFieldSpecified = value;
		}
	}

	[XmlElement(Order = 1)]
	public DateTime CoSignDate
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

	[XmlIgnore]
	public bool CoSignDateSpecified
	{
		get
		{
			return coSignDateFieldSpecified;
		}
		set
		{
			coSignDateFieldSpecified = value;
		}
	}

	[XmlElement(Order = 2)]
	public BatchInfo BatchInfo
	{
		get
		{
			return batchInfoField;
		}
		set
		{
			batchInfoField = value;
		}
	}

	[XmlElement(Order = 3)]
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
