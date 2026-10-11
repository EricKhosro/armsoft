using DevExpress.Xpf.Core.MvvmSample;

namespace ArmSoft.SME.Trade.UI.MetroNavigation.ViewModel;

public class AdminToolsModel : Module
{
	public static AdminToolsModel Instance { get; set; }

	public AdminToolsModel()
	{
		IsPersistentModule = true;
		Instance = this;
	}
}
