using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class CR : AccountPayment
{
	public string ContractNo { get; set; }

	public RepaymentType RepaymentType { get; set; }

	public bool IsFullRepayment { get; set; }

	public decimal OverpaymentPrincipal { get; set; }

	public decimal OverpaymentFee { get; set; }
}
