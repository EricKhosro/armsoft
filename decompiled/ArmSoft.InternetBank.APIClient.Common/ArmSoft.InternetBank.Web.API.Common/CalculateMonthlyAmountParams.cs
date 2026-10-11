using System;
using ArmSoft.InternetBank.Web.API.Common.Models;

namespace ArmSoft.InternetBank.Web.API.Common;

public class CalculateMonthlyAmountParams
{
	public Guid DocumentId { get; set; }

	public decimal Amount { get; set; }

	public Duration Duration { get; set; }
}
