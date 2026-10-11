using System;
using System.Collections.Generic;

namespace ArmSoft.OpenBanking.Api.Common;

public class MerchantTransactionParams
{
	public DateTime StartDate { get; set; }

	public DateTime EndDate { get; set; }

	public List<string>? MerchantIds { get; set; }

	public List<string>? TerminalIds { get; set; }

	public string? AccountNumber { get; set; }
}
