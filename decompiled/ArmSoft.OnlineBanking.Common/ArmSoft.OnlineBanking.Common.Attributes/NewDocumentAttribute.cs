using System;

namespace ArmSoft.OnlineBanking.Common.Attributes;

public class NewDocumentAttribute : Attribute
{
	public string ModelName { get; set; }
}
