using System;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(1, "DealTypes")]
public class AccDealTypesSE : ASSmartEnum<AccDealTypesSE, short>
{
	public static readonly AccDealTypesSE SpecialTaxSystem = new AccDealTypesSE("SpecialTaxSystem", 0, "1", () => Resources.SpecialTaxSystem);

	public static readonly AccDealTypesSE VatExempt = new AccDealTypesSE("VatExempt", 1, "2", () => Resources.VatExempt);

	public static readonly AccDealTypesSE VatZeroRate = new AccDealTypesSE("VatZeroRate", 2, "3", () => Resources.VatZeroRate);

	public static readonly AccDealTypesSE ExcludedFromVAT = new AccDealTypesSE("ExcludedFromVAT", 5, "4", () => Resources.ExcludedFromVAT);

	public static readonly AccDealTypesSE Export = new AccDealTypesSE("Export", 3, "5", () => Resources.VatExport);

	public static readonly AccDealTypesSE ExportEAEUAir = new AccDealTypesSE("ExportEAEUAir", 4, "6", () => Resources.ExportEAEUAir);

	public static readonly AccDealTypesSE VATNonPayers = new AccDealTypesSE("VATNonPayers", 6, "7", () => Resources.VatNonPayers);

	public string ValueUI { get; set; }

	public AccDealTypesSE(string name, short value, string valueUI, Func<string> caption)
		: base(name, value, caption)
	{
		ValueUI = valueUI;
	}
}
