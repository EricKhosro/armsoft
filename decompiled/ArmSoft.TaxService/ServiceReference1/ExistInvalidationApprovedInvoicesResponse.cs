using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class ExistInvalidationApprovedInvoicesResponse : GenericResponse
{
	private bool hasInvalidationApprovedInvoicesField;

	private bool hasInvalidationApprovedInvoicesFieldSpecified;

	[XmlElement(Order = 0)]
	public bool HasInvalidationApprovedInvoices
	{
		get
		{
			return hasInvalidationApprovedInvoicesField;
		}
		set
		{
			hasInvalidationApprovedInvoicesField = value;
		}
	}

	[XmlIgnore]
	public bool HasInvalidationApprovedInvoicesSpecified
	{
		get
		{
			return hasInvalidationApprovedInvoicesFieldSpecified;
		}
		set
		{
			hasInvalidationApprovedInvoicesFieldSpecified = value;
		}
	}
}
