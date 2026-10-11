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
public class Mediator
{
	private string nameField;

	private string nameRuField;

	private string licenseNumberField;

	private DateTime licenseDateField;

	private bool licenseDateFieldSpecified;

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

	public string LicenseNumber
	{
		get
		{
			return licenseNumberField;
		}
		set
		{
			licenseNumberField = value;
		}
	}

	[XmlElement(DataType = "date")]
	public DateTime LicenseDate
	{
		get
		{
			return licenseDateField;
		}
		set
		{
			licenseDateField = value;
		}
	}

	[XmlIgnore]
	public bool LicenseDateSpecified
	{
		get
		{
			return licenseDateFieldSpecified;
		}
		set
		{
			licenseDateFieldSpecified = value;
		}
	}
}
