using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common;

public class UserResetPasswordParams
{
	public BankQuestion BankQuestion { get; set; }

	public string BankAnswer { get; set; }
}
