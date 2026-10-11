using ArmSoft.SME.Core.UI.HelpClasses;

namespace ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;

public class LabelPrinterInfo
{
	private static LabelPrinterInfo _instance;

	public string LabelPrinterName;

	public static LabelPrinterInfo GetInstance()
	{
		if (_instance == null)
		{
			_instance = new LabelPrinterInfo();
		}
		return _instance;
	}

	private LabelPrinterInfo()
	{
		LabelPrinterName = RegistryHelper.ReadStringFromRegistry("Label Printer", "LabelPrinter");
	}

	public void StoreSettings()
	{
		RegistryHelper.StoreToRegistry("Label Printer", LabelPrinterName, "LabelPrinter");
	}
}
