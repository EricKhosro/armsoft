using System;
using ArmSoft.TaxService.Enums;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(1, "IdentityDoc")]
public class IdentityTypeSE : ASSmartEnum<IdentityTypeSE, string>
{
	public static readonly IdentityTypeSE RAPassport = new IdentityTypeSE("RAPassport", Convert.ToString(Convert.ToInt16(IdentityType.RAPassport)), () => Resources.RAPassport);

	public static readonly IdentityTypeSE BirthCert = new IdentityTypeSE("BirthCert", Convert.ToString(Convert.ToInt16(IdentityType.BirthCert)), () => Resources.BirthCert);

	public static readonly IdentityTypeSE ResidenceCard = new IdentityTypeSE("ResidenceCard", Convert.ToString(Convert.ToInt16(IdentityType.ResidenceCard)), () => Resources.ResidenceCard);

	public static readonly IdentityTypeSE ConventionalTravelDoc = new IdentityTypeSE("ConventionalTravelDoc", Convert.ToString(Convert.ToInt16(IdentityType.ConventionalTravelDoc)), () => Resources.ConventionalTravelDoc);

	public static readonly IdentityTypeSE IdCard = new IdentityTypeSE("IdCard", Convert.ToString(Convert.ToInt16(IdentityType.IdCard)), () => Resources.IdCard);

	public static readonly IdentityTypeSE BiometricPassport = new IdentityTypeSE("BiometricPassport", Convert.ToString(Convert.ToInt16(IdentityType.BiometricPassport)), () => Resources.BiometricPassport);

	public static readonly IdentityTypeSE ForeignPassportWork = new IdentityTypeSE("ForeignPassportWork", Convert.ToString(Convert.ToInt16(IdentityType.ForeignPassportWork)), () => Resources.ForeignPassportWork);

	public static readonly IdentityTypeSE ForeignPassport = new IdentityTypeSE("ForeignPassport", Convert.ToString(Convert.ToInt16(IdentityType.ForeignPassport)), () => Resources.ForeignPassport);

	public IdentityTypeSE(string name, string value, Func<string> captionDelegate)
		: base(name, value, captionDelegate)
	{
	}
}
