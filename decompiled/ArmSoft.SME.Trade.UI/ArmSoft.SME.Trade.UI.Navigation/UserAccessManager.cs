using System.Collections.Generic;
using System.Xml;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Navigation;
using ArmSoft.SME.Core.UI.SmartEnums;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.Navigation;

public class UserAccessManager : UserAccessManagerBase
{
	private readonly bool _useBonus;

	private readonly bool _useDiscountServer;

	private readonly bool _enableSubstituteItems;

	private readonly bool _useDiscount;

	private readonly bool _retailAccounting;

	private readonly bool _mtMoveWithConfirmation;

	private readonly bool _useMarking;

	public UserAccessManager(bool checkAccess = true)
		: base(checkAccess)
	{
		_useBonus = Settings.ParametersService.UseBonus();
		_useDiscountServer = _useBonus && Settings.ParametersService.ConnectTob2barmsoftDiscountServer();
		_enableSubstituteItems = Settings.ParametersService.EnableSubstituteItems();
		_useDiscount = Settings.ParametersService.DiscountUse();
		_retailAccounting = Settings.ParametersService.RetailAccounting();
		_mtMoveWithConfirmation = Settings.ParametersService.MTMoveWithConfirmation();
		_useMarking = Settings.ParametersService.UseMarking();
		BuildAccessTree();
	}

	private void BuildAccessTree()
	{
		SubSystemRoots = new Dictionary<short, AsNavigationTreeNode>();
		BuildPurchaseTree();
		BuildSaleTree();
		BuildPointOfSaleTree();
		BuildStorageTree();
		BuildCashBankTree();
		BuildAnalysisReportsTree();
		BuildTaxServiceTree();
		if (Settings.LoginService.IsAdmin)
		{
			FillAdminToolsTree();
		}
		if (!Settings.IsOffline)
		{
			string text = Settings.ParametersService.UserAWPNavigatorName();
			if (!string.IsNullOrWhiteSpace(text))
			{
				BuildUserAWP(text);
			}
		}
	}

	public override void ExportAPWsMetadata(XmlWriter writer, Dictionary<string, string> stringResourcesDictionary)
	{
	}

	public override string GetCommandName(short command)
	{
		return SmartEnum<UserAccessIdSE, short>.FromValue(command).Name;
	}

	public override short GetCommandByName(string name)
	{
		return SmartEnum<UserAccessIdSE, short>.FromName(name).Value;
	}

