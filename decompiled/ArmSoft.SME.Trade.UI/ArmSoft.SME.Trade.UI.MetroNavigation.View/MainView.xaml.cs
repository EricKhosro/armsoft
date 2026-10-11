using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Navigation;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.UserAccess;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Core.UI.HelpClasses;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.DataView;
using ArmSoft.SME.Trade.UI.MetroNavigation.ViewModel;
using DevExpress.Xpf.Core.MvvmSample;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Trade.UI.MetroNavigation.View;

public partial class MainView : ModuleView, IComponentConnector
{
	private bool _firstActivated = true;

	private bool _setTileSize_firstCall = true;

	public MainView()
	{
		InitializeComponent();
		PurchasesTile.Command = NavigationMain.ShowPurchaseViewCommand;
		SalesTile.Command = NavigationMain.ShowSaleViewCommand;
		POSTile.Command = NavigationMain.ShowPointOfSaleViewCommand;
		StorageSectionTile.Command = NavigationMain.ShowStorageViewCommand;
		CashBankSectionTile.Command = NavigationMain.ShowCashBankViewCommand;
		AnalysisSectionTile.Command = NavigationMain.ShowAnalysisReportsCommand;
		EInvoicingSectionTile.Command = NavigationMain.ShowEInvoicingViewCommand;
		AdminToolsSectionTile.Command = NavigationMain.ShowAdminToolsViewCommand;
		UserAWPSSTile.Command = NavigationMain.ShowUserAWPViewCommand;
		AddSafeEventHandlers();
		ASFontHelper.SetFontSettings(this);
	}

