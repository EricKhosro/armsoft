using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class ExistInvalidationApprovedInvoicesForBuyerResponse : GenericResponse
{
	private bool hasInvalidationApprovedInvoicesForBuyerField;

	private bool hasInvalidationApprovedInvoicesForBuyerFieldSpecified;

	[XmlElement(Order = 0)]
	public bool HasInvalidationApprovedInvoicesForBuyer
	{
		get
		{
			return hasInvalidationApprovedInvoicesForBuyerField;
		}
		set
		{
			hasInvalidationApprovedInvoicesForBuyerField = value;
		}
	}

	[XmlIgnore]
	public bool HasInvalidationApprovedInvoicesForBuyerSpecified
	{
		get
		{
			return hasInvalidationApprovedInvoicesForBuyerFieldSpecified;
		}
		set
		{
			hasInvalidationApprovedInvoicesForBuyerFieldSpecified = value;
		}
	}
}
