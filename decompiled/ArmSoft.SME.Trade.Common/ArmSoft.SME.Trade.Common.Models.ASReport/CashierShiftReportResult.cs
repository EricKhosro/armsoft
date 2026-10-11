namespace ArmSoft.SME.Trade.Common.Models.ASReport;

public class CashierShiftReportResult
{
	public decimal SalesTotalSum { get; set; }

	public decimal SalesCashSum { get; set; }

	public decimal SalesPlasticCardSum { get; set; }

	public decimal SalesWithPrepaymentSum { get; set; }

	public decimal SalesPayedByBonusSum { get; set; }

	public decimal SalesCreditSum { get; set; }

	public decimal ReturnsTotalSum { get; set; }

	public decimal ReturnsCashSum { get; set; }

	public decimal ReturnsPlasticCardSum { get; set; }

	public decimal ReturnWithPrepaymentSum { get; set; }

	public decimal ReturnPayedByBonusSum { get; set; }

	public decimal CashInTotalSum { get; set; }

	public decimal CashInCashSumm { get; set; }

	public decimal CashInByPlastic { get; set; }

	public decimal CashOutTotalSum { get; set; }

	public decimal CashOutCashSumm { get; set; }

	public decimal AccumulatedBonus { get; set; }

	public decimal AddedBonus { get; set; }

	public decimal DiscountSum { get; set; }

	public decimal Postponed { get; set; }

	public decimal Calcelled { get; set; }

	public decimal AverageSumOfBills { get; set; }

	public decimal AverageSumOfRows { get; set; }

	public decimal AverageRowQty { get; set; }

	public int CountOfBills { get; set; }

	public int CountOfAllBillRows { get; set; }

	public decimal OtherCashin { get; set; }

	public decimal OtherCashOut { get; set; }

	public decimal InitialRemain { get; set; }

	public decimal EndRemain { get; set; }
}
