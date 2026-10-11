using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class Child
{
	private byte[] docField;

	private int statusField;

	[XmlElement(DataType = "base64Binary", Order = 0)]
	public byte[] Doc
	{
		get
		{
			return docField;
		}
		set
		{
			docField = value;
		}
	}

	[XmlElement(Order = 1)]
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
}
