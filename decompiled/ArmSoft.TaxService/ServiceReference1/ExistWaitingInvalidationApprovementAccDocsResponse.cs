using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class ExistWaitingInvalidationApprovementAccDocsResponse : GenericResponse
{
	private bool hasWaitingInvalidationApprovalAccDocsField;

	private bool hasWaitingInvalidationApprovalAccDocsFieldSpecified;

	[XmlElement(Order = 0)]
	public bool HasWaitingInvalidationApprovalAccDocs
	{
		get
		{
			return hasWaitingInvalidationApprovalAccDocsField;
		}
		set
		{
			hasWaitingInvalidationApprovalAccDocsField = value;
		}
	}

	[XmlIgnore]
	public bool HasWaitingInvalidationApprovalAccDocsSpecified
	{
		get
		{
			return hasWaitingInvalidationApprovalAccDocsFieldSpecified;
		}
		set
		{
			hasWaitingInvalidationApprovalAccDocsFieldSpecified = value;
		}
	}
}
