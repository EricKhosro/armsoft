using System.Collections.Generic;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OpenBanking.Api.Common;

public class AccountLatestTransactions
{
	public IEnumerable<AccountTransaction> Transactions { get; set; } = new List<AccountTransaction>();

	public Period Period { get; set; }
}
