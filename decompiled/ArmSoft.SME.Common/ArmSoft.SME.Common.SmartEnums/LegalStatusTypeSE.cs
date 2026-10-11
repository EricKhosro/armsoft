using System;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(1, "LegalStatuses")]
public class LegalStatusTypeSE : ASSmartEnum<LegalStatusTypeSE, string>
{
	public static readonly LegalStatusTypeSE LegalEntity = new LegalStatusTypeSE("LegalEntity", "1", () => Resources.LegalEntity);

	public static readonly LegalStatusTypeSE FizEntity = new LegalStatusTypeSE("FizEntity", "2", () => Resources.FizPerson);

	public static readonly LegalStatusTypeSE IndividualEntity = new LegalStatusTypeSE("IndividualEntity", "3", () => Resources.PrivateEntity);

	public const string LegalEntityConst = "1";

	public const string FizEntityConst = "2";

	public const string IndividualEntityConst = "3";

	public LegalStatusTypeSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
