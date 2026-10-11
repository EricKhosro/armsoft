using System;
using System.Collections.Generic;
using ArmSoft.SME.Common.Models.InternetBank;

namespace ArmSoft.SME.Common.Models.DPR;

public class ImportIBTransactionsRequestModel
{
	public DateOnly StartDate { get; set; }

	public DateOnly EndDate { get; set; }

	public List<string> Accounts { get; set; }

	public List<IBImportTransactionData> OnlineTransactions { get; set; }
}