	private void MainView_Initialized(object sender, EventArgs e)
	{
		if (!_firstActivated)
		{
			return;
		}
		short num = 0;
		List<SubsystemAccess> accessibleSubsystems = Settings.UserAccessService.GetAccessibleSubsystems();
		CreateHyperlinkItem(CloudLogoText, ArmSoft.SME.Common.Resources.CloudBenefits.ToUpper());
		EInvoicingSectionTile.Visibility = Visibility.Collapsed;
		if (accessibleSubsystems.Any((SubsystemAccess p) => p.SubsystemId == 1) && !Settings.IsOffline)
		{
			PurchasesTile.Visibility = Visibility.Visible;
			num++;
		}
		else
		{
			PurchasesTile.Visibility = Visibility.Collapsed;
		}
		if (accessibleSubsystems.Any((SubsystemAccess p) => p.SubsystemId == 2) && !Settings.IsOffline)
		{
			SalesTile.Visibility = Visibility.Visible;
			num++;
		}
		else
		{
			SalesTile.Visibility = Visibility.Collapsed;
		}
		if (accessibleSubsystems.Any((SubsystemAccess p) => p.SubsystemId == 7) || (Settings.IsOffline && accessibleSubsystems.Any((SubsystemAccess p) => p.SubsystemId == 2) && Settings.UserAccessService.GetNavigatorNodePermission(2, UserAccessIdSE.MTBill).AllowView))
		{
			POSTile.Visibility = Visibility.Visible;
			num++;
		}
		else
		{
			POSTile.Visibility = Visibility.Collapsed;
		}
		if (accessibleSubsystems.Any((SubsystemAccess p) => p.SubsystemId == 3) && !Settings.IsOffline)
		{
			StorageSectionTile.Visibility = Visibility.Visible;
			num++;
		}
		else
		{
			StorageSectionTile.Visibility = Visibility.Collapsed;
		}
		if (accessibleSubsystems.Any((SubsystemAccess p) => p.SubsystemId == 4) && !Settings.IsOffline)
		{
			CashBankSectionTile.Visibility = Visibility.Visible;
			num++;
		}
		else
		{
			CashBankSectionTile.Visibility = Visibility.Collapsed;
		}
		if (accessibleSubsystems.Any((SubsystemAccess p) => p.SubsystemId == 5) && !Settings.IsOffline)
		{
			AnalysisSectionTile.Visibility = Visibility.Visible;
			num++;
		}
		else
		{
			AnalysisSectionTile.Visibility = Visibility.Collapsed;
		}
		if (accessibleSubsystems.Any((SubsystemAccess p) => p.SubsystemId == 8) && !Settings.IsOffline)
		{
			EInvoicingSectionTile.Visibility = Visibility.Visible;
			num++;
		}
		else
		{
			EInvoicingSectionTile.Visibility = Visibility.Collapsed;
		}
		if (Settings.LoginService.IsAdmin && !Settings.IsOffline)
		{
			AdminToolsSectionTile.Visibility = Visibility.Visible;
			num++;
		}
		else
		{
			AdminToolsSectionTile.Visibility = Visibility.Collapsed;
		}
		if (accessibleSubsystems.Any((SubsystemAccess p) => p.SubsystemId == 100) && !string.IsNullOrWhiteSpace(Settings.ParametersService.UserAWPNavigatorName()) && !Settings.IsOffline)
		{
			UserAWPSSTile.Visibility = Visibility.Visible;
			num++;
		}
		else
		{
			UserAWPSSTile.Visibility = Visibility.Collapsed;
		}
		bool tilesCountOdd = num % 2 != 0;
		if (PurchasesTile.Visibility == Visibility.Visible)
		{
			SetTileSize(tilesCountOdd, ref PurchasesTile);
		}
		if (SalesTile.Visibility == Visibility.Visible)
		{
			SetTileSize(tilesCountOdd, ref SalesTile);
		}
		if (POSTile.Visibility == Visibility.Visible)
		{
			SetTileSize(tilesCountOdd, ref POSTile);
		}
		if (StorageSectionTile.Visibility == Visibility.Visible)
		{
			SetTileSize(tilesCountOdd, ref StorageSectionTile);
		}
		if (CashBankSectionTile.Visibility == Visibility.Visible)
		{
			SetTileSize(tilesCountOdd, ref CashBankSectionTile);
		}
		if (AnalysisSectionTile.Visibility == Visibility.Visible)
		{
			SetTileSize(tilesCountOdd, ref AnalysisSectionTile);
		}
		_ = EInvoicingSectionTile.Visibility;
		GetSectionsCollapseStates();
		AS_Trade_Header.Content = ArmSoft.SME.Trade.Common.Resources.ProgramName;
		AS_Trade_MobileApp_Header.Content = ArmSoft.SME.Common.Resources.MobileApp;
		AS_Trade_QuickStart_Header.Content = ArmSoft.SME.Common.Resources.QuickStartCentre;
		AS_Trade_Helper_Header.Content = ArmSoft.SME.Common.Resources.HelperCentre;
		AS_Accountant_Remote_Support.Content = ArmSoft.SME.Common.Resources.Support;
		MobileAppText.Text = ArmSoft.SME.Common.Resources.ScanQRAandInstallMobileApp;
		CompanyTileTextBlock.Text = ArmSoft.SME.Common.Resources.CompanyInformation;
		GoodsTileTextBlock.Text = ArmSoft.SME.Trade.Common.Resources.AddGoodsAndServices;
		GoodRemainsTileTextBlock.Text = ArmSoft.SME.Trade.Common.Resources.EnterGoodsRemains;
		PartnersTileTextBlock.Text = ArmSoft.SME.Common.Resources.AddPartners;
		PartnersRemCorrectTextBlock.Text = ArmSoft.SME.Trade.Common.Resources.EnterPartAccBalance;
		CashesTileTextBlock.Text = ArmSoft.SME.Trade.Common.Resources.EnterYourCashes;
		BankAccountsTileTextBlock.Text = ArmSoft.SME.Common.Resources.AddSettlementAccounts;
		YouTubeLearningVideosLessonsTileTextBlock.Text = ArmSoft.SME.Trade.Common.Resources.ASTrade7TrainingLessons.ToUpper();
		YouTubeLearningVideosTileTextBlock.Text = ArmSoft.SME.Trade.Common.Resources.WatchASTrade7TrainingVideos.ToUpper();
		TeamViewerTileLabel.Content = "TeamViewer";
		TeamViewerTileContentTextBlock.Text = ArmSoft.SME.Common.Resources.RemoteConnectionSupport;
		AnyDeskTileTitleLabel.Content = "AnyDesk";
		AnyDeskTileContentTextBlock.Text = ArmSoft.SME.Common.Resources.RemoteConnectionSupport;
		TelegramTileLabel.Content = "Telegram";
		TelegramTileTextBlock.Text = ArmSoft.SME.Common.Resources.TextForTelegramSupport;
		if (!Settings.IsOffline)
		{
			if (accessibleSubsystems.Any((SubsystemAccess p) => p.SubsystemId == 1) || accessibleSubsystems.Any((SubsystemAccess p) => p.SubsystemId == 3))
			{
				GoodsTile.Visibility = Visibility.Visible;
				GoodRemainsTile.Visibility = Visibility.Visible;
			}
			else
			{
				if (accessibleSubsystems.Any((SubsystemAccess p) => p.SubsystemId == 2))
				{
					GoodsTile.Visibility = Visibility.Visible;
				}
				else
				{
					GoodsTile.Visibility = Visibility.Collapsed;
				}
				GoodRemainsTile.Visibility = Visibility.Collapsed;
			}
			if (accessibleSubsystems.Any((SubsystemAccess p) => p.SubsystemId == 1) || accessibleSubsystems.Any((SubsystemAccess p) => p.SubsystemId == 2))
			{
				PartnersTile.Visibility = Visibility.Visible;
				PartnersRemCorrectTile.Visibility = Visibility.Visible;
				CashesTile.Visibility = Visibility.Visible;
				BankAccountsTile.Visibility = Visibility.Visible;
			}
			else
			{
				if (accessibleSubsystems.Any((SubsystemAccess p) => p.SubsystemId == 4))
				{
					PartnersTile.Visibility = Visibility.Visible;
					CashesTile.Visibility = Visibility.Visible;
					BankAccountsTile.Visibility = Visibility.Visible;
				}
				else
				{
					PartnersTile.Visibility = Visibility.Collapsed;
					CashesTile.Visibility = Visibility.Collapsed;
					BankAccountsTile.Visibility = Visibility.Collapsed;
				}
				PartnersRemCorrectTile.Visibility = Visibility.Collapsed;
			}
		}
		if (!Settings.ParametersService.ShowQuickStartCentre())
		{
			AS_Trade_QuickStart_Grid.Visibility = Visibility.Collapsed;
		}
		if (Settings.ParametersService.ShowOnlyUserAWP())
		{
			PurchasesTile.Visibility = Visibility.Collapsed;
			SalesTile.Visibility = Visibility.Collapsed;
			StorageSectionTile.Visibility = Visibility.Collapsed;
			CashBankSectionTile.Visibility = Visibility.Collapsed;
			AnalysisSectionTile.Visibility = Visibility.Collapsed;
			EInvoicingSectionTile.Visibility = Visibility.Collapsed;
		}
		_firstActivated = false;
	}

