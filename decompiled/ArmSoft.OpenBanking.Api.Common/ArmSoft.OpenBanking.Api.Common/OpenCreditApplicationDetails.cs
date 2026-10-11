using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OpenBanking.Api.Common;

public class OpenCreditApplicationDetails : DocumentDetails<OpenCreditApplication>
{
	public decimal ActualPercent { get; set; }

	public decimal InterestRate { get; set; }

	public OpenCreditApplicationInformation? Information { get; set; }

	public decimal ServiceFee { get; set; }

	public decimal? ServiceFeeRate { get; set; }

	public ServiceFeeRateDivisorCategory ServiceFeeRateDivisorCategory { get; set; }

	public string ServiceFeeRateDivisorCategoryDescription { get; set; } = string.Empty;
}
