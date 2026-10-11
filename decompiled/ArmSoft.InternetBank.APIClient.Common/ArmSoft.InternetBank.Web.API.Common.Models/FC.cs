using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class FC : AccountPayment
{
	public string ContractNo { get; set; }

	public string ContractDetails { get; set; }

	public string PassportNo { get; set; }

	public int ContractStatus { get; set; }

	public bool IsPassportNumberRequiredToGetContract { get; set; }

	public bool CanGetContract { get; set; }

	public FriendCreditSearchType FriendCreditSearchType { get; set; }
}
