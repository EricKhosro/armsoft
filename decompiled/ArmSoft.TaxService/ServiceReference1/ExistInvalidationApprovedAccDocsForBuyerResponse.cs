using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class ExistInvalidationApprovedAccDocsForBuyerResponse : GenericResponse
{
	private bool hasInvalidationApprovedAccDocsForBuyerField;

	private bool hasInvalidationApprovedAccDocsForBuyerFieldSpecified;

	[XmlElement(Order = 0)]
	public bool HasInvalidationApprovedAccDocsForBuyer
	{
		get
		{
			return hasInvalidationApprovedAccDocsForBuyerField;
		}
		set
		{
			hasInvalidationApprovedAccDocsForBuyerField = value;
		}
	}

	[XmlIgnore]
	public bool HasInvalidationApprovedAccDocsForBuyerSpecified
	{
		get
		{
			return hasInvalidationApprovedAccDocsForBuyerFieldSpecified;
		}
		set
		{
			hasInvalidationApprovedAccDocsForBuyerFieldSpecified = value;
		}
	}
}
