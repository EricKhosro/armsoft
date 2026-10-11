using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.DBIntegrity;
using ArmSoft.SME.Core.UI.DataViews.Directories;
using ArmSoft.SME.Core.UI.DataViews.Reports;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.Navigation;
using ArmSoft.SME.Trade.Client.Enums;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;
using ArmSoft.SME.Trade.UI.DataView;
using ArmSoft.SME.Trade.UI.DataView.Offline;
using ArmSoft.SME.Trade.UI.DataView.Reports;
using ArmSoft.SME.Trade.UI.DataView.Reports.Analysis;
using ArmSoft.SME.Trade.UI.Directory;
using ArmSoft.SME.Trade.UI.Documents.Goods;
using ArmSoft.SME.Trade.UI.Helpers;
using ArmSoft.SME.Trade.UI.OtherWindows;
using ArmSoft.SME.Trade.UI.ReportViewers;
using ArmSoft.SME.Trade.UI.ReportViewers.Cash_Bank;
using ArmSoft.SME.Trade.UI.ReportViewers.Goods;
using ArmSoft.SME.Trade.UI.ReportViewers.Partner;
using ArmSoft.SME.Trade.UI.SmartEnums;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.NavBar;

namespace ArmSoft.SME.Trade.UI.Navigation;

public class NavigationBarsController : NavigationBarsControllerBase
{
	protected override object GetUserAccessDefinition(short accessId)
	{
		return SmartEnum<ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE, short>.FromValue(accessId);
	}

	public override void Fill(NavBarControl navBar, UserAccessManagerBase mUserAccessManager)
	{
		navBar.Groups.Clear();
		foreach (KeyValuePair<short, AsNavigationTreeNode> item in mUserAccessManager.AccessTree)
		{
			if (!Settings.IsOffline)
			{
				AddSubsystem(navBar, item.Value, item.Key);
			}
			else if (item.Key == 7)
			{
				AddSubsystem(navBar, item.Value, item.Key);
			}
		}
		if (!Settings.ParametersService.ShowOnlyUserAWP() && !Settings.IsOffline)
		{
			return;
		}
		foreach (NavBarGroup group in NavigatorPanel.NavPanel.NavBarControl.Groups)
		{
			if (Settings.IsOffline)
			{
				group.IsVisible = group.Name == SubSystemTypes.PointOfSaleSS.ToString();
			}
			else
			{
				group.IsVisible = group.Name == "UserAPW" || group.Name == SubSystemTypes.PointOfSaleSS.ToString();
			}
		}
		((NavigationPaneView)NavigatorPanel.NavPanel.NavBarControl.View).IsOverflowPanelVisible = false;
	}

