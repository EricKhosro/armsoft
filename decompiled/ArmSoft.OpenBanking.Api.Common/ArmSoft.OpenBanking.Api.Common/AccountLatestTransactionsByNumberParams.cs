using System.ComponentModel;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OpenBanking.Api.Common;

public class AccountLatestTransactionsByNumberParams
{
	[DefaultValue("")]
	public required string AccountNumber { get; set; }

	public bool ShowAMD { get; set; }

	public int Count { get; set; }

	public TransactionFilterOption FilterOption { get; set; }
}
