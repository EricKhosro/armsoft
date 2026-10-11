using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class InvoiceNumberStatus
{
	private int statusField;

	private bool isPaperField;

	[XmlElement(Order = 0)]
	public int Status
	{
		get
		{
			return statusField;
		}
		set
		{
			statusField = value;
		}
	}

	[XmlElement(Order = 1)]
	public bool IsPaper
	{
		get
		{
			return isPaperField;
		}
		set
		{
			isPaperField = value;
		}
	}
}
