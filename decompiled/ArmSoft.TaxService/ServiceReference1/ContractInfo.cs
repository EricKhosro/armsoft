using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class ContractInfo
{
	private DateTime contractDateField;

	private bool contractDateFieldSpecified;

	private string contractNumberField;

	[XmlElement(DataType = "date", Order = 0)]
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

	[XmlElement(Order = 1)]
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
