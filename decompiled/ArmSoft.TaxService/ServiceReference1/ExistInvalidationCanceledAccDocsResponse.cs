using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class ExistInvalidationCanceledAccDocsResponse : GenericResponse
{
	private bool hasInvalidationCanceledAccDocsField;

	private bool hasInvalidationCanceledAccDocsFieldSpecified;

	[XmlElement(Order = 0)]
	public bool HasInvalidationCanceledAccDocs
	{
		get
		{
			return hasInvalidationCanceledAccDocsField;
		}
		set
		{
			hasInvalidationCanceledAccDocsField = value;
		}
	}

	[XmlIgnore]
	public bool HasInvalidationCanceledAccDocsSpecified
	{
		get
		{
			return hasInvalidationCanceledAccDocsFieldSpecified;
		}
		set
		{
			hasInvalidationCanceledAccDocsFieldSpecified = value;
		}
	}
}
