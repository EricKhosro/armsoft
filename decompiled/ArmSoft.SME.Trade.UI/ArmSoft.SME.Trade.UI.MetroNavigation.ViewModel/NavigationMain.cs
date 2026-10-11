using System.Windows.Input;
using DevExpress.Xpf.Core.MvvmSample;

namespace ArmSoft.SME.Trade.UI.MetroNavigation.ViewModel;

public class NavigationMain : MainModule
{
	public static ICommand ShowPurchaseViewCommand;

	public static ICommand ShowSaleViewCommand;

	public static ICommand ShowPointOfSaleViewCommand;

	public static ICommand ShowStorageViewCommand;

	public static ICommand ShowCashBankViewCommand;

	public static ICommand ShowAnalysisReportsCommand;

	public static ICommand ShowEInvoicingViewCommand;

	public static ICommand ShowAdminToolsViewCommand;

	public static ICommand ShowUserAWPViewCommand;

	public int GroupIndex { get; set; }

	public static ICommand ShowMainCommand { get; set; }

	protected override void InitializeCommands()
	{
		base.InitializeCommands();
		ShowMainCommand = CreateShowModuleCommand<NavigationMain>();
		ShowPurchaseViewCommand = CreateShowModuleCommand<PurchaseModel>();
		ShowSaleViewCommand = CreateShowModuleCommand<SaleModel>();
		ShowPointOfSaleViewCommand = CreateShowModuleCommand<PointOfSaleModel>();
		ShowStorageViewCommand = CreateShowModuleCommand<StorageModel>();
		ShowCashBankViewCommand = CreateShowModuleCommand<CashBankModel>();
		ShowAnalysisReportsCommand = CreateShowModuleCommand<AnalysisReportsModel>();
		ShowEInvoicingViewCommand = CreateShowModuleCommand<eInvoicingModel>();
		ShowAdminToolsViewCommand = CreateShowModuleCommand<AdminToolsModel>();
		ShowUserAWPViewCommand = CreateShowModuleCommand<UserAWPModel>();
	}
}
