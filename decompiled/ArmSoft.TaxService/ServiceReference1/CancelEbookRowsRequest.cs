using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class CancelEbookRowsRequest : GenericAuthenticatedRequest
{
	private string bookNumberField;

	private int[] rowNumberField;

	[XmlElement(Order = 0)]
	public string BookNumber
	{
		get
		{
			return bookNumberField;
		}
		set
		{
			bookNumberField = value;
		}
	}

	[XmlElement("RowNumber", Order = 1)]
	public int[] RowNumber
	{
		get
		{
			return rowNumberField;
		}
		set
		{
			rowNumberField = value;
		}
	}
}
