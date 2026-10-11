using System;

namespace ArmSoft.InternetBank.Web.API.Common;

public class AddSubuserParams
{
	public string Username { get; set; }

	public string Password { get; set; }

	public string FirstName { get; set; }

	public string LastName { get; set; }

	public DateTime BirthDate { get; set; }

	public string Passport { get; set; }

	public string SocialCard { get; set; }

	public string Mobile { get; set; }

	public string Email { get; set; }

	public bool IgnoreWarnings { get; set; }
}
