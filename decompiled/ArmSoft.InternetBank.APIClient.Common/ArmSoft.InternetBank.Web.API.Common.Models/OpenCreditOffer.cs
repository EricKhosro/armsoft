using System;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class OpenCreditOffer
{
	public DateTime Date { get; set; }

	public string Code { get; set; }

	public string Name { get; set; }

	public decimal Amount { get; set; }

	public string Currency { get; set; }

	public string ApplicationType { get; set; }

	public string ApplicationTypeName { get; set; }

	public string ApplicationGroup { get; set; }

	public string ApplicationGroupName { get; set; }

	public InterestRate InterestRate { get; set; }

	public Duration Duration { get; set; }

	public DateTime EndDate { get; set; }
}
