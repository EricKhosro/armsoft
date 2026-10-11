using System;
using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.InternetBank;

public class ImportedIBPaymentDataGetRequest
{
	public DateOnly StartDate { get; set; }

	public DateOnly EndDate { get; set; }

	public List<string> AccountNumbers { get; set; }
}
