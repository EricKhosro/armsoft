using System.Collections.Generic;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class OpenCreditType
{
	public string Code { get; set; }

	public string Name { get; set; }

	public Duration MinDuration { get; set; }

	public Duration MaxDuration { get; set; }

	public InterestRate MinRate { get; set; }

	public InterestRate MaxRate { get; set; }

	public List<CreditTemplate> Templates { get; set; }
}
