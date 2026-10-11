using System;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(2, "DocumentStates")]
[SmartEnumPartial(SmartEnumType.Base)]
public class DocumentStateSE : ASSmartEnum<DocumentStateSE, byte>
{
	public static readonly DocumentStateSE Draft = new DocumentStateSE("Draft", 1, "1", () => Resources.Draft);

	public static readonly DocumentStateSE Processed = new DocumentStateSE("Processed", 2, "2", () => Resources.Processed);

	public static readonly DocumentStateSE Deleted = new DocumentStateSE("Deleted", 99, "3", () => Resources.DeletedState);

	public string ValueUI { get; set; }

	protected DocumentStateSE(string name, byte value, Func<string> caption)
		: base(name, value, caption)
	{
	}

	protected DocumentStateSE(string name, byte value, string valueUI, Func<string> caption)
		: base(name, value, caption)
	{
		ValueUI = valueUI;
	}
}
