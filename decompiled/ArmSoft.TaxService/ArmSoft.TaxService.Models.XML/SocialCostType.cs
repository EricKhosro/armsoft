using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace ArmSoft.TaxService.Models.XML;

[Serializable]
[GeneratedCode("xsd", "4.8.3928.0")]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public enum SocialCostType
{
	HEALTH_INSURANCE,
	EDUCATIONAL_COST,
	DENTAL_SERVICES,
	OTHER
}
