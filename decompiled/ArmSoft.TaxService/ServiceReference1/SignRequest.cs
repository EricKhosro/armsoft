using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class SignRequest : GenericAuthenticatedRequest
{
	private string usernameField;

	private string passwordField;

	private byte[] dataField;

	[XmlElement(Order = 0)]
	public string Username
	{
		get
		{
			return usernameField;
		}
		set
		{
			usernameField = value;
		}
	}

	[XmlElement(Order = 1)]
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

	[XmlElement(DataType = "base64Binary", Order = 2)]
	public byte[] Data
	{
		get
		{
			return dataField;
		}
		set
		{
			dataField = value;
		}
	}
}
