using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ArmSoft.TaxService.Models.XML;

[Serializable]
[GeneratedCode("xsd", "4.8.3928.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
[XmlRoot(Namespace = "http://www.taxservice.am/tp3/invoice/definitions", IsNullable = false)]
public class Data
{
	private SignableData signableDataField;

	public SignableData SignableData
	{
		get
		{
			return signableDataField;
		}
		set
		{
			signableDataField = value;
		}
	}
}
