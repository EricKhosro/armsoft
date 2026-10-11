using System;
using ArmSoft.SME.Common.PersistableObject;

namespace ArmSoft.SME.Trade.Common.PersistableObject;

public class PartnerFieldDefinition : StringFieldDefinition
{
	public bool IsSupplier { get; }

	public bool IsCustomer { get; }

	public PartnerFieldDefinition(string name, Func<string> captionDelegate, bool isSupplier, bool isCustomer, bool isRequired = false)
		: base(name, captionDelegate, 20, isRequired)
	{
		IsSupplier = isSupplier;
		IsCustomer = isCustomer;
	}
}
