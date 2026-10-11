using System;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "DeviceType")]
public class DeviceTypesSE : ASSmartEnum<DeviceTypesSE, string>
{
	public static readonly DeviceTypesSE ElScale = new DeviceTypesSE("ElScale", "1", () => Resources.ElScale);

	public static readonly DeviceTypesSE Terminal = new DeviceTypesSE("Terminal", "2", () => Resources.Terminal);

	public const string ElScaleConst = "1";

	public const string TerminalConst = "2";

	public DeviceTypesSE(string name, string value, Func<string> captionDelegate)
		: base(name, value, captionDelegate)
	{
	}
}
