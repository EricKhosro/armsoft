using System;
using System.Collections.Generic;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class AmioLoanDetailsInfo
{
	public string DeclineReason { get; set; }

	public string ProductName { get; set; }

	public decimal Amount { get; set; }

	public int AmountDivider { get; set; }

	public decimal? Rate { get; set; }

	public decimal ActualPercent { get; set; }

	public int Term { get; set; }

	public DateTime? PaymentDate { get; set; }

	public List<OpenCreditFilesLink> FileLinks { get; set; }
}
