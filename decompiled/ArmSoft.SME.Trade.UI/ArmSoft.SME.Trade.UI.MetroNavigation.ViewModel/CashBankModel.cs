using DevExpress.Xpf.Core.MvvmSample;

namespace ArmSoft.SME.Trade.UI.MetroNavigation.ViewModel;

public class CashBankModel : MainModule
{
	public static CashBankModel Instance { get; set; }

	public CashBankModel()
	{
		IsPersistentModule = true;
		Instance = this;
	}
}
