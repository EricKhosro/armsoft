using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace ArmSoft.TaxService.Models.XML.AccDoc;

[Serializable]
[GeneratedCode("xsd", "4.8.3928.0")]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public enum EeuCountry
{
	AM,
	BY,
	KZ,
	KG,
	RU
}