	private void GetSectionsCollapseStates()
	{
		IValueConverter converter = (IValueConverter)FindResource("ImageByNameConverter");
		Binding binding = new Binding
		{
			Converter = converter
		};
		if (RegistryHelper.ReadBoolFromRegistry("MobileAppSectionIsCollapsed"))
		{
			MobileAppContentSectionScrollView.Visibility = Visibility.Collapsed;
			binding.ConverterParameter = "down_16.svg";
		}
		else
		{
			MobileAppContentSectionScrollView.Visibility = Visibility.Visible;
			binding.ConverterParameter = "up_16.svg";
		}
		BindingOperations.SetBinding((DependencyObject)(object)MobileAppSectionColapseImage, Image.SourceProperty, binding);
		Binding binding2 = new Binding
		{
			Converter = converter
		};
		if (RegistryHelper.ReadBoolFromRegistry("QuickStartSectionIsCollapsed"))
		{
			AS_Trade_QuickStart_Header_ScrollViewer.Visibility = Visibility.Collapsed;
			binding2.ConverterParameter = "down_16.svg";
		}
		else
		{
			AS_Trade_QuickStart_Header_ScrollViewer.Visibility = Visibility.Visible;
			binding2.ConverterParameter = "up_16.svg";
		}
		BindingOperations.SetBinding((DependencyObject)(object)QuickStartSectionCollapseImage, Image.SourceProperty, binding2);
		Binding binding3 = new Binding
		{
			Converter = converter
		};
		if (RegistryHelper.ReadBoolFromRegistry("WorkingHelperSectionIsCollapsed"))
		{
			AS_Trade_Helper_Header_ScrollViewer.Visibility = Visibility.Collapsed;
			binding3.ConverterParameter = "down_16.svg";
		}
		else
		{
			AS_Trade_Helper_Header_ScrollViewer.Visibility = Visibility.Visible;
			binding3.ConverterParameter = "up_16.svg";
		}
		BindingOperations.SetBinding((DependencyObject)(object)WorkingHelperSectionCollapseImage, Image.SourceProperty, binding3);
		Binding binding4 = new Binding
		{
			Converter = converter
		};
		if (RegistryHelper.ReadBoolFromRegistry("RemoteSupportSectionIsCollapsed"))
		{
			AS_Trade_Remote_Support_ScrollViewer.Visibility = Visibility.Collapsed;
			binding4.ConverterParameter = "down_16.svg";
		}
		else
		{
			AS_Trade_Remote_Support_ScrollViewer.Visibility = Visibility.Visible;
			binding4.ConverterParameter = "up_16.svg";
		}
		BindingOperations.SetBinding((DependencyObject)(object)RemoteSupportSectionColapseImage, Image.SourceProperty, binding4);
	}

