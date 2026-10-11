using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "OpType")]
public class ChangeDocOrderNumTypesSE : ASSmartEnum<ChangeDocOrderNumTypesSE, string>
{
	public static readonly ChangeDocOrderNumTypesSE BringToDayBegin = new ChangeDocOrderNumTypesSE("BringToDayBegin", "1", () => ArmSoft.SME.Common.Resources.BringToDayBegin);

	public static readonly ChangeDocOrderNumTypesSE BringToDayEnd = new ChangeDocOrderNumTypesSE("BringToDayEnd", "2", () => ArmSoft.SME.Common.Resources.BringToDayEnd);

	public const string BringToDayBeginConst = "1";

	public const string BringToDayEndConst = "2";

	public ChangeDocOrderNumTypesSE(string name, string value, Func<string> captionDelegate)
		: base(name, value, captionDelegate)
	{
	}
}
