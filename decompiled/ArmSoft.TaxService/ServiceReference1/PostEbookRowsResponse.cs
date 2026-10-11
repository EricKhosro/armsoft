using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class PostEbookRowsResponse : GenericResponse
{
	private string[] errorsListField;

	private int[] successIndexesListField;

	[XmlElement("ErrorsList", Order = 0)]
	public string[] ErrorsList
	{
		get
		{
			return errorsListField;
		}
		set
		{
			errorsListField = value;
		}
	}

	[XmlElement("SuccessIndexesList", Order = 1)]
	public int[] SuccessIndexesList
	{
		get
		{
			return successIndexesListField;
		}
		set
		{
			successIndexesListField = value;
		}
	}
}
