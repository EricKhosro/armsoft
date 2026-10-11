namespace ArmSoft.InternetBank.Web.API.Common.Models.Evoca;

public class EvocaLoanData
{
	public string Code { get; set; }

	public string Name { get; set; }

	public string Currency { get; set; }

	public decimal MinimumAmount { get; set; }

	public decimal MaximumAmount { get; set; }

	public decimal Rate { get; set; }

	public int RateDivision { get; set; }

	public int MinimumMonth { get; set; }

	public int MaximumMonth { get; set; }

	public int MonthStep { get; set; }

	public decimal MonthlyPaymentPercent { get; set; }

	public decimal ServiceFeeFixed { get; set; }

	public decimal ServiceFeePercent { get; set; }

	public int AmountStep { get; set; }

	public string Note { get; set; }

	public int MaximumAge { get; set; }

	public int MinimumAge { get; set; }
}
