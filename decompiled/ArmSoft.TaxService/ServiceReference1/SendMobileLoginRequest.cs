using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class SendMobileLoginRequest
{
	private string tinField;

	private string loginField;

	private string phoneField;

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
	public string Login
	{
		get
		{
			return loginField;
		}
		set
		{
			loginField = value;
		}
	}

	[XmlAttribute]
	public string Phone
	{
		get
		{
			return phoneField;
		}
		set
		{
			phoneField = value;
		}
	}
}
