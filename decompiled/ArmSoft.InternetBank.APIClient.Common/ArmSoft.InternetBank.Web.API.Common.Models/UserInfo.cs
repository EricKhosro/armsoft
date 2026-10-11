using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class UserInfo
{
	public string UserName { get; set; }

	public string FirstName { get; set; }

	public string LastName { get; set; }

	public string BirthDate { get; set; }

	public string Passport { get; set; }

	public string Mobile { get; set; }

	public string Email { get; set; }

	public BankQuestion BankQuestion { get; set; }

	public string BankAnswer { get; set; }

	public bool AllowUserVerificationByEmail { get; set; }

	public bool AllowUserVerificationBySMS { get; set; }

	public string SecurityProvider { get; set; }

	public bool? IsPassportUpdateAutomatically { get; set; }

	public string BankPassword { get; set; }

	public bool HasPhoto { get; set; }

	public byte[] UserPhoto { get; set; }

	public bool DeletePhoto { get; set; }

	public UserInfo()
	{
		IsPassportUpdateAutomatically = null;
	}
}
