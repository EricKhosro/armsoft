using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class AccDocTaxpayerInfo
{
	private string itemField;

	private ItemChoiceType1 itemElementNameField;

	private string nameField;

	private string addressField;

	private BankAccount bankAccountField;

	private string additionalDataField;

	private string organizationalTypeField;

	private string credentialsNumberField;

	private bool tinNotRequiredField;

	private bool tinNotRequiredFieldSpecified;

	private bool principalTinNotRequiredField;

	private bool principalTinNotRequiredFieldSpecified;

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
	public ItemChoiceType1 ItemElementName
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

	[XmlElement(Order = 4)]
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

	[XmlElement(Order = 5)]
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

	[XmlElement(Order = 6)]
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

	[XmlElement(Order = 7)]
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

	[XmlElement(Order = 8)]
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

	[XmlElement(Order = 9)]
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
}
