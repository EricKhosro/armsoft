using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class DealInfo
{
	private DateTime dealDateField;

	private bool dealDateFieldSpecified;

	private string dealNumberField;

	private int deliveryBookPageField;

	private bool deliveryBookPageFieldSpecified;

	private int deliveryBookLineField;

	private bool deliveryBookLineFieldSpecified;

	private int deliveryBookNumberField;

	private bool deliveryBookNumberFieldSpecified;

	[XmlElement(DataType = "date", Order = 0)]
	public DateTime DealDate
	{
		get
		{
			return dealDateField;
		}
		set
		{
			dealDateField = value;
		}
	}

	[XmlIgnore]
	public bool DealDateSpecified
	{
		get
		{
			return dealDateFieldSpecified;
		}
		set
		{
			dealDateFieldSpecified = value;
		}
	}

	[XmlElement(Order = 1)]
	public string DealNumber
	{
		get
		{
			return dealNumberField;
		}
		set
		{
			dealNumberField = value;
		}
	}

	[XmlElement(Order = 2)]
	public int DeliveryBookPage
	{
		get
		{
			return deliveryBookPageField;
		}
		set
		{
			deliveryBookPageField = value;
		}
	}

	[XmlIgnore]
	public bool DeliveryBookPageSpecified
	{
		get
		{
			return deliveryBookPageFieldSpecified;
		}
		set
		{
			deliveryBookPageFieldSpecified = value;
		}
	}

	[XmlElement(Order = 3)]
	public int DeliveryBookLine
	{
		get
		{
			return deliveryBookLineField;
		}
		set
		{
			deliveryBookLineField = value;
		}
	}

	[XmlIgnore]
	public bool DeliveryBookLineSpecified
	{
		get
		{
			return deliveryBookLineFieldSpecified;
		}
		set
		{
			deliveryBookLineFieldSpecified = value;
		}
	}

	[XmlElement(Order = 4)]
	public int DeliveryBookNumber
	{
		get
		{
			return deliveryBookNumberField;
		}
		set
		{
			deliveryBookNumberField = value;
		}
	}

	[XmlIgnore]
	public bool DeliveryBookNumberSpecified
	{
		get
		{
			return deliveryBookNumberFieldSpecified;
		}
		set
		{
			deliveryBookNumberFieldSpecified = value;
		}
	}
}
