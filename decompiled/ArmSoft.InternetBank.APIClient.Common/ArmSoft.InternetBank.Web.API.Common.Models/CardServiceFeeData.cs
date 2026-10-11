using System;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class CardServiceFeeData
{
	public decimal Fee { get; set; }

	public string Currency { get; set; }

	public short Period { get; set; }

	public decimal NextDebt { get; set; }

	public DateTime? NextPaymentDate { get; set; }
}
