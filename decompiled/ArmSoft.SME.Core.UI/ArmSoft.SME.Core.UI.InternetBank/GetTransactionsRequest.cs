using System;
using System.Collections.Generic;

namespace ArmSoft.SME.Core.UI.InternetBank;

public class GetTransactionsRequest
{
	public List<string> AccountList { get; set; }

	public DateTime? StartDate { get; set; }

	public DateTime? EndDate { get; set; }
}
