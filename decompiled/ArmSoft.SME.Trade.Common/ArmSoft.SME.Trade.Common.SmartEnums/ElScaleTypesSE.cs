using System;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "ScaleTypes")]
public class ElScaleTypesSE : ASSmartEnum<ElScaleTypesSE, string>
{
	public static readonly ElScaleTypesSE DIGI = new ElScaleTypesSE("DIGI", "1", () => Resources.Digi);

	public static readonly ElScaleTypesSE TM_A = new ElScaleTypesSE("TM_A", "2", () => Resources.TMA);

	public static readonly ElScaleTypesSE CAS = new ElScaleTypesSE("CAS", "3", () => Resources.Cas);

	public static readonly ElScaleTypesSE Shtrih_Slim_POS2 = new ElScaleTypesSE("Shtrih_Slim_POS2", "4", () => Resources.ShtrihPrint);

	public static readonly ElScaleTypesSE CASNew = new ElScaleTypesSE("CASNew", "5", () => Resources.CasNew);

	public static readonly ElScaleTypesSE MassaK = new ElScaleTypesSE("MassaK", "6", () => Resources.MassaK);

	public static readonly ElScaleTypesSE CasDirect = new ElScaleTypesSE("CasDirect", "7", () => Resources.CasDirectConnection);

	public static readonly ElScaleTypesSE DigiDirect = new ElScaleTypesSE("DigiDirect", "8", () => Resources.DigiDirectConnection);

	public static readonly ElScaleTypesSE TMADirect = new ElScaleTypesSE("TMADirect", "9", () => Resources.TMADirectConnection);

	public static readonly ElScaleTypesSE MertechDirect = new ElScaleTypesSE("MertechDirect", "10", () => Resources.MertechDirectConnection);

	public const string DIGIConst = "1";

	public const string TM_AConst = "2";

	public const string CASConst = "3";

	public const string Shtrih_Slim_POS2Const = "4";

	public const string CASNewConst = "5";

	public const string MassaKConst = "6";

	public const string CasDirectConst = "7";

	public const string DigiDirectConst = "8";

	public const string TMADirectConst = "9";

	public const string MertechDirectConst = "10";

	public ElScaleTypesSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
