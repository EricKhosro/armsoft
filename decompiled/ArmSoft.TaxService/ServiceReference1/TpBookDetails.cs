using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class TpBookDetails
{
	private long idField;

	private bool idFieldSpecified;

	private string tinField;

	private string bookNumberField;

	private int rowNumberField;

	private bool rowNumberFieldSpecified;

	private DateTime transferDocDateField;

	private bool transferDocDateFieldSpecified;

	private DateTime transferDateField;

	private bool transferDateFieldSpecified;

	private decimal amountField;

	private bool amountFieldSpecified;

	private string receiverField;

	private string destinationField;

	private string addressField;

	private int refRowField;

	private bool refRowFieldSpecified;

	private int statusField;

	private bool statusFieldSpecified;

	private string statusNameField;

	private string transferDocNumberField;

	[XmlElement(Order = 0)]
	public long Id
	{
		get
		{
			return idField;
		}
		set
		{
			idField = value;
		}
	}

	[XmlIgnore]
	public bool IdSpecified
	{
		get
		{
			return idFieldSpecified;
		}
		set
		{
			idFieldSpecified = value;
		}
	}

	[XmlElement(Order = 1)]
	public string Tin
	{
		get
		{
			return tinField;
		}
		set
		{
			tinField = value;
		}
	}

	[XmlElement(Order = 2)]
	public string BookNumber
	{
		get
		{
			return bookNumberField;
		}
		set
		{
			bookNumberField = value;
		}
	}

	[XmlElement(Order = 3)]
	public int RowNumber
	{
		get
		{
			return rowNumberField;
		}
		set
		{
			rowNumberField = value;
		}
	}

	[XmlIgnore]
	public bool RowNumberSpecified
	{
		get
		{
			return rowNumberFieldSpecified;
		}
		set
		{
			rowNumberFieldSpecified = value;
		}
	}

	[XmlElement(DataType = "date", Order = 4)]
	public DateTime TransferDocDate
	{
		get
		{
			return transferDocDateField;
		}
		set
		{
			transferDocDateField = value;
		}
	}

	[XmlIgnore]
	public bool TransferDocDateSpecified
	{
		get
		{
			return transferDocDateFieldSpecified;
		}
		set
		{
			transferDocDateFieldSpecified = value;
		}
	}

	[XmlElement(DataType = "date", Order = 5)]
	public DateTime TransferDate
	{
		get
		{
			return transferDateField;
		}
		set
		{
			transferDateField = value;
		}
	}

	[XmlIgnore]
	public bool TransferDateSpecified
	{
		get
		{
			return transferDateFieldSpecified;
		}
		set
		{
			transferDateFieldSpecified = value;
		}
	}

	[XmlElement(Order = 6)]
	public decimal Amount
	{
		get
		{
			return amountField;
		}
		set
		{
			amountField = value;
		}
	}

	[XmlIgnore]
	public bool AmountSpecified
	{
		get
		{
			return amountFieldSpecified;
		}
		set
		{
			amountFieldSpecified = value;
		}
	}

	[XmlElement(Order = 7)]
	public string Receiver
	{
		get
		{
			return receiverField;
		}
		set
		{
			receiverField = value;
		}
	}

	[XmlElement(Order = 8)]
	public string Destination
	{
		get
		{
			return destinationField;
		}
		set
		{
			destinationField = value;
		}
	}

	[XmlElement(Order = 9)]
	public string Address
	{
		get
		{
			return addressField;
		}
		set
		{
			addressField = value;
		}
	}

	[XmlElement(Order = 10)]
	public int RefRow
	{
		get
		{
			return refRowField;
		}
		set
		{
			refRowField = value;
		}
	}

	[XmlIgnore]
	public bool RefRowSpecified
	{
		get
		{
			return refRowFieldSpecified;
		}
		set
		{
			refRowFieldSpecified = value;
		}
	}

	[XmlElement(Order = 11)]
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

	[XmlElement(Order = 12)]
	public string StatusName
	{
		get
		{
			return statusNameField;
		}
		set
		{
			statusNameField = value;
		}
	}

	[XmlElement(Order = 13)]
	public string TransferDocNumber
	{
		get
		{
			return transferDocNumberField;
		}
		set
		{
			transferDocNumberField = value;
		}
	}
}
