namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class PensionStatement
{
	public string SocialCard { get; set; }

	public string FirstName { get; set; }

	public string LastName { get; set; }

	public string BirthDate { get; set; }

	public decimal Balance { get; set; }

	public string Currency { get; set; }
}