	public override bool CheckUserAccess(short command)
	{
		IEnumerator enumerator = Enum.GetValues(typeof(SubSystemTypes)).GetEnumerator();
		try
		{
			if (enumerator.MoveNext())
			{
				_ = (SubSystemTypes)enumerator.Current;
				return true;
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
		return false;
	}

	private void AddSubsystem(NavBarControl navBar, AsNavigationTreeNode rootNode, short subSystem)
	{
		NavBarGroup group = new NavBarGroup();
		TreeViewControl treeViewControl = AddGroup(navBar, ref group, ((SubSystemTypes)rootNode.Command/*cast due to constrained. prefix*/).ToString(), rootNode.Header, rootNode.Image);
		NavigationBarsControllerBase.AddSubTree(treeViewControl, rootNode.ChildrenList, subSystem);
		if (treeViewControl.VisibleItems.Count == 0)
		{
			navBar.Groups.Remove(group);
		}
	}

	public override void ExecuteCommand(short command, short? subsystem = null)
	{
		try
		{
			dynamic userAccessDefinition = GetUserAccessDefinition(command);
			CallsRecordingHelper.RecordCallStep(StatisticTypeSE.Navigator, userAccessDefinition.Name.ToString());
			short num = command;
			if (num == ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.ChangeActualnessDate.Value)
			{
				ActualnessDateHelpers.ChangeActualnessDate();
				return;
			}
			if (num == ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.ImportDataFromASAccountantWindow.Value)
			{
				try
				{
					if (ImportDataFromASAccountantWindow.ImportDataFromASAccountant(out var repHeadersFooters, out var repRows) && !string.IsNullOrWhiteSpace(repHeadersFooters[0]))
					{
						if (repRows.Count == 0)
						{
							ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.WgNoInform, ArmSoft.SME.Trade.Common.Resources.ImportDataFromASAccountant, MessageBoxButton.OK, MessageBoxImage.Exclamation);
						}
						else
						{
							new ImportDataFromASAccountantReportViewer(repHeadersFooters, repRows).Show();
						}
					}
					return;
				}
				catch (Exception ex)
				{
					Settings.ErrorHandler(ex);
					return;
				}
			}
			if (num == ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepTAccount.Value)
			{
				if (!Settings.UserAccessService.GetProviderPermission(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.MatRepTAccount.Value).AllowView)
				{
					throw new ASException(string.Format(ArmSoft.SME.Common.Resources.DontHavePermissionToExecuteData, ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.MatRepTAccount.Caption), 20008);
				}
				MTAccountReportViewer mTAccountReportViewer = new MTAccountReportViewer();
				if (subsystem.HasValue)
				{
					mTAccountReportViewer.AllowOperation = Settings.UserAccessService.GetNavigatorNodePermission(subsystem.Value, command).AllowOperation;
				}
				mTAccountReportViewer.Show(useParamValues: false, openAsChild: false);
				return;
			}
			if (num == ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartRepCard.Value)
			{
				if (!Settings.UserAccessService.GetProviderPermission(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.PartRepCard.Value).AllowView)
				{
					throw new ASException(string.Format(ArmSoft.SME.Common.Resources.DontHavePermissionToExecuteData, ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.PartRepCard.Caption), 20008);
				}
				PartnerTAccountReportViewer partnerTAccountReportViewer = new PartnerTAccountReportViewer();
				if (subsystem.HasValue)
				{
					partnerTAccountReportViewer.AllowOperation = Settings.UserAccessService.GetNavigatorNodePermission(subsystem.Value, command).AllowOperation;
				}
				partnerTAccountReportViewer.Show(useParamValues: false, openAsChild: false);
				return;
			}
			if (num == ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.BonusTAccount.Value)
			{
				BonusTAccountReportViewer bonusTAccountReportViewer = new BonusTAccountReportViewer();
				if (subsystem.HasValue)
				{
					bonusTAccountReportViewer.AllowOperation = Settings.UserAccessService.GetNavigatorNodePermission(subsystem.Value, command).AllowOperation;
				}
				bonusTAccountReportViewer.Show(useParamValues: false, openAsChild: false);
				return;
			}
			if (num == ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.CashDeskTAccount.Value)
			{
				CashTAccountReportViewer cashTAccountReportViewer = new CashTAccountReportViewer();
				if (subsystem.HasValue)
				{
					cashTAccountReportViewer.AllowOperation = Settings.UserAccessService.GetNavigatorNodePermission(subsystem.Value, command).AllowOperation;
				}
				cashTAccountReportViewer.Show(useParamValues: false, openAsChild: false);
				return;
			}
			if (num == ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.BankTAccount.Value)
			{
				BankTAccountReportViewer bankTAccountReportViewer = new BankTAccountReportViewer();
				if (subsystem.HasValue)
				{
					bankTAccountReportViewer.AllowOperation = Settings.UserAccessService.GetNavigatorNodePermission(subsystem.Value, command).AllowOperation;
				}
				bankTAccountReportViewer.Show(useParamValues: false, openAsChild: false);
				return;
			}
			if (num == ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.ECRReport.Value)
			{
				ECRReportViewer eCRReportViewer = new ECRReportViewer();
				if (subsystem.HasValue)
				{
					eCRReportViewer.AllowOperation = Settings.UserAccessService.GetNavigatorNodePermission(subsystem.Value, command).AllowOperation;
				}
				eCRReportViewer.Show(useParamValues: false, openAsChild: false);
				return;
			}
			if (num == ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.CashierShiftReport.Value)
			{
				CashierShiftReportViewer cashierShiftReportViewer = new CashierShiftReportViewer();
				if (subsystem.HasValue)
				{
					cashierShiftReportViewer.AllowOperation = Settings.UserAccessService.GetNavigatorNodePermission(subsystem.Value, command).AllowOperation;
				}
				cashierShiftReportViewer.Show(useParamValues: false, openAsChild: false);
				return;
			}
			if (num == ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.LocalDBDocumnets.Value)
			{
				new LocalDBDocumentsDataView().Show(useParamValues: false, openAsChild: false);
				return;
			}
			if (num == ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.ExportForElScalesAndTerminals.Value)
			{
				new GroupElScaleAndTerminalExportWindow(null, DocumentHelpers.GetSourceList()).ShowDialog();
				return;
			}
			if (num == ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.SalesAnalysisBestSellersReport.Value)
			{
				BestSellersDiagram bestSellersDiagram = new BestSellersDiagram();
				if (subsystem.HasValue)
				{
					bestSellersDiagram.AllowOperation = Settings.UserAccessService.GetNavigatorNodePermission(subsystem.Value, command).AllowOperation;
				}
				bestSellersDiagram.Show(useParamValues: false, openAsChild: false);
				return;
			}
			if (num == ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.CashBankAnalysisReport.Value)
			{
				CashBankDiagram cashBankDiagram = new CashBankDiagram();
				if (subsystem.HasValue)
				{
					cashBankDiagram.AllowOperation = Settings.UserAccessService.GetNavigatorNodePermission(subsystem.Value, command).AllowOperation;
				}
				cashBankDiagram.Show(useParamValues: false, openAsChild: false);
				return;
			}
			if (num == ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.SalesAnalysisDiagram.Value)
			{
				SalesAnalysisDiagram salesAnalysisDiagram = new SalesAnalysisDiagram();
				if (subsystem.HasValue)
				{
					salesAnalysisDiagram.AllowOperation = Settings.UserAccessService.GetNavigatorNodePermission(subsystem.Value, command).AllowOperation;
				}
				salesAnalysisDiagram.Show(useParamValues: false, openAsChild: false);
				return;
			}
			if (num == ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.AnalysisDebtorsCreditorsRems.Value)
			{
				DebitorsCreditorsDiagram debitorsCreditorsDiagram = new DebitorsCreditorsDiagram();
				if (subsystem.HasValue)
				{
					debitorsCreditorsDiagram.AllowOperation = Settings.UserAccessService.GetNavigatorNodePermission(subsystem.Value, command).AllowOperation;
				}
				debitorsCreditorsDiagram.Show(useParamValues: false, openAsChild: false);
				return;
			}
			if (num == ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.AnalysisDashBoard.Value)
			{
				Mouse.OverrideCursor = Cursors.Wait;
				AnalysisDashBoardWindow analysisDashBoardWindow = new AnalysisDashBoardWindow();
				analysisDashBoardWindow.InitRefresh();
				Settings.HostingEnvironment.AddPanelToDocumentGroup(analysisDashBoardWindow);
				analysisDashBoardWindow.Caption = ArmSoft.SME.Common.Resources.DashBoardWnd1;
				FocusManager.SetFocusedElement((DependencyObject)(object)analysisDashBoardWindow, analysisDashBoardWindow.BestSellersPanel);
				Settings.HostingEnvironment.LayoutManager.Activate(analysisDashBoardWindow);
				Mouse.OverrideCursor = null;
				return;
			}
			if (num == ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.TaxServiceTaxInvoicesDraft.Value)
			{
				if (Settings.TaxServiceHelper.EnsureTaxServiceAuthenticationCompleted())
				{
					DraftTaxInvoices draftTaxInvoices = new DraftTaxInvoices();
					if (subsystem.HasValue)
					{
						draftTaxInvoices.AllowOperation = Settings.UserAccessService.GetNavigatorNodePermission(subsystem.Value, command).AllowOperation;
					}
					draftTaxInvoices.Show(useParamValues: false, openAsChild: false);
				}
				return;
			}
			if (num == ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.TaxServiceAccDocsDraft.Value)
			{
				if (Settings.TaxServiceHelper.EnsureTaxServiceAuthenticationCompleted())
				{
					DraftAccDocs draftAccDocs = new DraftAccDocs();
					if (subsystem.HasValue)
					{
						draftAccDocs.AllowOperation = Settings.UserAccessService.GetNavigatorNodePermission(subsystem.Value, command).AllowOperation;
					}
					draftAccDocs.Show(useParamValues: false, openAsChild: false);
				}
				return;
			}
			if (num == ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.TaxServiceTaxInvoicesReceived.Value)
			{
				if (Settings.TaxServiceHelper.EnsureTaxServiceAuthenticationCompleted())
				{
					TradeReceivedTaxInvoices tradeReceivedTaxInvoices = new TradeReceivedTaxInvoices();
					if (subsystem.HasValue)
					{
						tradeReceivedTaxInvoices.AllowOperation = Settings.UserAccessService.GetNavigatorNodePermission(subsystem.Value, command).AllowOperation;
					}
					tradeReceivedTaxInvoices.Show(useParamValues: false, openAsChild: false);
				}
				return;
			}
			if (num == ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.TaxServiceAccDocsReceived.Value)
			{
				if (Settings.TaxServiceHelper.EnsureTaxServiceAuthenticationCompleted())
				{
					TradeReceivedAccDocs tradeReceivedAccDocs = new TradeReceivedAccDocs();
					if (subsystem.HasValue)
					{
						tradeReceivedAccDocs.AllowOperation = Settings.UserAccessService.GetNavigatorNodePermission(subsystem.Value, command).AllowOperation;
					}
					tradeReceivedAccDocs.Show(useParamValues: false, openAsChild: false);
				}
				return;
			}
			if (num == ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.TaxServiceTaxInvoicesSubmitted.Value)
			{
				if (Settings.TaxServiceHelper.EnsureTaxServiceAuthenticationCompleted())
				{
					SubmittedTaxInvoices submittedTaxInvoices = new SubmittedTaxInvoices();
					if (subsystem.HasValue)
					{
						submittedTaxInvoices.AllowOperation = Settings.UserAccessService.GetNavigatorNodePermission(subsystem.Value, command).AllowOperation;
					}
					submittedTaxInvoices.Show(useParamValues: false, openAsChild: false);
				}
				return;
			}
			if (num == ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.TaxServiceAccDocsSubmitted.Value)
			{
				if (Settings.TaxServiceHelper.EnsureTaxServiceAuthenticationCompleted())
				{
					SubmittedAccDocs submittedAccDocs = new SubmittedAccDocs();
					if (subsystem.HasValue)
					{
						submittedAccDocs.AllowOperation = Settings.UserAccessService.GetNavigatorNodePermission(subsystem.Value, command).AllowOperation;
					}
					submittedAccDocs.Show(useParamValues: false, openAsChild: false);
				}
				return;
			}
			if (num == ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.AssignTaxInvNums.Value)
			{
				TradeTaxServiceHelper.AssignTaxInvNums();
				return;
			}
			if (num == ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.AssignAccDocInvNums.Value)
			{
				TradeTaxServiceHelper.AssignTaxInvNums(isAccDoc: true);
				return;
			}
			if (num == ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.ImportTaxInvoice.Value)
			{
				TradeTaxServiceHelper.ImportReceivedTaxDocuments();
				return;
			}
			short num2 = num;
			if (num2 == (short)ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.Sale || (subsystem.HasValue && subsystem.Value == 7 && (num2 == (short)ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MTBill || num2 == (short)ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MTBillReturn || num2 == (short)ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.ReturnFromCustomer)))
			{
				ShowSaleWindow();
			}
			else if (num == ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartnersGroupRevaluation.Value)
			{
				ShowPartnersRemains(ArmSoft.SME.Trade.Common.Resources.RevPartnerFromRemains);
			}
			else if (num == ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MTRevaluation.Value)
			{
				ShowGoodsRemains(ArmSoft.SME.Trade.Common.Resources.RevMaterialFromRemains);
			}
			else if (num == ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PrintECRReport.Value)
			{
				new ECRPrintReportWindow().ShowDialog();
			}
			else if (num == ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.SyncECR.Value)
			{
				new ECRSyncWindow().ShowDialog();
			}
			else if (num == ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.OpenCashierShift.Value)
			{
				CashierShiftHelper.OpenCashierShift();
			}
			else if (num == ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.CloseCashierShift.Value)
			{
				CashierShiftHelper.CloseCashierShift();
			}
			else if (num == ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.ECRConfiguration.Value)
			{
				OpenECRConfiguration();
			}
			else if (num == ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.OnlineAccountStatement.Value)
			{
				BankAccountStatementReportViewer bankAccountStatementReportViewer = new BankAccountStatementReportViewer();
				if (subsystem.HasValue)
				{
					bankAccountStatementReportViewer.AllowOperation = Settings.UserAccessService.GetNavigatorNodePermission(subsystem.Value, command).AllowOperation;
				}
				bankAccountStatementReportViewer.Show(useParamValues: false, openAsChild: false);
			}
			else if (num == ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.DebugFinalizeTracer.Value)
			{
				FinalizeTracer.ShowWindow();
			}
			else if (num == ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.DebugNotificationTest.Value)
			{
				Settings.HostingEnvironment.TestNotification();
			}
			else if (num == ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.ImportBanksFromB2B.Value)
			{
				Banks.ImportBanksFromB2B();
			}
			else if (num == (short)ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.CBAImportExchangeRates)
			{
				new ImportExchangeRates();
			}
			else if (num == (short)ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.DebugAppTokenViewer)
			{
				AppTokenViewerWindow.ShowWindow();
			}
			else if (num == ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.CheckIntegrity.Value)
			{
				new TradeCheckIntegrityDialogWindow().ShowDialog();
			}
			else if (num == ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.Archive.Value)
			{
				new DBArchivationDialogWindow("TradeArchivation").ShowDialog();
			}
			else if (num == ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.DebugCreateStoredProcedures.Value)
			{
				if (ArmSoft.SME.Core.UI.MessageBox.Show("Are you sure to create stored procedures ?", Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
				{
					Mouse.OverrideCursor = Cursors.Wait;
					Settings.CreateApiClient().CommonCoreRoutes.CreateStoredProcedures();
					Mouse.OverrideCursor = null;
					ArmSoft.SME.Core.UI.MessageBox.Show("Stored procedures are created !");
				}
			}
			else
			{
				base.ExecuteCommand(command, subsystem);
			}
		}
		catch (Exception ex2)
		{
			Settings.ErrorHandler(ex2);
		}
	}

	private static void ShowPartnersRemains(string message)
	{
		PartnersRemains partnersRemains = new PartnersRemains();
		ArmSoft.SME.Core.UI.MessageBox.Show(message, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk, MessageBoxResult.OK, Settings.HostingEnvironment, -1, partnersRemains.HelpId);
		partnersRemains.ShowForReval = true;
		partnersRemains.ReportCurrTypes = "3";
		partnersRemains.DetailsByContracts = true;
		partnersRemains.Date = Settings.HostingEnvironment.CurrentDate;
		partnersRemains.Show(useParamValues: true, openAsChild: false);
	}

	private static void ShowGoodsRemains(string message)
	{
		GoodsRemains goodsRemains = new GoodsRemains();
		ArmSoft.SME.Core.UI.MessageBox.Show(message, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk, MessageBoxResult.OK, Settings.HostingEnvironment, -1, goodsRemains.HelpId);
		goodsRemains.ShowForReval = true;
		goodsRemains.DetailByStorage = true;
		goodsRemains.ShowByDifferentParties = true;
		goodsRemains.RemDate = Settings.HostingEnvironment.CurrentDate;
		goodsRemains.Show(useParamValues: true, openAsChild: false);
	}

	private static void ShowSaleWindow()
	{
		try
		{
			bool flag = CashierShiftHelper.ExistsOpenedCashierShift() || Settings.IsOffline || (CashierShiftHelper.OpenCashierShift() ? true : false);
			if (Settings.IsOffline && SaleWindowHelper.IsDefaultStorageRetail())
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.StorageMustBeWholesale);
			}
			if (flag)
			{
				if (Settings.ParametersService.AutoSave() && (TradeDocument)Settings.LocalDBDocumentService.GetLastAutoSavedDocument() != null)
				{
					ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.HappendEmergencyStopOfApplication + "\n" + ArmSoft.SME.Trade.Common.Resources.FoundAutoSavedSale, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
				}
				CashierDisplayInfo instance = CashierDisplayInfo.GetInstance();
				if (instance.TouchMode == POSScreenModesSE.SampleScreen.Value || instance.TouchMode == POSScreenModesSE.SampleScreenWithFavorites.Value)
				{
					new SamplePOSSaleWindow(Settings.HostingEnvironment, instance.TouchMode, instance.FullScreen, instance.Width, instance.Height).ShowDialog();
				}
				else
				{
					new SaleWindow(Settings.HostingEnvironment, instance.TouchMode, instance.FullScreen, instance.Width, instance.Height).ShowDialog();
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	public static void OpenECRConfiguration()
	{
		string text = Settings.ParametersService.DefaultCash();
		if (string.IsNullOrWhiteSpace(text))
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.FieldIsEmpty, ArmSoft.SME.Common.Resources.Cash), Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}
		else
		{
			new CashDialogWindow(Settings.DirectoryService.Load<Cash>(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Cash.Value, text, withoutAccessCheck: true), DialogWindowMode.EditMode, null, fromPOS: true).ShowDialog();
		}
	}
}
