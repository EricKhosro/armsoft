using System.Collections.Generic;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class AccountLatestTransactions
{
	public IEnumerable<AccountTransaction> Transactions { get; set; }

	public Period Period { get; set; }

	public AccountLatestTransactions()
	{
		Transactions = new List<AccountTransaction>();
	}
}
