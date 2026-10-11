using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class ExistOnBehalfOfSupplierAccDocsResponse : GenericResponse
{
	private bool hasOnBehalfOfSupplierAccDocsField;

	private bool hasOnBehalfOfSupplierAccDocsFieldSpecified;

	[XmlElement(Order = 0)]
	public bool HasOnBehalfOfSupplierAccDocs
	{
		get
		{
			return hasOnBehalfOfSupplierAccDocsField;
		}
		set
		{
			hasOnBehalfOfSupplierAccDocsField = value;
		}
	}

	[XmlIgnore]
	public bool HasOnBehalfOfSupplierAccDocsSpecified
	{
		get
		{
			return hasOnBehalfOfSupplierAccDocsFieldSpecified;
		}
		set
		{
			hasOnBehalfOfSupplierAccDocsFieldSpecified = value;
		}
	}
}
