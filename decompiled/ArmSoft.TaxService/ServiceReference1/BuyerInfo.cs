using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class BuyerInfo
{
	private string vATNumberField;

	private TaxpayerInfo taxpayerField;

	private Mediator mediatorField;

	private string deliveryMethodField;

	private string deliveryLocationField;

	private string buyerEmailField;

	private string buyerResidenceAddressField;

	private string factualBuyerAddressField;

	[XmlElement(Order = 0)]
	public string VATNumber
	{
		get
		{
			return vATNumberField;
		}
		set
		{
			vATNumberField = value;
		}
	}

	[XmlElement(Order = 1)]
	public TaxpayerInfo Taxpayer
	{
		get
		{
			return taxpayerField;
		}
		set
		{
			taxpayerField = value;
		}
	}

	[XmlElement(Order = 2)]
	public Mediator Mediator
	{
		get
		{
			return mediatorField;
		}
		set
		{
			mediatorField = value;
		}
	}

	[XmlElement(Order = 3)]
	public string DeliveryMethod
	{
		get
		{
			return deliveryMethodField;
		}
		set
		{
			deliveryMethodField = value;
		}
	}

	[XmlElement(Order = 4)]
	public string DeliveryLocation
	{
		get
		{
			return deliveryLocationField;
		}
		set
		{
			deliveryLocationField = value;
		}
	}

	[XmlElement(Order = 5)]
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

	[XmlElement(Order = 6)]
	public string BuyerResidenceAddress
	{
		get
		{
			return buyerResidenceAddressField;
		}
		set
		{
			buyerResidenceAddressField = value;
		}
	}

	[XmlElement(Order = 7)]
	public string FactualBuyerAddress
	{
		get
		{
			return factualBuyerAddressField;
		}
		set
		{
			factualBuyerAddressField = value;
		}
	}
}
