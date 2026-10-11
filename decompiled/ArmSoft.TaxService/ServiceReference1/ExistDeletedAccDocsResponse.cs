using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class ExistDeletedAccDocsResponse : GenericResponse
{
	private bool hasDeletedAccDocsField;

	private bool hasDeletedAccDocsFieldSpecified;

	[XmlElement(Order = 0)]
	public bool HasDeletedAccDocs
	{
		get
		{
			return hasDeletedAccDocsField;
		}
		set
		{
			hasDeletedAccDocsField = value;
		}
	}

	[XmlIgnore]
	public bool HasDeletedAccDocsSpecified
	{
		get
		{
			return hasDeletedAccDocsFieldSpecified;
		}
		set
		{
			hasDeletedAccDocsFieldSpecified = value;
		}
	}
}
