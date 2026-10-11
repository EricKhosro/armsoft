using System;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class PurchasedSecurityScheduleRow
{
	public DateTime Date { get; set; }

	public decimal Amount { get; set; }

	public decimal Interest { get; set; }

	public decimal Total { get; set; }
}
