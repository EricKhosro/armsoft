using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class ExistInvalidationApprovedAccDocsResponse : GenericResponse
{
	private bool hasInvalidationApprovedAccDocsField;

	private bool hasInvalidationApprovedAccDocsFieldSpecified;

	[XmlElement(Order = 0)]
	public bool HasInvalidationApprovedAccDocs
	{
		get
		{
			return hasInvalidationApprovedAccDocsField;
		}
		set
		{
			hasInvalidationApprovedAccDocsField = value;
		}
	}

	[XmlIgnore]
	public bool HasInvalidationApprovedAccDocsSpecified
	{
		get
		{
			return hasInvalidationApprovedAccDocsFieldSpecified;
		}
		set
		{
			hasInvalidationApprovedAccDocsFieldSpecified = value;
		}
	}
}
