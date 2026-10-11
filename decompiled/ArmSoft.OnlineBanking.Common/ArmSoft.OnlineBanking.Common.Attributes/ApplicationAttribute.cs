using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OnlineBanking.Common.Attributes;

public class ApplicationAttribute : DocumentTypeBaseAttribute
{
	public override DocumentCategories Category => DocumentCategories.Document | DocumentCategories.Application;
}
