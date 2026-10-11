using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class StopPendingRequest
{
	private string tinField;

	private string transactionIdField;

	[XmlAttribute]
	public string Tin
	{
		get
		{
			return tinField;
		}
		set
		{
			tinField = value;
		}
	}

	[XmlAttribute]
	public string TransactionId
	{
		get
		{
			return transactionIdField;
		}
		set
		{
			transactionIdField = value;
		}
	}
}
