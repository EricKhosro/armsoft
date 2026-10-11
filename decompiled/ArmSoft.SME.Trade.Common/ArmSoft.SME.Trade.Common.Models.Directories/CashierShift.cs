using System;
using ArmSoft.SME.Trade.Common.Models.Caches;

namespace ArmSoft.SME.Trade.Common.Models.Directories;

public class CashierShift : TradeDirectory
{
	public string CashDesk { get; set; }

	public string ShiftNumber { get; set; }

	public DateTime StartDate { get; set; }

	public DateTime? EndDate { get; set; }

	public decimal InitialRemain { get; set; }

	public object GetKey()
	{
		return new CashierShiftCacheRow.RowKey
		{
			CashDeskCode = CashDesk,
			ShiftNumber = ShiftNumber
		};
	}
}
