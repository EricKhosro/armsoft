using DevExpress.Xpf.Core.MvvmSample;

namespace ArmSoft.SME.Trade.UI.MetroNavigation.ViewModel;

public class PointOfSaleModel : MainModule
{
	public static PointOfSaleModel Instance { get; set; }

	public PointOfSaleModel()
	{
		IsPersistentModule = true;
		Instance = this;
	}
}
