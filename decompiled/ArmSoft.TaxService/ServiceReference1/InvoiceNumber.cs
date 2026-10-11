using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class InvoiceNumber
{
	private string numberField;

	private string seriesField;

	[XmlElement(Order = 0)]
	public string Number
	{
		get
		{
			return numberField;
		}
		set
		{
			numberField = value;
		}
	}

	[XmlElement(Order = 1)]
	public string Series
	{
		get
		{
			return seriesField;
		}
		set
		{
			seriesField = value;
		}
	}
}
