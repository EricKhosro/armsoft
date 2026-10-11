using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ArmSoft.TaxService.Models.XML.AccDoc;

[Serializable]
[GeneratedCode("xsd", "4.8.3928.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class AccDocOnBehalfOfSupplierInfo
{
	private AccDocTaxpayerInfo taxpayerField;

	public AccDocTaxpayerInfo Taxpayer
	{
		get
		{
			return taxpayerField;
		}
		set
		{
			taxpayerField = value;
		}
	}
}
