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
public class BankAccount
{
	private string bankNameField;

	private string bankAccountNumberField;

	public string BankName
	{
		get
		{
			return bankNameField;
		}
		set
		{
			bankNameField = value;
		}
	}

	public string BankAccountNumber
	{
		get
		{
			return bankAccountNumberField;
		}
		set
		{
			bankAccountNumberField = value;
		}
	}
}
