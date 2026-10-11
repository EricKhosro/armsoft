using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class ExistOnBehalfOfSupplierInvoicesResponse : GenericResponse
{
	private bool hasOnBehalfOfSupplierInvoicesField;

	private bool hasOnBehalfOfSupplierInvoicesFieldSpecified;

	[XmlElement(Order = 0)]
	public bool HasOnBehalfOfSupplierInvoices
	{
		get
		{
			return hasOnBehalfOfSupplierInvoicesField;
		}
		set
		{
			hasOnBehalfOfSupplierInvoicesField = value;
		}
	}

	[XmlIgnore]
	public bool HasOnBehalfOfSupplierInvoicesSpecified
	{
		get
		{
			return hasOnBehalfOfSupplierInvoicesFieldSpecified;
		}
		set
		{
			hasOnBehalfOfSupplierInvoicesFieldSpecified = value;
		}
	}
}
