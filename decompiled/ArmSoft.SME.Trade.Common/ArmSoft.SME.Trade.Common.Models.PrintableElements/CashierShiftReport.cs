using ArmSoft.SME.Common.Models.PrintableElements;
using ArmSoft.SME.Trade.Common.Models.Caches;

namespace ArmSoft.SME.Trade.Common.Models.PrintableElements;

public class CashierShiftReport : PrintableElement
{
	public string Caption { get; set; }

	public CashierShiftCacheRow CashierShiftRow { get; set; }
}
