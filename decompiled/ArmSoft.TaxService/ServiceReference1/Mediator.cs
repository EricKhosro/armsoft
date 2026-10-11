using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class Mediator
{
	private string nameField;

	private string licenseNumberField;

	private DateTime licenseDateField;

	private bool licenseDateFieldSpecified;

	[XmlElement(Order = 0)]
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

	[XmlElement(Order = 1)]
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

	[XmlElement(DataType = "date", Order = 2)]
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
