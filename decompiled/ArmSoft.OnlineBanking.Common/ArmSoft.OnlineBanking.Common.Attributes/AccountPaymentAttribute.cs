using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OnlineBanking.Common.Attributes;

public class AccountPaymentAttribute : DocumentTypeBaseAttribute
{
	public override DocumentCategories Category => DocumentCategories.Document | DocumentCategories.Payment | DocumentCategories.AccountPayment;

	public bool IsOnlyAmd { get; set; }
}
