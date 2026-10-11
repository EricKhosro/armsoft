using System.Collections.Generic;

namespace ArmSoft.InternetBank.Web.API.Common.Models.Evoca;

public class EvocaMobileLoanSettings
{
	public int LoanSettingId { get; set; }

	public int LoanTypeId { get; set; }

	public int UnivercityCourseTypeId { get; set; }

	public string Currency { get; set; }

	public int MinSum { get; set; }

	public int MaxSum { get; set; }

	public int MinMonth { get; set; }

	public int MaxMonth { get; set; }

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
