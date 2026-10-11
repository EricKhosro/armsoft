using System;

namespace ArmSoft.SME.Trade.Common.Models.ASReport;

public class MTAccountReportParam
{
	public DateOnly StartDate { get; set; }

	public DateOnly EndDate { get; set; }

	public string DocTypes { get; set; }

	public string Storage { get; set; } = string.Empty;

	public string Group { get; set; } = string.Empty;

	public string GoodCode { get; set; }

	public bool ShowAltUnit { get; set; }

	public bool ShowSumms { get; set; }

	public bool ShowSummsWithVAT { get; set; }

	public bool ShowCurrSumms { get; set; }

	public bool ShowSaleSumms { get; set; }

	public bool ShowParties { get; set; }

	public bool DetailByStorage { get; set; }

	public bool DetailByOper { get; set; }

	public bool ShowAlsoZeroRows { get; set; }
}
