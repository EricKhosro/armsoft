using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class TaxpayerInfo
{
	private string itemField;

	private ItemChoiceType itemElementNameField;

	private string nameField;

	private string supplierNameSurnameField;

	private string countryField;

	private string addressField;

	private BankAccount bankAccountField;

	private string additionalDataField;

	private string organizationalTypeField;

	private string credentialsNumberField;

	private ContractInfo contractInfoField;

	private string distNameField;

	private string statusField;

	private bool tinNotRequiredField;

	private bool tinNotRequiredFieldSpecified;

	private bool principalTinNotRequiredField;

	private bool principalTinNotRequiredFieldSpecified;

	private string tpTypeField;

	private string factualAddressField;

	[XmlElement("Passport", typeof(string), Order = 0)]
	[XmlElement("TIN", typeof(string), Order = 0)]
	[XmlChoiceIdentifier("ItemElementName")]
	public string Item
	{
		get
		{
			return itemField;
		}
		set
		{
			itemField = value;
		}
	}

	[XmlElement(Order = 1)]
	[XmlIgnore]
	public ItemChoiceType ItemElementName
	{
		get
		{
			return itemElementNameField;
		}
		set
		{
			itemElementNameField = value;
		}
	}

	[XmlElement(Order = 2)]
	public string Name
	{
		get
		{
			return nameField;
		}
		set
		{
			nameField = value;
		}
	}

	[XmlElement(Order = 3)]
	public string SupplierNameSurname
	{
		get
		{
			return supplierNameSurnameField;
		}
		set
		{
			supplierNameSurnameField = value;
		}
	}

	[XmlElement(Order = 4)]
	public string Country
	{
		get
		{
			return countryField;
		}
		set
		{
			countryField = value;
		}
	}

	[XmlElement(Order = 5)]
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

	[XmlElement(Order = 6)]
	public BankAccount BankAccount
	{
		get
		{
			return bankAccountField;
		}
		set
		{
			bankAccountField = value;
		}
	}

	[XmlElement(Order = 7)]
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

	[XmlElement(Order = 8)]
	public string OrganizationalType
	{
		get
		{
			return organizationalTypeField;
		}
		set
		{
			organizationalTypeField = value;
		}
	}

	[XmlElement(Order = 9)]
	public string CredentialsNumber
	{
		get
		{
			return credentialsNumberField;
		}
		set
		{
			credentialsNumberField = value;
		}
	}

	[XmlElement(Order = 10)]
	public ContractInfo ContractInfo
	{
		get
		{
			return contractInfoField;
		}
		set
		{
			contractInfoField = value;
		}
	}

	[XmlElement(Order = 11)]
	public string DistName
	{
		get
		{
			return distNameField;
		}
		set
		{
			distNameField = value;
		}
	}

	[XmlElement(Order = 12)]
	public string Status
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

	[XmlElement(Order = 13)]
	public bool TinNotRequired
	{
		get
		{
			return tinNotRequiredField;
		}
		set
		{
			tinNotRequiredField = value;
		}
	}

	[XmlIgnore]
	public bool TinNotRequiredSpecified
	{
		get
		{
			return tinNotRequiredFieldSpecified;
		}
		set
		{
			tinNotRequiredFieldSpecified = value;
		}
	}

	[XmlElement(Order = 14)]
	public bool PrincipalTinNotRequired
	{
		get
		{
			return principalTinNotRequiredField;
		}
		set
		{
			principalTinNotRequiredField = value;
		}
	}

	[XmlIgnore]
	public bool PrincipalTinNotRequiredSpecified
	{
		get
		{
			return principalTinNotRequiredFieldSpecified;
		}
		set
		{
			principalTinNotRequiredFieldSpecified = value;
		}
	}

	[XmlElement(Order = 15)]
	public string TpType
	{
		get
		{
			return tpTypeField;
		}
		set
		{
			tpTypeField = value;
		}
	}

	[XmlElement(Order = 16)]
	public string FactualAddress
	{
		get
		{
			return factualAddressField;
		}
		set
		{
			factualAddressField = value;
		}
	}
}
