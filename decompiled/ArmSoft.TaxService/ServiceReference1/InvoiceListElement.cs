using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class InvoiceListElement
{
	private int statusField;

	private bool statusFieldSpecified;

	private int invoiceStatusField;

	private bool invoiceStatusFieldSpecified;

	private InvoiceNumber invoiceNumberField;

	private string buyerNameField;

	private string buyerTinField;

	private string supplierNameField;

	private string supplierTinField;

	private double totalWithoutVatField;

	private double totalVatField;

	private double totalEnvironmentalFeeField;

	private double totalExciseField;

	private string ecrReceiptField;

	private int adjustmentDiffFlagField;

	private bool adjustmentDiffFlagFieldSpecified;

	private string buyerEmailField;

	private int? procedureField;

	private DateTime submissionDateField;

	private DateTime supplyDateField;

	private bool signedByBuyerField;

	private DateTime buyerSignDateField;

	private DateTime invalidationDateField;

	private int invoiceTypeField;

	private string additionalDataField;

	private string buyerPassportField;

	private InvoiceListElement parentInvoiceNumberField;

	private InvoiceListElement[] returnedInvoiceListField;

	private string invalidationReasonField;

	private int invalidationTypeField;

	private DateTime invalidationAcceptOrRejectDateField;

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

	[XmlElement(Order = 2)]
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
	public double TotalWithoutVat
	{
		get
		{
			return totalWithoutVatField;
		}
		set
		{
			totalWithoutVatField = value;
		}
	}

	[XmlElement(Order = 8)]
	public double TotalVat
	{
		get
		{
			return totalVatField;
		}
		set
		{
			totalVatField = value;
		}
	}

	[XmlElement(Order = 9)]
	public double TotalEnvironmentalFee
	{
		get
		{
			return totalEnvironmentalFeeField;
		}
		set
		{
			totalEnvironmentalFeeField = value;
		}
	}

	[XmlElement(Order = 10)]
	public double TotalExcise
	{
		get
		{
			return totalExciseField;
		}
		set
		{
			totalExciseField = value;
		}
	}

	[XmlElement(Order = 11)]
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

	[XmlElement(Order = 12)]
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

	[XmlElement(Order = 13)]
	public string BuyerEmail
	{
		get
		{
			return buyerEmailField;
		}
		set
		{
			buyerEmailField = value;
		}
	}

	[XmlElement(IsNullable = true, Order = 14)]
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

	[XmlElement(Order = 15)]
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

	[XmlElement(DataType = "date", Order = 16)]
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

	[XmlElement(Order = 17)]
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

	[XmlElement(Order = 18)]
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

	[XmlElement(Order = 19)]
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

	[XmlElement(Order = 20)]
	public int InvoiceType
	{
		get
		{
			return invoiceTypeField;
		}
		set
		{
			invoiceTypeField = value;
		}
	}

	[XmlElement(Order = 21)]
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

	[XmlElement(Order = 22)]
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

	[XmlElement(Order = 23)]
	public InvoiceListElement ParentInvoiceNumber
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

	[XmlElement("ReturnedInvoiceList", Order = 24)]
	public InvoiceListElement[] ReturnedInvoiceList
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

	[XmlElement(Order = 25)]
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

	[XmlElement(Order = 26)]
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

	[XmlElement(Order = 27)]
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
}
