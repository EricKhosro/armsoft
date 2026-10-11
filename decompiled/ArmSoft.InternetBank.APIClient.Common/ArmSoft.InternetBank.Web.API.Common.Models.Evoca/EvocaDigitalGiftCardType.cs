using ArmSoft.OnlineBanking.Common.Evoca;

namespace ArmSoft.InternetBank.Web.API.Common.Models.Evoca;

public class EvocaDigitalGiftCardType : EvocaGiftCardType
{
	public string Currency { get; set; }

	public decimal Commission { get; set; }

	public EvocaDigitalGiftCardCommissionType CommissionType { get; set; }
}
