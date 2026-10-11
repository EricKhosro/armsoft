using System;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Trade.Common;

namespace ArmSoft.SME.Trade.UI.SmartEnums;

public class POSCodeSearchModesSE : ASSmartEnum<POSCodeSearchModesSE, string>
{
	public static readonly POSCodeSearchModesSE ViaBarCode = new POSCodeSearchModesSE("ViaBarCode", "1", () => Resources.SearchViaBarcode, showInDropDown: true);

	public static readonly POSCodeSearchModesSE ViaProductCode = new POSCodeSearchModesSE("ViaProductCode", "2", () => Resources.SearchViaProductCode, showInDropDown: true);

	public static readonly POSCodeSearchModesSE MixedSearch = new POSCodeSearchModesSE("MixedSearch", "3", () => Resources.SearchViaBarcodeAndProductCode, showInDropDown: true);

	public bool ShowInDropDown;

	public POSCodeSearchModesSE(string name, string value, Func<string> caption, bool showInDropDown)
		: base(name, value, caption)
	{
		ShowInDropDown = showInDropDown;
	}
}
