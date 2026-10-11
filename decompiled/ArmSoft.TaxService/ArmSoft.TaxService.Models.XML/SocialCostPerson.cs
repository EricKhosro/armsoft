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
public class SocialCostPerson
{
	private SocialCostType costTypeField;

	private bool costTypeFieldSpecified;

	private string pSNOrAbsenceField;

	private string fullNameField;

	private IdentityType identityField;

	private bool identityFieldSpecified;

	private string serialNumberField;

	public SocialCostType CostType
	{
		get
		{
			return costTypeField;
		}
		set
		{
			costTypeField = value;
		}
	}

	[XmlIgnore]
	public bool CostTypeSpecified
	{
		get
		{
			return costTypeFieldSpecified;
		}
		set
		{
			costTypeFieldSpecified = value;
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

	public IdentityType Identity
	{
		get
		{
			return identityField;
		}
		set
		{
			identityField = value;
		}
	}

	[XmlIgnore]
	public bool IdentitySpecified
	{
		get
		{
			return identityFieldSpecified;
		}
		set
		{
			identityFieldSpecified = value;
		}
	}

	public string SerialNumber
	{
		get
		{
			return serialNumberField;
		}
		set
		{
			serialNumberField = value;
		}
	}
}
