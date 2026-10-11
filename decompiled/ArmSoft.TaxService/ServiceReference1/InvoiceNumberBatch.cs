using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class InvoiceNumberBatch : SignableData
{
	private InvoiceNumber[] invoiceNumberField;

	private int batchSizeField;

	private bool batchSizeFieldSpecified;

	private int batchIdField;

	private bool batchIdFieldSpecified;

	[XmlElement("InvoiceNumber", Order = 0)]
	public InvoiceNumber[] InvoiceNumber
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

	[XmlAttribute]
	public int BatchSize
	{
		get
		{
			return batchSizeField;
		}
		set
		{
			batchSizeField = value;
		}
	}

	[XmlIgnore]
	public bool BatchSizeSpecified
	{
		get
		{
			return batchSizeFieldSpecified;
		}
		set
		{
			batchSizeFieldSpecified = value;
		}
	}

	[XmlAttribute]
	public int BatchId
	{
		get
		{
			return batchIdField;
		}
		set
		{
			batchIdField = value;
		}
	}

	[XmlIgnore]
	public bool BatchIdSpecified
	{
		get
		{
			return batchIdFieldSpecified;
		}
		set
		{
			batchIdFieldSpecified = value;
		}
	}
}
