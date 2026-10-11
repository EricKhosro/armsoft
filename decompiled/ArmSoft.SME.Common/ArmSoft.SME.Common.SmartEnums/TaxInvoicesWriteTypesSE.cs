using System;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(1, "TaxInvoiceExportForm")]
public class TaxInvoicesWriteTypesSE : ASSmartEnum<TaxInvoicesWriteTypesSE, string>
{
	public static readonly TaxInvoicesWriteTypesSE TaxInvExportFormElectronic = new TaxInvoicesWriteTypesSE("TaxInvExportFormElectronic", "1", () => Resources.TaxInvExportFormElectronic);

	public static readonly TaxInvoicesWriteTypesSE TaxInvExportFormPaper = new TaxInvoicesWriteTypesSE("TaxInvExportFormPaper", "2", () => Resources.TaxInvExportFormPaper);

	public const string TaxInvExportFormElectronicConst = "1";

	public const string TaxInvExportFormPaperConst = "2";

	public TaxInvoicesWriteTypesSE(string name, string value, Func<string> captionDelegate)
		: base(name, value, captionDelegate)
	{
	}
}
