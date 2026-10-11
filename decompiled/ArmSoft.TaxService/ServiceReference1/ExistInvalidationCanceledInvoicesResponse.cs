using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class ExistInvalidationCanceledInvoicesResponse : GenericResponse
{
	private bool hasInvalidationCanceledInvoicesField;

	private bool hasInvalidationCanceledInvoicesFieldSpecified;

	[XmlElement(Order = 0)]
	public bool HasInvalidationCanceledInvoices
	{
		get
		{
			return hasInvalidationCanceledInvoicesField;
		}
		set
		{
			hasInvalidationCanceledInvoicesField = value;
		}
	}

	[XmlIgnore]
	public bool HasInvalidationCanceledInvoicesSpecified
	{
		get
		{
			return hasInvalidationCanceledInvoicesFieldSpecified;
		}
		set
		{
			hasInvalidationCanceledInvoicesFieldSpecified = value;
		}
	}
}
