using System;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class BondSchedule
{
	[JsonConverter(typeof(DateConverter))]
	public DateTime ScheduleDate { get; set; }

	public decimal Amount { get; set; }

	public decimal Interest { get; set; }

	public decimal Total { get; set; }

	public string Currency { get; set; }
}
