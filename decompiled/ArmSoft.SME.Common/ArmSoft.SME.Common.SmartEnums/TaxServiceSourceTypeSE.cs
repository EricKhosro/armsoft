using System;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(1, "ImportExportSources")]
public class TaxServiceSourceTypeSE : ASSmartEnum<TaxServiceSourceTypeSE, short>
{
	public static readonly TaxServiceSourceTypeSE EInvoicing = new TaxServiceSourceTypeSE("EInvoicing", 1, () => Resources.EInvoicing + " " + Resources.DirectConnectionWEB);

	public static readonly TaxServiceSourceTypeSE File = new TaxServiceSourceTypeSE("File", 2, () => Resources.EInvoicing + " (" + Resources.File + ")");

	protected TaxServiceSourceTypeSE(string name, byte value, Func<string> caption)
		: base(name, (short)value, caption)
	{
	}
}
