using ArmSoft.SME.Core.UI.HelpClasses;
using ArmSoft.SME.Core.UI.VersionManagement;
using ArmSoft.SME.Trade.UI.SmartEnums;

namespace ArmSoft.SME.Trade.UI.UIVersionManagement;

public class UpgradeTo90234 : UpgradeTo
{
	[Change(1)]
	public static void AddPOSCodeSearchModeKey()
	{
		RegistryHelper.StoreToRegistry("POSCodeSearchButtonMode", POSCodeSearchModesSE.MixedSearch.Value, "Document Windows\\POS");
	}
}
