using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class AccDocListElement
{
	private int statusField;

	private bool statusFieldSpecified;

	private InvoiceNumber accDocNumberField;

	private int invoiceStatusField;

	private bool invoiceStatusFieldSpecified;

	private string buyerNameField;

	private string buyerTinField;

	private string supplierNameField;

	private string supplierTinField;

	private DateTime submissionDateField;

	private DateTime supplyDateField;

	private bool signedByBuyerField;

	private DateTime buyerSignDateField;

	private DateTime invalidationDateField;

	private int accDocTypeField;

	private string additionalDataField;

	private string buyerPassportField;

	private double totalField;

	private string ecrReceiptField;

	private int adjustmentDiffFlagField;

	private bool adjustmentDiffFlagFieldSpecified;

	private AccDocListElement parentInvoiceNumberField;

	private AccDocListElement[] returnedInvoiceListField;

	private string invalidationReasonField;

	private int invalidationTypeField;

	private DateTime invalidationAcceptOrRejectDateField;

	private int? procedureField;

	[XmlElement(Order = 0)]
	public int Status
	{
		get
		{
			return statusField;
		}
		set
		{
			statusField = value;
		}
	}

	[XmlIgnore]
	public bool StatusSpecified
	{
		get
		{
			return statusFieldSpecified;
		}
		set
		{
			statusFieldSpecified = value;
		}
	}

	[XmlElement(Order = 1)]
	public InvoiceNumber AccDocNumber
	{
		get
		{
			return accDocNumberField;
		}
		set
		{
			accDocNumberField = value;
		}
	}

	[XmlElement(Order = 2)]
	public int InvoiceStatus
	{
		get
		{
			return invoiceStatusField;
		}
		set
		{
			invoiceStatusField = value;
		}
	}

	[XmlIgnore]
	public bool InvoiceStatusSpecified
	{
		get
		{
			return invoiceStatusFieldSpecified;
		}
		set
		{
			invoiceStatusFieldSpecified = value;
		}
	}

	[XmlElement(Order = 3)]
	public string BuyerName
	{
		get
		{
			return buyerNameField;
		}
		set
		{
			buyerNameField = value;
		}
	}

	[XmlElement(Order = 4)]
	public string BuyerTin
	{
		get
		{
			return buyerTinField;
		}
		set
		{
			buyerTinField = value;
		}
	}

	[XmlElement(Order = 5)]
	public string SupplierName
	{
		get
		{
			return supplierNameField;
		}
		set
		{
			supplierNameField = value;
		}
	}

	[XmlElement(Order = 6)]
	public string SupplierTin
	{
		get
		{
			return supplierTinField;
		}
		set
		{
			supplierTinField = value;
		}
	}

	[XmlElement(Order = 7)]
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

	[XmlElement(DataType = "date", Order = 8)]
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

	[XmlElement(Order = 9)]
	public bool SignedByBuyer
	{
		get
		{
			return signedByBuyerField;
		}
		set
		{
			signedByBuyerField = value;
		}
	}

	[XmlElement(Order = 10)]
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

	[XmlElement(Order = 11)]
	public DateTime InvalidationDate
	{
		get
		{
			return invalidationDateField;
		}
		set
		{
			invalidationDateField = value;
		}
	}

	[XmlElement(Order = 12)]
	public int AccDocType
	{
		get
		{
			return accDocTypeField;
		}
		set
		{
			accDocTypeField = value;
		}
	}

	[XmlElement(Order = 13)]
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

	[XmlElement(Order = 14)]
	public string BuyerPassport
	{
		get
		{
			return buyerPassportField;
		}
		set
		{
			buyerPassportField = value;
		}
	}

	[XmlElement(Order = 15)]
	public double Total
	{
		get
		{
			return totalField;
		}
		set
		{
			totalField = value;
		}
	}

	[XmlElement(Order = 16)]
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

	[XmlElement(Order = 17)]
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

	[XmlElement(Order = 18)]
	public AccDocListElement ParentInvoiceNumber
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

	[XmlElement("ReturnedInvoiceList", Order = 19)]
	public AccDocListElement[] ReturnedInvoiceList
	{
		get
		{
			return returnedInvoiceListField;
		}
		set
		{
			returnedInvoiceListField = value;
		}
	}

	[XmlElement(Order = 20)]
	public string InvalidationReason
	{
		get
		{
			return invalidationReasonField;
		}
		set
		{
			invalidationReasonField = value;
		}
	}

	[XmlElement(Order = 21)]
	public int InvalidationType
	{
		get
		{
			return invalidationTypeField;
		}
		set
		{
			invalidationTypeField = value;
		}
	}

	[XmlElement(Order = 22)]
	public DateTime InvalidationAcceptOrRejectDate
	{
		get
		{
			return invalidationAcceptOrRejectDateField;
		}
		set
		{
			invalidationAcceptOrRejectDateField = value;
		}
	}

	[XmlElement(IsNullable = true, Order = 23)]
	public int? Procedure
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
}
