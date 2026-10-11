using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class RegionType
{
	private string regionCodeField;

	private string regionNameField;

	[XmlElement(Order = 0)]
	public string RegionCode
	{
		get
		{
			return regionCodeField;
		}
		set
		{
			regionCodeField = value;
		}
	}

	[XmlElement(Order = 1)]
	public string RegionName
	{
		get
		{
			return regionNameField;
		}
		set
		{
			regionNameField = value;
		}
	}
}
