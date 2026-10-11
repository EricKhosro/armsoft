using DevExpress.Xpf.Core.MvvmSample;

namespace ArmSoft.SME.Trade.UI.MetroNavigation.ViewModel;

public class eInvoicingModel : MainModule
{
	public static eInvoicingModel Instance { get; set; }

	public eInvoicingModel()
	{
		IsPersistentModule = true;
		Instance = this;
	}
}
