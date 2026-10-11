using System;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(4, "DegreeOfRound")]
public class RoundDegreesSE : ASSmartEnum<RoundDegreesSE, string>
{
	public static readonly RoundDegreesSE Round001 = new RoundDegreesSE("Round001", "0.01", () => string.Format(Resources.RoundTo, "0.01"));

	public static readonly RoundDegreesSE Round005 = new RoundDegreesSE("Round005", "0.05", () => string.Format(Resources.RoundTo, "0.05"));

	public static readonly RoundDegreesSE Round01 = new RoundDegreesSE("Round01", "0.1", () => string.Format(Resources.RoundTo, "0.1"));

	public static readonly RoundDegreesSE Round05 = new RoundDegreesSE("Round05", "0.5", () => string.Format(Resources.RoundTo, "0.5"));

	public static readonly RoundDegreesSE Round1 = new RoundDegreesSE("Round1", "1", () => string.Format(Resources.RoundTo, "1"));

	public static readonly RoundDegreesSE Round5 = new RoundDegreesSE("Round5", "5", () => string.Format(Resources.RoundTo, "5"));

	public static readonly RoundDegreesSE Round10 = new RoundDegreesSE("Round10", "10", () => string.Format(Resources.RoundTo, "10"));

	public static readonly RoundDegreesSE Round50 = new RoundDegreesSE("Round50", "50", () => string.Format(Resources.RoundTo, "50"));

	public static readonly RoundDegreesSE Round100 = new RoundDegreesSE("Round100", "100", () => string.Format(Resources.RoundTo, "100"));

	public static readonly RoundDegreesSE Round500 = new RoundDegreesSE("Round500", "500", () => string.Format(Resources.RoundTo, "500"));

	public static readonly RoundDegreesSE Round1000 = new RoundDegreesSE("Round1000", "1000", () => string.Format(Resources.RoundTo, "1000"));

	public const string Round001Const = "0.01";

	public const string Round005Const = "0.05";

	public const string Round01Const = "0.1";

	public const string Round05Const = "0.5";

	public const string Round1Const = "1";

	public const string Round5Const = "5";

	public const string Round10Const = "10";

	public const string Round50Const = "50";

	public const string Round100Const = "100";

	public const string Round500Const = "500";

	public const string Round1000Const = "1000";

	public RoundDegreesSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
