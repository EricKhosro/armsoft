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
public class AccDocGoodsInfo
{
	private Good[] goodField;

	private AccDocTotal totalField;

	[XmlElement("Good")]
	public Good[] Good
	{
		get
		{
			return goodField;
		}
		set
		{
			goodField = value;
		}
	}

	public AccDocTotal Total
	{
		get
		{
			return totalField;
		}
		set
		{
			totalField = value;
		}
	}
}
