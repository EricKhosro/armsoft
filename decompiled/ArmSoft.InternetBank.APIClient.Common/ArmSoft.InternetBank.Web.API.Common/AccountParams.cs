using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common;

public class AccountParams
{
	public AccountAccessLevels DesiredAccess { get; set; }

	public string Currency { get; set; }

	public bool IncludeLinks { get; set; }

	public bool IncludeBalances { get; set; }

	public bool IncludeCardAccounts { get; set; }

	public int? SubUserId { get; set; }

	public AccountParams()
	{
		DesiredAccess = AccountAccessLevels.View;
		IncludeLinks = true;
	}
}
