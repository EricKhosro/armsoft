using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace ArmSoft.TaxService.Models.XML.AccDoc;

[Serializable]
[GeneratedCode("xsd", "4.8.3928.0")]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public enum PIDType
{
	RA_PASSPORT,
	BIRTH_CERT,
	RESIDENCE_CARD,
	CONVENTIONAL_TRAVEL_DOCUMENT,
	ID_CARD,
	BIOMETRIC_PASSPORT,
	FOREIGN_PASSPORT_WORK,
	FOREIGN_PASSPORT,
	FOREIGN_BIRTH_CERT
}
