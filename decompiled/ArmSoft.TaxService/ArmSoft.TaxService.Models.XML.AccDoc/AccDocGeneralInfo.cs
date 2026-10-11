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
public class AccDocGeneralInfo
{
	private string parentInvoiceNumberField;

	private string ecrReceiptField;

	private DateTime parentInvoiceSubmitDateField;

	private bool parentInvoiceSubmitDateFieldSpecified;

	private InvoiceNumber invoiceNumberField;

	private int adjustmentDiffFlagField;

	private bool adjustmentDiffFlagFieldSpecified;

	private DateTime deliveryDateField;

	private bool deliveryDateFieldSpecified;

	private int procedureField;

	private bool procedureFieldSpecified;

	private AccDocDealInfo dealInfoField;

	private string additionalDataField;

	private bool adjustmentAccountField;

	private bool adjustmentAccountFieldSpecified;

	private bool commissionAgreementField;

	private bool commissionAgreementFieldSpecified;

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

	[XmlElement(DataType = "date")]
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

	[XmlElement(DataType = "date")]
	public DateTime DeliveryDate
	{
		get
		{
			return deliveryDateField;
		}
		set
		{
			deliveryDateField = value;
		}
	}

	[XmlIgnore]
	public bool DeliveryDateSpecified
	{
		get
		{
			return deliveryDateFieldSpecified;
		}
		set
		{
			deliveryDateFieldSpecified = value;
		}
	}

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

	public AccDocDealInfo DealInfo
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

	public bool CommissionAgreement
	{
		get
		{
			return commissionAgreementField;
		}
		set
		{
			commissionAgreementField = value;
		}
	}

	[XmlIgnore]
	public bool CommissionAgreementSpecified
	{
		get
		{
			return commissionAgreementFieldSpecified;
		}
		set
		{
			commissionAgreementFieldSpecified = value;
		}
	}
}
