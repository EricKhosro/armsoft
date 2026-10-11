namespace ArmSoft.SME.Trade.Common.Models.ASReport;

public class CashierShiftReportParam
{
	public string CashDeskCode { get; set; }

	public string ShiftNumber { get; set; }

	public CashierShiftReportParam()
	{
	}

	public CashierShiftReportParam(string cashDeskCode, string shiftNumber)
	{
		CashDeskCode = cashDeskCode;
		ShiftNumber = shiftNumber;
	}
}
