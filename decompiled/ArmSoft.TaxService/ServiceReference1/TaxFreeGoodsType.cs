using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class TaxFreeGoodsType
{
	private string codeField;

	private string nameField;

	private int notRefundableField;

	private bool notRefundableFieldSpecified;

	[XmlElement(Order = 0)]
	public string Code
	{
		get
		{
			return codeField;
		}
		set
		{
			codeField = value;
		}
	}

	[XmlElement(Order = 1)]
	public string Name
	{
		get
		{
			return nameField;
		}
		set
		{
			nameField = value;
		}
	}

	[XmlElement(Order = 2)]
	public int NotRefundable
	{
		get
		{
			return notRefundableField;
		}
		set
		{
			notRefundableField = value;
		}
	}

	[XmlIgnore]
	public bool NotRefundableSpecified
	{
		get
		{
			return notRefundableFieldSpecified;
		}
		set
		{
			notRefundableFieldSpecified = value;
		}
	}
}
