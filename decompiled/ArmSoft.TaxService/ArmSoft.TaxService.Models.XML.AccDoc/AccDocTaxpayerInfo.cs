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
public class AccDocTaxpayerInfo
{
	private string itemField;

	private string passportField;

	private PIDType pIDTypeField;

	private bool pIDTypeFieldSpecified;

	private string nameField;

	private string nameRuField;

	private string fullNameField;

	private string fullNameRuField;

	private string addressField;

	private string addressRuField;

	private BankAccount bankAccountField;

	private string additionalDataField;

	private string additionalDataRuField;

	private string organizationalTypeField;

	private AccDocContractInfo accDocContractInfoField;

	private string credentialsNumberField;

	private bool tinNotRequiredField;

	private bool tinNotRequiredFieldSpecified;

	private bool principalTinNotRequiredField;

	private bool principalTinNotRequiredFieldSpecified;

	private bool isNaturalField;

	private bool isNaturalFieldSpecified;

	private string pSNOrAbsenceField;

	[XmlElement("TIN")]
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

	public string Passport
	{
		get
		{
			return passportField;
		}
		set
		{
			passportField = value;
		}
	}

	public PIDType PIDType
	{
		get
		{
			return pIDTypeField;
		}
		set
		{
			pIDTypeField = value;
		}
	}

	[XmlIgnore]
	public bool PIDTypeSpecified
	{
		get
		{
			return pIDTypeFieldSpecified;
		}
		set
		{
			pIDTypeFieldSpecified = value;
		}
	}

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

	public string NameRu
	{
		get
		{
			return nameRuField;
		}
		set
		{
			nameRuField = value;
		}
	}

	public string FullName
	{
		get
		{
			return fullNameField;
		}
		set
		{
			fullNameField = value;
		}
	}

	public string FullNameRu
	{
		get
		{
			return fullNameRuField;
		}
		set
		{
			fullNameRuField = value;
		}
	}

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

	public string AddressRu
	{
		get
		{
			return addressRuField;
		}
		set
		{
			addressRuField = value;
		}
	}

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

	public string AdditionalDataRu
	{
		get
		{
			return additionalDataRuField;
		}
		set
		{
			additionalDataRuField = value;
		}
	}

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

	public AccDocContractInfo AccDocContractInfo
	{
		get
		{
			return accDocContractInfoField;
		}
		set
		{
			accDocContractInfoField = value;
		}
	}

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

	public bool IsNatural
	{
		get
		{
			return isNaturalField;
		}
		set
		{
			isNaturalField = value;
		}
	}

	[XmlIgnore]
	public bool IsNaturalSpecified
	{
		get
		{
			return isNaturalFieldSpecified;
		}
		set
		{
			isNaturalFieldSpecified = value;
		}
	}

	public string PSNOrAbsence
	{
		get
		{
			return pSNOrAbsenceField;
		}
		set
		{
			pSNOrAbsenceField = value;
		}
	}
}
