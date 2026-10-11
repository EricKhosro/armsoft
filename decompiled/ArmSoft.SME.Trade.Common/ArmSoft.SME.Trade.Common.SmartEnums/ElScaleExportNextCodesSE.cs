using System;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "ASECRCheckNamesHeader")]
public class ElScaleExportNextCodesSE : ASSmartEnum<ElScaleExportNextCodesSE, string>
{
	public static readonly ElScaleExportNextCodesSE MaterialCode = new ElScaleExportNextCodesSE("MaterialCode", "1", () => Resources.TheMaterialCode);

	public static readonly ElScaleExportNextCodesSE PLUCode = new ElScaleExportNextCodesSE("PLUCode", "2", () => Resources.PLUCodeInElScale);

	public static readonly ElScaleExportNextCodesSE BarCode = new ElScaleExportNextCodesSE("BarCode", "3", () => Resources.TheBarCode);

	public const string MaterialCodeConst = "1";

	public const string PLUCodeConst = "2";

	public const string BarCodeConst = "3";

	public ElScaleExportNextCodesSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
