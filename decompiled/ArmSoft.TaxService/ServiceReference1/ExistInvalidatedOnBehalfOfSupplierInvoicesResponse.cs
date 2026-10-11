using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class ExistInvalidatedOnBehalfOfSupplierInvoicesResponse : GenericResponse
{
	private bool hasInvalidatedOnBehalfOfSupplierInvoicesField;

	private bool hasInvalidatedOnBehalfOfSupplierInvoicesFieldSpecified;

	[XmlElement(Order = 0)]
	public bool HasInvalidatedOnBehalfOfSupplierInvoices
	{
		get
		{
			return hasInvalidatedOnBehalfOfSupplierInvoicesField;
		}
		set
		{
			hasInvalidatedOnBehalfOfSupplierInvoicesField = value;
		}
	}

	[XmlIgnore]
	public bool HasInvalidatedOnBehalfOfSupplierInvoicesSpecified
	{
		get
		{
			return hasInvalidatedOnBehalfOfSupplierInvoicesFieldSpecified;
		}
		set
		{
			hasInvalidatedOnBehalfOfSupplierInvoicesFieldSpecified = value;
		}
	}
}
