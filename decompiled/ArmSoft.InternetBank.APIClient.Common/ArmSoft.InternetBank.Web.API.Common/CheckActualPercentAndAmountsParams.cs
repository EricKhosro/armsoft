using System.Collections.Generic;
using ArmSoft.InternetBank.Web.API.Common.Models;

namespace ArmSoft.InternetBank.Web.API.Common;

public class CheckActualPercentAndAmountsParams
{
	public decimal ActualPercent { get; set; }

	public decimal Amount { get; set; }

	public decimal TotalRepaymentAmount { get; set; }

	public string AmountCheckText { get; set; }

	public List<OpenCreditFilesLink> FileLinks { get; set; }
}
