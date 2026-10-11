using System;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(1, "Types")]
public class UnboundColumnTypeSE : ASSmartEnum<UnboundColumnTypeSE, short>
{
	public static readonly UnboundColumnTypeSE String = new UnboundColumnTypeSE("String", 0, () => Resources.String, 0);

	public static readonly UnboundColumnTypeSE Decimal = new UnboundColumnTypeSE("Decimal", 2, () => Resources.NumberWord, 1);

	public static readonly UnboundColumnTypeSE Boolean = new UnboundColumnTypeSE("Boolean", 4, () => Resources.Boolean, 2);

	public static readonly UnboundColumnTypeSE DateOnly = new UnboundColumnTypeSE("DateOnly", 7, () => Resources.DateTime, 3);

	public static readonly UnboundColumnTypeSE DateTime = new UnboundColumnTypeSE("DateTime", 1, () => Resources.DateTimeWithHours, 4);

	public short Order { get; }

	public UnboundColumnTypeSE(string name, short value, Func<string> caption, short order)
		: base(name, value, caption)
	{
		Order = order;
	}
}
