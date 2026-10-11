using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumPartial(SmartEnumType.Special)]
[SmartEnumProviderSupport(2, "DocumentStates")]
[SmartEnumPartial(SmartEnumType.Base)]
public class DocumentStateSE : ASSmartEnum<DocumentStateSE, byte>
{
	public static readonly DocumentStateSE DraftOrProcessed = new DocumentStateSE("DraftOrProcessed", 0, "0", () => ArmSoft.SME.Common.Resources.DraftOrProcessed);

	public static readonly DocumentStateSE Draft = new DocumentStateSE("Draft", 1, "1", () => ArmSoft.SME.Common.Resources.Draft);

	public static readonly DocumentStateSE Processed = new DocumentStateSE("Processed", 2, "2", () => ArmSoft.SME.Common.Resources.Processed);

	public static readonly DocumentStateSE Deleted = new DocumentStateSE("Deleted", 99, "3", () => ArmSoft.SME.Common.Resources.DeletedState);

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
