using System;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class CreditSchedule
{
	[JsonConverter(typeof(DateConverter))]
	public DateTime ScheduleDate { get; set; }

	public decimal Amount { get; set; }

	public decimal Interest { get; set; }

	public decimal Grant { get; set; }

	public decimal Fee { get; set; }

	public decimal Total { get; set; }

	public string Currency { get; set; }

	public string FeeCurrency { get; set; }
}
