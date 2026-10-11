using System;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common;

public class TransactionsParams
{
	public Period? Period { get; set; }

	public DateTime? DateFrom { get; set; }

	public DateTime? DateTo { get; set; }

	public bool ShowAMD { get; set; }

	public int? Count { get; set; }

	public TransactionFilterOption FilterOption { get; set; }

	public TransactionsParams()
	{
		ShowAMD = false;
		FilterOption = TransactionFilterOption.All;
	}
}
