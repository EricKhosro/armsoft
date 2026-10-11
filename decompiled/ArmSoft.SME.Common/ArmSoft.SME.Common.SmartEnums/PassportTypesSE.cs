using System;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(1, "IDDocTypes")]
public class PassportTypesSE : ASSmartEnum<PassportTypesSE, string>
{
	public static readonly PassportTypesSE PassportRA = new PassportTypesSE("PassportRA", "1", () => Resources.RAPassport);

	public static readonly PassportTypesSE PassportForeign = new PassportTypesSE("PassportForeign", "2", () => Resources.ForeignPassport);

	public static readonly PassportTypesSE DeathCertificate = new PassportTypesSE("DeathCertificate", "3", () => Resources.DeathCertificate);

	public const string PassportRAConst = "1";

	public const string PassportForeignConst = "2";

	public const string DeathCertificateConst = "3";

	public PassportTypesSE(string name, string value, Func<string> captionDelegate)
		: base(name, value, captionDelegate)
	{
	}
}
