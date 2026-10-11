using System;
using ArmSoft.OnlineBanking.Common.Evoca;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class EM : Application
{
	public int? ApplicationId { get; set; }

	public int LoanTypeId { get; set; }

	public string LoanIcon { get; set; }

	public EvocaMobileLoanApplicationStatus Status { get; set; }

	public int LoanSettingId { get; set; }

	public EvocaMobileLoanType LoanType { get; set; }

	public string LoanName { get; set; }

	public string Currency { get; set; }

	public decimal Percent { get; set; }

	public int Duration { get; set; }

	public string AccountNumber { get; set; }

	public string CardNumber { get; set; }

	public string DefaultCardNumber { get; set; }

	public int PreferredDay { get; set; }

	public decimal Amount { get; set; }

	public decimal ActualPercent { get; set; }

	public int SumStep { get; set; }

	[Obsolete("Use MinPeriod instead.")]
	public int MinMonth { get; set; }

	[Obsolete("Use MaxPeriod instead.")]
	public int MaxMonth { get; set; }

	[Obsolete("Use PeriodIncrementStep instead.")]
	public int MonthStep { get; set; }

	public int MinPeriod { get; set; }

	public int MaxPeriod { get; set; }

	public int PeriodIncrementStep { get; set; }

	public EvocaMobileLoanDurationType DurationType { get; set; }

	public int MinAmount { get; set; }

	public int ActualAmount { get; set; }

	public int ActualDuration { get; set; }

	public double AmountIndex { get; set; }

	public string DepositContract { get; set; }

	public EM()
	{
		AmountIndex = 0.0;
	}
}
