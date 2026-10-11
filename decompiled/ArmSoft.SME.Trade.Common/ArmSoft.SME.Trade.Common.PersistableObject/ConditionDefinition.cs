using System;
using System.Collections.Generic;
using ArmSoft.SME.Common.PersistableObject;
using ArmSoft.SME.Trade.Common.Models.Directories;

namespace ArmSoft.SME.Trade.Common.PersistableObject;

public class ConditionDefinition : FieldDefinition
{
	public StringFieldDefinition Scope { get; init; }

	public SmartEnumFieldDefinition Type { get; init; }

	public QuantityFieldDefinition Limit { get; init; }

	public DateFieldDefinition ConditionStart { get; init; }

	public DateFieldDefinition ConditionEnd { get; init; }

	public BoolFieldDefinition IncludeCurrentSale { get; init; }

	public StringFieldDefinition PartnerGroup { get; init; }

	public StringFieldDefinition Segment { get; init; }

	public BoolFieldDefinition SegmentNegation { get; init; }

	public BoolFieldDefinition IsBirthDayDiscount { get; init; }

	public DateFieldDefinition BeforBDay { get; init; }

	public DateFieldDefinition AfterBDay { get; init; }

	public Dictionary<int, Discount.ConditionDaysAndHours> DaysHours { get; init; }

	public BoolFieldDefinition UseExtension { get; init; }

	public StringFieldDefinition UserScript { get; init; }

	public ConditionDefinition(string name, Func<string> captionDelegate, bool isRequired = false)
		: base(name, captionDelegate, isRequired)
	{
	}
}
