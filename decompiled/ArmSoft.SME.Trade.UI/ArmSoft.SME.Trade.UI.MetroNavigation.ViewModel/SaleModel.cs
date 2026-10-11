using DevExpress.Xpf.Core.MvvmSample;

namespace ArmSoft.SME.Trade.UI.MetroNavigation.ViewModel;

public class SaleModel : MainModule
{
	public static SaleModel Instance { get; set; }

	public SaleModel()
	{
		IsPersistentModule = true;
		Instance = this;
	}
}
