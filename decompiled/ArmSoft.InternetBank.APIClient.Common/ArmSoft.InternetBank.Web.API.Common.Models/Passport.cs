using System;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class Passport
{
	public string Type { get; set; }

	public string Number { get; set; }

	public string Authority { get; set; }

	public DateTime? DateOfIssue { get; set; }

	public DateTime? ExpirationDate { get; set; }
}
