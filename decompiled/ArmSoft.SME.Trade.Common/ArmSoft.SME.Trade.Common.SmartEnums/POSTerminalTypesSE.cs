using System;
using System.Text.Json.Serialization;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "PluggedDevice")]
public class POSTerminalTypesSE : ASSmartEnum<POSTerminalTypesSE, string>
{
	[JsonIgnore]
	public readonly Func<string> captionShortDelegate;

	public static readonly POSTerminalTypesSE None = new POSTerminalTypesSE("None", "0", () => "");

	public static readonly POSTerminalTypesSE ECRPOS = new POSTerminalTypesSE("ECRPOS", "1", () => Resources.ECRPOS);

	public static readonly POSTerminalTypesSE IngenicoACRUS = new POSTerminalTypesSE("IngenicoACRUS", "2", () => "ARCUS 2 POS (Ingenico/SUNMI)", () => "ARCUS 2 POS");

	public static readonly POSTerminalTypesSE FeitianPOSSOFT = new POSTerminalTypesSE("FeitianPOSSOFT", "4", () => "Possoft Pay POS (Feitian)", () => "Possoft Pay POS");

	public const string NoneConst = "0";

	public const string ECRPOSConst = "1";

	public const string IngenicoACRUSConst = "2";

	public const string FeitianPOSSOFTConst = "4";

	public string CaptionShort => CaptionDelegate();

	public POSTerminalTypesSE(string name, string value, Func<string> caption, Func<string> captionShort = null)
		: base(name, value, caption)
	{
		captionShortDelegate = captionShort ?? caption;
	}
}
