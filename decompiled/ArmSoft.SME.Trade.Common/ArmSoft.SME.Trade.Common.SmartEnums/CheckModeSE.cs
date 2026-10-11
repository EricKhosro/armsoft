using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "CheckMode")]
public class CheckModeSE : ASSmartEnum<CheckModeSE, string>
{
	public static readonly CheckModeSE DontCheck = new CheckModeSE("DontCheck", "1", () => ArmSoft.SME.Common.Resources.NoCheck);

	public static readonly CheckModeSE Warning = new CheckModeSE("Warning", "2", () => ArmSoft.SME.Common.Resources.CheckOnlyAsking);

	public static readonly CheckModeSE Check = new CheckModeSE("Check", "3", () => ArmSoft.SME.Common.Resources.StrictCheck);

	public CheckModeSE(string name, string value, Func<string> captionDelegate)
		: base(name, value, captionDelegate)
	{
	}
}
