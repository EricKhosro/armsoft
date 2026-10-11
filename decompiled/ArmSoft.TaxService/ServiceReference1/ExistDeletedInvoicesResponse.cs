using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class ExistDeletedInvoicesResponse : GenericResponse
{
	private bool hasDeletedInvoicesField;

	private bool hasDeletedInvoicesFieldSpecified;

	[XmlElement(Order = 0)]
	public bool HasDeletedInvoices
	{
		get
		{
			return hasDeletedInvoicesField;
		}
		set
		{
			hasDeletedInvoicesField = value;
		}
	}

	[XmlIgnore]
	public bool HasDeletedInvoicesSpecified
	{
		get
		{
			return hasDeletedInvoicesFieldSpecified;
		}
		set
		{
			hasDeletedInvoicesFieldSpecified = value;
		}
	}
}
