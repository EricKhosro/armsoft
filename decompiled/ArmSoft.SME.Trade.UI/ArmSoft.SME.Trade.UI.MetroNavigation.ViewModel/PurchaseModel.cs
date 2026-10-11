using DevExpress.Xpf.Core.MvvmSample;

namespace ArmSoft.SME.Trade.UI.MetroNavigation.ViewModel;

public class PurchaseModel : MainModule
{
	public static PurchaseModel Instance { get; set; }

	public PurchaseModel()
	{
		IsPersistentModule = true;
		Instance = this;
	}
}
