using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OnlineBanking.Common.Attributes;

public class CancellationRequestAttribute : DocumentTypeBaseAttribute
{
	public override DocumentCategories Category => DocumentCategories.Document | DocumentCategories.CancellationRequest;
}
