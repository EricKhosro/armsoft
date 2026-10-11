using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.UI.MetroNavigation.View;
using ArmSoft.SME.Trade.UI.MetroNavigation.ViewModel;
using DevExpress.Xpf.Core.MvvmSample;

namespace ArmSoft.SME.Trade.UI.MetroNavigation;

public partial class MainNavigationWindow : UserControl, IComponentConnector
{
	public MainNavigationWindow()
	{
		InitializeComponent();
		if (!Settings.IsOffline && !Settings.LoginService.IsAdmin && Settings.UserAccessService.GetAccessibleSubsystems().Count == 1)
		{
			MvvmRoot.BeginInit();
			switch (Settings.UserAccessService.GetAccessibleSubsystems()[0].SubsystemId)
			{
			case 1:
				MvvmRoot.Modules.Add(new ModuleDescription
				{
					ModuleType = typeof(PurchaseModel),
					ViewType = typeof(PurchaseView)
				});
				break;
			case 2:
				MvvmRoot.Modules.Add(new ModuleDescription
				{
					ModuleType = typeof(SaleModel),
					ViewType = typeof(SaleView)
				});
				break;
			case 7:
				MvvmRoot.Modules.Add(new ModuleDescription
				{
					ModuleType = typeof(PointOfSaleModel),
					ViewType = typeof(PointOfSaleView)
				});
				break;
			case 3:
				MvvmRoot.Modules.Add(new ModuleDescription
				{
					ModuleType = typeof(StorageModel),
					ViewType = typeof(StorageView)
				});
				break;
			case 4:
				MvvmRoot.Modules.Add(new ModuleDescription
				{
					ModuleType = typeof(CashBankModel),
					ViewType = typeof(CashBankView)
				});
				break;
			case 5:
				MvvmRoot.Modules.Add(new ModuleDescription
				{
					ModuleType = typeof(AnalysisReportsModel),
					ViewType = typeof(AnalysisReportsView)
				});
				break;
			case 8:
				MvvmRoot.Modules.Add(new ModuleDescription
				{
					ModuleType = typeof(eInvoicingModel),
					ViewType = typeof(eInvoicingView)
				});
				break;
			case 9:
				MvvmRoot.Modules.Add(new ModuleDescription
				{
					ModuleType = typeof(AdminToolsModel),
					ViewType = typeof(AdminToolsView)
				});
				break;
			}
			MvvmRoot.EndInit();
		}
		else
		{
			MvvmRoot.BeginInit();
			MvvmRoot.Modules.Add(new ModuleDescription
			{
				ModuleType = typeof(NavigationMain),
				ViewType = typeof(MainView)
			});
			MvvmRoot.EndInit();
			MvvmRoot.Modules.Add(new ModuleDescription
			{
				ModuleType = typeof(PurchaseModel),
				ViewType = typeof(PurchaseView)
			});
			MvvmRoot.Modules.Add(new ModuleDescription
			{
				ModuleType = typeof(CashBankModel),
				ViewType = typeof(CashBankView)
			});
			MvvmRoot.Modules.Add(new ModuleDescription
			{
				ModuleType = typeof(StorageModel),
				ViewType = typeof(StorageView)
			});
			MvvmRoot.Modules.Add(new ModuleDescription
			{
				ModuleType = typeof(SaleModel),
				ViewType = typeof(SaleView)
			});
			MvvmRoot.Modules.Add(new ModuleDescription
			{
				ModuleType = typeof(PointOfSaleModel),
				ViewType = typeof(PointOfSaleView)
			});
			MvvmRoot.Modules.Add(new ModuleDescription
			{
				ModuleType = typeof(AnalysisReportsModel),
				ViewType = typeof(AnalysisReportsView)
			});
			MvvmRoot.Modules.Add(new ModuleDescription
			{
				ModuleType = typeof(eInvoicingModel),
				ViewType = typeof(eInvoicingView)
			});
			MvvmRoot.Modules.Add(new ModuleDescription
			{
				ModuleType = typeof(AdminToolsModel),
				ViewType = typeof(AdminToolsView)
			});
			if (!string.IsNullOrWhiteSpace(Settings.ParametersService.UserAWPNavigatorName()))
			{
				MvvmRoot.Modules.Add(new ModuleDescription
				{
					ModuleType = typeof(UserAWPModel),
					ViewType = typeof(UserAWPView)
				});
			}
		}
		ASFontHelper.SetFontSettings(this);
	}

	public string GetCurrentModuleCode()
	{
		Type type = MvvmRoot.MainModule.CurrentModule.GetType();
		if ((object)type == typeof(NavigationMain))
		{
			return "Main";
		}
		if ((object)type == typeof(PurchaseModel))
		{
			return "PurchaseModel";
		}
		if ((object)type == typeof(SaleModel))
		{
			return "SaleModel";
		}
		if ((object)type == typeof(PointOfSaleModel))
		{
			return "PointOfSaleModel";
		}
		if ((object)type == typeof(StorageModel))
		{
			return "StorageModel";
		}
		if ((object)type == typeof(CashBankModel))
		{
			return "CashBankModel";
		}
		if ((object)type == typeof(AnalysisReportsModel))
		{
			return "AnalysisReportsModel";
		}
		if ((object)type == typeof(eInvoicingModel))
		{
			return "EInvoicing";
		}
		if ((object)type == typeof(AdminToolsModel))
		{
			return "AdminToolsModel";
		}
		if ((object)type == typeof(UserAWPModel))
		{
			return "UserAWPModel";
		}
		return "Main";
	}

	public void NavigateToCommandByKey(string sKey)
	{
		ICommand command = null;
		switch (sKey ?? "")
		{
		case "PurchaseModel":
			command = NavigationMain.ShowPurchaseViewCommand;
			break;
		case "SaleModel":
			command = NavigationMain.ShowSaleViewCommand;
			break;
		case "PointOfSaleModel":
			command = NavigationMain.ShowPointOfSaleViewCommand;
			break;
		case "StorageModel":
			command = NavigationMain.ShowStorageViewCommand;
			break;
		case "CashBankModel":
			command = NavigationMain.ShowCashBankViewCommand;
			break;
		case "AnalysisReportsModel":
			command = NavigationMain.ShowAnalysisReportsCommand;
			break;
		case "EInvoicing":
			command = NavigationMain.ShowEInvoicingViewCommand;
			break;
		case "AdminToolsModel":
			command = NavigationMain.ShowAdminToolsViewCommand;
			break;
		case "UserAWPModel":
			command = NavigationMain.ShowUserAWPViewCommand;
			break;
		}
		command?.Execute(null);
	}

	public short GetSubSystemByKey(string sKey)
	{
		return sKey switch
		{
			"PurchaseModel" => (short)1, 
			"SaleModel" => (short)2, 
			"PointOfSaleModel" => (short)7, 
			"StorageModel" => (short)3, 
			"CashBankModel" => (short)4, 
			"AnalysisReportsModel" => (short)5, 
			"B2BModel" => (short)6, 
			"EInvoicing" => (short)8, 
			"AdminToolsModel" => (short)9, 
			"UserAWPModel" => (short)100, 
			_ => (short)0, 
		};
	}
}
