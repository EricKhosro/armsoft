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
public class InvoiceNumber
{
	private string numberField;

	private string seriesField;

	public string Number
	{
		get
		{
			return numberField;
		}
		set
		{
			numberField = value;
		}
	}

	public string Series
	{
		get
		{
			return seriesField;
		}
		set
		{
			seriesField = value;
		}
	}
}
