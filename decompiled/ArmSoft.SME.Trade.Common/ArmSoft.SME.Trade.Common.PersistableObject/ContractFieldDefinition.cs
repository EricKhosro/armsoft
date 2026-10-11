using System;
using ArmSoft.SME.Common.PersistableObject;

namespace ArmSoft.SME.Trade.Common.PersistableObject;

public class ContractFieldDefinition : StringFieldDefinition
{
	public bool IsBonusCard { get; }

	public ContractFieldDefinition(string name, Func<string> captionDelegate, bool isBonusCard = false, bool isRequired = false)
		: base(name, captionDelegate, 3, isRequired)
	{
		IsBonusCard = isBonusCard;
	}
}
