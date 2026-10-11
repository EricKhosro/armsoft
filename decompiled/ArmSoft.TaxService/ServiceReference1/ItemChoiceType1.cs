using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions", IncludeInSchema = false)]
public enum ItemChoiceType1
{
	Passport,
	TIN
}
