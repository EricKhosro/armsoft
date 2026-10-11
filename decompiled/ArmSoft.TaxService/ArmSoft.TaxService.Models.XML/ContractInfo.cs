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
public class ContractInfo
{
	private DateTime contractDateField;

	private bool contractDateFieldSpecified;

	private string contractNumberField;

	[XmlElement(DataType = "date")]
	public DateTime ContractDate
	{
		get
		{
			return contractDateField;
		}
		set
		{
			contractDateField = value;
		}
	}

	[XmlIgnore]
	public bool ContractDateSpecified
	{
		get
		{
			return contractDateFieldSpecified;
		}
		set
		{
			contractDateFieldSpecified = value;
		}
	}

	public string ContractNumber
	{
		get
		{
			return contractNumberField;
		}
		set
		{
			contractNumberField = value;
		}
	}
}
