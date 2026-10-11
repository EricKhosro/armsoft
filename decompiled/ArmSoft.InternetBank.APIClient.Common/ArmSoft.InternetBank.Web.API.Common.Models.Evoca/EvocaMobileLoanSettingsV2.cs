using System.Collections.Generic;
using ArmSoft.OnlineBanking.Common.Evoca;

namespace ArmSoft.InternetBank.Web.API.Common.Models.Evoca;

public class EvocaMobileLoanSettingsV2
{
	public int LoanSettingId { get; set; }

	public int LoanTypeId { get; set; }

	public int UnivercityCourseTypeId { get; set; }

	public string Currency { get; set; }

	public int MinSum { get; set; }

	public int MaxSum { get; set; }

	public int MinPeriod { get; set; }

	public int MaxPeriod { get; set; }

	public EvocaMobileLoanDurationType DurationType { get; set; }

	public decimal LoanPercent { get; set; }

	public int SumIncrementStep { get; set; }

	public int ProvisionFeePercent { get; set; }

	public int CommissionType { get; set; }

	public int CommissionPercent { get; set; }

	public int MonthlyCommissionFixed { get; set; }

	public int MinAge { get; set; }

	public int MaxAge { get; set; }

	public int MinActualRate { get; set; }

	public int MaxActualRate { get; set; }

	public bool IsDafault { get; set; }

	public IEnumerable<EvocaMobileLoanPaymentMethod> PaymentMethods { get; set; }
}
