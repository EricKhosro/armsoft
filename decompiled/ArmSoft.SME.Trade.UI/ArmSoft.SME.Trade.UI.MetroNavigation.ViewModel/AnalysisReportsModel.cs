using DevExpress.Xpf.Core.MvvmSample;

namespace ArmSoft.SME.Trade.UI.MetroNavigation.ViewModel;

public class AnalysisReportsModel : MainModule
{
	public static AnalysisReportsModel Instance { get; set; }

	public AnalysisReportsModel()
	{
		IsPersistentModule = true;
		Instance = this;
	}
}
