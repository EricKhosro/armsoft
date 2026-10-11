using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.UserAccess;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.UI.MetroNavigation.ViewModel;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Navigation;

namespace ArmSoft.SME.Trade.UI.MetroNavigation.View;

public class BaseView
{
	public static List<TileBarItem> SystemsUIList(CommonConst.SubSystemTypes currentSystem)
	{
		List<TileBarItem> list = new List<TileBarItem>();
		List<SubsystemAccess> accessibleSubsystems = Settings.UserAccessService.GetAccessibleSubsystems();
		if (!Settings.IsOffline)
		{
			if (!Settings.ParametersService.ShowOnlyUserAWP())
			{
				if (currentSystem != CommonConst.SubSystemTypes.PurchasesSS && accessibleSubsystems.Any((SubsystemAccess p) => p.SubsystemId == 1))
				{
					list.Add(CreateSysItem(ArmSoft.SME.Trade.Common.Resources.Purchases, NavigationMain.ShowPurchaseViewCommand));
				}
				if (currentSystem != CommonConst.SubSystemTypes.SalesSS && accessibleSubsystems.Any((SubsystemAccess p) => p.SubsystemId == 2))
				{
					list.Add(CreateSysItem(ArmSoft.SME.Common.Resources.Sales, NavigationMain.ShowSaleViewCommand));
				}
				if (currentSystem != CommonConst.SubSystemTypes.PointOfSaleSS && accessibleSubsystems.Any((SubsystemAccess p) => p.SubsystemId == 7))
				{
					list.Add(CreateSysItem(ArmSoft.SME.Trade.Common.Resources.PointOfSalePOS, NavigationMain.ShowPointOfSaleViewCommand));
				}
				if (currentSystem != CommonConst.SubSystemTypes.StorageSS && accessibleSubsystems.Any((SubsystemAccess p) => p.SubsystemId == 3))
				{
					list.Add(CreateSysItem(ArmSoft.SME.Common.Resources.Storage, NavigationMain.ShowStorageViewCommand));
				}
				if (currentSystem != CommonConst.SubSystemTypes.CashBankSS && accessibleSubsystems.Any((SubsystemAccess p) => p.SubsystemId == 4))
				{
					list.Add(CreateSysItem(ArmSoft.SME.Trade.Common.Resources.CashBank, NavigationMain.ShowCashBankViewCommand));
				}
				if (currentSystem != CommonConst.SubSystemTypes.AnalysisReports && accessibleSubsystems.Any((SubsystemAccess p) => p.SubsystemId == 5))
				{
					list.Add(CreateSysItem(ArmSoft.SME.Common.Resources.AnalysisCaption, NavigationMain.ShowAnalysisReportsCommand));
				}
				if (currentSystem != CommonConst.SubSystemTypes.eInvoicingSS && accessibleSubsystems.Any((SubsystemAccess p) => p.SubsystemId == 8))
				{
					list.Add(CreateSysItem(ArmSoft.SME.Common.Resources.EInvoicing, NavigationMain.ShowEInvoicingViewCommand));
				}
			}
			else if (currentSystem != CommonConst.SubSystemTypes.PointOfSaleSS && accessibleSubsystems.Any((SubsystemAccess p) => p.SubsystemId == 7))
			{
				list.Add(CreateSysItem(ArmSoft.SME.Trade.Common.Resources.PointOfSalePOS, NavigationMain.ShowPointOfSaleViewCommand));
			}
			if (currentSystem != CommonConst.SubSystemTypes.AdminTools && Settings.LoginService.IsAdmin)
			{
				list.Add(CreateSysItem(ArmSoft.SME.Common.Resources.AdministrativeTools, NavigationMain.ShowAdminToolsViewCommand));
			}
		}
		return list;
	}

	private static TileBarItem CreateSysItem(string systemName, ICommand cmd, string image = "")
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		TileBarItem tileBarItem = new TileBarItem();
		if (!string.IsNullOrWhiteSpace(image))
		{
			DXImage content = new DXImage
			{
				Source = ImageService.GetImage(image)
			};
			tileBarItem.Content = content;
			tileBarItem.Command = cmd;
			tileBarItem.ToolTip = systemName;
			tileBarItem.FontSize = 12.0;
		}
		else
		{
			Hyperlink hyperlink = new Hyperlink();
			Run item = new Run(systemName);
			hyperlink.Inlines.Add(item);
			tileBarItem.Content = hyperlink;
			hyperlink.Command = cmd;
			hyperlink.Foreground = Settings.GetForegroundBrush();
			hyperlink.TextDecorations = null;
			tileBarItem.Margin = new Thickness(5.0);
			tileBarItem.FontSize = 14.0;
			tileBarItem.FontWeight = FontWeights.Normal;
			tileBarItem.VerticalAlignment = VerticalAlignment.Center;
		}
		return tileBarItem;
	}
}
