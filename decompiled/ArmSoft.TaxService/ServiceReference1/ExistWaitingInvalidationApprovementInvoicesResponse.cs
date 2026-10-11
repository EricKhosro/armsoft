using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class ExistWaitingInvalidationApprovementInvoicesResponse : GenericResponse
{
	private bool hasWaitingInvalidationApprovalInvoicesField;

	private bool hasWaitingInvalidationApprovalInvoicesFieldSpecified;

	[XmlElement(Order = 0)]
	public bool HasWaitingInvalidationApprovalInvoices
	{
		get
		{
			return hasWaitingInvalidationApprovalInvoicesField;
		}
		set
		{
			hasWaitingInvalidationApprovalInvoicesField = value;
		}
	}

	[XmlIgnore]
	public bool HasWaitingInvalidationApprovalInvoicesSpecified
	{
		get
		{
			return hasWaitingInvalidationApprovalInvoicesFieldSpecified;
		}
		set
		{
			hasWaitingInvalidationApprovalInvoicesFieldSpecified = value;
		}
	}
}
