using System.Collections.Generic;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class OpenCreditApplicationInformation
{
	public decimal ActualPercent { get; set; }

	public decimal Amount { get; set; }

	public decimal TotalRepaymentAmount { get; set; }

	public decimal? ServiceFee { get; set; }

	public decimal? ServiceFeeRate { get; set; }

	public ServiceFeeRateDivisorCategory ServiceFeeRateDivisorCategory { get; set; }

	public string ServiceFeeRateDivisorCategoryDescription { get; set; }

	public decimal MinAmount { get; set; }

	public decimal MaxAmount { get; set; }

	public Duration MinDuration { get; set; }

	public Duration MaxDuration { get; set; }

	public int AmountDivider { get; set; }

	public bool AmountIsGiven { get; set; }

	public string TransitionName { get; set; }

	public OpenCreditTransitionType TransitionType { get; set; }

	public bool IsAmountsCheckRequired { get; set; }

	public string AmountsCheckText { get; set; }

	public int MaxRepaymentDay { get; set; }

	public List<CreditReceiveMethod> AllowableCreditReceiveMethods { get; set; }

	public List<OpenCreditFilesLink> FileLinks { get; set; }
}
