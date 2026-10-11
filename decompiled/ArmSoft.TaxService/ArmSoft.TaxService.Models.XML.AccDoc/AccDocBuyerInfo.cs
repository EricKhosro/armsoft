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
public class AccDocBuyerInfo
{
	private string vATNumberField;

	private AccDocTaxpayerInfo taxpayerField;

	private Mediator mediatorField;

	private string deliveryMethodField;

	private string deliveryAdditionalDataField;

	private string deliveryAdditionalDataRuField;

	private EeuCountry eeuCountryField;

	private bool eeuCountryFieldSpecified;

	private string destinationLocationField;

	private string destinationLocationRuField;

	private string deliveryLocationField;

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

	public AccDocTaxpayerInfo Taxpayer
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

	public string DeliveryAdditionalData
	{
		get
		{
			return deliveryAdditionalDataField;
		}
		set
		{
			deliveryAdditionalDataField = value;
		}
	}

	public string DeliveryAdditionalDataRu
	{
		get
		{
			return deliveryAdditionalDataRuField;
		}
		set
		{
			deliveryAdditionalDataRuField = value;
		}
	}

	public EeuCountry EeuCountry
	{
		get
		{
			return eeuCountryField;
		}
		set
		{
			eeuCountryField = value;
		}
	}

	[XmlIgnore]
	public bool EeuCountrySpecified
	{
		get
		{
			return eeuCountryFieldSpecified;
		}
		set
		{
			eeuCountryFieldSpecified = value;
		}
	}

	public string DestinationLocation
	{
		get
		{
			return destinationLocationField;
		}
		set
		{
			destinationLocationField = value;
		}
	}

	public string DestinationLocationRu
	{
		get
		{
			return destinationLocationRuField;
		}
		set
		{
			destinationLocationRuField = value;
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