	private void BuildPurchaseTree()
	{
		short num = 1;
		if (IsSubsystemAccessible(num, out var checkNodeAccess))
		{
			AsNavigationTreeNode asNavigationTreeNode = new AsNavigationTreeNode(num, ArmSoft.SME.Trade.Common.Resources.Purchases, "Purchase_24.svg", isViewChangeable: false, isOperChangeable: false);
			AsNavigationTreeNode asNavigationTreeNode2 = new AsNavigationTreeNode(UserAccessIdSE.PurchaseDocuments.Value, ArmSoft.SME.Common.Resources.Documents, "doc_new_16.svg", isViewChangeable: false, isOperChangeable: false, isExpanded: true, "", AWPCommandType.SystemCall, "", isInFirstColumnInHomePage: true);
			asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MTIncome.Value, ArmSoft.SME.Common.Resources.DocMTIncome, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MTIncomeImport.Value, ArmSoft.SME.Trade.Common.Resources.DocMTIncomeImport, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MTIncomeRetail.Value, ArmSoft.SME.Trade.Common.Resources.DocMTIncomeRetail, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MTOverheadExpenses.Value, ArmSoft.SME.Common.Resources.DocMTOverheadExpenses, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.ReturnToSupplier.Value, ArmSoft.SME.Trade.Common.Resources.DocMTReturnToSupplier, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MTPurchaseOrder.Value, ArmSoft.SME.Common.Resources.DocMTPurchaseOrder, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			AsNavigationTreeNode asNavigationTreeNode3 = new AsNavigationTreeNode(UserAccessIdSE.PurchaseSubDocuments.Value, ArmSoft.SME.Trade.Common.Resources.SubDocuments, "doc_new_16.svg", isViewChangeable: false, isOperChangeable: false, isExpanded: false);
			asNavigationTreeNode3.AddChild(new AsNavigationTreeNode(UserAccessIdSE.CashOut.Value, ArmSoft.SME.Common.Resources.CashOutputOrder, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode3.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PayOrd.Value, ArmSoft.SME.Common.Resources.PaymentOrder, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode3.AddChild(new AsNavigationTreeNode(UserAccessIdSE.BankOut.Value, ArmSoft.SME.Trade.Common.Resources.BankOut, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode3.AddChild(new AsNavigationTreeNode(UserAccessIdSE.BoughtServices.Value, ArmSoft.SME.Common.Resources.BoughtServices, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode3.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PartRemCorrect.Value, ArmSoft.SME.Trade.Common.Resources.PartnerRemCorrection, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode3.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PartRemGroupCorrect.Value, ArmSoft.SME.Trade.Common.Resources.PartnerRemGroupCorrection, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode3.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PartRemMove.Value, ArmSoft.SME.Trade.Common.Resources.PartnerRemMove, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode3.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PartnersGroupRevaluation.Value, ArmSoft.SME.Trade.Common.Resources.PartnersGroupRevaluation, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			if (_useBonus)
			{
				asNavigationTreeNode3.AddChild(new AsNavigationTreeNode(UserAccessIdSE.BonusGroupCorrect.Value, ArmSoft.SME.Trade.Common.Resources.BonusGroupCorrect, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			}
			asNavigationTreeNode3.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MTIncomeRem.Value, ArmSoft.SME.Trade.Common.Resources.DocMTIncomeRem, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode3.AddChild(new AsNavigationTreeNode(UserAccessIdSE.ReconciliationStatementDoc.Value, ArmSoft.SME.Common.Resources.ReconciliationStatement, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode2.AddChild(asNavigationTreeNode3, num, checkNodeAccess, addIfHasChild: true);
			asNavigationTreeNode.AddChild(asNavigationTreeNode2, num, checkNodeAccess, addIfHasChild: true);
			AsNavigationTreeNode asNavigationTreeNode4 = new AsNavigationTreeNode(UserAccessIdSE.PurchaseOperations.Value, ArmSoft.SME.Common.Resources.Operations, "action_16.svg", isViewChangeable: false, isOperChangeable: false, isExpanded: true, "", AWPCommandType.SystemCall, "", isInFirstColumnInHomePage: true);
			asNavigationTreeNode4.AddChild(new AsNavigationTreeNode(UserAccessIdSE.ImportTaxInvoice.Value, ArmSoft.SME.Common.Resources.ReceivedTaxInvImport, "action_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode.AddChild(asNavigationTreeNode4, num, checkNodeAccess, addIfHasChild: true);
			AsNavigationTreeNode asNavigationTreeNode5 = new AsNavigationTreeNode(UserAccessIdSE.PurchaseJournals.Value, ArmSoft.SME.Common.Resources.Journals, "journal_16.svg", isViewChangeable: false, isOperChangeable: false, isExpanded: true, "", AWPCommandType.SystemCall, "", isInFirstColumnInHomePage: true);
			asNavigationTreeNode5.AddChild(new AsNavigationTreeNode(UserAccessIdSE.DocumentsJournal, ArmSoft.SME.Trade.Common.Resources.AllDocuments + " (Ctrl+J)", "journal_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode5.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PurchaseDocumentsJournal.Value, ArmSoft.SME.Trade.Common.Resources.PurchaseDocumentsJournal, "journal_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode5.AddChild(new AsNavigationTreeNode(UserAccessIdSE.ReceivedInvoicesJournal.Value, ArmSoft.SME.Trade.Common.Resources.ReceivedTaxInvoicesJournal, "journal_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode5.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PurchaseOrdersDocsJournal.Value, ArmSoft.SME.Common.Resources.PurchaseOrderDocsJournal, "journal_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode5.AddChild(new AsNavigationTreeNode(UserAccessIdSE.ReconciliationStatementsJournal.Value, ArmSoft.SME.Trade.Common.Resources.ReconciliationStatementsJournal, "journal_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode.AddChild(asNavigationTreeNode5, num, checkNodeAccess, addIfHasChild: true);
			AsNavigationTreeNode asNavigationTreeNode6 = new AsNavigationTreeNode(UserAccessIdSE.PurchaseReports.Value, ArmSoft.SME.Common.Resources.Reports, "report_16.svg", isViewChangeable: false, isOperChangeable: false);
			asNavigationTreeNode6.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MatRepAvailability.Value, ArmSoft.SME.Trade.Common.Resources.MatRepAvail, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode6.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MatRepAvailabilityTreeView.Value, ArmSoft.SME.Trade.Common.Resources.MatRepAvailTreeView, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode6.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MatRepTurnOver.Value, ArmSoft.SME.Trade.Common.Resources.MatRepTurn, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode6.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MatRepOperationsJournal.Value, ArmSoft.SME.Trade.Common.Resources.MatRepJournal, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode6.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MatRepTAccount, ArmSoft.SME.Trade.Common.Resources.TAccount + " (Ctrl+L)", "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode6.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MTPriceList.Value, ArmSoft.SME.Common.Resources.PriceList, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode6.AddChild(new AsNavigationTreeNode(UserAccessIdSE.GoodsCostCalculation.Value, ArmSoft.SME.Trade.Common.Resources.GoodsCostCalculation, "report_16.svg"), num, checkNodeAccess);
			AsNavigationTreeNode asNavigationTreeNode7 = new AsNavigationTreeNode(UserAccessIdSE.PurchasePartnersReports.Value, ArmSoft.SME.Common.Resources.PartnersReports, "report_16.svg", isViewChangeable: false, isOperChangeable: false, isExpanded: false);
			asNavigationTreeNode7.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PartRemains.Value, ArmSoft.SME.Trade.Common.Resources.PartnerRemains, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode7.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PartTurnOver.Value, ArmSoft.SME.Trade.Common.Resources.PartnerTurn, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode7.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PartOpJournal.Value, ArmSoft.SME.Trade.Common.Resources.PartnerOpJournal, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode7.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PartRepCard, ArmSoft.SME.Trade.Common.Resources.PartnerCard + " (Ctrl+B)", "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode6.AddChild(asNavigationTreeNode7, num, checkNodeAccess, addIfHasChild: true);
			if (_useBonus)
			{
				AsNavigationTreeNode asNavigationTreeNode8 = new AsNavigationTreeNode(UserAccessIdSE.PurchaseBonusReports.Value, ArmSoft.SME.Trade.Common.Resources.BonusReports, "report_16.svg", isViewChangeable: false, isOperChangeable: false, isExpanded: false);
				asNavigationTreeNode8.AddChild(new AsNavigationTreeNode(UserAccessIdSE.BonusRemains.Value, ArmSoft.SME.Trade.Common.Resources.BonusRemains, "report_16.svg"), num, checkNodeAccess);
				asNavigationTreeNode8.AddChild(new AsNavigationTreeNode(UserAccessIdSE.BonusTurnOver.Value, ArmSoft.SME.Trade.Common.Resources.BonusTurnOver, "report_16.svg"), num, checkNodeAccess);
				asNavigationTreeNode8.AddChild(new AsNavigationTreeNode(UserAccessIdSE.BonusOpJournal.Value, ArmSoft.SME.Trade.Common.Resources.BonusOpJournal, "report_16.svg"), num, checkNodeAccess);
				asNavigationTreeNode8.AddChild(new AsNavigationTreeNode(UserAccessIdSE.BonusTAccount.Value, ArmSoft.SME.Trade.Common.Resources.BonusTAccount, "report_16.svg"), num, checkNodeAccess);
				asNavigationTreeNode6.AddChild(asNavigationTreeNode8, num, checkNodeAccess, addIfHasChild: true);
			}
			AsNavigationTreeNode asNavigationTreeNode9 = new AsNavigationTreeNode(UserAccessIdSE.PurchaseCashBankReports.Value, ArmSoft.SME.Trade.Common.Resources.CashBankReports, "report_16.svg", isViewChangeable: false, isOperChangeable: false, isExpanded: false);
			asNavigationTreeNode9.AddChild(new AsNavigationTreeNode(UserAccessIdSE.CashRemains.Value, ArmSoft.SME.Trade.Common.Resources.KassaRemains, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode9.AddChild(new AsNavigationTreeNode(UserAccessIdSE.CashTurnOver.Value, ArmSoft.SME.Trade.Common.Resources.KassaTurn, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode9.AddChild(new AsNavigationTreeNode(UserAccessIdSE.CashOpJournal.Value, ArmSoft.SME.Trade.Common.Resources.KassaOpJournal, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode9.AddChild(new AsNavigationTreeNode(UserAccessIdSE.CashDeskTAccount.Value, ArmSoft.SME.Trade.Common.Resources.CashDeskTAccount, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode9.AddChild(new AsNavigationTreeNode(UserAccessIdSE.BankRemains.Value, ArmSoft.SME.Trade.Common.Resources.BankRemains, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode9.AddChild(new AsNavigationTreeNode(UserAccessIdSE.BankTurnOver.Value, ArmSoft.SME.Trade.Common.Resources.BankTurn, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode9.AddChild(new AsNavigationTreeNode(UserAccessIdSE.BankOpJournal.Value, ArmSoft.SME.Trade.Common.Resources.BankOpJournal, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode9.AddChild(new AsNavigationTreeNode(UserAccessIdSE.BankTAccount.Value, ArmSoft.SME.Trade.Common.Resources.BankTAccount, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode6.AddChild(asNavigationTreeNode9, num, checkNodeAccess, addIfHasChild: true);
			asNavigationTreeNode6.AddChild(new AsNavigationTreeNode(UserAccessIdSE.ReceivedServicesJournal.Value, ArmSoft.SME.Trade.Common.Resources.ReceivedServicesJournal, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode.AddChild(asNavigationTreeNode6, num, checkNodeAccess, addIfHasChild: true);
			AsNavigationTreeNode asNavigationTreeNode10 = new AsNavigationTreeNode(UserAccessIdSE.PurchaseDirectories.Value, ArmSoft.SME.Common.Resources.Directories, "table_16.svg", isViewChangeable: false, isOperChangeable: false);
			asNavigationTreeNode.AddChild(asNavigationTreeNode10, num, checkNodeAccess);
			asNavigationTreeNode10.AddChild(new AsNavigationTreeNode(UserAccessIdSE.Goods, ArmSoft.SME.Trade.Common.Resources.MaterialsAndServices + " (Ctrl+Shift+L)", "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode10.AddChild(new AsNavigationTreeNode(UserAccessIdSE.ImportFromUnitedProducts.Value, ArmSoft.SME.Common.Resources.ProductImportFromb2b, "cloud_16_red.svg"), num, checkNodeAccess);
			asNavigationTreeNode10.AddChild(new AsNavigationTreeNode(UserAccessIdSE.B2BProductsCheck.Value, ArmSoft.SME.Common.Resources.ProductsCheckWithB2B, "cloud_update_16.svg"), num, checkNodeAccess);
			AsNavigationTreeNode asNavigationTreeNode11 = new AsNavigationTreeNode(UserAccessIdSE.PurchaseGoodsDirectories.Value, ArmSoft.SME.Trade.Common.Resources.GoodsDirectories, "table_16.svg", isViewChangeable: false, isOperChangeable: false, isExpanded: false);
			asNavigationTreeNode11.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MTGroups.Value, ArmSoft.SME.Trade.Common.Resources.MaterGrps, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode11.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PartnersMT.Value, ArmSoft.SME.Trade.Common.Resources.PartnersMTAndServices, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode11.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MTComplects.Value, ArmSoft.SME.Common.Resources.Complects, "table_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode11.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MTComplectsParts.Value, ArmSoft.SME.Trade.Common.Resources.MTComplectsParts, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode11.AddChild(new AsNavigationTreeNode(UserAccessIdSE.QntUnits.Value, ArmSoft.SME.Common.Resources.MeasureUnits, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode11.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MTQntUnits.Value, ArmSoft.SME.Trade.Common.Resources.MTQntUnits, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode11.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MTBarCodes.Value, ArmSoft.SME.Common.Resources.BarCodes, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode11.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PriceTypes.Value, ArmSoft.SME.Common.Resources.PriceListTypes, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode11.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MTPrices.Value, ArmSoft.SME.Common.Resources.Prices, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode11.AddChild(new AsNavigationTreeNode(UserAccessIdSE.OrderMinMax.Value, ArmSoft.SME.Trade.Common.Resources.MTOrderMinMax, "table_16.svg"), num, checkNodeAccess);
			if (_enableSubstituteItems)
			{
				asNavigationTreeNode11.AddChild(new AsNavigationTreeNode(UserAccessIdSE.SubstituteItemsGroups.Value, ArmSoft.SME.Trade.Common.Resources.SubstituteItemsGroups, "table_16.svg"), num, checkNodeAccess);
			}
			asNavigationTreeNode10.AddChild(asNavigationTreeNode11, num, checkNodeAccess, addIfHasChild: true);
			asNavigationTreeNode10.AddChild(new AsNavigationTreeNode(UserAccessIdSE.Storages.Value, ArmSoft.SME.Common.Resources.StoragesAndShops, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode10.AddChild(new AsNavigationTreeNode(UserAccessIdSE.Partners, ArmSoft.SME.Common.Resources.Partners + " (Ctrl+Shift+B)", "table_16.svg"), num, checkNodeAccess);
			AsNavigationTreeNode asNavigationTreeNode12 = new AsNavigationTreeNode(UserAccessIdSE.PurchasePartnersDirectories.Value, ArmSoft.SME.Trade.Common.Resources.PartnersDirectories, "table_16.svg", isViewChangeable: false, isOperChangeable: false, isExpanded: false);
			asNavigationTreeNode12.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PartGroups.Value, ArmSoft.SME.Common.Resources.PartnersGroups, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode12.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PartContracts.Value, ArmSoft.SME.Common.Resources.Contracts, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode12.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PartnerAccountsInBank.Value, ArmSoft.SME.Trade.Common.Resources.PartnersAccountsInBank, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode10.AddChild(asNavigationTreeNode12, num, checkNodeAccess, addIfHasChild: true);
			AsNavigationTreeNode asNavigationTreeNode13 = new AsNavigationTreeNode(UserAccessIdSE.PurchaseOtherDirectories.Value, ArmSoft.SME.Common.Resources.OtherDirectories, "table_16.svg", isViewChangeable: false, isOperChangeable: false, isExpanded: false);
			asNavigationTreeNode13.AddChild(new AsNavigationTreeNode(UserAccessIdSE.CashDesks.Value, ArmSoft.SME.Common.Resources.Cashes, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode13.AddChild(new AsNavigationTreeNode(UserAccessIdSE.AccountsInBank.Value, ArmSoft.SME.Common.Resources.SettlementAccounts, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode13.AddChild(new AsNavigationTreeNode(UserAccessIdSE.Currencies.Value, ArmSoft.SME.Common.Resources.Curres, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode13.AddChild(new AsNavigationTreeNode(UserAccessIdSE.ExchangeRates.Value, ArmSoft.SME.Common.Resources.ExchangeRates, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode13.AddChild(new AsNavigationTreeNode(UserAccessIdSE.Banks.Value, ArmSoft.SME.Common.Resources.Banks, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode13.AddChild(new AsNavigationTreeNode(UserAccessIdSE.Countries.Value, ArmSoft.SME.Common.Resources.Countries, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode13.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PropertiesTypes.Value, ArmSoft.SME.Trade.Common.Resources.MTPropTypes, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode13.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PropertiesListsValues.Value, ArmSoft.SME.Trade.Common.Resources.PropertiesListsValues, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode13.AddChild(new AsNavigationTreeNode(UserAccessIdSE.Makers.Value, ArmSoft.SME.Common.Resources.Producers, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode13.AddChild(new AsNavigationTreeNode(UserAccessIdSE.StandardComments.Value, ArmSoft.SME.Common.Resources.StandartComments, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode10.AddChild(asNavigationTreeNode13, num, checkNodeAccess, addIfHasChild: true);
			asNavigationTreeNode.RemoveWithoutChildNode(asNavigationTreeNode10);
			SubSystemRoots.Add(1, asNavigationTreeNode);
		}
	}

	private void BuildSaleTree()
	{
		short num = 2;
		if (IsSubsystemAccessible(num, out var checkNodeAccess))
		{
			AsNavigationTreeNode asNavigationTreeNode = new AsNavigationTreeNode(num, ArmSoft.SME.Common.Resources.Sales, "SaleSS_24.svg", isViewChangeable: false, isOperChangeable: false);
			AsNavigationTreeNode asNavigationTreeNode2 = new AsNavigationTreeNode(UserAccessIdSE.SaleDocuments.Value, ArmSoft.SME.Common.Resources.Documents, "doc_new_16.svg", isViewChangeable: false, isOperChangeable: false, isExpanded: true, "", AWPCommandType.SystemCall, "", isInFirstColumnInHomePage: true);
			asNavigationTreeNode.AddChild(asNavigationTreeNode2, num, checkNodeAccess);
			asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MTInvoice.Value, ArmSoft.SME.Trade.Common.Resources.DocInvoice, "invoice_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MTBill.Value, ArmSoft.SME.Trade.Common.Resources.DocMTBill + " (F12)", "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MTSaleRetail.Value, ArmSoft.SME.Common.Resources.DocInvoiceRetail, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.InvoiceForPay.Value, ArmSoft.SME.Common.Resources.InvoiceForPay, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.ReturnFromCustomer.Value, ArmSoft.SME.Trade.Common.Resources.DocMTReturnFromCustomer, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MTBillReturn.Value, ArmSoft.SME.Trade.Common.Resources.DocMTBillReturn + " (Shift+F12)", "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			AsNavigationTreeNode asNavigationTreeNode3 = new AsNavigationTreeNode(UserAccessIdSE.SalesSubDocuments.Value, ArmSoft.SME.Trade.Common.Resources.SubDocuments, "doc_new_16.svg", isViewChangeable: false, isOperChangeable: false, isExpanded: false, "", AWPCommandType.SystemCall, "", isInFirstColumnInHomePage: true);
			asNavigationTreeNode3.AddChild(new AsNavigationTreeNode(UserAccessIdSE.CashIn.Value, ArmSoft.SME.Common.Resources.CashInputOrder, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode3.AddChild(new AsNavigationTreeNode(UserAccessIdSE.BankIn.Value, ArmSoft.SME.Trade.Common.Resources.BankIn, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode3.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PartRemCorrect.Value, ArmSoft.SME.Trade.Common.Resources.PartnerRemCorrection, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode3.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PartRemGroupCorrect.Value, ArmSoft.SME.Trade.Common.Resources.PartnerRemGroupCorrection, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode3.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PartRemMove.Value, ArmSoft.SME.Trade.Common.Resources.PartnerRemMove, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode3.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PartnersGroupRevaluation.Value, ArmSoft.SME.Trade.Common.Resources.PartnersGroupRevaluation, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			if (_useBonus)
			{
				asNavigationTreeNode3.AddChild(new AsNavigationTreeNode(UserAccessIdSE.BonusGroupCorrect.Value, ArmSoft.SME.Trade.Common.Resources.BonusGroupCorrect, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			}
			asNavigationTreeNode3.AddChild(new AsNavigationTreeNode(UserAccessIdSE.ReconciliationStatementDoc.Value, ArmSoft.SME.Common.Resources.ReconciliationStatement, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			if (_useMarking)
			{
				asNavigationTreeNode3.AddChild(new AsNavigationTreeNode(UserAccessIdSE.EMarkReceiptDoc.Value, ArmSoft.SME.Trade.Common.Resources.EMarkReceiptDoc, "doc_new_16_new.svg", isViewChangeable: true, isOperChangeable: false, isExpanded: true, "", AWPCommandType.SystemCall, "", isInFirstColumnInHomePage: false, FeatureStatusTypeSE.New), num, checkNodeAccess);
			}
			asNavigationTreeNode2.AddChild(asNavigationTreeNode3, num, checkNodeAccess, addIfHasChild: true);
			asNavigationTreeNode.RemoveWithoutChildNode(asNavigationTreeNode2);
			AsNavigationTreeNode asNavigationTreeNode4 = new AsNavigationTreeNode(UserAccessIdSE.SaleOperations.Value, ArmSoft.SME.Common.Resources.Operations, "action_16.svg", isViewChangeable: false, isOperChangeable: false, isExpanded: true, "", AWPCommandType.SystemCall, "", isInFirstColumnInHomePage: true);
			asNavigationTreeNode4.AddChild(new AsNavigationTreeNode(UserAccessIdSE.AssignTaxInvNums.Value, ArmSoft.SME.Common.Resources.AssignTaxInvNums, "action_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode4.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PrintECRReport.Value, ArmSoft.SME.Common.Resources.PrintECRXZReport, "action_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode4.AddChild(new AsNavigationTreeNode(UserAccessIdSE.SyncECR.Value, ArmSoft.SME.Common.Resources.SyncECR, "action_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode4.AddChild(new AsNavigationTreeNode(UserAccessIdSE.OpenCashierShift.Value, ArmSoft.SME.Trade.Common.Resources.OpenCashierShift, "action_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode4.AddChild(new AsNavigationTreeNode(UserAccessIdSE.CloseCashierShift.Value, ArmSoft.SME.Trade.Common.Resources.CloseCashierShift, "action_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode4.AddChild(new AsNavigationTreeNode(UserAccessIdSE.ExportForElScalesAndTerminals, ArmSoft.SME.Trade.Common.Resources.ExportForElScaleAndTerminal, "action_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode.AddChild(asNavigationTreeNode4, num, checkNodeAccess, addIfHasChild: true);
			AsNavigationTreeNode asNavigationTreeNode5 = new AsNavigationTreeNode(UserAccessIdSE.SaleJournals.Value, ArmSoft.SME.Common.Resources.Journals, "journal_16.svg", isViewChangeable: false, isOperChangeable: false, isExpanded: true, "", AWPCommandType.SystemCall, "", isInFirstColumnInHomePage: true);
			asNavigationTreeNode5.AddChild(new AsNavigationTreeNode(UserAccessIdSE.SaleDocumentsJournal.Value, ArmSoft.SME.Common.Resources.SaleDocuments, "journal_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode5.AddChild(new AsNavigationTreeNode(UserAccessIdSE.WrittenOffInvoicesForPay.Value, ArmSoft.SME.Common.Resources.SentInvoicesForPayment, "journal_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode5.AddChild(new AsNavigationTreeNode(UserAccessIdSE.TaxInvoicesJournal.Value, ArmSoft.SME.Trade.Common.Resources.WriteOffTaxInvoicesJournal, "journal_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode5.AddChild(new AsNavigationTreeNode(UserAccessIdSE.ReconciliationStatementsJournal.Value, ArmSoft.SME.Trade.Common.Resources.ReconciliationStatementsJournal, "journal_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode5.AddChild(new AsNavigationTreeNode(UserAccessIdSE.ECRCheckDocumentsJournal.Value, ArmSoft.SME.Common.Resources.ECRChecksJournal, "journal_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode5.AddChild(new AsNavigationTreeNode(UserAccessIdSE.LocalDBDocumnets.Value, ArmSoft.SME.Common.Resources.OfflineECRChecksJournal, "journal_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode.AddChild(asNavigationTreeNode5, num, checkNodeAccess, addIfHasChild: true);
			AsNavigationTreeNode asNavigationTreeNode6 = new AsNavigationTreeNode(UserAccessIdSE.SaleReports.Value, ArmSoft.SME.Common.Resources.Reports, "report_16.svg", isViewChangeable: false, isOperChangeable: false);
			asNavigationTreeNode6.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MatRepSalesAnalysis.Value, ArmSoft.SME.Trade.Common.Resources.MTRepSalesAnalysis, "report_16.svg"), num, checkNodeAccess);
			if (_useDiscount)
			{
				asNavigationTreeNode6.AddChild(new AsNavigationTreeNode(UserAccessIdSE.DiscountLogsJournal.Value, ArmSoft.SME.Trade.Common.Resources.DiscountLogsJournal, "report_16.svg"), num, checkNodeAccess);
			}
			asNavigationTreeNode6.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MTPriceList.Value, ArmSoft.SME.Common.Resources.PriceList, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode6.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MatRepSalesAverage.Value, ArmSoft.SME.Trade.Common.Resources.SalesAveragePrice, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode6.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MatRepAvailability.Value, ArmSoft.SME.Trade.Common.Resources.MatRepAvail, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode6.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MatRepAvailabilityTreeView.Value, ArmSoft.SME.Trade.Common.Resources.MatRepAvailTreeView, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode6.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MatRepTurnOver.Value, ArmSoft.SME.Trade.Common.Resources.MatRepTurn, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode6.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MatRepOperationsJournal.Value, ArmSoft.SME.Trade.Common.Resources.MatRepJournal, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode6.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MatRepTAccount.Value, ArmSoft.SME.Trade.Common.Resources.TAccount + " (Ctrl+L)", "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode6.AddChild(new AsNavigationTreeNode(UserAccessIdSE.CashierShiftReport.Value, ArmSoft.SME.Trade.Common.Resources.CashierShiftReport, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode6.AddChild(new AsNavigationTreeNode(UserAccessIdSE.ECRReport.Value, ArmSoft.SME.Trade.Common.Resources.ECRCrnReport, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode6.AddChild(new AsNavigationTreeNode(UserAccessIdSE.ECRCheckDocumentsListJournal.Value, ArmSoft.SME.Trade.Common.Resources.ECRChecksListJournal, "report_16.svg"), num, checkNodeAccess);
			if (_useMarking)
			{
				asNavigationTreeNode6.AddChild(new AsNavigationTreeNode(UserAccessIdSE.EMarkReceipListJournal.Value, ArmSoft.SME.Trade.Common.Resources.EMarkReceipListJournal, _newReportUri, isViewChangeable: true, isOperChangeable: true, isExpanded: true, "", AWPCommandType.SystemCall, "", isInFirstColumnInHomePage: false, FeatureStatusTypeSE.New), num, checkNodeAccess);
			}
			if (_retailAccounting)
			{
				AsNavigationTreeNode asNavigationTreeNode7 = new AsNavigationTreeNode(UserAccessIdSE.SalesRetailReports.Value, ArmSoft.SME.Trade.Common.Resources.RetailReports, "report_16.svg", isViewChangeable: false, isOperChangeable: false, isExpanded: false);
				asNavigationTreeNode7.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MatRepAvailabilityRetail.Value, ArmSoft.SME.Trade.Common.Resources.MTRepRetailAvail, "report_16.svg"), num, checkNodeAccess);
				asNavigationTreeNode7.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MatRepTurnOverRetail.Value, ArmSoft.SME.Trade.Common.Resources.MTRepRetailTurn, "report_16.svg"), num, checkNodeAccess);
				asNavigationTreeNode7.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MatRepOperationsRetail.Value, ArmSoft.SME.Trade.Common.Resources.MTRepRetailOpJournal, "report_16.svg"), num, checkNodeAccess);
				asNavigationTreeNode6.AddChild(asNavigationTreeNode7, num, checkNodeAccess, addIfHasChild: true);
			}
			AsNavigationTreeNode asNavigationTreeNode8 = new AsNavigationTreeNode(UserAccessIdSE.SalesPartnersReports.Value, ArmSoft.SME.Common.Resources.PartnersReports, "report_16.svg", isViewChangeable: false, isOperChangeable: false, isExpanded: false);
			asNavigationTreeNode8.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PartRemains.Value, ArmSoft.SME.Trade.Common.Resources.PartnerRemains, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode8.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PartTurnOver.Value, ArmSoft.SME.Trade.Common.Resources.PartnerTurn, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode8.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PartOpJournal.Value, ArmSoft.SME.Trade.Common.Resources.PartnerOpJournal, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode8.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PartRepCard.Value, ArmSoft.SME.Trade.Common.Resources.PartnerCard + " (Ctrl+B)", "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode6.AddChild(asNavigationTreeNode8, num, checkNodeAccess, addIfHasChild: true);
			if (_useBonus)
			{
				AsNavigationTreeNode asNavigationTreeNode9 = new AsNavigationTreeNode(UserAccessIdSE.SalesBonusReports.Value, ArmSoft.SME.Trade.Common.Resources.BonusReports, "report_16.svg", isViewChangeable: false, isOperChangeable: false, isExpanded: false);
				asNavigationTreeNode9.AddChild(new AsNavigationTreeNode(UserAccessIdSE.BonusRemains.Value, ArmSoft.SME.Trade.Common.Resources.BonusRemains, "report_16.svg"), num, checkNodeAccess);
				asNavigationTreeNode9.AddChild(new AsNavigationTreeNode(UserAccessIdSE.BonusTurnOver.Value, ArmSoft.SME.Trade.Common.Resources.BonusTurnOver, "report_16.svg"), num, checkNodeAccess);
				asNavigationTreeNode9.AddChild(new AsNavigationTreeNode(UserAccessIdSE.BonusOpJournal.Value, ArmSoft.SME.Trade.Common.Resources.BonusOpJournal, "report_16.svg"), num, checkNodeAccess);
				asNavigationTreeNode9.AddChild(new AsNavigationTreeNode(UserAccessIdSE.BonusTAccount.Value, ArmSoft.SME.Trade.Common.Resources.BonusTAccount, "report_16.svg"), num, checkNodeAccess);
				asNavigationTreeNode6.AddChild(asNavigationTreeNode9, num, checkNodeAccess, addIfHasChild: true);
			}
			AsNavigationTreeNode asNavigationTreeNode10 = new AsNavigationTreeNode(UserAccessIdSE.SalesCashBankReports.Value, ArmSoft.SME.Trade.Common.Resources.CashBankReports, "report_16.svg", isViewChangeable: false, isOperChangeable: false, isExpanded: false);
			asNavigationTreeNode10.AddChild(new AsNavigationTreeNode(UserAccessIdSE.CashRemains.Value, ArmSoft.SME.Trade.Common.Resources.KassaRemains, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode10.AddChild(new AsNavigationTreeNode(UserAccessIdSE.CashTurnOver.Value, ArmSoft.SME.Trade.Common.Resources.KassaTurn, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode10.AddChild(new AsNavigationTreeNode(UserAccessIdSE.CashOpJournal.Value, ArmSoft.SME.Trade.Common.Resources.KassaOpJournal, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode10.AddChild(new AsNavigationTreeNode(UserAccessIdSE.CashDeskTAccount.Value, ArmSoft.SME.Trade.Common.Resources.CashDeskTAccount, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode10.AddChild(new AsNavigationTreeNode(UserAccessIdSE.BankRemains.Value, ArmSoft.SME.Trade.Common.Resources.BankRemains, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode10.AddChild(new AsNavigationTreeNode(UserAccessIdSE.BankTurnOver.Value, ArmSoft.SME.Trade.Common.Resources.BankTurn, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode10.AddChild(new AsNavigationTreeNode(UserAccessIdSE.BankOpJournal.Value, ArmSoft.SME.Trade.Common.Resources.BankOpJournal, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode10.AddChild(new AsNavigationTreeNode(UserAccessIdSE.BankTAccount.Value, ArmSoft.SME.Trade.Common.Resources.BankTAccount, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode6.AddChild(asNavigationTreeNode10, num, checkNodeAccess, addIfHasChild: true);
			asNavigationTreeNode.AddChild(asNavigationTreeNode6, num, checkNodeAccess, addIfHasChild: true);
			AsNavigationTreeNode asNavigationTreeNode11 = new AsNavigationTreeNode(UserAccessIdSE.SaleDirectories.Value, ArmSoft.SME.Common.Resources.Directories, "table_16.svg", isViewChangeable: false, isOperChangeable: false);
			asNavigationTreeNode11.AddChild(new AsNavigationTreeNode(UserAccessIdSE.Goods, ArmSoft.SME.Trade.Common.Resources.MaterialsAndServices + " (Ctrl+Shift+L)", "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode11.AddChild(new AsNavigationTreeNode(UserAccessIdSE.ImportFromUnitedProducts.Value, ArmSoft.SME.Common.Resources.ProductImportFromb2b, "cloud_16_red.svg"), num, checkNodeAccess);
			asNavigationTreeNode11.AddChild(new AsNavigationTreeNode(UserAccessIdSE.B2BProductsCheck.Value, ArmSoft.SME.Common.Resources.ProductsCheckWithB2B, "cloud_update_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode11.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PriceTypes.Value, ArmSoft.SME.Common.Resources.PriceListTypes, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode11.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MTPrices.Value, ArmSoft.SME.Common.Resources.Prices, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode11.AddChild(new AsNavigationTreeNode(UserAccessIdSE.CPAClassifications.Value, ArmSoft.SME.Common.Resources.CPAClassifications, "table_16.svg"), num, checkNodeAccess);
			if (_useDiscount)
			{
				asNavigationTreeNode11.AddChild(new AsNavigationTreeNode(UserAccessIdSE.Discounts.Value, ArmSoft.SME.Trade.Common.Resources.Discounts, "table_16.svg"), num, checkNodeAccess);
				asNavigationTreeNode11.AddChild(new AsNavigationTreeNode(UserAccessIdSE.DiscountGroups.Value, ArmSoft.SME.Trade.Common.Resources.DiscountGroups, "table_16.svg"), num, checkNodeAccess);
			}
			if (_useBonus)
			{
				asNavigationTreeNode11.AddChild(new AsNavigationTreeNode(UserAccessIdSE.BonusScale.Value, ArmSoft.SME.Trade.Common.Resources.BonusScale, "table_16.svg"), num, checkNodeAccess);
			}
			AsNavigationTreeNode asNavigationTreeNode12 = new AsNavigationTreeNode(UserAccessIdSE.SalesGoodsDirectories.Value, ArmSoft.SME.Trade.Common.Resources.GoodsDirectories, "table_16.svg", isViewChangeable: false, isOperChangeable: false, isExpanded: false);
			asNavigationTreeNode12.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MTGroups.Value, ArmSoft.SME.Trade.Common.Resources.MaterGrps, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode12.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PartnersMT.Value, ArmSoft.SME.Trade.Common.Resources.PartnersMTAndServices, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode12.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MTComplects.Value, ArmSoft.SME.Common.Resources.Complects, "table_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode12.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MTComplectsParts.Value, ArmSoft.SME.Trade.Common.Resources.MTComplectsParts, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode12.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MTQntUnits.Value, ArmSoft.SME.Trade.Common.Resources.MTQntUnits, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode12.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MTBarCodes.Value, ArmSoft.SME.Common.Resources.BarCodes, "table_16.svg"), num, checkNodeAccess);
			if (_enableSubstituteItems)
			{
				asNavigationTreeNode12.AddChild(new AsNavigationTreeNode(UserAccessIdSE.SubstituteItemsGroups.Value, ArmSoft.SME.Trade.Common.Resources.SubstituteItemsGroups, "table_16.svg"), num, checkNodeAccess);
			}
			asNavigationTreeNode12.AddChild(new AsNavigationTreeNode(UserAccessIdSE.Segments.Value, ArmSoft.SME.Trade.Common.Resources.GoodsSegments, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode12.AddChild(new AsNavigationTreeNode(UserAccessIdSE.GoodsBySegment.Value, ArmSoft.SME.Trade.Common.Resources.GoodsBySegment, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode11.AddChild(asNavigationTreeNode12, num, checkNodeAccess, addIfHasChild: true);
			asNavigationTreeNode11.AddChild(new AsNavigationTreeNode(UserAccessIdSE.Partners, ArmSoft.SME.Common.Resources.Partners + " (Ctrl+Shift+B)", "table_16.svg"), num, checkNodeAccess);
			AsNavigationTreeNode asNavigationTreeNode13 = new AsNavigationTreeNode(UserAccessIdSE.SalesPartnersDirectories.Value, ArmSoft.SME.Trade.Common.Resources.PartnersDirectories, "table_16.svg", isViewChangeable: false, isOperChangeable: false, isExpanded: false);
			asNavigationTreeNode13.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PartGroups.Value, ArmSoft.SME.Common.Resources.PartnersGroups, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode13.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PartContracts.Value, ArmSoft.SME.Common.Resources.Contracts, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode13.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PartnerAccountsInBank.Value, ArmSoft.SME.Trade.Common.Resources.PartnersAccountsInBank, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode11.AddChild(asNavigationTreeNode13, num, checkNodeAccess, addIfHasChild: true);
			AsNavigationTreeNode asNavigationTreeNode14 = new AsNavigationTreeNode(UserAccessIdSE.SalesOtherDirectories.Value, ArmSoft.SME.Common.Resources.OtherDirectories, "table_16.svg", isViewChangeable: false, isOperChangeable: false, isExpanded: false);
			asNavigationTreeNode14.AddChild(new AsNavigationTreeNode(UserAccessIdSE.Employees.Value, ArmSoft.SME.Common.Resources.Employees, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode14.AddChild(new AsNavigationTreeNode(UserAccessIdSE.CashDesks.Value, ArmSoft.SME.Common.Resources.Cashes, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode14.AddChild(new AsNavigationTreeNode(UserAccessIdSE.CashierShift.Value, ArmSoft.SME.Trade.Common.Resources.CashierShifts, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode14.AddChild(new AsNavigationTreeNode(UserAccessIdSE.ElScalesAndTerminals, ArmSoft.SME.Trade.Common.Resources.ElScalesAndTerminals, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode14.AddChild(new AsNavigationTreeNode(UserAccessIdSE.AccountsInBank.Value, ArmSoft.SME.Common.Resources.SettlementAccounts, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode14.AddChild(new AsNavigationTreeNode(UserAccessIdSE.Currencies.Value, ArmSoft.SME.Common.Resources.Curres, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode14.AddChild(new AsNavigationTreeNode(UserAccessIdSE.ExchangeRates.Value, ArmSoft.SME.Common.Resources.ExchangeRates, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode14.AddChild(new AsNavigationTreeNode(UserAccessIdSE.Banks.Value, ArmSoft.SME.Common.Resources.Banks, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode11.AddChild(asNavigationTreeNode14, num, checkNodeAccess, addIfHasChild: true);
			if (_useDiscountServer)
			{
				AsNavigationTreeNode asNavigationTreeNode15 = new AsNavigationTreeNode(UserAccessIdSE.SalesOtherDirectories.Value, ArmSoft.SME.Trade.Common.Resources.DiscountServer, "table_cloud_16.svg", isViewChangeable: false, isOperChangeable: false);
				asNavigationTreeNode15.AddChild(new AsNavigationTreeNode(UserAccessIdSE.DiscountServerBonusCards, ArmSoft.SME.Trade.Common.Resources.BonusCards, "table_cloud_16.svg"));
				asNavigationTreeNode15.AddChild(new AsNavigationTreeNode(UserAccessIdSE.DiscountServerBonusTransactions, ArmSoft.SME.Trade.Common.Resources.BonusTransactions, "table_cloud_16.svg"));
				asNavigationTreeNode15.AddChild(new AsNavigationTreeNode(UserAccessIdSE.DiscountServerBonusSumComparison, ArmSoft.SME.Trade.Common.Resources.DiscountServerBonusSumComparison, "table_cloud_16.svg"));
				asNavigationTreeNode11.AddChild(asNavigationTreeNode15);
			}
			asNavigationTreeNode.AddChild(asNavigationTreeNode11, num, checkNodeAccess, addIfHasChild: true);
			SubSystemRoots.Add(2, asNavigationTreeNode);
		}
	}

	private void BuildPointOfSaleTree()
	{
		short num = 7;
		if (IsSubsystemAccessible(num, out var checkNodeAccess) || (Settings.IsOffline && Settings.UserAccessService.IsSubsystemAccessible(2) && Settings.UserAccessService.GetNavigatorNodePermission(2, UserAccessIdSE.MTBill).AllowView))
		{
			AsNavigationTreeNode asNavigationTreeNode = new AsNavigationTreeNode(num, ArmSoft.SME.Trade.Common.Resources.PointOfSalePOS, "POS_SS_24.svg", isViewChangeable: false, isOperChangeable: false);
			AsNavigationTreeNode asNavigationTreeNode2 = new AsNavigationTreeNode(UserAccessIdSE.PointOfSaleDocuments.Value, ArmSoft.SME.Common.Resources.Documents, "doc_new_16.svg", isViewChangeable: false, isOperChangeable: false, isExpanded: true, "", AWPCommandType.SystemCall, "", isInFirstColumnInHomePage: true);
			asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.Sale.Value, ArmSoft.SME.Trade.Common.Resources.Sale, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode.AddChild(asNavigationTreeNode2, num, checkNodeAccess, addIfHasChild: true);
			AsNavigationTreeNode asNavigationTreeNode3 = new AsNavigationTreeNode(UserAccessIdSE.PointOfSaleOperations.Value, ArmSoft.SME.Common.Resources.Operations, "action_16.svg", isViewChangeable: false, isOperChangeable: false, isExpanded: true, "", AWPCommandType.SystemCall, "", isInFirstColumnInHomePage: true);
			asNavigationTreeNode3.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PrintECRReportPOS.Value, ArmSoft.SME.Common.Resources.PrintECRXZReport, "action_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode3.AddChild(new AsNavigationTreeNode(UserAccessIdSE.SyncECRPOS.Value, ArmSoft.SME.Common.Resources.SyncECR, "action_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode3.AddChild(new AsNavigationTreeNode(UserAccessIdSE.CloseCashierShift.Value, ArmSoft.SME.Trade.Common.Resources.CashierShiftClose, "action_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode3.AddChild(new AsNavigationTreeNode(UserAccessIdSE.ECRConfiguration.Value, ArmSoft.SME.Trade.Common.Resources.ECRConfiguration, "action_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode.AddChild(asNavigationTreeNode3, num, checkNodeAccess, addIfHasChild: true);
			AsNavigationTreeNode asNavigationTreeNode4 = new AsNavigationTreeNode(UserAccessIdSE.PointOfSaleReports.Value, ArmSoft.SME.Common.Resources.Reports, "report_16.svg", isViewChangeable: false, isOperChangeable: false);
			asNavigationTreeNode4.AddChild(new AsNavigationTreeNode(UserAccessIdSE.CashierShiftReport.Value, ArmSoft.SME.Trade.Common.Resources.CashierShiftReport, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode.AddChild(asNavigationTreeNode4, num, checkNodeAccess, addIfHasChild: true);
			SubSystemRoots.Add(7, asNavigationTreeNode);
		}
	}

	private void BuildStorageTree()
	{
		short num = 3;
		if (IsSubsystemAccessible(num, out var checkNodeAccess))
		{
			AsNavigationTreeNode asNavigationTreeNode = new AsNavigationTreeNode(num, ArmSoft.SME.Common.Resources.Storage, "Materials_green_24.svg", isViewChangeable: false, isOperChangeable: false);
			AsNavigationTreeNode asNavigationTreeNode2 = new AsNavigationTreeNode(UserAccessIdSE.StorageDocuments.Value, ArmSoft.SME.Common.Resources.Documents, "doc_new_16.svg", isViewChangeable: false, isOperChangeable: false, isExpanded: true, "", AWPCommandType.SystemCall, "", isInFirstColumnInHomePage: true);
			asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MTIncome.Value, ArmSoft.SME.Common.Resources.DocMTIncome, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MTIncomeRetail.Value, ArmSoft.SME.Trade.Common.Resources.DocMTIncomeRetail, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MTMove.Value, ArmSoft.SME.Trade.Common.Resources.DocMTMove, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			if (_mtMoveWithConfirmation)
			{
				asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MTMoveWithConfirmation.Value, ArmSoft.SME.Trade.Common.Resources.MTMoveWithConfirmation, _docUriNew, isViewChangeable: true, isOperChangeable: false, isExpanded: true, "", AWPCommandType.SystemCall, "", isInFirstColumnInHomePage: false, FeatureStatusTypeSE.New), num, checkNodeAccess);
			}
			asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MTMoveOrder.Value, ArmSoft.SME.Trade.Common.Resources.MTMoveOrder, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MTExpense.Value, ArmSoft.SME.Common.Resources.DocMTExpense, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MTInventory.Value, ArmSoft.SME.Trade.Common.Resources.DocMTInventory, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			if (_retailAccounting)
			{
				asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MTInventoryRetail.Value, ArmSoft.SME.Trade.Common.Resources.DocMTInventoryRetail, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
				asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MTRevaluationRetail.Value, ArmSoft.SME.Trade.Common.Resources.DocMTRevaluationRetail, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			}
			asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MTComplectation.Value, ArmSoft.SME.Trade.Common.Resources.Complection, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MTDeComplectation.Value, ArmSoft.SME.Trade.Common.Resources.Decomplection, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MTRevaluation.Value, ArmSoft.SME.Trade.Common.Resources.MaterialRevaluation, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			AsNavigationTreeNode asNavigationTreeNode3 = new AsNavigationTreeNode(UserAccessIdSE.StorageSubDocuments.Value, ArmSoft.SME.Trade.Common.Resources.SubDocuments, "doc_new_16.svg", isViewChangeable: false, isOperChangeable: false, isExpanded: false);
			asNavigationTreeNode3.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MTIncomeImport.Value, ArmSoft.SME.Trade.Common.Resources.DocMTIncomeImport, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode3.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MTIncomeRem.Value, ArmSoft.SME.Trade.Common.Resources.DocMTIncomeRem, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			if (_useMarking)
			{
				asNavigationTreeNode3.AddChild(new AsNavigationTreeNode(UserAccessIdSE.EMarkReceiptDoc.Value, ArmSoft.SME.Trade.Common.Resources.EMarkReceiptDoc, "doc_new_16_new.svg", isViewChangeable: true, isOperChangeable: false, isExpanded: true, "", AWPCommandType.SystemCall, "", isInFirstColumnInHomePage: false, FeatureStatusTypeSE.New), num, checkNodeAccess);
			}
			asNavigationTreeNode2.AddChild(asNavigationTreeNode3, num, checkNodeAccess, addIfHasChild: true);
			asNavigationTreeNode.AddChild(asNavigationTreeNode2, num, checkNodeAccess, addIfHasChild: true);
			AsNavigationTreeNode asNavigationTreeNode4 = new AsNavigationTreeNode(UserAccessIdSE.StorageOperations.Value, ArmSoft.SME.Common.Resources.Operations, "action_16.svg", isViewChangeable: false, isOperChangeable: false, isExpanded: true, "", AWPCommandType.SystemCall, "", isInFirstColumnInHomePage: true);
			asNavigationTreeNode4.AddChild(new AsNavigationTreeNode(UserAccessIdSE.AssignAccDocInvNums.Value, ArmSoft.SME.Common.Resources.AssignAccDocTransportationInvNums, "action_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode.AddChild(asNavigationTreeNode4, num, checkNodeAccess, addIfHasChild: true);
			AsNavigationTreeNode asNavigationTreeNode5 = new AsNavigationTreeNode(UserAccessIdSE.StorageJournals.Value, ArmSoft.SME.Common.Resources.Journals, "journal_16.svg", isViewChangeable: false, isOperChangeable: false, isExpanded: true, "", AWPCommandType.SystemCall, "", isInFirstColumnInHomePage: true);
			asNavigationTreeNode5.AddChild(new AsNavigationTreeNode(UserAccessIdSE.WarehouseDocumentsJournal.Value, ArmSoft.SME.Trade.Common.Resources.WarehouseDocumentsJournal, "journal_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode5.AddChild(new AsNavigationTreeNode(UserAccessIdSE.TransportationDocumentsJournal.Value, ArmSoft.SME.Common.Resources.TransportationDocumentsJournal, "journal_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode.AddChild(asNavigationTreeNode5, num, checkNodeAccess, addIfHasChild: true);
			AsNavigationTreeNode asNavigationTreeNode6 = new AsNavigationTreeNode(UserAccessIdSE.StorageReports.Value, ArmSoft.SME.Common.Resources.Reports, "report_16.svg", isViewChangeable: false, isOperChangeable: false);
			asNavigationTreeNode6.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MatRepAvailability.Value, ArmSoft.SME.Trade.Common.Resources.MatRepAvail, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode6.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MatRepAvailabilityTreeView.Value, ArmSoft.SME.Trade.Common.Resources.MatRepAvailTreeView, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode6.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MatRepTurnOver.Value, ArmSoft.SME.Trade.Common.Resources.MatRepTurn, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode6.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MatRepOperationsJournal.Value, ArmSoft.SME.Trade.Common.Resources.MatRepJournal, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode6.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MatRepTAccount, ArmSoft.SME.Trade.Common.Resources.TAccount + " (Ctrl+L)", "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode6.AddChild(new AsNavigationTreeNode(UserAccessIdSE.GoodsStockTakingRep.Value, ArmSoft.SME.Trade.Common.Resources.GoodsStockTakingRep, "report_16.svg"), num, checkNodeAccess);
			if (_retailAccounting)
			{
				AsNavigationTreeNode asNavigationTreeNode7 = new AsNavigationTreeNode(UserAccessIdSE.StorageRetailReports.Value, ArmSoft.SME.Trade.Common.Resources.RetailReports, "report_16.svg", isViewChangeable: false, isOperChangeable: false, isExpanded: false);
				asNavigationTreeNode7.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MatRepAvailabilityRetail.Value, ArmSoft.SME.Trade.Common.Resources.MTRepRetailAvail, "report_16.svg"), num, checkNodeAccess);
				asNavigationTreeNode7.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MatRepTurnOverRetail.Value, ArmSoft.SME.Trade.Common.Resources.MTRepRetailTurn, "report_16.svg"), num, checkNodeAccess);
				asNavigationTreeNode7.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MatRepOperationsRetail.Value, ArmSoft.SME.Trade.Common.Resources.MTRepRetailOpJournal, "report_16.svg"), num, checkNodeAccess);
				asNavigationTreeNode6.AddChild(asNavigationTreeNode7, num, checkNodeAccess, addIfHasChild: true);
			}
			if (_useMarking)
			{
				asNavigationTreeNode6.AddChild(new AsNavigationTreeNode(UserAccessIdSE.EMarkReceipListJournal.Value, ArmSoft.SME.Trade.Common.Resources.EMarkReceipListJournal, _newReportUri, isViewChangeable: true, isOperChangeable: true, isExpanded: true, "", AWPCommandType.SystemCall, "", isInFirstColumnInHomePage: false, FeatureStatusTypeSE.New), num, checkNodeAccess);
			}
			asNavigationTreeNode.AddChild(asNavigationTreeNode6, num, checkNodeAccess, addIfHasChild: true);
			AsNavigationTreeNode asNavigationTreeNode8 = new AsNavigationTreeNode(UserAccessIdSE.StorageDirectories.Value, ArmSoft.SME.Common.Resources.Directories, "table_16.svg", isViewChangeable: false, isOperChangeable: false);
			asNavigationTreeNode8.AddChild(new AsNavigationTreeNode(UserAccessIdSE.Goods, ArmSoft.SME.Trade.Common.Resources.MaterialsAndServices + " (Ctrl+Shift+L)", "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode8.AddChild(new AsNavigationTreeNode(UserAccessIdSE.ImportFromUnitedProducts.Value, ArmSoft.SME.Common.Resources.ProductImportFromb2b, "cloud_16_red.svg"), num, checkNodeAccess);
			asNavigationTreeNode8.AddChild(new AsNavigationTreeNode(UserAccessIdSE.B2BProductsCheck.Value, ArmSoft.SME.Common.Resources.ProductsCheckWithB2B, "cloud_update_16.svg"), num, checkNodeAccess);
			AsNavigationTreeNode asNavigationTreeNode9 = new AsNavigationTreeNode(UserAccessIdSE.StorageGoodsDirectories.Value, ArmSoft.SME.Trade.Common.Resources.GoodsDirectories, "table_16.svg", isViewChangeable: false, isOperChangeable: false);
			asNavigationTreeNode9.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MTGroups.Value, ArmSoft.SME.Trade.Common.Resources.MaterGrps, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode9.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PartnersMT.Value, ArmSoft.SME.Trade.Common.Resources.PartnersMTAndServices, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode9.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MTComplects.Value, ArmSoft.SME.Common.Resources.Complects, "table_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode9.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MTComplectsParts.Value, ArmSoft.SME.Trade.Common.Resources.MTComplectsParts, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode9.AddChild(new AsNavigationTreeNode(UserAccessIdSE.QntUnits.Value, ArmSoft.SME.Common.Resources.MeasureUnits, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode9.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MTQntUnits.Value, ArmSoft.SME.Trade.Common.Resources.MTQntUnits, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode9.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MTBarCodes.Value, ArmSoft.SME.Common.Resources.BarCodes, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode9.AddChild(new AsNavigationTreeNode(UserAccessIdSE.OrderMinMax.Value, ArmSoft.SME.Trade.Common.Resources.MTOrderMinMax, "table_16.svg"), num, checkNodeAccess);
			if (_enableSubstituteItems)
			{
				asNavigationTreeNode9.AddChild(new AsNavigationTreeNode(UserAccessIdSE.SubstituteItemsGroups.Value, ArmSoft.SME.Trade.Common.Resources.SubstituteItemsGroups, "table_16.svg"), num, checkNodeAccess);
			}
			asNavigationTreeNode8.AddChild(asNavigationTreeNode9, num, checkNodeAccess, addIfHasChild: true);
			asNavigationTreeNode8.AddChild(new AsNavigationTreeNode(UserAccessIdSE.Storages.Value, ArmSoft.SME.Common.Resources.StoragesAndShops, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode8.AddChild(new AsNavigationTreeNode(UserAccessIdSE.CPAClassifications.Value, ArmSoft.SME.Common.Resources.CPAClassifications, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode8.AddChild(new AsNavigationTreeNode(UserAccessIdSE.Countries.Value, ArmSoft.SME.Common.Resources.Countries, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode8.AddChild(new AsNavigationTreeNode(UserAccessIdSE.Makers.Value, ArmSoft.SME.Common.Resources.Producers, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode8.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PropertiesTypes.Value, ArmSoft.SME.Trade.Common.Resources.MTPropTypes, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode8.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PropertiesListsValues.Value, ArmSoft.SME.Trade.Common.Resources.PropertiesListsValues, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode8.AddChild(new AsNavigationTreeNode(UserAccessIdSE.StandardComments.Value, ArmSoft.SME.Common.Resources.StandartComments, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode.AddChild(asNavigationTreeNode8, num, checkNodeAccess, addIfHasChild: true);
			SubSystemRoots.Add(3, asNavigationTreeNode);
		}
	}

	private void BuildCashBankTree()
	{
		short num = 4;
		if (IsSubsystemAccessible(num, out var checkNodeAccess))
		{
			AsNavigationTreeNode asNavigationTreeNode = new AsNavigationTreeNode(num, ArmSoft.SME.Trade.Common.Resources.CashBank, "Bank_green_24.svg", isViewChangeable: false, isOperChangeable: false);
			AsNavigationTreeNode asNavigationTreeNode2 = new AsNavigationTreeNode(UserAccessIdSE.CashBankDocuments.Value, ArmSoft.SME.Common.Resources.Documents, "doc_new_16.svg", isViewChangeable: false, isOperChangeable: false, isExpanded: true, "", AWPCommandType.SystemCall, "", isInFirstColumnInHomePage: true);
			asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.CashIn.Value, ArmSoft.SME.Common.Resources.CashInputOrder, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.CashOut.Value, ArmSoft.SME.Common.Resources.CashOutputOrder, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PayOrd.Value, ArmSoft.SME.Common.Resources.PaymentOrder, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.BankIn.Value, ArmSoft.SME.Trade.Common.Resources.BankIn, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.BankOut.Value, ArmSoft.SME.Trade.Common.Resources.BankOut, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.Revaluation.Value, ArmSoft.SME.Common.Resources.Revaluation, "doc_new_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode.AddChild(asNavigationTreeNode2, num, checkNodeAccess, addIfHasChild: true);
			AsNavigationTreeNode asNavigationTreeNode3 = new AsNavigationTreeNode(UserAccessIdSE.CashBankOperations.Value, ArmSoft.SME.Common.Resources.Operations, "action_16.svg", isViewChangeable: false, isOperChangeable: false, isExpanded: true, "", AWPCommandType.SystemCall, "", isInFirstColumnInHomePage: true);
			asNavigationTreeNode3.AddChild(new AsNavigationTreeNode(UserAccessIdSE.CBAImportExchangeRates.Value, ArmSoft.SME.Trade.Common.Resources.ImportExchangeRates, "action_16.svg", isViewChangeable: true, isOperChangeable: false), num, checkNodeAccess);
			asNavigationTreeNode3.AddChild(new AsNavigationTreeNode(UserAccessIdSE.ImportBanksFromB2B.Value, ArmSoft.SME.Common.Resources.ImportBanksFromB2B, "action_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode.AddChild(asNavigationTreeNode3, num, checkNodeAccess, addIfHasChild: true);
			AsNavigationTreeNode asNavigationTreeNode4 = new AsNavigationTreeNode(UserAccessIdSE.OnlineBankQuering.Value, ArmSoft.SME.Trade.Common.Resources.OnlineBankQueries, "action_16.svg", isViewChangeable: false, isOperChangeable: false);
			asNavigationTreeNode4.AddChild(new AsNavigationTreeNode(UserAccessIdSE.OnlineAccountStatement.Value, ArmSoft.SME.Trade.Common.Resources.OnlineBankStatement, "action_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode4.AddChild(new AsNavigationTreeNode(UserAccessIdSE.OnlineAccountsRemains.Value, ArmSoft.SME.Common.Resources.AccountsBalance, "action_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode4.AddChild(new AsNavigationTreeNode(UserAccessIdSE.MerchantStatements.Value, ArmSoft.SME.Trade.Common.Resources.MerchantStatements, "action_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode.AddChild(asNavigationTreeNode4, num, checkNodeAccess, addIfHasChild: true);
			AsNavigationTreeNode asNavigationTreeNode5 = new AsNavigationTreeNode(UserAccessIdSE.CashBankJournals.Value, ArmSoft.SME.Common.Resources.Journals, "journal_16.svg", isViewChangeable: false, isOperChangeable: false, isExpanded: true, "", AWPCommandType.SystemCall, "", isInFirstColumnInHomePage: true);
			asNavigationTreeNode5.AddChild(new AsNavigationTreeNode(UserAccessIdSE.CashDocumentsJournal.Value, ArmSoft.SME.Trade.Common.Resources.CashDocumentsJournal, "journal_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode5.AddChild(new AsNavigationTreeNode(UserAccessIdSE.BankDocumentsJournal.Value, ArmSoft.SME.Trade.Common.Resources.BankDocumentsJournal, "journal_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode5.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PaymentOrdersJournal.Value, ArmSoft.SME.Common.Resources.PaymentOrders, "journal_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode5.AddChild(new AsNavigationTreeNode(UserAccessIdSE.IBTransactions.Value, ArmSoft.SME.Common.Resources.IBTransactions, "journal_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode.AddChild(asNavigationTreeNode5, num, checkNodeAccess, addIfHasChild: true);
			AsNavigationTreeNode asNavigationTreeNode6 = new AsNavigationTreeNode(UserAccessIdSE.CashBankReports.Value, ArmSoft.SME.Common.Resources.Reports, "report_16.svg", isViewChangeable: false, isOperChangeable: false);
			asNavigationTreeNode6.AddChild(new AsNavigationTreeNode(UserAccessIdSE.CashRemains.Value, ArmSoft.SME.Trade.Common.Resources.KassaRemains, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode6.AddChild(new AsNavigationTreeNode(UserAccessIdSE.CashTurnOver.Value, ArmSoft.SME.Trade.Common.Resources.KassaTurn, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode6.AddChild(new AsNavigationTreeNode(UserAccessIdSE.CashOpJournal.Value, ArmSoft.SME.Trade.Common.Resources.KassaOpJournal, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode6.AddChild(new AsNavigationTreeNode(UserAccessIdSE.CashDeskTAccount.Value, ArmSoft.SME.Trade.Common.Resources.CashDeskTAccount, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode6.AddChild(new AsNavigationTreeNode(UserAccessIdSE.BankRemains.Value, ArmSoft.SME.Trade.Common.Resources.BankRemains, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode6.AddChild(new AsNavigationTreeNode(UserAccessIdSE.BankTurnOver.Value, ArmSoft.SME.Trade.Common.Resources.BankTurn, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode6.AddChild(new AsNavigationTreeNode(UserAccessIdSE.BankOpJournal.Value, ArmSoft.SME.Trade.Common.Resources.BankOpJournal, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode6.AddChild(new AsNavigationTreeNode(UserAccessIdSE.BankTAccount.Value, ArmSoft.SME.Trade.Common.Resources.BankTAccount, "report_16.svg"), num, checkNodeAccess);
			AsNavigationTreeNode asNavigationTreeNode7 = new AsNavigationTreeNode(UserAccessIdSE.CashBankPartnersReports.Value, ArmSoft.SME.Common.Resources.PartnersReports, "report_16.svg", isViewChangeable: false, isOperChangeable: false, isExpanded: false);
			asNavigationTreeNode7.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PartRemains.Value, ArmSoft.SME.Trade.Common.Resources.PartnerRemains, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode7.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PartTurnOver.Value, ArmSoft.SME.Trade.Common.Resources.PartnerTurn, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode7.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PartOpJournal.Value, ArmSoft.SME.Trade.Common.Resources.PartnerOpJournal, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode7.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PartRepCard, ArmSoft.SME.Trade.Common.Resources.PartnerCard + " (Ctrl+B)", "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode6.AddChild(asNavigationTreeNode7, num, checkNodeAccess, addIfHasChild: true);
			asNavigationTreeNode.AddChild(asNavigationTreeNode6, num, checkNodeAccess, addIfHasChild: true);
			AsNavigationTreeNode asNavigationTreeNode8 = new AsNavigationTreeNode(UserAccessIdSE.CashBankDirectories.Value, ArmSoft.SME.Common.Resources.Directories, "table_16.svg", isViewChangeable: false, isOperChangeable: false);
			asNavigationTreeNode8.AddChild(new AsNavigationTreeNode(UserAccessIdSE.Partners.Value, ArmSoft.SME.Common.Resources.Partners + " (Ctrl+Shift+B)", "table_16.svg"), num, checkNodeAccess);
			AsNavigationTreeNode asNavigationTreeNode9 = new AsNavigationTreeNode(UserAccessIdSE.CashBankPartnersDirectories.Value, ArmSoft.SME.Trade.Common.Resources.PartnersDirectories, "table_16.svg", isViewChangeable: false, isOperChangeable: false, isExpanded: false);
			asNavigationTreeNode9.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PartGroups.Value, ArmSoft.SME.Common.Resources.PartnersGroups, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode9.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PartContracts.Value, ArmSoft.SME.Common.Resources.Contracts, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode9.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PartnerAccountsInBank.Value, ArmSoft.SME.Trade.Common.Resources.PartnersAccountsInBank, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode8.AddChild(asNavigationTreeNode9, num, checkNodeAccess, addIfHasChild: true);
			asNavigationTreeNode8.AddChild(new AsNavigationTreeNode(UserAccessIdSE.CashDesks.Value, ArmSoft.SME.Common.Resources.Cashes, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode8.AddChild(new AsNavigationTreeNode(UserAccessIdSE.CashierShift.Value, ArmSoft.SME.Trade.Common.Resources.CashierShifts, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode8.AddChild(new AsNavigationTreeNode(UserAccessIdSE.AccountsInBank.Value, ArmSoft.SME.Common.Resources.SettlementAccounts, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode8.AddChild(new AsNavigationTreeNode(UserAccessIdSE.Currencies.Value, ArmSoft.SME.Common.Resources.Curres, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode8.AddChild(new AsNavigationTreeNode(UserAccessIdSE.ExchangeRates.Value, ArmSoft.SME.Common.Resources.ExchangeRates, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode8.AddChild(new AsNavigationTreeNode(UserAccessIdSE.Banks.Value, ArmSoft.SME.Common.Resources.Banks, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode8.AddChild(new AsNavigationTreeNode(UserAccessIdSE.StandardComments.Value, ArmSoft.SME.Common.Resources.StandartComments, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode.AddChild(asNavigationTreeNode8, num, checkNodeAccess, addIfHasChild: true);
			SubSystemRoots.Add(4, asNavigationTreeNode);
		}
	}

	private void BuildAnalysisReportsTree()
	{
		short num = 5;
		if (IsSubsystemAccessible(num, out var checkNodeAccess))
		{
			AsNavigationTreeNode asNavigationTreeNode = new AsNavigationTreeNode(num, ArmSoft.SME.Common.Resources.AnalysisCaption, "Analysis_green_24.svg", isViewChangeable: false, isOperChangeable: false);
			AsNavigationTreeNode asNavigationTreeNode2 = new AsNavigationTreeNode(UserAccessIdSE.AnalysisReports.Value, ArmSoft.SME.Common.Resources.Reports, "report_16.svg", isViewChangeable: false, isOperChangeable: false, isExpanded: true, "", AWPCommandType.SystemCall, "", isInFirstColumnInHomePage: true);
			asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.AnalysisDashBoard.Value, ArmSoft.SME.Common.Resources.DashBoardWnd1, "Analysis_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.SalesAnalysisDiagram.Value, ArmSoft.SME.Common.Resources.Sales, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.SalesAnalysisBestSellersReport.Value, ArmSoft.SME.Common.Resources.BestSellers, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.GoodsABCAnalysisReport.Value, ArmSoft.SME.Trade.Common.Resources.ABCAnalysisByGoods, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PartnersABCAnalysisReport.Value, ArmSoft.SME.Trade.Common.Resources.ABCAnalysisByPartners, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.AnalysisDebtorsCreditorsRems.Value, ArmSoft.SME.Common.Resources.DbCrRems, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.CashBankAnalysisReport.Value, ArmSoft.SME.Common.Resources.CashBankReport, "report_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode.AddChild(asNavigationTreeNode2, num, checkNodeAccess, addIfHasChild: true);
			SubSystemRoots.Add(5, asNavigationTreeNode);
		}
	}

	private void BuildTaxServiceTree()
	{
		short num = 8;
		if (IsSubsystemAccessible(num, out var checkNodeAccess))
		{
			AsNavigationTreeNode asNavigationTreeNode = new AsNavigationTreeNode(num, ArmSoft.SME.Common.Resources.EInvoicing, "TaxService_line_24.svg", isViewChangeable: false, isOperChangeable: false);
			AsNavigationTreeNode asNavigationTreeNode2 = new AsNavigationTreeNode(UserAccessIdSE.eInvWriteOffJournals.Value, ArmSoft.SME.Common.Resources.WorkWithSubmittedDocs, "journal_16.svg");
			asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.TaxInvoicesJournal.Value, ArmSoft.SME.Trade.Common.Resources.WriteOffTaxInvoicesJournal, "journal_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.TransportationDocumentsJournal.Value, ArmSoft.SME.Common.Resources.TransportationDocumentsJournal, "journal_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.AssignTaxInvNums.Value, ArmSoft.SME.Common.Resources.AssignTaxInvNums, "action_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.AssignAccDocInvNums.Value, ArmSoft.SME.Common.Resources.AssignAccDocTransportationInvNums, "action_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode.AddChild(asNavigationTreeNode2, num, checkNodeAccess, addIfHasChild: true);
			asNavigationTreeNode2 = new AsNavigationTreeNode(UserAccessIdSE.eInvReceivedJournals.Value, ArmSoft.SME.Common.Resources.WorkWithReceivedDocs, "journal_16.svg", isViewChangeable: false, isOperChangeable: false);
			asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.ReceivedInvoicesJournal.Value, ArmSoft.SME.Trade.Common.Resources.ReceivedTaxInvoicesJournal, "journal_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.ImportTaxInvoice.Value, ArmSoft.SME.Common.Resources.ReceivedTaxInvImport, "action_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.PurchaseDocumentsJournal.Value, ArmSoft.SME.Trade.Common.Resources.PurchaseDocumentsJournal, "journal_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode.AddChild(asNavigationTreeNode2, num, checkNodeAccess, addIfHasChild: true);
			asNavigationTreeNode2 = new AsNavigationTreeNode(UserAccessIdSE.TaxService.Value, ArmSoft.SME.Common.Resources.OnlineView + " " + ArmSoft.SME.Common.Resources.DirrectConnection, "table_16.svg", isViewChangeable: false, isOperChangeable: false);
			AsNavigationTreeNode asNavigationTreeNode3 = new AsNavigationTreeNode(UserAccessIdSE.TaxServiceTaxInvoices.Value, ArmSoft.SME.Common.Resources.TaxInvoices, "table_16.svg", isViewChangeable: false, isOperChangeable: false);
			asNavigationTreeNode3.AddChild(new AsNavigationTreeNode(UserAccessIdSE.TaxServiceTaxInvoicesSubmitted.Value, ArmSoft.SME.Common.Resources.EInvoicingSubmitted, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode3.AddChild(new AsNavigationTreeNode(UserAccessIdSE.TaxServiceTaxInvoicesReceived.Value, ArmSoft.SME.Common.Resources.EInvoicingReceived, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode3.AddChild(new AsNavigationTreeNode(UserAccessIdSE.TaxServiceTaxInvoicesDraft.Value, ArmSoft.SME.Common.Resources.EInvoicingDraft, "table_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode2.AddChild(asNavigationTreeNode3, num, checkNodeAccess, addIfHasChild: true);
			asNavigationTreeNode3 = new AsNavigationTreeNode(UserAccessIdSE.TaxServiceAccDocs.Value, ArmSoft.SME.Common.Resources.TaxAccDocs, "table_red_16.svg", isViewChangeable: false, isOperChangeable: false);
			asNavigationTreeNode3.AddChild(new AsNavigationTreeNode(UserAccessIdSE.TaxServiceAccDocsSubmitted.Value, ArmSoft.SME.Common.Resources.EInvoicingSubmitted, "table_red_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode3.AddChild(new AsNavigationTreeNode(UserAccessIdSE.TaxServiceAccDocsReceived.Value, ArmSoft.SME.Common.Resources.EInvoicingReceived, "table_red_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode3.AddChild(new AsNavigationTreeNode(UserAccessIdSE.TaxServiceAccDocsDraft.Value, ArmSoft.SME.Common.Resources.EInvoicingDraft, "table_red_16.svg"), num, checkNodeAccess);
			asNavigationTreeNode2.AddChild(asNavigationTreeNode3, num, checkNodeAccess, addIfHasChild: true);
			asNavigationTreeNode.AddChild(asNavigationTreeNode2, num, checkNodeAccess, addIfHasChild: true);
			SubSystemRoots.Add(8, asNavigationTreeNode);
		}
	}

	private void FillAdminToolsTree()
	{
		AsNavigationTreeNode asNavigationTreeNode = new AsNavigationTreeNode(9, ArmSoft.SME.Common.Resources.AdministrativeTools, "admin tools_green_24.svg", isViewChangeable: false, isOperChangeable: false);
		AsNavigationTreeNode asNavigationTreeNode2 = new AsNavigationTreeNode(UserAccessIdSE.None.Value, ArmSoft.SME.Common.Resources.AdministrativeTools, "tools_sm_24.svg", isViewChangeable: false, isOperChangeable: false, isExpanded: true, "", AWPCommandType.SystemCall, "", isInFirstColumnInHomePage: true);
		asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.ChangeActualnessDate.Value, ArmSoft.SME.Trade.Common.Resources.ChangeActualnessDate, "error_go_16.svg", isViewChangeable: false, isOperChangeable: false));
		asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.Users.Value, ArmSoft.SME.Common.Resources.Users, "users_16.svg", isViewChangeable: false, isOperChangeable: false));
		asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.DatabaseLog.Value, ArmSoft.SME.Common.Resources.UsersLoginHistory, "login_history_16.svg", isViewChangeable: false, isOperChangeable: false));
		asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.Templates.Value, ArmSoft.SME.Common.Resources.PrintTemplates, "doc_edit_16.svg", isViewChangeable: false, isOperChangeable: false));
		asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.ImportData.Value, ArmSoft.SME.Common.Resources.ImportData, "Import_16.svg", isViewChangeable: false, isOperChangeable: false));
		asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.ImportDataFromASAccountantWindow.Value, ArmSoft.SME.Trade.Common.Resources.ImportDataFromASAccountant, _newOthersUri, isViewChangeable: false, isOperChangeable: false));
		asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.ExportData.Value, ArmSoft.SME.Common.Resources.ExportData, "export_16.svg", isViewChangeable: false, isOperChangeable: false));
		asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.DeletedDocumentsJournal.Value, ArmSoft.SME.Common.Resources.DeletedDocumentsJournal, "journal_16.svg", isViewChangeable: false, isOperChangeable: false));
		asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.DocLogs.Value, ArmSoft.SME.Common.Resources.DocumentsLog, "journal_16.svg", isViewChangeable: false, isOperChangeable: false));
		asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.DirsLog.Value, ArmSoft.SME.Trade.Common.Resources.DirectoriesLog, "journal_16.svg", isViewChangeable: false, isOperChangeable: false));
		asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.DataViewLayouts.Value, ArmSoft.SME.Common.Resources.DataViewLayouts, "layout_icon16.svg"));
		asNavigationTreeNode2.AddChild(new AsNavigationTreeNode(UserAccessIdSE.RefreshServiceCaches.Value, ArmSoft.SME.Common.Resources.RefreshCacheInService, "action_16.svg"));
		asNavigationTreeNode.AddChild(asNavigationTreeNode2);
		AsNavigationTreeNode asNavigationTreeNode3 = new AsNavigationTreeNode(UserAccessIdSE.DatabasesOperations.Value, ArmSoft.SME.Common.Resources.Databases, "databases_16.svg", isViewChangeable: false, isOperChangeable: false, isExpanded: true, "", AWPCommandType.SystemCall, "", isInFirstColumnInHomePage: false, null, isInSecondColumnInHomePage: true);
		asNavigationTreeNode3.AddChild(new AsNavigationTreeNode(UserAccessIdSE.Databases.Value, ArmSoft.SME.Common.Resources.Databases, "databases_16.svg", isViewChangeable: false, isOperChangeable: false));
		asNavigationTreeNode3.AddChild(new AsNavigationTreeNode(UserAccessIdSE.Backups.Value, ArmSoft.SME.Common.Resources.Backups, "database_save_16.svg", isViewChangeable: false, isOperChangeable: false));
		asNavigationTreeNode3.AddChild(new AsNavigationTreeNode(UserAccessIdSE.OnlineLicenses.Value, ArmSoft.SME.Common.Resources.OnlineLicenses, "computer_network_red_16.svg", isViewChangeable: false, isOperChangeable: false));
		asNavigationTreeNode3.AddChild(new AsNavigationTreeNode(UserAccessIdSE.DatabaseConnections.Value, ArmSoft.SME.Common.Resources.DatabaseConnections, "database_connect_16.svg", isViewChangeable: false, isOperChangeable: false));
		asNavigationTreeNode3.AddChild(new AsNavigationTreeNode(UserAccessIdSE.CheckIntegrity.Value, ArmSoft.SME.Common.Resources.DataIntegrityChecking, "database_error_16.svg", isViewChangeable: false, isOperChangeable: false));
		asNavigationTreeNode3.AddChild(new AsNavigationTreeNode(UserAccessIdSE.Archive.Value, ArmSoft.SME.Common.Resources.Archive, "database_lightning_16.svg", isViewChangeable: false, isOperChangeable: false));
		asNavigationTreeNode.AddChild(asNavigationTreeNode3);
		AsNavigationTreeNode asNavigationTreeNode4 = new AsNavigationTreeNode(UserAccessIdSE.Integrations.Value, ArmSoft.SME.Common.Resources.Integrations, "wingear_16.svg", isViewChangeable: false, isOperChangeable: false, isExpanded: true, "", AWPCommandType.SystemCall, "", isInFirstColumnInHomePage: false, null, isInSecondColumnInHomePage: true);
		asNavigationTreeNode4.AddChild(new AsNavigationTreeNode(UserAccessIdSE.ApiKeys.Value, ArmSoft.SME.Common.Resources.ApiKeys, "wingear_16.svg", isViewChangeable: true, isOperChangeable: true, isExpanded: true, "", AWPCommandType.SystemCall, "", isInFirstColumnInHomePage: false, FeatureStatusTypeSE.New));
		if (_useMarking)
		{
			asNavigationTreeNode4.AddChild(new AsNavigationTreeNode(UserAccessIdSE.EMarkReceiptApp.Value, ArmSoft.SME.Trade.Common.Resources.EMarkReceiptApp, "wingear_16.svg", isViewChangeable: true, isOperChangeable: true, isExpanded: true, "", AWPCommandType.SystemCall, "", isInFirstColumnInHomePage: false, FeatureStatusTypeSE.New));
		}
		asNavigationTreeNode.AddChild(asNavigationTreeNode4);
		string image = "file_xml_16.svg";
		AsNavigationTreeNode asNavigationTreeNode5 = new AsNavigationTreeNode(UserAccessIdSE.Extensions.Value, ArmSoft.SME.Common.Resources.Extensions, image, isViewChangeable: false, isOperChangeable: false);
		asNavigationTreeNode5.AddChild(new AsNavigationTreeNode(UserAccessIdSE.ImportExtensions.Value, ArmSoft.SME.Common.Resources.ImportExtensions, image, isViewChangeable: false, isOperChangeable: false));
		asNavigationTreeNode5.AddChild(new AsNavigationTreeNode(UserAccessIdSE.DataViewsExtensions.Value, ArmSoft.SME.Common.Resources.DataViewsExtensions, image, isViewChangeable: false, isOperChangeable: false));
		asNavigationTreeNode5.AddChild(new AsNavigationTreeNode(UserAccessIdSE.UserScriptsDefinitions.Value, ArmSoft.SME.Common.Resources.UserScriptsDefinitions, image, isViewChangeable: false, isOperChangeable: false));
		asNavigationTreeNode5.AddChild(new AsNavigationTreeNode(UserAccessIdSE.AWPDefinitions.Value, ArmSoft.SME.Common.Resources.AWPDefinitions, image, isViewChangeable: false, isOperChangeable: false));
		asNavigationTreeNode.AddChild(asNavigationTreeNode5);
		SubSystemRoots.Add(9, asNavigationTreeNode);
	}
}
