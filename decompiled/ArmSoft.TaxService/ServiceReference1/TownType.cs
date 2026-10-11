using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class TownType
{
	private long townIdField;

	private string townNameField;

	[XmlElement(Order = 0)]
	public long TownId
	{
		get
		{
			return townIdField;
		}
		set
		{
			townIdField = value;
		}
	}

	[XmlElement(Order = 1)]
	public string TownName
	{
		get
		{
			return townNameField;
		}
		set
		{
			townNameField = value;
		}
	}
}
