using System;
using System.ComponentModel;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OpenBanking.Api.Common;

public class AccountListItemParams
{
	public AccountAccessLevels DesiredAccess { get; set; } = AccountAccessLevels.View;

	public bool IncludeBalances { get; set; }

	public bool IncludeCardAccounts { get; set; }

	public bool OnlyCardAccounts { get; set; }

	public bool OnlyActive { get; set; } = true;

	[DefaultValue(null)]
	public string? Currency { get; set; }

	[DefaultValue(null)]
	public DateTime? BalanceDate { get; set; }
}
