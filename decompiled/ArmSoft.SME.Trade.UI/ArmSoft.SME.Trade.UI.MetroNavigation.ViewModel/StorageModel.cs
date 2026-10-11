using DevExpress.Xpf.Core.MvvmSample;

namespace ArmSoft.SME.Trade.UI.MetroNavigation.ViewModel;

public class StorageModel : MainModule
{
	public static StorageModel Instance { get; set; }

	public StorageModel()
	{
		IsPersistentModule = true;
		Instance = this;
	}
}
