using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class AccDocFilter
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

	private string accDocNumberField;

	private string supplierTinField;

	private string supplierNameField;

	private decimal totalMinimumField;

	private bool totalMinimumFieldSpecified;

	private decimal totalMaximumField;

	private bool totalMaximumFieldSpecified;

	private string buyerTinField;

	private string buyerNameField;

	private int adjustingTypeField;

	private bool adjustingTypeFieldSpecified;

	private bool resignNeededField;

	private bool resignNeededFieldSpecified;

	private int[] nonPermittedAccDocTypeListField;

	private DateTime supplyDateStartField;

	private bool supplyDateStartFieldSpecified;

	private DateTime supplyDateEndField;

	private bool supplyDateEndFieldSpecified;

	private DateTime buyerSignDateStartField;

	private bool buyerSignDateStartFieldSpecified;

	private DateTime buyerSignDateEndField;

	private bool buyerSignDateEndFieldSpecified;

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
	public string AccDocNumber
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
	public decimal TotalMinimum
	{
		get
		{
			return totalMinimumField;
		}
		set
		{
			totalMinimumField = value;
		}
	}

	[XmlIgnore]
	public bool TotalMinimumSpecified
	{
		get
		{
			return totalMinimumFieldSpecified;
		}
		set
		{
			totalMinimumFieldSpecified = value;
		}
	}

	[XmlElement(Order = 9)]
	public decimal TotalMaximum
	{
		get
		{
			return totalMaximumField;
		}
		set
		{
			totalMaximumField = value;
		}
	}

	[XmlIgnore]
	public bool TotalMaximumSpecified
	{
		get
		{
			return totalMaximumFieldSpecified;
		}
		set
		{
			totalMaximumFieldSpecified = value;
		}
	}

	[XmlElement(Order = 10)]
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

	[XmlElement(Order = 11)]
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

	[XmlElement(Order = 12)]
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

	[XmlElement(Order = 13)]
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

	[XmlElement("NonPermittedAccDocTypeList", Order = 14)]
	public int[] NonPermittedAccDocTypeList
	{
		get
		{
			return nonPermittedAccDocTypeListField;
		}
		set
		{
			nonPermittedAccDocTypeListField = value;
		}
	}

	[XmlElement(DataType = "date", Order = 15)]
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

	[XmlElement(DataType = "date", Order = 16)]
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

	[XmlElement(DataType = "date", Order = 17)]
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

	[XmlElement(DataType = "date", Order = 18)]
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
}
