using System.IO.Ports;
using Ardalis.SmartEnum;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(1, "TypeWord")]
public class ParitiesSE : SmartEnum<ParitiesSE, string>
{
	public static readonly ParitiesSE None = new ParitiesSE("None", "NO", Parity.None);

	public static readonly ParitiesSE Odd = new ParitiesSE("Odd", "ODD", Parity.Odd);

	public static readonly ParitiesSE Even = new ParitiesSE("Even", "EVEN", Parity.Even);

	public static readonly ParitiesSE Mark = new ParitiesSE("Mark", "MARK", Parity.Mark);

	public static readonly ParitiesSE Space = new ParitiesSE("Space", "SPACE", Parity.Space);

	public const string NoneConst = "NO";

	public const string OddConst = "ODD";

	public const string EvenConst = "EVEN";

	public const string MarkConst = "MARK";

	public const string SpaceConst = "SPACE";

	public Parity Parity;

	public ParitiesSE(string name, string value, Parity parity)
		: base(name, value)
	{
		Parity = parity;
	}
}
