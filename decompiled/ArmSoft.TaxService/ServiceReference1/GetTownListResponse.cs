using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class GetTownListResponse : GenericResponse
{
	private TownType[] townListField;

	[XmlElement("TownList", Order = 0)]
	public TownType[] TownList
	{
		get
		{
			return townListField;
		}
		set
		{
			townListField = value;
		}
	}
}
