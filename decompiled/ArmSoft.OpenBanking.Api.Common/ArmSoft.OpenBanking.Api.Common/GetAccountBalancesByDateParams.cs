using System;
using System.ComponentModel;

namespace ArmSoft.OpenBanking.Api.Common;

public class GetAccountBalancesByDateParams
{
	public required string Number { get; set; }

	public DateTime DateFrom { get; set; }

	[DefaultValue(null)]
	public DateTime? DateTo { get; set; }
}
