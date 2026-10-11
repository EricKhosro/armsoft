using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class BankAccount
{
	private string bankNameField;

	private string bankAccountNumberField;

	[XmlElement(Order = 0)]
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

	[XmlElement(Order = 1)]
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
