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
public class CarInfo
{
	private string makeField;

	private string plateNumberField;

	public string Make
	{
		get
		{
			return makeField;
		}
		set
		{
			makeField = value;
		}
	}

	public string PlateNumber
	{
		get
		{
			return plateNumberField;
		}
		set
		{
			plateNumberField = value;
		}
	}
}
