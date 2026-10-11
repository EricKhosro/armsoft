using System;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OpenBanking.Api.Common;

public class CreditOnlineData : BaseContractOnlineData
{
	public required string CreditCode { get; set; }

	public decimal CurrentLimit { get; set; }

	public decimal UnusedPart { get; set; }

	public required string SettlementAccount { get; set; }

	public CreditDebt? DebtInfo { get; set; }

	public CreditOverdueStatus OverdueStatus { get; set; }

	public decimal DebtTotalAmount { get; set; }

	public string DebtDetails { get; set; } = string.Empty;

	public string CalculatedDebtDate { get; set; } = string.Empty;

	public CreditType CreditType { get; set; }

	public string? LetterCode { get; set; }

	public decimal? UnusedPartsInterestOfLetter { get; set; }

	public decimal? OverduePrincipalOfLetter { get; set; }

	public decimal? OverdueInterestOfLetter { get; set; }

	public decimal? OverdueUnusedPartsInterestOfLetter { get; set; }

	public decimal? UnusedRateValueOfLetter { get; set; }

	public int? UnusedRateDeviationOfLetter { get; set; }

	public decimal MainContractAmount { get; set; }

	public DateTime SigningDate { get; set; }
}
