using System;

namespace ArmSoft.SME.Trade.Client.Models;

public class CashierShiftCache
{
	public string CashDesk { get; set; }

	public string ShiftNumber { get; set; }

	public DateTime StartDate { get; set; }

	public DateTime? EndDate { get; set; }

	public decimal InitialRemain { get; set; }
}
