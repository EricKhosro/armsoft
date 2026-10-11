using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class GeneralInfo
{
	private string parentInvoiceNumberField;

	private string ecrReceiptField;

	private int adjustmentDiffFlagField;

	private bool adjustmentDiffFlagFieldSpecified;

	private DateTime parentInvoiceSubmitDateField;

	private bool parentInvoiceSubmitDateFieldSpecified;

	private InvoiceNumber invoiceNumberField;

	private DateTime supplyDateField;

	private int procedureField;

	private bool procedureFieldSpecified;

	private DealInfo dealInfoField;

	private string additionalDataField;

	private bool adjustmentAccountField;

	private bool adjustmentAccountFieldSpecified;

	[XmlElement(Order = 0)]
	public string ParentInvoiceNumber
	{
		get
		{
			return parentInvoiceNumberField;
		}
		set
		{
			parentInvoiceNumberField = value;
		}
	}

	[XmlElement(Order = 1)]
	public string EcrReceipt
	{
		get
		{
			return ecrReceiptField;
		}
		set
		{
			ecrReceiptField = value;
		}
	}

	[XmlElement(Order = 2)]
	public int AdjustmentDiffFlag
	{
		get
		{
			return adjustmentDiffFlagField;
		}
		set
		{
			adjustmentDiffFlagField = value;
		}
	}

	[XmlIgnore]
	public bool AdjustmentDiffFlagSpecified
	{
		get
		{
			return adjustmentDiffFlagFieldSpecified;
		}
		set
		{
			adjustmentDiffFlagFieldSpecified = value;
		}
	}

	[XmlElement(DataType = "date", Order = 3)]
	public DateTime ParentInvoiceSubmitDate
	{
		get
		{
			return parentInvoiceSubmitDateField;
		}
		set
		{
			parentInvoiceSubmitDateField = value;
		}
	}

	[XmlIgnore]
	public bool ParentInvoiceSubmitDateSpecified
	{
		get
		{
			return parentInvoiceSubmitDateFieldSpecified;
		}
		set
		{
			parentInvoiceSubmitDateFieldSpecified = value;
		}
	}

	[XmlElement(Order = 4)]
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

	[XmlElement(DataType = "date", Order = 5)]
	public DateTime SupplyDate
	{
		get
		{
			return supplyDateField;
		}
		set
		{
			supplyDateField = value;
		}
	}

	[XmlElement(Order = 6)]
	public int Procedure
	{
		get
		{
			return procedureField;
		}
		set
		{
			procedureField = value;
		}
	}

	[XmlIgnore]
	public bool ProcedureSpecified
	{
		get
		{
			return procedureFieldSpecified;
		}
		set
		{
			procedureFieldSpecified = value;
		}
	}

	[XmlElement(Order = 7)]
	public DealInfo DealInfo
	{
		get
		{
			return dealInfoField;
		}
		set
		{
			dealInfoField = value;
		}
	}

	[XmlElement(Order = 8)]
	public string AdditionalData
	{
		get
		{
			return additionalDataField;
		}
		set
		{
			additionalDataField = value;
		}
	}

	[XmlElement(Order = 9)]
	public bool AdjustmentAccount
	{
		get
		{
			return adjustmentAccountField;
		}
		set
		{
			adjustmentAccountField = value;
		}
	}

	[XmlIgnore]
	public bool AdjustmentAccountSpecified
	{
		get
		{
			return adjustmentAccountFieldSpecified;
		}
		set
		{
			adjustmentAccountFieldSpecified = value;
		}
	}
}
