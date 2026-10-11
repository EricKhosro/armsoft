using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class CalculatedMonthlyAmountResponse : CalculatedAmountResponse
{
	public decimal ChargeAmount { get; set; }

	public string ChargeAmountCurrency { get; set; }

	public decimal ServiceFee { get; set; }

	public decimal? ServiceFeeRate { get; set; }

	public ServiceFeeRateDivisorCategory ServiceFeeRateDivisorCategory { get; set; }

	public string ServiceFeeRateDivisorCategoryDescription { get; set; }
}
