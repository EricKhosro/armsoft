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
public class BuyerInfo
{
	private string vATNumberField;

	private TaxpayerInfo taxpayerField;

	private string buyerEmailField;

	private Mediator mediatorField;

	private string deliveryMethodField;

	private string deliveryLocationField;

	private string buyerResidenceAddressField;

	private string factualBuyerAddressField;

	private bool isCitizenArmeniaField;

	private bool isCitizenArmeniaFieldSpecified;

	private bool insuringEmployerField;

	private bool insuringEmployerFieldSpecified;

	private SocialCostPerson socialCostPersonField;

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

	public bool IsCitizenArmenia
	{
		get
		{
			return isCitizenArmeniaField;
		}
		set
		{
			isCitizenArmeniaField = value;
		}
	}

	[XmlIgnore]
	public bool IsCitizenArmeniaSpecified
	{
		get
		{
			return isCitizenArmeniaFieldSpecified;
		}
		set
		{
			isCitizenArmeniaFieldSpecified = value;
		}
	}

	public bool InsuringEmployer
	{
		get
		{
			return insuringEmployerField;
		}
		set
		{
			insuringEmployerField = value;
		}
	}

	[XmlIgnore]
	public bool InsuringEmployerSpecified
	{
		get
		{
			return insuringEmployerFieldSpecified;
		}
		set
		{
			insuringEmployerFieldSpecified = value;
		}
	}

	public SocialCostPerson SocialCostPerson
	{
		get
		{
			return socialCostPersonField;
		}
		set
		{
			socialCostPersonField = value;
		}
	}
}
