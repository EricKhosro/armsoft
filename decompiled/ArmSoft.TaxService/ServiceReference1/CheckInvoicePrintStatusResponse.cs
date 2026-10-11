using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class CheckInvoicePrintStatusResponse : GenericResponse
{
	private bool validForPrintField;

	[XmlElement(Order = 0)]
	public bool ValidForPrint
	{
		get
		{
			return validForPrintField;
		}
		set
		{
			validForPrintField = value;
		}
	}
}
