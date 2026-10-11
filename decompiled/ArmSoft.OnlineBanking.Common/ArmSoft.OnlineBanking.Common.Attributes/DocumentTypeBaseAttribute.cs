using System;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OnlineBanking.Common.Attributes;

public abstract class DocumentTypeBaseAttribute : Attribute
{
	public abstract DocumentCategories Category { get; }

	public virtual bool IsCustomDocument { get; set; }

	public bool IsAvailableForBank { get; set; } = true;

	public bool IsAvailableForCreditOrganization { get; set; }

	public virtual string ReceiverNameMask { get; set; }

	public virtual string ReceiverIdMask { get; set; }

	public virtual bool IsMaskDependsOnDocumentState { get; set; }

	public bool IsDetailsVisible { get; set; } = true;
}
