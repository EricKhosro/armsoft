using System;
using ArmSoft.InternetBank.Web.API.Common.Models;

namespace ArmSoft.InternetBank.Web.API.Common;

public class CalculateMaxAmountParams
{
	public Guid DocumentId { get; set; }

	public Duration Duration { get; set; }
}
