using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common;

public class CreditRepaymentDocumentInfoParams
{
	public string DocumentType { get; set; }

	public string ContractNo { get; set; }

	public RepaymentType RepaymentType { get; set; }
}
