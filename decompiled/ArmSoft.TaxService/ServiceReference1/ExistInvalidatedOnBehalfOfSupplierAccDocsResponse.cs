using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class ExistInvalidatedOnBehalfOfSupplierAccDocsResponse : GenericResponse
{
	private bool hasInvalidatedOnBehalfOfSupplierAccDocsField;

	private bool hasInvalidatedOnBehalfOfSupplierAccDocsFieldSpecified;

	[XmlElement(Order = 0)]
	public bool HasInvalidatedOnBehalfOfSupplierAccDocs
	{
		get
		{
			return hasInvalidatedOnBehalfOfSupplierAccDocsField;
		}
		set
		{
			hasInvalidatedOnBehalfOfSupplierAccDocsField = value;
		}
	}

	[XmlIgnore]
	public bool HasInvalidatedOnBehalfOfSupplierAccDocsSpecified
	{
		get
		{
			return hasInvalidatedOnBehalfOfSupplierAccDocsFieldSpecified;
		}
		set
		{
			hasInvalidatedOnBehalfOfSupplierAccDocsFieldSpecified = value;
		}
	}
}
