using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class VatPayerInfo
{
	private int yearField;

	private DateTime sinceDateField;

	[XmlElement(Order = 0)]
	public int year
	{
		get
		{
			return yearField;
		}
		set
		{
			yearField = value;
		}
	}

	[XmlElement(DataType = "date", Order = 1)]
	public DateTime sinceDate
	{
		get
		{
			return sinceDateField;
		}
		set
		{
			sinceDateField = value;
		}
	}
}
