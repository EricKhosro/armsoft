using System.Xml.Serialization;

namespace ArmSoft.TaxService.Models.Authentication;

[XmlRoot("LoginWebRequest", Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class LoginWebRequest
{
	[XmlAttribute]
	public string Tin { get; set; }

	[XmlAttribute]
	public string Login { get; set; }

	[XmlAttribute]
	public string Password { get; set; }

	[XmlAttribute]
	public string ClientType { get; set; }

	[XmlAttribute]
	public string ClientDescription { get; set; }
}
