using System;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class AccountBalanceByDate
{
	public DateTime Date { get; set; }

	public decimal Balance { get; set; }
}
