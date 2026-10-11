using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OnlineBanking.Common.Attributes;

public class AttachedCardPaymentAttribute : DocumentTypeBaseAttribute
{
	public override DocumentCategories Category => DocumentCategories.Document | DocumentCategories.Payment | DocumentCategories.AttachedCardPayment;
}
