using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class GetTaxFreeCountryListResponse : GenericResponse
{
	private CountryType[] taxFreeCountryField;

	[XmlElement("TaxFreeCountry", Order = 0)]
	public CountryType[] TaxFreeCountry
	{
		get
		{
			return taxFreeCountryField;
		}
		set
		{
			taxFreeCountryField = value;
		}
	}
}
