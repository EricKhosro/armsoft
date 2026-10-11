using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class LoginRequest
{
	private string tinField;

	private string loginField;

	private string passwordField;

	private string clientTypeField;

	private string clientDescriptionField;

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
	public string Password
	{
		get
		{
			return passwordField;
		}
		set
		{
			passwordField = value;
		}
	}

	[XmlAttribute]
	public string ClientType
	{
		get
		{
			return clientTypeField;
		}
		set
		{
			clientTypeField = value;
		}
	}

	[XmlAttribute]
	public string ClientDescription
	{
		get
		{
			return clientDescriptionField;
		}
		set
		{
			clientDescriptionField = value;
		}
	}
}
