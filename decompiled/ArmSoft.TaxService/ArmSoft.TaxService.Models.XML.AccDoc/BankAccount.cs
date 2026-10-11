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
public class BankAccount
{
	private string bankNameField;

	private string bankNameRuField;

	private string bankAccountNumberField;

	private string bankAdditionalDataField;

	private string bankAdditionalDataRuField;

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

	public string BankNameRu
	{
		get
		{
			return bankNameRuField;
		}
		set
		{
			bankNameRuField = value;
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

	public string BankAdditionalData
	{
		get
		{
			return bankAdditionalDataField;
		}
		set
		{
			bankAdditionalDataField = value;
		}
	}

	public string BankAdditionalDataRu
	{
		get
		{
			return bankAdditionalDataRuField;
		}
		set
		{
			bankAdditionalDataRuField = value;
		}
	}
}
