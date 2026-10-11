using System;
using ArmSoft.SME.Common.PersistableObject;

namespace ArmSoft.SME.Trade.Common.PersistableObject;

public class PartnerContractGroupDefinition : FieldDefinition
{
	public PartnerFieldDefinition PartnerFieldDefinition { get; init; }

	public ContractFieldDefinition ContractFieldDefinition { get; init; }

	public SumFieldDefinition SummaPartFieldDefinition { get; init; }

	public PartnerContractGroupDefinition(string name, Func<string> captionDelegate, bool partnerIsRequired = false)
		: base(name, captionDelegate, partnerIsRequired)
	{
	}
}
