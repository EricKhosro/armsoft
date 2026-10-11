using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class InvoiceFilter
{
	private SortCondition sortConditionField;

	private int startPositionField;

	private bool startPositionFieldSpecified;

	private int countField;

	private bool countFieldSpecified;

	private DateTime submitDateStartField;

	private bool submitDateStartFieldSpecified;

	private DateTime submitDateEndField;

	private bool submitDateEndFieldSpecified;

	private string invoiceNumberField;

	private string supplierTinField;

	private string supplierNameField;

	private string buyerTinField;

	private string buyerNameField;

	private decimal totalWithoutVatMinimumField;

	private bool totalWithoutVatMinimumFieldSpecified;

	private decimal totalWithoutVatMaximumField;

	private bool totalWithoutVatMaximumFieldSpecified;

	private decimal totalVatMinimumField;

	private bool totalVatMinimumFieldSpecified;

	private decimal totalVatMaximumField;

	private bool totalVatMaximumFieldSpecified;

	private DateTime supplyDateStartField;

	private bool supplyDateStartFieldSpecified;

	private DateTime supplyDateEndField;

	private bool supplyDateEndFieldSpecified;

	private DateTime buyerSignDateStartField;

	private bool buyerSignDateStartFieldSpecified;

	private DateTime buyerSignDateEndField;

	private bool buyerSignDateEndFieldSpecified;

	private int adjustingTypeField;

	private bool adjustingTypeFieldSpecified;

	private bool resignNeededField;

	private bool resignNeededFieldSpecified;

	private int[] nonPermittedInvoiceTypeListField;

	[XmlElement(Order = 0)]
	public SortCondition SortCondition
	{
		get
		{
			return sortConditionField;
		}
		set
		{
			sortConditionField = value;
		}
	}

	[XmlElement(Order = 1)]
	public int StartPosition
	{
		get
		{
			return startPositionField;
		}
		set
		{
			startPositionField = value;
		}
	}

	[XmlIgnore]
	public bool StartPositionSpecified
	{
		get
		{
			return startPositionFieldSpecified;
		}
		set
		{
			startPositionFieldSpecified = value;
		}
	}

	[XmlElement(Order = 2)]
	public int Count
	{
		get
		{
			return countField;
		}
		set
		{
			countField = value;
		}
	}

	[XmlIgnore]
	public bool CountSpecified
	{
		get
		{
			return countFieldSpecified;
		}
		set
		{
			countFieldSpecified = value;
		}
	}

	[XmlElement(DataType = "date", Order = 3)]
	public DateTime SubmitDateStart
	{
		get
		{
			return submitDateStartField;
		}
		set
		{
			submitDateStartField = value;
		}
	}

	[XmlIgnore]
	public bool SubmitDateStartSpecified
	{
		get
		{
			return submitDateStartFieldSpecified;
		}
		set
		{
			submitDateStartFieldSpecified = value;
		}
	}

	[XmlElement(DataType = "date", Order = 4)]
	public DateTime SubmitDateEnd
	{
		get
		{
			return submitDateEndField;
		}
		set
		{
			submitDateEndField = value;
		}
	}

	[XmlIgnore]
	public bool SubmitDateEndSpecified
	{
		get
		{
			return submitDateEndFieldSpecified;
		}
		set
		{
			submitDateEndFieldSpecified = value;
		}
	}

	[XmlElement(Order = 5)]
	public string InvoiceNumber
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

	[XmlElement(Order = 8)]
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

	[XmlElement(Order = 9)]
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

	[XmlElement(Order = 10)]
	public decimal TotalWithoutVatMinimum
	{
		get
		{
			return totalWithoutVatMinimumField;
		}
		set
		{
			totalWithoutVatMinimumField = value;
		}
	}

	[XmlIgnore]
	public bool TotalWithoutVatMinimumSpecified
	{
		get
		{
			return totalWithoutVatMinimumFieldSpecified;
		}
		set
		{
			totalWithoutVatMinimumFieldSpecified = value;
		}
	}

	[XmlElement(Order = 11)]
	public decimal TotalWithoutVatMaximum
	{
		get
		{
			return totalWithoutVatMaximumField;
		}
		set
		{
			totalWithoutVatMaximumField = value;
		}
	}

	[XmlIgnore]
	public bool TotalWithoutVatMaximumSpecified
	{
		get
		{
			return totalWithoutVatMaximumFieldSpecified;
		}
		set
		{
			totalWithoutVatMaximumFieldSpecified = value;
		}
	}

	[XmlElement(Order = 12)]
	public decimal TotalVatMinimum
	{
		get
		{
			return totalVatMinimumField;
		}
		set
		{
			totalVatMinimumField = value;
		}
	}

	[XmlIgnore]
	public bool TotalVatMinimumSpecified
	{
		get
		{
			return totalVatMinimumFieldSpecified;
		}
		set
		{
			totalVatMinimumFieldSpecified = value;
		}
	}

	[XmlElement(Order = 13)]
	public decimal TotalVatMaximum
	{
		get
		{
			return totalVatMaximumField;
		}
		set
		{
			totalVatMaximumField = value;
		}
	}

	[XmlIgnore]
	public bool TotalVatMaximumSpecified
	{
		get
		{
			return totalVatMaximumFieldSpecified;
		}
		set
		{
			totalVatMaximumFieldSpecified = value;
		}
	}

	[XmlElement(DataType = "date", Order = 14)]
	public DateTime SupplyDateStart
	{
		get
		{
			return supplyDateStartField;
		}
		set
		{
			supplyDateStartField = value;
		}
	}

	[XmlIgnore]
	public bool SupplyDateStartSpecified
	{
		get
		{
			return supplyDateStartFieldSpecified;
		}
		set
		{
			supplyDateStartFieldSpecified = value;
		}
	}

	[XmlElement(DataType = "date", Order = 15)]
	public DateTime SupplyDateEnd
	{
		get
		{
			return supplyDateEndField;
		}
		set
		{
			supplyDateEndField = value;
		}
	}

	[XmlIgnore]
	public bool SupplyDateEndSpecified
	{
		get
		{
			return supplyDateEndFieldSpecified;
		}
		set
		{
			supplyDateEndFieldSpecified = value;
		}
	}

	[XmlElement(DataType = "date", Order = 16)]
	public DateTime BuyerSignDateStart
	{
		get
		{
			return buyerSignDateStartField;
		}
		set
		{
			buyerSignDateStartField = value;
		}
	}

	[XmlIgnore]
	public bool BuyerSignDateStartSpecified
	{
		get
		{
			return buyerSignDateStartFieldSpecified;
		}
		set
		{
			buyerSignDateStartFieldSpecified = value;
		}
	}

	[XmlElement(DataType = "date", Order = 17)]
	public DateTime BuyerSignDateEnd
	{
		get
		{
			return buyerSignDateEndField;
		}
		set
		{
			buyerSignDateEndField = value;
		}
	}

	[XmlIgnore]
	public bool BuyerSignDateEndSpecified
	{
		get
		{
			return buyerSignDateEndFieldSpecified;
		}
		set
		{
			buyerSignDateEndFieldSpecified = value;
		}
	}

	[XmlElement(Order = 18)]
	public int AdjustingType
	{
		get
		{
			return adjustingTypeField;
		}
		set
		{
			adjustingTypeField = value;
		}
	}

	[XmlIgnore]
	public bool AdjustingTypeSpecified
	{
		get
		{
			return adjustingTypeFieldSpecified;
		}
		set
		{
			adjustingTypeFieldSpecified = value;
		}
	}

	[XmlElement(Order = 19)]
	public bool ResignNeeded
	{
		get
		{
			return resignNeededField;
		}
		set
		{
			resignNeededField = value;
		}
	}

	[XmlIgnore]
	public bool ResignNeededSpecified
	{
		get
		{
			return resignNeededFieldSpecified;
		}
		set
		{
			resignNeededFieldSpecified = value;
		}
	}

	[XmlElement("NonPermittedInvoiceTypeList", Order = 20)]
	public int[] NonPermittedInvoiceTypeList
	{
		get
		{
			return nonPermittedInvoiceTypeListField;
		}
		set
		{
			nonPermittedInvoiceTypeListField = value;
		}
	}
}
