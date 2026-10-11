using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OnlineBanking.Common.Attributes;

public class DocumentTypeAttribute : DocumentTypeBaseAttribute
{
	public override DocumentCategories Category => DocumentCategories.Document;
}