	private void AddSafeEventHandlers()
	{
		Loaded += new SafeEventHandler<EventArgs>(MainView_Initialized).Handler;
		CompanyTile.Click += new SafeEventHandler<EventArgs>(CompanyTile_Click).Handler;
		GoodsTile.Click += new SafeEventHandler<EventArgs>(GoodsTile_Click).Handler;
		GoodRemainsTile.Click += new SafeEventHandler<EventArgs>(GoodRemainsTile_Click).Handler;
		PartnersTile.Click += new SafeEventHandler<EventArgs>(PartnersTile_Click).Handler;
		PartnersRemCorrectTile.Click += new SafeEventHandler<EventArgs>(PartnersRemCorrectTile_Click).Handler;
		CashesTile.Click += new SafeEventHandler<EventArgs>(CashesTile_Click).Handler;
		BankAccountsTile.Click += new SafeEventHandler<EventArgs>(BankAccountsTile_Click).Handler;
		CloudLogoTile.Click += new SafeEventHandler<EventArgs>(CloudLogoTile_Click).Handler;
		CloudLogoText.MouseLeftButtonUp += new SafeEventHandler<EventArgs>(CloudLogoTile_Click).Handler;
		YouTubeLearningVideosTile.Click += new SafeEventHandler<EventArgs>(YouTubeLearningVideosTile_Click).Handler;
		YouTubeLearningVideosLessonsTile.Click += new SafeEventHandler<EventArgs>(YouTubeLearningVideosLessonsTile_Click).Handler;
		TeamViewerTile.Click += new SafeEventHandler<EventArgs>(TeamViewerTile_Click).Handler;
		AnyDeskTile.Click += new SafeEventHandler<EventArgs>(AnyDeskTile_Click).Handler;
		TelegramTile.Click += new SafeEventHandler<EventArgs>(TelegramTile_Click).Handler;
		CloudLogoTile.Click += new SafeEventHandler<EventArgs>(CloudLogoTile_Click).Handler;
		CloudLogoText.MouseLeftButtonUp += new SafeEventHandler<EventArgs>(CloudLogoTile_Click).Handler;
	}

	private static void CreateHyperlinkItem(TextBlock txt, string cmdCaption)
	{
		Hyperlink hyperlink = new Hyperlink();
		Run item = new Run(cmdCaption);
		hyperlink.Name = txt.Name;
		hyperlink.Inlines.Add(item);
		txt.Inlines.Add(hyperlink);
		hyperlink.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#66676a");
		hyperlink.NavigateUri = new Uri("http://www.armsoft.am");
		hyperlink.RequestNavigate += new SafeEventHandler<RequestNavigateEventArgs>(NavigateHyperLink).Handler;
	}

