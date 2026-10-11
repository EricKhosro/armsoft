using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class AccDocDealInfo
{
	private DateTime dealDateField;

	private bool dealDateFieldSpecified;

	private string dealNumberField;

	[XmlElement(DataType = "date", Order = 0)]
	public DateTime DealDate
	{
		get
		{
			return dealDateField;
		}
		set
		{
			dealDateField = value;
		}
	}

	[XmlIgnore]
	public bool DealDateSpecified
	{
		get
		{
			return dealDateFieldSpecified;
		}
		set
		{
			dealDateFieldSpecified = value;
		}
	}

	[XmlElement(Order = 1)]
	public string DealNumber
	{
		get
		{
			return dealNumberField;
		}
		set
		{
			dealNumberField = value;
		}
	}
}
