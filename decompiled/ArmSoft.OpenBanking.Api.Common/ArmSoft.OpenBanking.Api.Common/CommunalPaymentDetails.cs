using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OpenBanking.Api.Common;

public class CommunalPaymentDetails : DocumentDetails<CommunalPayment>
{
	public UtilityType UtilityType { get; set; }
}
