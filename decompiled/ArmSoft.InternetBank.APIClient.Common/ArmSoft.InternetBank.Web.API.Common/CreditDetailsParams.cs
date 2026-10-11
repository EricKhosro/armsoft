using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common;

public class CreditDetailsParams
{
	public string ContractNumber { get; set; }

	public RepaymentType RepaymentType { get; set; }

	public decimal Amount { get; set; }

	public bool IsFullRepayment { get; set; }

	public bool IncludePrematureRepaymentFee { get; set; }

	public CreditDetailsParams()
	{
		IncludePrematureRepaymentFee = true;
	}
}
