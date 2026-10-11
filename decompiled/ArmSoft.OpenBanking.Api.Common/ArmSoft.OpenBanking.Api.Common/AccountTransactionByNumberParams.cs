using System;
using System.ComponentModel;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OpenBanking.Api.Common;

public class AccountTransactionByNumberParams
{
	[DefaultValue("")]
	public required string AccountNumber { get; set; }

	public DateTime DateFrom { get; set; }

	public DateTime DateTo { get; set; }

	public bool ShowAMD { get; set; }

	[DefaultValue(null)]
	public int? Count { get; set; }

	public TransactionFilterOption FilterOption { get; set; }
}
