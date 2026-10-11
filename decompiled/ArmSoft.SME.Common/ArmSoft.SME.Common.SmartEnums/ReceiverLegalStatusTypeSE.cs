using System;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(1, "LegalStatuses")]
public class ReceiverLegalStatusTypeSE : ASSmartEnum<ReceiverLegalStatusTypeSE, string>
{
	public static readonly ReceiverLegalStatusTypeSE LegalEntity = new ReceiverLegalStatusTypeSE("LegalEntity", "1", () => Resources.LegalEntityPE);

	public static readonly ReceiverLegalStatusTypeSE FizEntity = new ReceiverLegalStatusTypeSE("FizEntity", "2", () => Resources.FizPerson);

	public const string LegalEntityConst = "1";

	public const string FizEntityConst = "2";

	public ReceiverLegalStatusTypeSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
