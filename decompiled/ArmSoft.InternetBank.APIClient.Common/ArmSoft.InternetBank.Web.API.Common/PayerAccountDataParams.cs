using System.Collections.Generic;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common;

public class PayerAccountDataParams
{
	public string Currency { get; set; }

	public List<AccountType> AccountTypes { get; set; }

	public bool IncludeAccountBalances { get; set; }

	public bool IncludeCardOnlineBalances { get; set; }

	public bool IsForCharge { get; set; }

	public string DocumentType { get; set; }

	public bool IsForPercentAccount { get; set; }

	public AccountAccessLevels AccessLevel { get; set; }

	public PayerAccountDataParams()
	{
		AccountTypes = null;
		IncludeAccountBalances = false;
		IncludeCardOnlineBalances = false;
		IsForCharge = false;
		DocumentType = null;
		IsForPercentAccount = false;
		AccessLevel = AccountAccessLevels.MakeOperation;
	}
}
