using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OpenBanking.Api.Common;

public class LocalPayment : BaseLocalPayment
{
	public ReceiverLegalStatus ReceiverLegalStatus { get; set; }
}
