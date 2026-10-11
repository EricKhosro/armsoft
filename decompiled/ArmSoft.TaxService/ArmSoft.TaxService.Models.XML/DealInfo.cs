using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ArmSoft.TaxService.Models.XML;

[Serializable]
[GeneratedCode("xsd", "4.8.3928.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
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

	[XmlElement(DataType = "date")]
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
