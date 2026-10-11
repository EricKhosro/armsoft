namespace ArmSoft.InternetBank.Web.API.Common;

public class CashWithdrawalCommissionParams
{
	public string PayerAccount { get; set; }

	public decimal Amount { get; set; }

	public string Currency { get; set; }

	public string BranchCode { get; set; }
}
