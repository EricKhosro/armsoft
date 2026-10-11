using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "VATCalculationType")]
public class VatTypesSE : ASSmartEnum<VatTypesSE, string>
{
	public static readonly VatTypesSE CostWithVAT = new VatTypesSE("CostWithVAT", "1", () => ArmSoft.SME.Common.Resources.CostWithVAT);

	public static readonly VatTypesSE CostWithInVAT = new VatTypesSE("CostWithInVAT", "2", () => ArmSoft.SME.Common.Resources.CostWithInVAT);

	public static readonly VatTypesSE CostWithoutVAT = new VatTypesSE("CostWithoutVAT", "3", () => ArmSoft.SME.Common.Resources.CostWithoutVAT);

	public static readonly VatTypesSE CostZeroVAT = new VatTypesSE("CostZeroVAT", "4", () => ArmSoft.SME.Common.Resources.CostZeroVAT);

	public static readonly VatTypesSE CostWithInVATRetail = new VatTypesSE("CostWithInVATRetail", "5", () => ArmSoft.SME.Common.Resources.CostWithInVAT + "%");

	public const string CostWithVATConst = "1";

	public const string CostWithInVATConst = "2";

	public const string CostWithoutVATConst = "3";

	public const string CostZeroVATConst = "4";

	public const string CostWithInVATRetailConst = "5";

	public VatTypesSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