	private void SetTileSize(bool tilesCountOdd, ref Tile tile)
	{
		if (_setTileSize_firstCall & tilesCountOdd)
		{
			tile.Size = TileSize.Large;
			tile.Width = 298.0;
		}
		else
		{
			tile.Width = 298.0;
		}
		tile.Height = 86.0;
		if (_setTileSize_firstCall)
		{
			_setTileSize_firstCall = false;
		}
	}

	private static void NavigateHyperLink(object sender, RequestNavigateEventArgs e)
	{
		if (((Hyperlink)sender).Name == "CloudLogoText")
		{
			Process.Start(new ProcessStartInfo
			{
				FileName = "https://www.armsoft.am/?p=cloud_version_arevtur&c=hamakarger_hc_arevtur&lang=am",
				UseShellExecute = true
			});
		}
	}

	private void CompanyTile_Click(object sender, EventArgs e)
	{
		Settings.HostingEnvironment.ShowOptionsWindow();
	}

	private void GoodsTile_Click(object sender, EventArgs e)
	{
		if (Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.Goods).AllowView)
		{
			new Goods().Show();
		}
		else
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.MenuItemNotAllowedForUser, Settings.LoginService.UserName, ArmSoft.SME.Trade.Common.Resources.MaterialsAndServices), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK);
		}
	}

	private void GoodRemainsTile_Click(object sender, EventArgs e)
	{
		if (Settings.UserAccessService.GetDocumentPermission(DocumentTypeSE.MTIncome).AllowAdd)
		{
			Settings.DocumentService.ShowWindow(Settings.DocumentService.Create(DocumentTypeSE.MTIncome), WindowViewMode.NewCreate);
		}
		else
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.MenuItemNotAllowedForUser, Settings.LoginService.UserName, ArmSoft.SME.Trade.Common.Resources.DocMTIncomeRem), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK);
		}
	}

	private void YouTubeLearningVideosTile_Click(object sender, EventArgs e)
	{
		Process.Start(new ProcessStartInfo
		{
			FileName = "https://www.youtube.com/playlist?list=PL0knAScOnicZjJMflRlBfRW-k6dqzkZYK",
			UseShellExecute = true
		});
	}

	private void PartnersTile_Click(object sender, EventArgs e)
	{
		if (Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.Partners).AllowView)
		{
			new Partners().Show();
		}
		else
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.MenuItemNotAllowedForUser, Settings.LoginService.UserName, ArmSoft.SME.Common.Resources.Partners), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK);
		}
	}

	private void PartnersRemCorrectTile_Click(object sender, EventArgs e)
	{
		if (Settings.UserAccessService.GetDocumentPermission(DocumentTypeSE.PartnerRemGroupCorrect).AllowAdd)
		{
			Settings.DocumentService.ShowWindow(Settings.DocumentService.Create(DocumentTypeSE.PartnerRemGroupCorrect), WindowViewMode.NewCreate);
		}
		else
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.MenuItemNotAllowedForUser, Settings.LoginService.UserName, ArmSoft.SME.Trade.Common.Resources.PartnerRemGroupCorrection), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK);
		}
	}

	private void CashesTile_Click(object sender, EventArgs e)
	{
		if (Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.CashDesks).AllowView)
		{
			new Cashes().Show();
		}
		else
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.MenuItemNotAllowedForUser, Settings.LoginService.UserName, ArmSoft.SME.Common.Resources.Cashes), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK);
		}
	}

	private void BankAccountsTile_Click(object sender, EventArgs e)
	{
		if (Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.AccountsInBank).AllowView)
		{
			new AccountInBanks().Show();
		}
		else
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.MenuItemNotAllowedForUser, Settings.LoginService.UserName, ArmSoft.SME.Common.Resources.SettlementAccounts), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK);
		}
	}

	private void YouTubeLearningVideosLessonsTile_Click(object sender, EventArgs e)
	{
		Process.Start(new ProcessStartInfo
		{
			FileName = "https://www.youtube.com/watch?v=pMl9t6LvSDw&list=PL0knAScOnicb_059jAJZ3SVemlaMAlZgf",
			UseShellExecute = true
		});
	}

	private void AS_Trade_Helper_Tiles_MouseEnter(object sender, MouseEventArgs e)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		Tile tile = (Tile)sender;
		Brush applicationBrush = Settings.GetApplicationBrush();
		LinearGradientBrush linearGradientBrush = new LinearGradientBrush();
		linearGradientBrush.StartPoint = new Point(0.5, 0.0);
		linearGradientBrush.EndPoint = new Point(0.5, 1.0);
		linearGradientBrush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#F38020"), 0.0));
		linearGradientBrush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FFFFFF"), 1.3));
		if (tile.Name == "YouTubeLearningVideosLessonsTile")
		{
			YouTubeLearningVideosLessonsTileBorder.Background = linearGradientBrush;
			YouTubeLearningVideosLessonsTileTextBlock.Foreground = applicationBrush;
		}
		else if (tile.Name == "YouTubeLearningVideosTile")
		{
			YouTubeLearningVideosTileBorder.Background = linearGradientBrush;
			YouTubeLearningVideosTileTextBlock.Foreground = applicationBrush;
		}
		else if (tile.Name == "TeamViewerTile")
		{
			TeamViewerTileContentTextBlock.Foreground = applicationBrush;
			TeamViewerTileLabel.Foreground = applicationBrush;
		}
		else if (tile.Name == "AnyDeskTile")
		{
			AnyDeskTileContentTextBlock.Foreground = applicationBrush;
			AnyDeskTileTitleLabel.Foreground = applicationBrush;
		}
		else if (tile.Name == "TelegramTile")
		{
			TelegramTileTextBlock.Foreground = applicationBrush;
			TelegramTileLabel.Foreground = applicationBrush;
		}
	}

	private void AS_Trade_Helper_Tiles_MouseLeave(object sender, MouseEventArgs e)
	{
		Tile tile = (Tile)sender;
		SolidColorBrush background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#439467"));
		Brush foregroundBrush = Settings.GetForegroundBrush();
		if (tile.Name == "YouTubeLearningVideosLessonsTile")
		{
			YouTubeLearningVideosLessonsTileBorder.Background = background;
			YouTubeLearningVideosLessonsTileTextBlock.Foreground = foregroundBrush;
		}
		else if (tile.Name == "YouTubeLearningVideosTile")
		{
			YouTubeLearningVideosTileBorder.Background = background;
			YouTubeLearningVideosTileTextBlock.Foreground = foregroundBrush;
		}
		else if (tile.Name == "TeamViewerTile")
		{
			TeamViewerTileContentTextBlock.Foreground = foregroundBrush;
			TeamViewerTileLabel.Foreground = foregroundBrush;
		}
		else if (tile.Name == "AnyDeskTile")
		{
			AnyDeskTileContentTextBlock.Foreground = foregroundBrush;
			AnyDeskTileTitleLabel.Foreground = foregroundBrush;
		}
		else if (tile.Name == "TelegramTile")
		{
			TelegramTileTextBlock.Foreground = foregroundBrush;
			TelegramTileLabel.Foreground = foregroundBrush;
		}
	}

	private void AS_Trade_QuickStart_Tiles_MouseEnter(object sender, MouseEventArgs e)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		Tile tile = (Tile)sender;
		Brush applicationBrush = Settings.GetApplicationBrush();
		if (tile.Name == "CompanyTile")
		{
			CompanyTileTextBlock.Foreground = applicationBrush;
			CompanyTileImage.Source = ImageService.GetImage("building_tools_Trade_green_32.svg");
		}
		else if (tile.Name == "GoodsTile")
		{
			GoodsTileTextBlock.Foreground = applicationBrush;
			GoodsTileImage.Source = ImageService.GetImage("package_front_Trade_green_32.svg");
		}
		else if (tile.Name == "GoodRemainsTile")
		{
			GoodRemainsTileTextBlock.Foreground = applicationBrush;
			GoodRemainsTileImage.Source = ImageService.GetImage("package_front_Trade_green_32.svg");
		}
		else if (tile.Name == "PartnersTile")
		{
			PartnersTileTextBlock.Foreground = applicationBrush;
			PartnersTileImage.Source = ImageService.GetImage("fill_partners_info_Trade_green_32.svg");
		}
		else if (tile.Name == "PartnersRemCorrectTile")
		{
			PartnersRemCorrectTextBlock.Foreground = applicationBrush;
			PartnersRemCorrectTileImage.Source = ImageService.GetImage("fill_partners_rem_Trade_green_32.svg");
		}
		else if (tile.Name == "CashesTile")
		{
			CashesTileTextBlock.Foreground = applicationBrush;
			CashesTileImage.Source = ImageService.GetImage("fill_cashes_Trade_green_32.svg");
		}
		else if (tile.Name == "BankAccountsTile")
		{
			BankAccountsTileTextBlock.Foreground = applicationBrush;
			BankAccountsTileImage.Source = ImageService.GetImage("building_coins_Trade_green_32.svg");
		}
	}

	private void AS_Trade_QuickStart_Tiles_MouseLeave(object sender, MouseEventArgs e)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		Tile tile = (Tile)sender;
		Brush foregroundBrush = Settings.GetForegroundBrush();
		if (tile.Name == "CompanyTile")
		{
			CompanyTileTextBlock.Foreground = foregroundBrush;
			CompanyTileImage.Source = ImageService.GetImage("building_tools_Trade_gray_32.svg");
		}
		else if (tile.Name == "GoodsTile")
		{
			GoodsTileTextBlock.Foreground = foregroundBrush;
			GoodsTileImage.Source = ImageService.GetImage("package_front_Trade_gray_32.svg");
		}
		else if (tile.Name == "GoodRemainsTile")
		{
			GoodRemainsTileTextBlock.Foreground = foregroundBrush;
			GoodRemainsTileImage.Source = ImageService.GetImage("package_front_Trade_gray_32.svg");
		}
		else if (tile.Name == "PartnersTile")
		{
			PartnersTileTextBlock.Foreground = foregroundBrush;
			PartnersTileImage.Source = ImageService.GetImage("fill_partners_info_Trade_gray_32.svg");
		}
		else if (tile.Name == "PartnersRemCorrectTile")
		{
			PartnersRemCorrectTextBlock.Foreground = foregroundBrush;
			PartnersRemCorrectTileImage.Source = ImageService.GetImage("fill_partners_rem_Trade_gray_32.svg");
		}
		else if (tile.Name == "CashesTile")
		{
			CashesTileTextBlock.Foreground = foregroundBrush;
			CashesTileImage.Source = ImageService.GetImage("fill_cashes_Trade_gray_32.svg");
		}
		else if (tile.Name == "BankAccountsTile")
		{
			BankAccountsTileTextBlock.Foreground = foregroundBrush;
			BankAccountsTileImage.Source = ImageService.GetImage("building_coins_Trade_gray_32.svg");
		}
	}

	private void CloudLogoTile_Click(object sender, EventArgs e)
	{
		Process.Start(new ProcessStartInfo
		{
			FileName = "https://www.armsoft.am/?p=cloud_version_arevtur&c=hamakarger_hc_arevtur&lang=am",
			UseShellExecute = true
		});
	}

	private void AnyDeskTile_Click(object sender, EventArgs e)
	{
		try
		{
			string text = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "AnyDesk.exe";
			if (!File.Exists(text))
			{
				text = "https://download.anydesk.com/AnyDesk.exe";
			}
			Process.Start(new ProcessStartInfo
			{
				FileName = text,
				UseShellExecute = true
			});
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void TeamViewerTile_Click(object sender, EventArgs e)
	{
		try
		{
			string text = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "ASRemoteSupport.exe";
			if (!File.Exists(text))
			{
				text = "https://download.armsoft.am/TechSupport/AsRemoteSupport.exe";
			}
			Process.Start(new ProcessStartInfo
			{
				FileName = text,
				UseShellExecute = true
			});
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void TelegramTile_Click(object sender, EventArgs e)
	{
		try
		{
			Process.Start(new ProcessStartInfo
			{
				FileName = "https://t.me/+J-zFNtGhRX5lNmVi",
				UseShellExecute = true
			});
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void CollapseMobileAppSection_Clicked(object sender, RoutedEventArgs e)
	{
		IValueConverter converter = (IValueConverter)FindResource("ImageByNameConverter");
		Binding binding = new Binding
		{
			Converter = converter
		};
		if (MobileAppContentSectionScrollView.Visibility == Visibility.Collapsed)
		{
			MobileAppContentSectionScrollView.Visibility = Visibility.Visible;
			binding.ConverterParameter = "up_16.svg";
			RegistryHelper.StoreToRegistry("MobileAppSectionIsCollapsed", false);
		}
		else
		{
			MobileAppContentSectionScrollView.Visibility = Visibility.Collapsed;
			binding.ConverterParameter = "down_16.svg";
			RegistryHelper.StoreToRegistry("MobileAppSectionIsCollapsed", true);
		}
		BindingOperations.SetBinding((DependencyObject)(object)MobileAppSectionColapseImage, Image.SourceProperty, binding);
	}

	private void CollapseQuickStartSection_Clicked(object sender, RoutedEventArgs e)
	{
		IValueConverter converter = (IValueConverter)FindResource("ImageByNameConverter");
		Binding binding = new Binding
		{
			Converter = converter
		};
		if (AS_Trade_QuickStart_Header_ScrollViewer.Visibility == Visibility.Collapsed)
		{
			AS_Trade_QuickStart_Header_ScrollViewer.Visibility = Visibility.Visible;
			binding.ConverterParameter = "up_16.svg";
			RegistryHelper.StoreToRegistry("QuickStartSectionIsCollapsed", false);
		}
		else
		{
			AS_Trade_QuickStart_Header_ScrollViewer.Visibility = Visibility.Collapsed;
			binding.ConverterParameter = "down_16.svg";
			RegistryHelper.StoreToRegistry("QuickStartSectionIsCollapsed", true);
		}
		BindingOperations.SetBinding((DependencyObject)(object)QuickStartSectionCollapseImage, Image.SourceProperty, binding);
	}

	private void CollapseHelperSection_Clicked(object sender, RoutedEventArgs e)
	{
		IValueConverter converter = (IValueConverter)FindResource("ImageByNameConverter");
		Binding binding = new Binding
		{
			Converter = converter
		};
		if (AS_Trade_Helper_Header_ScrollViewer.Visibility == Visibility.Collapsed)
		{
			AS_Trade_Helper_Header_ScrollViewer.Visibility = Visibility.Visible;
			binding.ConverterParameter = "up_16.svg";
			RegistryHelper.StoreToRegistry("WorkingHelperSectionIsCollapsed", false);
		}
		else
		{
			AS_Trade_Helper_Header_ScrollViewer.Visibility = Visibility.Collapsed;
			binding.ConverterParameter = "down_16.svg";
			RegistryHelper.StoreToRegistry("WorkingHelperSectionIsCollapsed", true);
		}
		BindingOperations.SetBinding((DependencyObject)(object)WorkingHelperSectionCollapseImage, Image.SourceProperty, binding);
	}

	private void CollapseRemoteSupportSection_Clicked(object sender, RoutedEventArgs e)
	{
		IValueConverter converter = (IValueConverter)FindResource("ImageByNameConverter");
		Binding binding = new Binding
		{
			Converter = converter
		};
		if (AS_Trade_Remote_Support_ScrollViewer.Visibility == Visibility.Collapsed)
		{
			AS_Trade_Remote_Support_ScrollViewer.Visibility = Visibility.Visible;
			binding.ConverterParameter = "up_16.svg";
			RegistryHelper.StoreToRegistry("RemoteSupportSectionIsCollapsed", false);
		}
		else
		{
			AS_Trade_Remote_Support_ScrollViewer.Visibility = Visibility.Collapsed;
			binding.ConverterParameter = "down_16.svg";
			RegistryHelper.StoreToRegistry("RemoteSupportSectionIsCollapsed", true);
		}
		BindingOperations.SetBinding((DependencyObject)(object)RemoteSupportSectionColapseImage, Image.SourceProperty, binding);
	}
}
