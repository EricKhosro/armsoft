using DevExpress.Xpf.Core.MvvmSample;

namespace ArmSoft.SME.Trade.UI.MetroNavigation.ViewModel;

public class UserAWPModel : MainModule
{
	public static UserAWPModel Instance { get; set; }

	public UserAWPModel()
	{
		IsPersistentModule = true;
		Instance = this;
	}
}
